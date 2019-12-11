using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public interface IExicoHFDbService: IDisposable
    {
        Task<HfUserJob> Get(long userJobId);
        Task<HfUserJob> Update(long userJobId, string hfJobId, IBaseTaskOptions options);
        Task<HfUserJob> Create( string name, string note, IBaseTaskOptions options);
        Task<HfUserJob> UpdateStatus(HfUserJob data, string status);

        Task<bool> Delete(HfUserJob job);
    }
}
