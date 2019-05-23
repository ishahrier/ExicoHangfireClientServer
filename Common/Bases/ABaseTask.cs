using System.Collections.Generic;
using System.Threading.Tasks;
using Exico.HF.Common.Interfaces;
using Hangfire;
using Newtonsoft.Json;

namespace Exico.HF.Common.Bases
{
    public abstract class ABaseTask : IBaseTask
    {
        protected ABaseTask(IBaseTaskOptions options) => this._Options = options;
        private IBaseTaskOptions _Options { get; set; }
        private void InitiaLizeOption(string jsonOptoins)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            _Options.InitializeFromDictionary(JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonOptoins, setting));
        }

        protected abstract Task Run(IJobCancellationToken cancellationToken, IBaseTaskOptions options);        
        public abstract void UpdateTaskStatus();

        public virtual async Task Run(string jsonOptions, IJobCancellationToken cancellationToken)
        {
            InitiaLizeOption(jsonOptions);
            await Run(cancellationToken, _Options);
        }


    }
}