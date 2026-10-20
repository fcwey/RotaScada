using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using Entities.Dtos;
using Microsoft.EntityFrameworkCore;
namespace Business.Concrete
{
    public class HistorianManager : IHistorianService
    {
        private readonly NorthwindContext _context;

        public HistorianManager(NorthwindContext context)
        {
            _context = context;
        }

        public async Task<IResult> LogProcessDataAsync(ProcessDataLog dataLog)
        {
            await _context.ProcessDataLogs.AddAsync(dataLog);
            await _context.SaveChangesAsync();
            return new SuccessResult("Telemetri verisi kaydedildi.");
        }

        public async Task<IDataResult<List<TrendPointDto>>> GetTrendDataAsync(string tagName, DateTime start, DateTime end, int targetPointCount = 500)
        {
            // Zaman aralığındaki ham veriyi çeker
            var rawLogs = await _context.ProcessDataLogs
                .AsNoTracking()
                .Where(p => p.Timestamp >= start && p.Timestamp <= end)
                .OrderBy(p => p.Timestamp)
                .ToListAsync();

            if (!rawLogs.Any())
                return new SuccessDataResult<List<TrendPointDto>>(new List<TrendPointDto>());

            // Downsampling (Veri Sıkıştırma) Mantığı
            int totalCount = rawLogs.Count;
            int step = Math.Max(1, totalCount / targetPointCount);

            var trendPoints = new List<TrendPointDto>();
            for (int i = 0; i < totalCount; i += step)
            {
                var item = rawLogs[i];
                float value = ExtractValueByTag(item, tagName);

                trendPoints.Add(new TrendPointDto
                {
                    Timestamp = item.Timestamp,
                    Value = value
                });
            }

            return new SuccessDataResult<List<TrendPointDto>>(trendPoints);
        }

        public async Task<IResult> CompressOldLogsAsync(DateTime olderThan)
        {
            var oldLogs = await _context.ProcessDataLogs
                .Where(p => p.Timestamp < olderThan)
                .ToListAsync();

            _context.ProcessDataLogs.RemoveRange(oldLogs);
            await _context.SaveChangesAsync();

            return new SuccessResult($"{oldLogs.Count} adet eski telemetri kaydı temizlendi.");
        }

        private float ExtractValueByTag(ProcessDataLog log, string tagName) => tagName switch
        {
            "Zone1Temperature" => log.Zone1Temperature,
            "Zone2Temperature" => log.Zone2Temperature,
            "ChamberPressure" => log.ChamberPressure,
            "GasFlowRate" => log.GasFlowRate,
            _ => 0f
        };
    }
}
