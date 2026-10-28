using Core.Extensions;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;

namespace Core.Utilities.IoC
{
    public interface IUserContextHelper
    {
        int CurrentUserId { get; }
        string CurrentUserEmail { get; }
        List<string> CurrentUserRoles { get; }
        string ClientIpAddress { get; }
    }

    public class UserContextHelper : IUserContextHelper
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserContextHelper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        private HttpContext? CurrentContext => _httpContextAccessor.HttpContext;

        public int CurrentUserId => CurrentContext?.User?.GetUserId() ?? 0;

        public string CurrentUserEmail => CurrentContext?.User?.GetUserEmail() ?? "System";

        public List<string> CurrentUserRoles => CurrentContext?.User?.GetClaimRoles() ?? new List<string>();

        public string ClientIpAddress
        {
            get
            {
                if (CurrentContext == null) return "127.0.0.1";

                string? ip = CurrentContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

                if (string.IsNullOrEmpty(ip))
                {
                    ip = CurrentContext.Connection.RemoteIpAddress?.ToString();
                }

                return ip ?? "127.0.0.1";
            }
        }
    }
}