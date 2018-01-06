﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityMicro.Entities.IdentityModels
{
    public class UserExternalProvider
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public User User { get; set; }
        public string ProviderName { get; set; }
        [StringLength(250)]
        public string ProviderSubjectId { get; set; }
    }
}