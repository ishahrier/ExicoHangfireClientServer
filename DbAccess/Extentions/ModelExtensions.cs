using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Exico.HF.DbAccess.Extentions
{
    public static class ModelExtensions
    {
        public static HfUserJobModel ToDomainModel(this HfUserJob data)
        {
            return new HfUserJobModel()
            {
                JobType = data.JobType,
                CreatedOn = data.CreatedOn,
                Name = data.Name,
                Note = data.Note,
                Status = data.Status,
                TimeZone = data.TimeZoneId,
                UpdatedOn = data.UpdatedOn,
                UserId = data.UserId,
                WorkDataId = data.WorkDataId,
                WorkerClass = data.WorkerClass,
                Id = data.Id
            };
        }

        public static HfUserJob ToDbModel(this HfUserJobModel data)
        {
            return new HfUserJob()
            {
                JobType = data.JobType,
                CreatedOn = data.CreatedOn,
                Name = data.Name,
                Note = data.Note,
                Status = data.Status,
                TimeZoneId = data.TimeZone,
                UpdatedOn = data.UpdatedOn,
                UserId = data.UserId,
                WorkDataId = data.WorkDataId,
                WorkerClass = data.WorkerClass,
                Id = data.Id
            };
        }


        public static bool IsRecurringJob(this HfUserJob record) => record.JobType == JobType.Recurring;
        public static bool IsScheduledJob(this HfUserJob record) => record.JobType == JobType.Scheduled;
        public static bool IsFireAndForgetJob(this HfUserJob record) => record.JobType == JobType.FireAndForget;
        public static bool IsFireAndForgetOrScheduled(this HfUserJob record) => IsScheduledJob(record) || IsFireAndForgetJob(record);
        public static bool IsRecurringOrScheduled(this HfUserJob record) => IsScheduledJob(record) || IsFireAndForgetJob(record);


        public static bool IsRecurringJob(this HfUserJobModel record) => record.JobType == JobType.Recurring;
        public static bool IsScheduledJob(this HfUserJobModel record) => record.JobType == JobType.Scheduled;
        public static bool IsFireAndForgetJob(this HfUserJobModel record) => record.JobType == JobType.FireAndForget;
        public static bool IsFireAndForgetOrScheduled(this HfUserJobModel record) => IsScheduledJob(record) || IsFireAndForgetJob(record);
        public static bool IsRecurringOrScheduled(this HfUserJobModel record) => IsScheduledJob(record) || IsFireAndForgetJob(record);


    }
}
