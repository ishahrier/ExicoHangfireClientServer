using System;
using System.Collections.Generic;
using Exico.HF.Common.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Exico.HF.Common.Bases
{
    public abstract class ABaseTaskOptions : IBaseTaskOptions
    {
        protected ABaseTaskOptions()
        {
            _Options = new Dictionary<string, object>();
        }

        public Dictionary<string, object> _Options;

        public string GetUserId() => GetOption<string>("UserId");
        public string GetTimeZoneId() => GetOption<string>("TimeZoneId");
        public long GetUserTaskId() => GetOption<long>("UserTaskId");
        public string GetJobType() => GetOption<string>("JobType");
        public string GetHfJobId() => GetOption<string>("HfJobId");

        public void SetHfJobId(string id) => SetOption("HfJobId", id);
        public void SetUserId(string id) => SetOption("UserId", id);
        public void SetTimeZoneId(string id) => SetOption("TimeZoneId", id);
        public void SetUserTaskId(long id) => SetOption("UserTaskId", id);
        public void SetJobType(string type) => SetOption("JobType", type);

        public Dictionary<string, object> BuildObjectDictionary() => _Options;

        public T GetOption<T>(string key) => _Options.ContainsKey(key) ? (T)_Options[key] : default(T);

        public bool HasOption(string key) => _Options.ContainsKey(key);

        public void InitializeFromDictionary(Dictionary<string, object> options) => this._Options = options;

        public void SetOption(string key, object value) => _Options[key] = value;

        public string ToJson()
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All, Formatting = Formatting.Indented };
            return JsonConvert.SerializeObject(this._Options, setting);
        }

        public abstract bool Validate();

    }
}