using IdentityMicro.Entities.IdentityModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicro.Entities.Interfaces
{
    public interface IUserRepository
    {

        Task<User> GetBySubjectId(Guid subjectId);

        Task<User> GetByUserName(string userName);

        Task<User> GetByExternalProviderInfo(string providerName, string providerSubjectId);

        Task CreateUser(User user, string providerName, string providerSubjectId);
    }
}
