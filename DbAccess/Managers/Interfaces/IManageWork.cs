using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Hangfire;
using System.Threading.Tasks;

namespace Exico.HF.DbAccess.Managers.Interfaces
{
    /// <summary>
    /// A 'work' is a defined steps that is needed to be executed.
    /// A work has an associated table, that contains work parameters.
    /// 
    /// A work can be, i.e. 'delete all products that has zero unit on hand' or 'Email a report of revenue'
    /// 
    /// A 'work' is not a UserJob or HangFire job. A UserJob is created to 
    /// schedule a 'Work'
    /// </summary>
    public interface IManageWork
    {        
        public Task<bool> ExecuteWorkerAsync(WorkArguments args, IJobCancellationToken cancellationToken);
    }
}
