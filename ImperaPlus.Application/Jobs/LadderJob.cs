﻿using Autofac;
using Hangfire;
using Hangfire.Server;
using ImperaPlus.Domain.Repositories;
using ImperaPlus.Domain.Services;
using ImperaPlus.Utils;
using Microsoft.EntityFrameworkCore;

namespace ImperaPlus.Application.Jobs
{
    [Queue(JobQueues.Critical)]
    [DisableConcurrentExecution(60)]
    [AutomaticRetry(Attempts = 0)]
    public class LadderJob : Job
    {
        private IUnitOfWork unitOfWork;
        private ILadderService ladderService;
        private IRandomGenProvider randomGenProvider;

        public LadderJob(ILifetimeScope scope)
            : base(scope)
        {
            this.unitOfWork = this.LifetimeScope.Resolve<IUnitOfWork>();
            this.ladderService = this.LifetimeScope.Resolve<ILadderService>();
            this.randomGenProvider = this.LifetimeScope.Resolve<IRandomGenProvider>();
        }

        public override void Handle(PerformContext performContext)
        {
            base.Handle(performContext);

            TraceContext.Trace("Processing ladder", () =>
            {
                try
                {
                    this.ladderService.CheckAndCreateMatches(this.randomGenProvider.GetRandomGen());
                }
                catch (DbUpdateConcurrencyException)
                {
                    this.Log.Log(Domain.LogLevel.Error, "DbUpdateConcurrencyException while processing ladders");
                }
            });
        }
    }
}
