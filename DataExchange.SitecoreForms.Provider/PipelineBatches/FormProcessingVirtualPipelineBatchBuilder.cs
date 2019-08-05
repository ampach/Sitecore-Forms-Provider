using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Common;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.DataExchange.Extensions;
using Sitecore.DataExchange.Local.Extensions;
using Sitecore.DataExchange.Loggers;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.DataExchange.VerificationLog;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.PipelineBatches
{
    public class FormProcessingVirtualPipelineBatchBuilder
    {
        public const string TenantTemplateId = "{327A381B-59F8-4E88-B331-BEBC7BD87E4E}";
        public const string SitecoreFormsPipelineTemplateId = "{56BAECBA-F431-4442-9C50-BF59610F4718}";

        public static List<PipelineBatch> GetVirtualPipelineBatches(string formId)
        {
            var result = new List<PipelineBatch>();
            var pipelines = FindAccotiatedPipelines(formId);
            if (pipelines == null)
                return null;

            var db = Sitecore.Configuration.Factory.GetDatabase("master");

            foreach (var pipeline in pipelines)
            {
                var pipelineModel = GetPipeline(pipeline);

                var virtualBatch = new PipelineBatch();
                virtualBatch.Enabled = true;
                virtualBatch.Name = "VirtualBatch." + pipelineModel.Name.Replace(" ", ".");
                virtualBatch.Identifier = pipelineModel.Identifier;
                virtualBatch.PipelineBatchProcessor = new VirtualPipelineBatchProcessor();
                virtualBatch.Tenant = GetTenant(db.GetItem(pipeline.GetItemId().ToID()));

                if (pipeline != null)
                {
                    virtualBatch.Pipelines.Add(pipelineModel);
                }
                AddRequiredPlugins(virtualBatch);
                AddPlugins(virtualBatch);
                AddVerificationLogPlugin(virtualBatch);

                result.Add(virtualBatch);
            }

            return result;
        }


        public static IEnumerable<ItemModel> FindAccotiatedPipelines(string formId)
        {
            //associated_forms
            ItemSearchSettings settings = new ItemSearchSettings();
            settings.SearchFilters.Add(new SearchFilter
            {
                FieldName = "Associated Forms",
                Value = formId
            });
            settings.SearchFilters.Add(new SearchFilter
            {
                FieldName = "TemplateID",
                Value = SitecoreFormsPipelineTemplateId
            });

            var source = Sitecore.DataExchange.Context.ItemModelRepository.Search(settings).Where(q => q.GetFieldValueAsString(ItemModel.ItemPath).Contains("/sitecore/system/Data Exchange"));
            return source;
        }

        public static Tenant GetTenant(Item item)
        {

            if (item != null)
            {
                var tenantTemplateId = new ID(TenantTemplateId);
                var tenantItem = item.Axes.GetAncestors().Reverse().FirstOrDefault(x => x.TemplateID == tenantTemplateId);

                var tenantModel = tenantItem?.GetItemModel();

                var converter = tenantModel?.GetConverter<Tenant>(Sitecore.DataExchange.Context.ItemModelRepository);
                if (converter == null) return null;

                var convertResult = converter.Convert(tenantModel);

                return convertResult.WasConverted ? convertResult.ConvertedValue : null;
            }

            return null;
        }

        protected static Pipeline GetPipeline(ItemModel itemModel)
        {
            var converter = itemModel?.GetConverter<Pipeline>(Sitecore.DataExchange.Context.ItemModelRepository);

            if (converter == null) return null;

            var convertResult = converter.Convert(itemModel);

            return convertResult.WasConverted ? convertResult.ConvertedValue : null;
        }

        protected static void AddPlugins(PipelineBatch pipelineBatch)
        {
            TelemetryActivitySettings telemetryPlugin = new TelemetryActivitySettings()
            {
                Enabled = false
            };
            pipelineBatch.AddPlugin<TelemetryActivitySettings>(telemetryPlugin);
            MultiModeSupportSettings newPlugin2 = new MultiModeSupportSettings()
            {
                SupportedModes = new List<string>()
            };
            pipelineBatch.AddPlugin<MultiModeSupportSettings>(newPlugin2);
        }

        protected static void AddRequiredPlugins(PipelineBatch pipelineBatch)
        {
            PipelineBatchSummary pipelineBatchSummary = new PipelineBatchSummary()
            {
                IncludeStackTraceForExceptions = true
            };
            AddLogLevel(pipelineBatchSummary);
            pipelineBatch.AddPlugin<PipelineBatchSummary>(pipelineBatchSummary);
            SitecoreItemSettings newPlugin = new SitecoreItemSettings()
            {
                ItemId = Guid.Parse(pipelineBatch.Identifier)
            };
            pipelineBatch.AddPlugin(newPlugin);
        }
        protected static void AddVerificationLogPlugin(PipelineBatch pipelineBatch)
        {
            VerificationLogSettings newPlugin = new VerificationLogSettings()
            {
                SaveJson = false,
                VerificationEnabled = false,
                VerificationLog = null
            };
            pipelineBatch.AddPlugin(newPlugin);
        }

        protected static void AddLogLevel(PipelineBatchSummary taskSettings)
        {
            taskSettings.LogLevels.Add(LogLevel.Debug);
            taskSettings.LogLevels.Add(LogLevel.Error);
            taskSettings.LogLevels.Add(LogLevel.Fatal);
            taskSettings.LogLevels.Add(LogLevel.Info);
            taskSettings.LogLevels.Add(LogLevel.Warn);
        }
    }
}