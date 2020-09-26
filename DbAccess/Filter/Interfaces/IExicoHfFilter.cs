using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Filter.Interfaces
{
    public interface IExicoHfFilter :
       IClientFilter,
       IServerFilter,
       IElectStateFilter,
       IApplyStateFilter
    {
    
    }

}
