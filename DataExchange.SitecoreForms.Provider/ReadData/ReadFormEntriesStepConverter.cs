using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.ReadData
{
    [SupportedIds(ReadTextFileStepTemplateId)]
    public class ReadFormEntriesStepConverter : BasePipelineStepConverter
    {

        public const string ReadTextFileStepTemplateId = "{396155D1-33D7-49DF-8182-4F6C9EDF3DC2}";
        public const string TemplateFieldEndpointFrom = "EndpointFrom";
        public const string TemplateFieldFrom = "From";
        public const string TemplateFieldTo = "To";
        public const string TemplateFieldForm = "Form";
        public ReadFormEntriesStepConverter(IItemModelRepository repository) : base(repository)
        {
        }
        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
            var settings = new EndpointSettings
            {
                EndpointFrom = ConvertReferenceToModel<Endpoint>(source, TemplateFieldEndpointFrom)
            };
            pipelineStep.AddPlugin(settings);

            IPlugin readDataSettings = GetReadDataSettings(source);
            if (readDataSettings == null)
                return;
            pipelineStep.AddPlugin(readDataSettings);
        }

        protected ReadDataSettings GetReadDataSettings(ItemModel source)
        {
            var settings = new ReadDataSettings
            {
                From = GetDateTimeValue(source, TemplateFieldFrom),
                To = GetDateTimeValue(source, TemplateFieldTo)
            };

            var formModel = GetReferenceAsModel(source, TemplateFieldForm);
            settings.FormID = GetGuidValue(formModel, ItemModel.ItemID);

            return settings;
        }
    }
}