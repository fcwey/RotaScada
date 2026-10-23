using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class ReportManager : IReportService
    {
        private readonly NorthwindContext _context;

        public ReportManager(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IDataResult<byte[]>> GenerateBatchPdfReportAsync(int batchId)
        {
            var batch = await _context.Batches
                .Include(b => b.Recipe)
                .FirstOrDefaultAsync(b => b.Id == batchId);

            if (batch == null)
                return new ErrorDataResult<byte[]>("Raporlanacak şarj/parti kaydı bulunamadı.");

            var logs = await _context.ProcessDataLogs
                .AsNoTracking()
                .Where(p => p.BatchId == batchId)
                .OrderBy(p => p.Timestamp)
                .ToListAsync();

            // Rapor Özet Değerleri
            float maxTemp = logs.Any() ? logs.Max(l => l.Zone1Temperature) : 0f;
            float avgTemp = logs.Any() ? logs.Average(l => l.Zone1Temperature) : 0f;

            // Rapor Metni / HTML Yapısı
            var sb = new StringBuilder();
            sb.AppendLine("=================================================");
            sb.AppendLine("          ISIL İŞLEM ŞARJ VE PROSES RAPORU       ");
            sb.AppendLine("=================================================");
            sb.AppendLine($"Şarj Kodu      : {batch.BatchCode}");
            sb.AppendLine($"Reçete Adı     : {batch.Recipe?.RecipeName ?? "N/A"}");
            sb.AppendLine($"Başlangıç      : {batch.StartTime:dd.MM.yyyy HH:mm:ss}");
            sb.AppendLine($"Bitiş          : {(batch.EndTime.HasValue ? batch.EndTime.Value.ToString("dd.MM.yyyy HH:mm:ss") : "Devam Ediyor")}");
            sb.AppendLine($"Durum          : {batch.Status}");
            sb.AppendLine("-------------------------------------------------");
            sb.AppendLine($"Maksimum Sıcaklık : {maxTemp:F1} °C");
            sb.AppendLine($"Ortalama Sıcaklık : {avgTemp:F1} °C");
            sb.AppendLine($"Toplam Log Sayısı : {logs.Count}");
            sb.AppendLine("=================================================");

            byte[] reportBytes = Encoding.UTF8.GetBytes(sb.ToString());
            return new SuccessDataResult<byte[]>(reportBytes, "Şarj raporu başarıyla üretildi.");
        }

        public async Task<IDataResult<byte[]>> ExportTrendToCsvAsync(DateTime start, DateTime end, string[] tagNames)
        {
            var logs = await _context.ProcessDataLogs
                .AsNoTracking()
                .Where(p => p.Timestamp >= start && p.Timestamp <= end)
                .OrderBy(p => p.Timestamp)
                .ToListAsync();

            var csvBuilder = new StringBuilder();
            csvBuilder.AppendLine("Timestamp;Zone1Temperature;Zone2Temperature;ChamberPressure;GasFlowRate;BurnerStatus");

            foreach (var item in logs)
            {
                csvBuilder.AppendLine($"{item.Timestamp:yyyy-MM-dd HH:mm:ss};{item.Zone1Temperature};{item.Zone2Temperature};{item.ChamberPressure};{item.GasFlowRate};{item.BurnerStatus}");
            }

            byte[] csvBytes = Encoding.UTF8.GetBytes(csvBuilder.ToString());
            return new SuccessDataResult<byte[]>(csvBytes, "Trend CSV dışa aktarımı tamamlandı.");
        }
    }
}