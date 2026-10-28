using Business.Abstract;
using Core.Constants;
using Core.Utilities.IoC;
using Core.Utilities.Results;
using DataAccess.Abstract;
using DataAccess.Concrete.EntityFramework.Contexts;
using Entities.Concrete;
using Entities.Dtos;
using S7.Net;
using System;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class SystemSettingManager : ISystemSettingService
    {
        private readonly NorthwindContext _context;
        private readonly IS7DriverDal _s7DriverDal;
        private readonly IUserContextHelper _userContext;

        public SystemSettingManager(NorthwindContext context, IS7DriverDal s7DriverDal, IUserContextHelper userContext)
        {
            _context = context;
            _s7DriverDal = s7DriverDal;
            _userContext = userContext;
        }

        public async Task<IResult> UpdateCriticalParameterAsync(ParameterUpdateDto dto)
        {
            // 1. Parametrenin Gerektirdiği Yetkiyi Belirle
            string requiredPermission = GetRequiredPermissionForParameter(dto.ParameterKey);

            // 2. Kullanıcının Bu Yetkiye Sahip Olup Olmadığını Kontrol Et
            var userRolesAndClaims = _userContext.CurrentUserRoles;
            if (!userRolesAndClaims.Contains(requiredPermission) && !userRolesAndClaims.Contains("Admin"))
            {
                // Yetkisiz Erişim Kaydını da AuditLog'a Düşüyoruz (Güvenlik İhlali Tespiti)
                await LogUnauthorizedAttemptAsync(dto.ParameterKey, requiredPermission);

                return new ErrorResult($"YETKİSİZ ERİŞİM: '{dto.ParameterKey}' parametresini değiştirmek için '{requiredPermission}' yetkisi gereklidir!");
            }

            // 3. Yetki Doğrulandı -> PLC'ye Veriyi Yaz
            if (!_s7DriverDal.IsConnected)
                return new ErrorResult("PLC Bağlantısı yok! Parametre yazılamadı.");

            await _s7DriverDal.WriteSetpointAsync(dto.DbNumber, dto.StartByte, dto.NewValue);

            // 4. Otomatik Audit Log Kaydı
            await _context.AuditLogs.AddAsync(new AuditLog
            {
                UserId = _userContext.CurrentUserId,
                Username = _userContext.CurrentUserEmail,
                UserIpAddress = _userContext.ClientIpAddress,
                ActionType = "PARAMETER_UPDATE",
                ModuleName = "SystemSettingManager",
                Description = $"'{dto.ParameterKey}' parametresi {dto.NewValue} olarak değiştirildi. (PLC DB{dto.DbNumber}.CBD{dto.StartByte})",
                IsSuccess = true,
                Timestamp = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return new SuccessResult($"'{dto.ParameterKey}' parametresi başarıyla güncellendi.");
        }

        // Parametreye Özel Yetki Eşleştirme Haritası
        private string GetRequiredPermissionForParameter(string parameterKey)
        {
            return parameterKey switch
            {
                "MAX_TEMP_SAFETY_LIMIT" => ParameterPermissions.SafetyLimitsWrite,
                "GAS_MASS_FLOW_OFFSET" => ParameterPermissions.CalibrationWrite,
                "ZONE1_SETPOINT_RAMP" => ParameterPermissions.SetpointBasicWrite,
                _ => ParameterPermissions.SafetyLimitsWrite // Bilinmeyen parametreleri en yüksek güvenlik seviyesine çek
            };
        }

        private async Task LogUnauthorizedAttemptAsync(string parameterKey, string requiredPermission)
        {
            await _context.AuditLogs.AddAsync(new AuditLog
            {
                UserId = _userContext.CurrentUserId,
                Username = _userContext.CurrentUserEmail,
                UserIpAddress = _userContext.ClientIpAddress,
                ActionType = "UNAUTHORIZED_PARAMETER_ATTEMPT",
                ModuleName = "SystemSettingManager",
                Description = $"YETKİSİZ GİRİŞİM: Kullanıcı '{parameterKey}' parametresini değiştirmeye çalıştı. Gerekli Yetki: {requiredPermission}",
                IsSuccess = false,
                Timestamp = DateTime.Now
            });
            await _context.SaveChangesAsync();
        }
    }
}