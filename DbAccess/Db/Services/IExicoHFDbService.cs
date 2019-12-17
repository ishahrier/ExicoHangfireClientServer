using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using System;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Db.Services
{
    public interface IExicoHfDbService : IDisposable
    {
        Task<bool> SetHfJobId(int userJobId, string hfJobId);
        //Task<HfUserJob> Get(int userJobId);
        Task<T> Get<T>(int userJobId) where T : HfUserJobModel;
        Task<HfUserJobModel> GetBase(int userJobId);
        Task<T> Create<T>(T data) where T : HfUserJobModel;
        Task<T> Update<T>(T t) where T : HfUserJobModel;
        Task<bool> UpdateStatus(int userJobId, JobStatus status);
        Task<bool> Delete(int userJobId);
        Task<string> GetHfJobId(int userJobId);



    }
}