using DataExchange.SitecoreForms.Provider.PipelineBatches;
using Sitecore.Data;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Local.Runners;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Runners;
using Sitecore.Jobs;
using Sitecore.Security.Accounts;
using Sitecore.Services.Core.Extensions;

namespace DataExchange.SitecoreForms.Provider
{
    public class BatchRunner : IBatchRunner
    {
        private bool _isRunnerSet;
        private static IPipelineBatchRunner<Job> _runner;

        public void RunVirtualBatch(string formId, IPlugin[] plugins)
        {
            var virtualBatches = FormProcessingVirtualPipelineBatchBuilder.GetVirtualPipelineBatches(formId);
            if(virtualBatches == null)
                return;

            var pipelineBatchRunner = (InProcessPipelineBatchRunner)PipelineBatchRunner;

            foreach (var virtualBatch in virtualBatches)
            {
                if (pipelineBatchRunner != null && (!pipelineBatchRunner.IsRunningRemotely(virtualBatch) || !PipelineBatchRunner.IsRunning(virtualBatch.Identifier)))
                {
                    const string category = "Data Exchange";

                    var parameters = new object[]
                    {
                        virtualBatch,
                        GetRunAsUser(),
                        plugins
                    };

                    var options = new JobOptions(virtualBatch.Name, category, "Data Exchange Framework", this, "RunPipelineBatch", parameters);
                    PipelineBatchRunner.CurrentProcesses[virtualBatch.Identifier] = JobManager.Start(options);
                }
            
            }

        }

        public void Run(ID batchItemId, IPlugin[] plugins)
        {

            var batchItem = Sitecore.Configuration.Factory.GetDatabase("master")
                .GetItem(batchItemId);


            if (PipelineBatchRunner == null || batchItem == null || !Helper.IsPipelineBatchItem(batchItem))
                return;

            var pipelineBatch = Helper.GetPipelineBatch(batchItem);

            if (pipelineBatch == null)
                return;

            var pipelineBatchRunner = (InProcessPipelineBatchRunner)PipelineBatchRunner;

            if (pipelineBatchRunner != null && (!pipelineBatchRunner.IsRunningRemotely(pipelineBatch) || !PipelineBatchRunner.IsRunning(pipelineBatch.Identifier)))
            {
                const string category = "Data Exchange";

                var parameters = new object[]
                {
                        pipelineBatch,
                        GetRunAsUser(),
                        plugins
                };

                var options = new JobOptions(batchItem.Name, category, "Data Exchange Framework", this, "RunPipelineBatch", parameters);
                PipelineBatchRunner.CurrentProcesses[pipelineBatch.Identifier] = JobManager.Start(options);
            }

        }

        public void RunPipelineBatch(PipelineBatch pipelineBatch, User currentUser, IPlugin[] plugins)
        {
            if (PipelineBatchRunner == null)
                return;
            if (currentUser == null)
                currentUser = Sitecore.Context.User;
            using (new UserSwitcher(currentUser))
            {
                var pipelineBatchContext = GetPipelineBatchContext(pipelineBatch);
                plugins.ForEach(q => pipelineBatchContext.AddPlugin(q));
                PipelineBatchRunner.Run(pipelineBatch, pipelineBatchContext);
            }
        }
        protected IPipelineBatchRunner<Job> PipelineBatchRunner
        {
            get
            {
                if (!_isRunnerSet && _runner == null)
                {
                    var pipelineBatchRunner = new InProcessPipelineBatchRunner();
                    var logger = Sitecore.DataExchange.Context.Logger;
                    pipelineBatchRunner.Logger = logger;
                    _runner = pipelineBatchRunner;
                    ((InProcessPipelineBatchRunner)_runner).SubscribeRemoteEvents();
                    _runner.Started += PipelineBatchRunnerOnStarted;
                    _runner.Finished += PipelineBatchRunnerOnFinished;
                }
                return _runner;
            }
            set
            {
                _isRunnerSet = true;
                _runner = value;
            }
        }
        protected virtual PipelineBatchContext GetPipelineBatchContext(PipelineBatch pipelineBatch)
        {
            var pipelineBatchContext = new PipelineBatchContext();
            var newPlugin = new PipelineBatchRuntimeSettings
            {
                ShouldPersistSummary = true,
                PipelineBatchMode = string.Empty
            };
            pipelineBatchContext.AddPlugin(newPlugin);
            return pipelineBatchContext;
        }

        protected virtual User GetRunAsUser()
        {
            return Sitecore.Context.User;
        }

        protected virtual void PipelineBatchRunnerOnFinished(object sender, PipelineBatchRunnerEventArgs pipelineBatchRunnerEventArgs)
        {
        }

        protected virtual void PipelineBatchRunnerOnStarted(object sender, PipelineBatchRunnerEventArgs pipelineBatchRunnerEventArgs)
        {
        }
    }
}