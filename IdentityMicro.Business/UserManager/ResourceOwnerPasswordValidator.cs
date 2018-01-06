using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityMicro.Entities.Interfaces;
using IdentityServer4.Models;
using IdentityServer4.Validation;


namespace IdentityMicro.Business.UserManager
{
    public class ResourceOwnerPasswordValidator: IResourceOwnerPasswordValidator
    {
        private readonly IUserRepository userRepository;

        public ResourceOwnerPasswordValidator(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }
        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
        {
            var user = await userRepository.GetByUserName(context.UserName);
            if (user == null)
                return;

            if (user.Password == context.Password.Sha256())
                context.Result = new GrantValidationResult(user.SubjectId.ToString(), "password");
        }
    }
}
