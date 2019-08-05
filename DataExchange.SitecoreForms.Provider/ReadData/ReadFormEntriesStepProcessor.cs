using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DataExchange.SitecoreForms.Provider.Endpoints;
using DataExchange.SitecoreForms.Provider.Extensions;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Contexts;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Processors.PipelineSteps;
using Sitecore.ExperienceForms.Data;
using Sitecore.ExperienceForms.Data.Entities;
using Sitecore.Services.Core.Diagnostics;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.ReadData
{
    [RequiredEndpointPlugins(typeof(FormsSettings))]
    public class ReadFormEntriesStepProcessor : BaseReadDataStepProcessor
    {
        protected override void ReadData(Endpoint endpoint, PipelineStep pipelineStep, PipelineContext pipelineContext, ILogger logger)
        {
            if (endpoint == null)
            {
                throw new ArgumentNullException(nameof(endpoint));
            }
            if (pipelineStep == null)
            {
                throw new ArgumentNullException(nameof(pipelineStep));
            }
            if (pipelineContext == null)
            {
                throw new ArgumentNullException(nameof(pipelineContext));
            }
            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            var formDataProvider = (IFormDataProvider)Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService(typeof(IFormDataProvider));
            if (formDataProvider == null)
            {
                throw new Exception("IFormDataProvider is missing");
            }

            var settings = endpoint.GetFormsSettings();
            if (settings == null)
            {
                return;
            }
            var readDataSettings = pipelineStep.GetPlugin<ReadDataSettings>();
            if (readDataSettings == null || readDataSettings.FormID == Guid.Empty)
            {
                logger.Error("Form is not selected for Read Form Entries Step Processor");
                return;
            }

            var data = this.GetIterableData(settings, readDataSettings, formDataProvider);
            var dataSettings = new IterableDataSettings(data);

            pipelineContext.AddPlugin(dataSettings);
        }

        protected virtual IEnumerable<FormEntry> GetIterableData(FormsSettings settings, ReadDataSettings readDataSettings, IFormDataProvider formDataProvider)
        {
            if (readDataSettings.To == DateTime.MinValue)
                readDataSettings.To = DateTime.MaxValue;

            IEnumerable<FormEntry> formEntries =
                formDataProvider.GetEntries(readDataSettings.FormID, readDataSettings.From, readDataSettings.To).OrderByDescending(q => q.Created);

            return formEntries;
        }
    }
}