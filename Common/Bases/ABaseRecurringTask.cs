using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.Bases
{
    public abstract class ABaseRecurringTask : ABaseTask,IRecurringTask
    {
        protected ABaseRecurringTask(IRecurringTaskOptions options):base(options)
        {
             
        }
    }
}