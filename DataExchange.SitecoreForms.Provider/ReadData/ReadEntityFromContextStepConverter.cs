using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.PipelineSteps;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Plugins;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.ReadData
{
    [SupportedIds("{CFFFF046-C972-4612-909D-886E9B00C089}")]
    public class ReadEntityFromContextStepConverter : BasePipelineStepConverter
    {
        
        public ReadEntityFromContextStepConverter(IItemModelRepository repository) : base(repository)
        {
        }

        protected override void AddPlugins(ItemModel source, PipelineStep pipelineStep)
        {
           
        }
    }
}