using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAuditTrailService
    {
        Task<IResult> LogActionAsync(int userId, string actionType, string description, string? oldValue, string? newValue, string clientIp);
        Task<IDataResult<List<AuditLogDto>>> GetAuditLogsAsync(DateTime start, DateTime end, int? userId = null, string? actionType = null);
    }
}