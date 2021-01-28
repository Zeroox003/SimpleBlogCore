using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SimpleBlogCore.Domain.Entities;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SimpleBlogCore.DataProvider.Identity
{
    public class AppClaimsPrincipalFactory : UserClaimsPrincipalFactory<User, IdentityRole<Guid>>
    {
        public AppClaimsPrincipalFactory(
            UserManager<User> userManager,
            RoleManager<IdentityRole<Guid>> roleManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, roleManager, optionsAccessor) 
        {
            
        }

        public async override Task<ClaimsPrincipal> CreateAsync(User user)
        {
            var principal = await base.CreateAsync(user);

            if (user.Id != null)
            {
                (principal.Identity as ClaimsIdentity).AddClaims(new[] {
                    new Claim("Id", user.Id.ToString()),
                });
            }

            return principal;
        }
    }
}
