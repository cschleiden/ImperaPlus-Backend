﻿using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Normal)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class GameCleanupJob : Job
    {
        public const string JobId = "GameCleanup";

        private IUnitOfWork unitOfWork;

        public GameCleanupJob(ILifetimeScope scope)
            : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            this.Log.Log(Domain.LogLevel.Info, "Cleaning up fun games...");

            int deletedCount = this.unitOfWork.Games.DeleteOpenPasswordFunGames();
            this.Log.Log(Domain.LogLevel.Info, "Removed {0} fun games", deletedCount);
            this.unitOfWork.Commit();

            int deletedEndedCount = this.unitOfWork.Games.DeleteEndedGames();
            this.Log.Log(Domain.LogLevel.Info, "Removed {0} ended games", deletedEndedCount);
            this.unitOfWork.Commit();

            this.Log.Log(Domain.LogLevel.Info, "Done");
        }
    }
}
