using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Structures;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using S7.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class RecipeManager : IRecipeService
    {
        private readonly NorthwindContext _context;
        private readonly IS7DriverDal _s7DriverDal;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public RecipeManager(NorthwindContext context, IS7DriverDal s7DriverDal, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _s7DriverDal = s7DriverDal;
            _httpContextAccessor = httpContextAccessor;
        }

        // İstemcinin IP adresini istekten okur, istek yoksa 127.0.0.1 döner.
        private string GetClientIpAddress()
        {
            var httpContext = _httpContextAccessor?.HttpContext;
            if (httpContext == null) return "127.0.0.1";

            string? ip = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = httpContext.Connection.RemoteIpAddress?.ToString();
            }
            return ip ?? "127.0.0.1";
        }

        // Tarih ve saatten parti kodu üretir (Örn: BATCH-20261028-012845).
        private string GenerateUniqueBatchCode()
        {
            return $"BATCH-{DateTime.Now:yyyyMMdd-HHmmss}";
        }

        // Tüm reçeteleri adımlarıyla birlikte listeler.
        public async Task<IDataResult<List<Recipe>>> GetAllRecipesAsync()
        {
            var recipes = await _context.Recipes.Include(r => r.Steps).ToListAsync();
            return new SuccessDataResult<List<Recipe>>(recipes, "Reçeteler listelendi.");
        }

        // Verilen id'ye ait reçeteyi adımlarıyla birlikte getirir.
        public async Task<IDataResult<Recipe>> GetRecipeByIdAsync(int id)
        {
            var recipe = await _context.Recipes.Include(r => r.Steps).FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null) return new ErrorDataResult<Recipe>("Reçete bulunamadı.");
            return new SuccessDataResult<Recipe>(recipe);
        }

        // Yeni reçeteyi veritabanına ekler.
        public async Task<IResult> AddRecipeAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync(); // NorthwindContext Interceptor otomatik AuditLog oluşturur
            return new SuccessResult("Reçete veritabanına eklendi.");
        }

        // Mevcut reçeteyi günceller.
        public async Task<IResult> UpdateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync(); // Interceptor otomatik AuditLog oluşturur
            return new SuccessResult("Reçete güncellendi.");
        }

        // Verilen id'ye ait reçeteyi siler.
        public async Task<IResult> DeleteRecipeAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return new ErrorResult("Silinecek reçete bulunamadı.");
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync(); // Interceptor otomatik AuditLog oluşturur
            return new SuccessResult("Reçete silindi.");
        }

        // Reçeteyi PLC'nin belirtilen DB bloğuna yazar ve audit log kaydı düşer.
        public async Task<IResult> DownloadRecipeToPlcAsync(int recipeId, int targetDbNumber = 2, int? userId = null, string? userIp = null)
        {
            userIp ??= GetClientIpAddress();

            var recipe = await _context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
                return new ErrorResult("PLC'ye aktarılacak reçete veritabanında bulunamadı.");

            if (!_s7DriverDal.IsConnected)
                return new ErrorResult("PLC bağlantısı yok! Reçete yüklenemedi.");

            var sortedSteps = recipe.Steps.OrderBy(s => s.StepNo).ToList();

            var plcRecipe = new PlcRecipeStruct
            {
                RecipeID = (short)recipe.RecipeID,
                RecipeType = (short)recipe.RecipeType,
                RecipeName = recipe.RecipeName.Length > 30 ? recipe.RecipeName.Substring(0, 30) : recipe.RecipeName,
                TotalStepCount = (short)Math.Min(sortedSteps.Count, 20)
            };

            for (int i = 0; i < 20; i++)
            {
                if (i < sortedSteps.Count)
                {
                    var step = sortedSteps[i];
                    plcRecipe.Steps[i] = new PlcStepStruct
                    {
                        StepType = (short)step.StepType,
                        Vac_SetPoint = step.Vac_SetPoint,
                        Vac_Timeout = (uint)step.Vac_TimeoutMs,
                        TargetTemp = step.TargetTemp,
                        StepTime = (uint)step.StepTimeMs,
                        GSoak = step.GSoak,
                        N2_Flow = step.N2_Flow,
                        NH3_Flow = step.NH3_Flow,
                        dNH3_Flow = step.dNH3_Flow,
                        Su_Flow = step.Su_Flow,
                        Oxygen_Set = step.Oxygen_Set
                    };
                }
                else
                {
                    plcRecipe.Steps[i] = new PlcStepStruct();
                }
            }

            await _s7DriverDal.WriteClassAsync(plcRecipe, targetDbNumber, startByte: 0);

            // Fiziki PLC İşlemi İçin Explicit Audit Log Kaydı
            await _context.AuditLogs.AddAsync(new AuditLog
            {
                UserId = userId,
                UserIpAddress = userIp,
                ActionType = "PLC_RECIPE_DOWNLOAD",
                ModuleName = "RecipeManager",
                Description = $"'{recipe.RecipeName}' (ID: {recipeId}) reçetesi PLC DB{targetDbNumber} alanına yazıldı.",
                IsSuccess = true,
                Timestamp = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return new SuccessResult($"'{recipe.RecipeName}' reçetesi PLC DB{targetDbNumber} alanına başarıyla indirildi.");
        }

        // Şarj kaydını açar, reçeteyi PLC'ye indirir ve start darbesi gönderir; hata olursa şarjı ABORTED yapar.
        public async Task<IResult> DownloadAndStartRecipeAsync(int recipeId, int? userId = null, string? userIp = null)
        {
            userIp ??= GetClientIpAddress();

            // A) Hızlı kontrol: Fırında aktif çalışan şarj var mı? (Asıl koruma veritabanındaki unique index'tir)
            var activeBatch = await _context.Batches.AsNoTracking().FirstOrDefaultAsync(b => b.Status == "RUNNING");
            if (activeBatch != null)
            {
                return new ErrorResult($"Fırında zaten çalışan bir şarj mevcut! ({activeBatch.BatchCode}). Yeni şarj başlatmadan önce mevcut şarjı sonlandırın.");
            }

            // B) Reçete var mı? (Şarj kaydı açılmadan önce kontrol edilir)
            bool recipeExists = await _context.Recipes.AnyAsync(r => r.Id == recipeId);
            if (!recipeExists)
            {
                return new ErrorResult("Başlatılacak reçete veritabanında bulunamadı.");
            }

            // C) Şarj kaydı PLC'ye dokunulmadan ÖNCE açılır; unique index sayesinde aynı anda yalnızca bir istek bunu başarır
            var newBatch = new Batch
            {
                BatchCode = GenerateUniqueBatchCode(),
                RecipeId = recipeId,
                OperatorUserId = userId ?? 1,
                StartTime = DateTime.Now,
                Status = "RUNNING",
                CompletionNote = "Fırın otomasyonu başlatıldı."
            };

            try
            {
                await _context.Batches.AddAsync(newBatch);
                await _context.SaveChangesAsync(); // newBatch.Id burada oluşur
            }
            catch (DbUpdateException)
            {
                // Başarısız kayıt, paylaşılan context'in takibinde kalıp sonraki SaveChanges çağrılarını bozmasın
                _context.Entry(newBatch).State = EntityState.Detached;
                return new ErrorResult("Fırında zaten çalışan bir şarj mevcut! Aynı anda başka bir başlatma isteği işlendi.");
            }

            try
            {
                // D) Reçeteyi PLC DB2'ye indir
                var downloadResult = await DownloadRecipeToPlcAsync(recipeId, targetDbNumber: 2, userId, userIp);
                if (!downloadResult.IsSuccess)
                {
                    await AbortBatchAsync(newBatch, $"Reçete PLC'ye indirilemedi: {downloadResult.Message}", userId, userIp);
                    return downloadResult;
                }

                // E) DB3.DBX0.0 adresine 1 saniyelik Start darbesi gönderilir
                await _s7DriverDal.PulseBitAsync(DataType.DataBlock, db: 3, startByte: 0, bitNumber: 0, durationMs: 1000);

                // F) Audit Log Kaydı
                await _context.AuditLogs.AddAsync(new AuditLog
                {
                    UserId = userId,
                    UserIpAddress = userIp,
                    ActionType = "BATCH_START_PULSE",
                    ModuleName = "RecipeManager",
                    Description = $"'{newBatch.BatchCode}' (Batch ID: {newBatch.Id}) şarjı başlatıldı ve DB3.DBX0.0 adresine 1000ms Start darbesi gönderildi.",
                    IsSuccess = true,
                    Timestamp = DateTime.Now
                });
                await _context.SaveChangesAsync();

                return new SuccessDataResult<int>(newBatch.Id, $"Reçete PLC'ye indirildi. '{newBatch.BatchCode}' kodlu Şarj (Batch ID: {newBatch.Id}) başlatıldı.");
            }
            catch (Exception ex)
            {
                // Şarj kaydı RUNNING bırakılmaz; aksi halde sonraki tüm başlatmalar bloke olur
                bool aborted = await AbortBatchAsync(newBatch, $"Şarj başlatılırken hata oluştu: {ex.Message}", userId, userIp);

                string abortInfo = aborted
                    ? "Şarj kaydı ABORTED olarak kapatıldı."
                    : "Şarj kaydı ABORTED yapılamadı, veritabanını manuel kontrol edin.";

                return new ErrorResult($"Şarj başlatılamadı: {ex.Message} {abortInfo} Fırının durumunu PLC üzerinden kontrol edin.");
            }
        }

        // Şarjı ABORTED olarak kapatır ve audit log düşer; başarılıysa true döner.
        private async Task<bool> AbortBatchAsync(Batch batch, string note, int? userId, string userIp)
        {
            try
            {
                batch.Status = "ABORTED";
                batch.EndTime = DateTime.Now;
                batch.CompletionNote = note;
                _context.Batches.Update(batch);

                await _context.AuditLogs.AddAsync(new AuditLog
                {
                    UserId = userId,
                    UserIpAddress = userIp,
                    ActionType = "BATCH_START_ERROR",
                    ModuleName = "RecipeManager",
                    Description = $"'{batch.BatchCode}' şarjı iptal edildi (ABORTED). {note}",
                    IsSuccess = false,
                    Timestamp = DateTime.Now
                });
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Çalışan (RUNNING) şarjı COMPLETED olarak kapatır.
        public async Task<IResult> StopActiveBatchAsync(string completionNote = "Proses tamamlandı.", int? userId = null, string? userIp = null)
        {
            userIp ??= GetClientIpAddress();

            var activeBatch = await _context.Batches.FirstOrDefaultAsync(b => b.Status == "RUNNING");
            if (activeBatch == null)
            {
                return new ErrorResult("Kapatılacak aktif bir şarj bulunamadı.");
            }

            activeBatch.EndTime = DateTime.Now;
            activeBatch.Status = "COMPLETED";
            activeBatch.CompletionNote = completionNote;

            _context.Batches.Update(activeBatch);

            await _context.AuditLogs.AddAsync(new AuditLog
            {
                UserId = userId,
                UserIpAddress = userIp,
                ActionType = "BATCH_COMPLETED",
                ModuleName = "RecipeManager",
                Description = $"'{activeBatch.BatchCode}' kodlu şarj tamamlandı olarak kapatıldı. Not: {completionNote}",
                IsSuccess = true,
                Timestamp = DateTime.Now
            });

            await _context.SaveChangesAsync();

            return new SuccessResult($"'{activeBatch.BatchCode}' kodlu şarj başarıyla tamamlandı.");
        }
    }
}