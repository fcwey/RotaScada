using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        // Belirtilen ClaimType'a ait tüm değerleri güvenli şekilde liste halinde döner
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList();
            return result ?? new List<string>();
        }

        // Rolleri liste olarak döner
        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role) ?? new List<string>();
        }

        // UserContextHelper ve yetkilendirme altyapısı için eklenen takma metot (Alias)
        public static List<string> GetClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.ClaimRoles() ?? new List<string>();
        }

        // UserId'yi int olarak güvenli şekilde döner (UserContextHelper ile tam uyumludur)
        public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
        {
            var userIdClaim = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (int.TryParse(userIdClaim, out int userId))
            {
                return userId;
            }
            return 0; // Token yoksa veya parse edilemezse 0 döner
        }

        // UserId'yi string olarak kullanmak istediğiniz durumlar için alternatif metot
        public static string GetUserStringId(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
        }

        // E-posta bilgisini güvenli şekilde çeker (Null-safe)
        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirst(ClaimTypes.Email)?.Value
                ?? claimsPrincipal?.FindFirst(ClaimTypes.Name)?.Value
                ?? "Anonymous";
        }

        // Kullanıcı Adını güvenli şekilde çeker (Null-safe)
        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }

        // Aktif kullanıcının verilen ID ile eşleşip eşleşmediğini kontrol eder
        public static bool IsCurrentUser(this ClaimsPrincipal claimsPrincipal, string id)
        {
            var currentUserId = claimsPrincipal?.GetUserStringId();
            return string.Equals(currentUserId, id, StringComparison.OrdinalIgnoreCase);
        }
    }
}