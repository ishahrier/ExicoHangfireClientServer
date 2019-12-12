using System;
using System.Collections.Generic;

namespace Exico.HF.Common.Interfaces
{

    public interface IBaseTaskOptions
    {
        string GetHfJobId();
        string GetUserId();
        string GetTimeZoneId();
        long GetUserTaskId();
        string GetJobType();

        void SetHfJobId(string id);
        void SetUserId(string id);
        void SetTimeZoneId(string id);
        void SetUserTaskId(long id);
        void SetJobType(string type);


        T GetOption<T>(string key);
        void SetOption(string key, object value);
        bool HasOption(string key);
        Dictionary<string, object> BuildObjectDictionary();
        void InitializeFromDictionary(Dictionary<string, object> options);
        string ToJson();
        bool Validate();
    }
}