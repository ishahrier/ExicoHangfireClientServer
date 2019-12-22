using Hangfire.Client;
using Hangfire.Server;
using Hangfire.States;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Filter
{
    public interface IExicoHfFilter :
       IClientFilter,
       IServerFilter,
       IElectStateFilter,
       IApplyStateFilter
    {
    
    }

}
