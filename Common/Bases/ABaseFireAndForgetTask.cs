using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.Bases
{
    
    public abstract class ABaseFireAndForgetTask : ABaseTask,IFireAndForgetTask
    {        
        public ABaseFireAndForgetTask(IFireAndForgetTaskOptions options):base(options)
        {
             
        }
    }
}