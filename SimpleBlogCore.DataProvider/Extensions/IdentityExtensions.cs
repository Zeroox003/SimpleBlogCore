using System;
using System.Security.Claims;
using System.Security.Principal;

namespace SimpleBlogCore.DataProvider.Extensions
{
    public static class IdentityExtensions
    {
        public static Guid GetUserId(this IIdentity identity)
        {
            var userId = (identity as ClaimsIdentity).FindFirst("Id");
            return (userId != null) ? Guid.Parse(userId.Value) : Guid.Empty;
        }
    }
}
