using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicro.Entities.Interfaces
{
    public interface IUserManager<TUser>
    {
        Task<IdentityResult> CreateAsync(TUser user, string password);

        Task<IdentityResult> AddToRoleAsync(TUser user, string role);

        Task<IdentityResult> AddClaimAsync(TUser user, Claim claim);
    }
}
