using Exico.HF.Common.DomainModels;
using Exico.HF.Common.Enums;
using Exico.HF.DbAccess.Db.Models;

namespace Exico.HF.DbAccess.Extentions
{
    public static class ModelExtensions
    {
        #region model to db
        public static HfUserJob ToDbModel(this HfUserFireAndForgetJobModel model)
        {
            return (model as HfUserJobModel).ToDbModel();
        }

        public static HfUserJob ToDbModel(this HfUserJobModel model)
        {
            return new HfUserJob()
            {
                Id = model.Id,
                JobType = model.JobType,
                CreatedOn = model.CreatedOn,
                Name = model.Name,
                Note = model.Note,
                Status = model.Status,
                TimeZoneId = model.TimeZoneId,
                UpdatedOn = model.UpdatedOn,
                UserId = model.UserId,
                WorkDataId = model.WorkDataId,
                WorkerClassName = model.WorkerClassName,
                WorkerAssemblyName = model.WorkerAssemblyName,
                HfJobId = model.HfJobId,
                
            };
        }

        public static HfUserRecurringJob ToDbModel(this HfUserRecurringJobModel model)
        {
            return new HfUserRecurringJob()
            {
                Id = model.HfUserRecurringJobModelId,
                HfUserJob = (model as HfUserJobModel).ToDbModel(),
                CronExpression = model.CronExpression,
                LastHfJobId = model.LastHfJobId,
                LastRun = model.LastRun,
                NextRun = model.NextRun
            };
        }

        public static HfUserScheduledJob ToDbModel(this HfUserScheduledJobModel model)
        {
            return new HfUserScheduledJob()
            {
                Id = model.HfUserScheduledJobModelId,
                ScheduledAt = model.ScheduledAt,
                HfUserJob = (model as HfUserJobModel).ToDbModel()
            };
        }
        #endregion

        #region db to model
        public static HfUserFireAndForgetJobModel ToDomainModel(this HfUserJob data)
        {
            return new HfUserFireAndForgetJobModel()
            {
                //JobType = data.JobType,
                CreatedOn = data.CreatedOn,
                Name = data.Name,
                Note = data.Note,
                Status = data.Status,
                TimeZoneId = data.TimeZoneId,
                UpdatedOn = data.UpdatedOn,
                UserId = data.UserId,
                WorkDataId = data.WorkDataId,
                WorkerClassName = data.WorkerClassName,
                WorkerAssemblyName = data.WorkerAssemblyName,
                Id = data.Id,
                HfJobId = data.HfJobId
            };
        }

        public static HfUserRecurringJobModel ToDomainModel(this HfUserRecurringJob data)
        {
            var model = new HfUserRecurringJobModel()
            {
                //JobType = data.HfUserJob.JobType,
                CreatedOn = data.HfUserJob.CreatedOn,
                Name = data.HfUserJob.Name,
                Note = data.HfUserJob.Note,
                Status = data.HfUserJob.Status,
                TimeZoneId = data.HfUserJob.TimeZoneId,
                UpdatedOn = data.HfUserJob.UpdatedOn,
                UserId = data.HfUserJob.UserId,
                WorkDataId = data.HfUserJob.WorkDataId,
                WorkerClassName = data.HfUserJob.WorkerClassName,
                WorkerAssemblyName = data.HfUserJob.WorkerAssemblyName,
                Id = data.HfUserJob.Id,
                HfJobId = data.HfUserJob.HfJobId
            };
            model.HfUserRecurringJobModelId = data.Id;
            model.CronExpression = data.CronExpression;
            model.NextRun = data.NextRun;
            model.LastRun = data.LastRun;
            model.LastHfJobId = data.LastHfJobId;
            return model;
        }

        public static HfUserScheduledJobModel ToDomainModel(this HfUserScheduledJob data)
        {
            var model = new HfUserScheduledJobModel()
            {
                //JobType = data.HfUserJob.JobType,
                CreatedOn = data.HfUserJob.CreatedOn,
                Name = data.HfUserJob.Name,
                Note = data.HfUserJob.Note,
                Status = data.HfUserJob.Status,
                TimeZoneId = data.HfUserJob.TimeZoneId,
                UpdatedOn = data.HfUserJob.UpdatedOn,
                UserId = data.HfUserJob.UserId,
                WorkDataId = data.HfUserJob.WorkDataId,
                WorkerClassName = data.HfUserJob.WorkerClassName,
                WorkerAssemblyName = data.HfUserJob.WorkerAssemblyName,
                Id = data.HfUserJob.Id,
                HfJobId = data.HfUserJob.HfJobId,
            };
            model.HfUserScheduledJobModelId = data.Id;
            model.ScheduledAt = data.ScheduledAt;
            return model;
        }
        #endregion

        #region base to specific model casting
        public static T CastToJobModel<T>(this HfUserJobModel data) where T : HfUserJobModel
        {
            return data as T;
        }
        public static HfUserFireAndForgetJobModel CastToFireAndForgetJobModel(this HfUserJobModel data) => data as HfUserFireAndForgetJobModel;
        public static HfUserScheduledJobModel CastToScheduledJobModel(this HfUserJobModel data) => data as HfUserScheduledJobModel;
        public static HfUserRecurringJobModel CastToRecurringJobModel(this HfUserJobModel data) => data as HfUserRecurringJobModel;
        #endregion

        #region model to work arg model
        public static WorkArguments ToWorkArguments(this HfUserJobModel data)
        {
            return new WorkArguments()
            {
                JobType = data.JobType,
                WorkDataId = data.WorkDataId,
                WorkerClassName = data.WorkerClassName,
                WorkerAssemlyName = data.WorkerAssemblyName,
                Name = data.Name,
                UserJobId = data.Id
            };
        }
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
