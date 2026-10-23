using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Structures; // ◄── PlcRecipeStruct ve PlcStepStruct için şarttır
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

        public RecipeManager(NorthwindContext context, IS7DriverDal s7DriverDal)
        {
            _context = context;
            _s7DriverDal = s7DriverDal;
        }

        public async Task<IDataResult<List<Recipe>>> GetAllRecipesAsync()
        {
            var recipes = await _context.Recipes.Include(r => r.Steps).ToListAsync();
            return new SuccessDataResult<List<Recipe>>(recipes, "Reçeteler listelendi.");
        }

        public async Task<IDataResult<Recipe>> GetRecipeByIdAsync(int id)
        {
            var recipe = await _context.Recipes.Include(r => r.Steps).FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null) return new ErrorDataResult<Recipe>("Reçete bulunamadı.");
            return new SuccessDataResult<Recipe>(recipe);
        }

        public async Task<IResult> AddRecipeAsync(Recipe recipe)
        {
            await _context.Recipes.AddAsync(recipe);
            await _context.SaveChangesAsync();
            return new SuccessResult("Reçete veritabanına eklendi.");
        }

        public async Task<IResult> UpdateRecipeAsync(Recipe recipe)
        {
            _context.Recipes.Update(recipe);
            await _context.SaveChangesAsync();
            return new SuccessResult("Reçete güncellendi.");
        }

        public async Task<IResult> DeleteRecipeAsync(int id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            if (recipe == null) return new ErrorResult("Silinecek reçete bulunamadı.");
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return new SuccessResult("Reçete silindi.");
        }

        // 1. Reçeteyi PLC DB2'ye İndir
        public async Task<IResult> DownloadRecipeToPlcAsync(int recipeId, int targetDbNumber = 2)
        {
            var recipe = await _context.Recipes
                .Include(r => r.Steps)
                .FirstOrDefaultAsync(r => r.Id == recipeId);

            if (recipe == null)
                return new ErrorResult("PLC'ye aktarılacak reçete veritabanında bulunamadı.");

            if (!_s7DriverDal.IsConnected)
                return new ErrorResult("PLC bağlantısı yok! Reçete yüklenemedi.");

            var plcRecipe = new PlcRecipeStruct
            {
                RecipeID = (short)recipe.RecipeID,
                RecipeType = (short)recipe.RecipeType,
                RecipeName = recipe.RecipeName.Length > 30 ? recipe.RecipeName.Substring(0, 30) : recipe.RecipeName,
                TotalStepCount = (short)recipe.Steps.Count
            };

            var sortedSteps = recipe.Steps.OrderBy(s => s.StepNo).ToList();
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
            return new SuccessResult($"'{recipe.RecipeName}' reçetesi PLC DB{targetDbNumber} alanına başarıyla indirildi.");
        }

        // 2. Reçeteyi İndir ve Fırını Başlat
        public async Task<IResult> DownloadAndStartRecipeAsync(int recipeId)
        {
            var downloadResult = await DownloadRecipeToPlcAsync(recipeId, targetDbNumber: 2);
            if (!downloadResult.IsSuccess)
                return downloadResult;

            try
            {
                // DB3.DBX0.0 adresine 1 saniyelik Start Darbesi gönderilir
                await _s7DriverDal.PulseBitAsync(DataType.DataBlock, db: 3, startByte: 0, bitNumber: 0, durationMs: 1000);
                return new SuccessResult("Reçete PLC'ye indirildi ve Fırın Start komutu tetiklendi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Reçete yüklendi fakat Start verilirken hata oluştu: {ex.Message}");
            }
        }
    }
}