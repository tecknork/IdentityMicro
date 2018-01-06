using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdentityMicro.Entities.IdentityModels
{
    public class User :IdentityUser
    {

        public Guid SubjectId { get; set; }
       
        public string Password { get; set; }
       
        public string Name { get; set; }
       
        public string Website { get; set; }
        public bool IsActive { get; set; }
        public List<UserExternalProvider> UserExternalProviders { get; set; }
    }
}
