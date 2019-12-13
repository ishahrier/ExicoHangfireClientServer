using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.Common.Interfaces;
using Exico.HF.DbAccess.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public interface IExicoHFDbService : IDisposable
    {
        Task<HfUserJob> Get(int userJobId);
        Task<HfUserJob> SetHfJobId(int userJobId, string hfJobId);
        Task<HfUserJob> Create(HfUserJob data);
        Task<HfUserJob> UpdateStatus(int userJobId, JobStatus status);
        Task<HfUserJob> Update(HfUserJob data);
        Task<bool> Delete(int userJobId);
    }
}
