using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityMicro.Entities.IdentityModels;
using IdentityMicro.Entities.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IdentityMicro.Data.DBContext
{
    public class UserRepository : IUserRepository
    {
        private readonly GADDBContext dbContext;

        public UserRepository(GADDBContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<User> GetBySubjectId(Guid subjectId)
        {
            
            return
                await dbContext.Users.SingleAsync(u => u.SubjectId == subjectId);
        }

        public async Task<User> GetByUserName(string userName)
        {

            return
                await dbContext.Users.SingleOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task<User> GetByExternalProviderInfo(string providerName, string providerSubjectId)
        {

            return
                await
                    dbContext.UserExternalProviders.Where(
                            e => e.ProviderName == providerName && e.ProviderSubjectId == providerSubjectId)
                        .Select(e => e.User).SingleOrDefaultAsync();
        }

        public async Task CreateUser(User user, string providerName, string providerSubjectId)
        {


            await dbContext.Users.AddAsync(user);
            var extProv = new UserExternalProvider
            {
                UserId = user.Id,
                ProviderName = providerName,
                ProviderSubjectId = providerSubjectId
            };
            await dbContext.UserExternalProviders.AddAsync(extProv);
            await dbContext.SaveChangesAsync();
        }
    }
}
