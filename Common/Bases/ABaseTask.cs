using System.Collections.Generic;
using System.Threading.Tasks;
using Exico.HF.Common.Interfaces;
using Hangfire;
using Newtonsoft.Json;

namespace Exico.HF.Common.Bases
{
    public abstract class ABaseTask : IBaseTask
    {
        public ABaseTask(IBaseTaskOptions options)
        {
            this._Options = options;
        }
        protected IBaseTaskOptions _Options { get; set; }
        public virtual async Task Run(string jsonOptions, IJobCancellationToken cancellationToken)
        {
            await Task.Run(() => InitiaLizeOption(jsonOptions));
        }

        public abstract void UpdateTaskStatus();

        protected void InitiaLizeOption(string jsonOptoins)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            _Options.InitializeFromDictionary(JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonOptoins, setting));
        }
       
    }
}