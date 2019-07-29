using System;
using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.Services.Core.Diagnostics;

namespace DataExchange.SitecoreForms.Provider.ReadData
{
    public class ReadEntityFromContextStepProcessor : BasePipelineStepProcessor
    {
        protected override void ProcessPipelineStep(PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        {
            var formSyncDataPlugin = pipelineContext.PipelineBatchContext.GetPlugin<FormSyncData>();
            if (formSyncDataPlugin == null)
            {
                pipelineContext.CriticalError = true;
                Log(logger.Error, pipelineContext, "FormSyncData Plugin is null.", Array.Empty<string>());
                return;
            }
            SetObjectOnPipelineContext(formSyncDataPlugin.FormSubmissionEntry, ItemIDs.PipelineContextStorageLocationSource, pipelineContext, logger);
        }
    }
}