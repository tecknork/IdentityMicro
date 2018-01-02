using IdentityMicro.Entities.Interfaces;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityMicro.Entities.IdentityModels
{
    public class GADUser : IdentityUser, IGADUsers
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
