using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.Bases
{
    public abstract class ABaseScheduledtTask : ABaseTask
    {
        public ABaseScheduledtTask(IScheduledTaskOptions options):base(options)
        {
             
        }
    }
}