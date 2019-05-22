using System;
using System.Collections.Generic;

namespace Exico.HF.Common.Interfaces {
    public struct JobType {
        public const string FireAndForget = "FireAndForget";
        public const string Scheduled = "Scheduled";
        public const string Recurring = "Recurring";
    }
    public struct RunType {
        public const string Sync = "Sync";
        public const string ASync = "ASync";
    }
    public interface IBaseTaskOptions {
        string HfJobId { get; set; }
        string UserId { get; set; }
        TimeZoneInfo TimeZone { get; set; }
        long UserTaskId { get; set; }
        string JobType { get;  }
        string RunType { get;  }
        T GetOption<T> (string key);
        void SetOption (string key, object value);
        bool HasOption (string key);
        Dictionary<string, object> BuildObjectDictionary ();
        void InitializeFromDictionary (Dictionary<string, object> options);
        string ToJson ();
        bool Validate();
    }
}