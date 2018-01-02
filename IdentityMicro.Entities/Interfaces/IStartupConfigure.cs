using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace IdentityMicro.Entities.Interfaces
{
    public interface IStartupConfigure
    {

        void ConfigureIOC(IServiceCollection serviceCollection);


    }
}
