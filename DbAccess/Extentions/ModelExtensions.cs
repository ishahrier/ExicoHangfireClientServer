using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Models;

namespace Exico.HF.DbAccess.Extentions
{
    public static class ModelExtensions
    {
        #region model to db
        public static HfUserJob ToDbModel(this HfUserFireAndForgetJobModel data)
        {
            return (data as HfUserJobModel).ToDbModel();
        }

        public static HfUserJob ToDbModel(this HfUserJobModel data)
        {
            return new HfUserJob()
            {
                Id = data.Id,                
                JobType = data.JobType,
                CreatedOn = data.CreatedOn,
                Name = data.Name,
                Note = data.Note,
                Status = data.Status,
                TimeZoneId = data.TimeZoneId,
                UpdatedOn = data.UpdatedOn,
                UserId = data.UserId,
                WorkDataId = data.WorkDataId,
                WorkerClass = data.WorkerClass
            };
        }

        public static HfUserRecurringJob ToDbModel(this HfUserRecurringJobModel data)
        {
            return new HfUserRecurringJob()
            {
                Id = data.HfUserRecurringJobModelId,
                HfUserJob =   (data as HfUserJobModel).ToDbModel(),
                CronExpression = data.CronExpression,
                LastHfJobId = data.LastHfJobId,
                LastRun = data.LastRun,
                NextRun = data.NextRun
            };
        }

        public static HfUserScheduledJob ToDbModel(this HfUserScheduledJobModel data)
        {
            return new HfUserScheduledJob()
            {
                Id = data.HfUserScheduledJobModelId,
                ScheduledAt = data.ScheduledAt,
                HfUserJob = (data as HfUserJobModel).ToDbModel()                
            };
        }
        #endregion

        #region db to model
        public static HfUserJobModel ToDomainModel(this HfUserJob data)
        {
            return new HfUserFireAndForgetJobModel()
            {
                JobType = data.JobType,
                CreatedOn = data.CreatedOn,
                Name = data.Name,
                Note = data.Note,
                Status = data.Status,
                TimeZoneId = data.TimeZoneId,
                UpdatedOn = data.UpdatedOn,
                UserId = data.UserId,
                WorkDataId = data.WorkDataId,
                WorkerClass = data.WorkerClass,
                Id = data.Id,
                HfJobId = data.HfJobId
            };
        }

        public static HfUserRecurringJobModel ToDomainModel(this HfUserRecurringJob data)
        {
            var model = (HfUserRecurringJobModel) data.HfUserJob.ToDomainModel();
            model.HfUserRecurringJobModelId = data.Id;
            model.CronExpression = data.CronExpression;            
            model.NextRun = data.NextRun;
            model.LastRun = data.LastRun;
            model.LastHfJobId = data.LastHfJobId;
            return model;
        }

        public static HfUserScheduledJobModel ToDomainModel(this HfUserScheduledJob data)
        {
            var model = (HfUserScheduledJobModel)data.HfUserJob.ToDomainModel();
            model.Id = data.HfUserJobId;
            model.ScheduledAt = data.ScheduledAt;
            return model;
        }
        #endregion

        #region base to specific model casting
        public static T CastToJobModel <T>(this HfUserJobModel data) where T : HfUserJobModel
        {
            return data as T;
        }
        public static HfUserFireAndForgetJobModel CastToFireAndForgetJobModel(this HfUserJobModel data) => data as HfUserFireAndForgetJobModel;
        public static HfUserScheduledJobModel CastToScheduledJobModel(this HfUserJobModel data) => data as HfUserScheduledJobModel;
        public static HfUserRecurringJobModel CastToRecurringJobModel(this HfUserJobModel data) => data as HfUserRecurringJobModel;
        #endregion

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
