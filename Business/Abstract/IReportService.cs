using Core.Utilities.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IReportService
    {
        // Şarj/Parti bazlı PDF rapor çıktısı oluşturur (Byte dizisi döner)
        Task<IDataResult<byte[]>> GenerateBatchPdfReportAsync(int batchId);

        // Belirtilen tarih aralığındaki telemetri verilerini CSV/Excel formatında fırlatır
        Task<IDataResult<byte[]>> ExportTrendToCsvAsync(DateTime start, DateTime end, string[] tagNames);
    }
}
