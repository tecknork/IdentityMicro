using IdentityMicro.Entities.ViewModels.Consent;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IdentityMicro.Entities.Interfaces
{
    public interface IConsentService
    {
         Task<ProcessConsentResult> ProcessConsent(ConsentInputModel model);
        Task<ConsentViewModel> BuildViewModelAsync(string returnUrl, ConsentInputModel model = null);

      

        ScopeViewModel CreateScopeViewModel(IdentityResource identity, bool check);

        ScopeViewModel CreateScopeViewModel(Scope scope, bool check);


    }
}
