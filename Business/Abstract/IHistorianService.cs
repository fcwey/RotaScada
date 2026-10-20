using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Abstract
{
    public interface IHistorianService
    {
        // Anlık telemetri nesnesini veritabanına ekler
        Task<IResult> LogProcessDataAsync(ProcessDataLog dataLog);

        // Trend grafikleri için çözünürlüğü ayarlanmış (Downsampled) noktaları döndürür
        Task<IDataResult<List<TrendPointDto>>> GetTrendDataAsync(string tagName, DateTime start, DateTime end, int targetPointCount = 500);

        // Eski log verilerini arşivler veya temizler
        Task<IResult> CompressOldLogsAsync(DateTime olderThan);
    }
}
