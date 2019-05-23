using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.Bases
{
    public abstract class ABaseScheduledtTask : ABaseTask,IScheduledTask
    {
        protected ABaseScheduledtTask(IScheduledTaskOptions options):base(options)
        {
             
        }
    }
}