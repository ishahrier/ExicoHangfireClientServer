using System;
using System.Collections.Generic;

namespace Exico.HF.Common.Interfaces
{
    public struct JobType
    {
        public const string FireAndForget = "FireAndForget";
        public const string Scheduled = "Scheduled";
        public const string Recurring = "Recurring";
    }
    public struct RunType
    {
        public const string Sync = "Sync";
        public const string ASync = "ASync";
    }
    public interface IBaseTaskOptions
    {
        string GetHfJobId();
        string GetUserId();
        string GetTimeZoneId();
        long GetUserTaskId();
        string GetJobType();
        string GetRunType();

        void SetHfJobId(string id);
        void SetUserId(string id);
        void SetTimeZoneId(string id);
        void SetUserTaskId(long id);
        void SetJobType(string type);
        void SetRunType(string type);


        T GetOption<T>(string key);
        void SetOption(string key, object value);
        bool HasOption(string key);
        Dictionary<string, object> BuildObjectDictionary();
        void InitializeFromDictionary(Dictionary<string, object> options);
        string ToJson();
        bool Validate();
    }
}