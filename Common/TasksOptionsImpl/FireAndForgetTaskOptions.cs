using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.TasksOptionsImpl {
    public class FireAndForgetTaskOptions : ABaseOptions, IFireAndForgetTaskOptions {

        public FireAndForgetTaskOptions():base()
        {
            JobType = Interfaces.JobType.FireAndForget;
        }
    }
}