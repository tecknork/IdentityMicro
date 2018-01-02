using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityMicro.Entities.Interfaces
{
    public interface IGADUsers
    {
        string FirstName { get; set; }

        string LastName { get; set; }

       
    }
}
