using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public static class ClaimsPrincipalExtension
    {
        public static List<string> Claims(this ClaimsPrincipal claimsPrincipal, string claimType)
        {
            var result = claimsPrincipal?.FindAll(claimType)?.Select(x => x.Value).ToList(); //Claim typelları listeledik  ?: varmı demek  
            return result;
        }
        public static List<string> ClaimRoles(this ClaimsPrincipal claimsPrincipal)
        {
            return claimsPrincipal?.Claims(ClaimTypes.Role);
        }
        public static string GetUserId(this ClaimsPrincipal claimsPrincipal)

        {

            return claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier).Value;

        }

        public static string GetUserEmail(this ClaimsPrincipal claimsPrincipal)

        {

            return claimsPrincipal.FindFirst(ClaimTypes.Email).Value;

        }

        public static string GetUserName(this ClaimsPrincipal claimsPrincipal)

        {

            return claimsPrincipal.FindFirst(ClaimTypes.Name).Value;

        }

        public static bool IsCurrentUser(this ClaimsPrincipal claimsPrincipal, string id)

        {

            var currentUserId = GetUserId(claimsPrincipal);



            return string.Equals(currentUserId, id, StringComparison.OrdinalIgnoreCase);

        }
    }
}
