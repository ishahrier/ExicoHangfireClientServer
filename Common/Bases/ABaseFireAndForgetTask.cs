using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.Bases
{
    
    public abstract class ABaseFireAndForgetTask : ABaseTask,IFireAndForgetTask
    {
        protected ABaseFireAndForgetTask(IFireAndForgetTaskOptions options):base(options)
        {
             
        }
    }
}