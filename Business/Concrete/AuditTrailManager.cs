using Business.Abstract;
using Core.Utilities.Results;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
   
    public class AuditTrailManager : IAuditTrailService
    {
        private readonly IAuditLogDal _auditLogDal;

        public AuditTrailManager(IAuditLogDal auditLogDal)
        {
            _auditLogDal = auditLogDal;
        }

        public async Task<IResult> LogActionAsync(int userId, string actionType, string description, string? oldValue, string? newValue, string clientIp)
        {
            var log = new AuditLog
            {
                UserId = userId,
                ActionType = actionType,
                Description = description,
                OldValues = oldValue,
                NewValues = newValue,
                UserIpAddress = clientIp,
                Timestamp = DateTime.Now
            };

            await _auditLogDal.AddAsync(log);
            return new SuccessResult("Operatör eylemi kaydedildi.");
        }

        public async Task<IDataResult<List<AuditLogDto>>> GetAuditLogsAsync(DateTime start, DateTime end, int? userId = null, string? actionType = null)
        {
            var logs = await _auditLogDal.GetListAsync(l =>
                l.Timestamp >= start &&
                l.Timestamp <= end &&
                (!userId.HasValue || l.UserId == userId.Value) &&
                (string.IsNullOrEmpty(actionType) || l.ActionType == actionType));

            var dtos = logs.Select(l => new AuditLogDto
            {
                Id = l.Id,
                UserId = l.UserId,
                ActionType = l.ActionType,
                Description = l.Description,
                OldValue = l.OldValues,
                NewValue = l.NewValues,
                ClientIp = l.UserIpAddress,
                Timestamp = l.Timestamp
            }).ToList();

            return new SuccessDataResult<List<AuditLogDto>>(dtos);
        }
    }
}
