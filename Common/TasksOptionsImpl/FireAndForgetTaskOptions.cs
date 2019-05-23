using Exico.HF.Common.Bases;
using Exico.HF.Common.Interfaces;

namespace Exico.HF.Common.TasksOptionsImpl {
    public class FireAndForgetTaskOptions : ABaseTaskOptions, IFireAndForgetTaskOptions {

        public FireAndForgetTaskOptions() : base() => SetJobType(JobType.FireAndForget);

        //todo do actuall validation
        public override bool Validate()
        {
            return true;
        }
    }
}