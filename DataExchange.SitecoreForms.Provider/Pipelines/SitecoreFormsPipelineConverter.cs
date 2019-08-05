using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.Pipelines;
using Sitecore.DataExchange.Repositories;

namespace DataExchange.SitecoreForms.Provider.Pipelines
{
    [SupportedIds(new string[] { "{56BAECBA-F431-4442-9C50-BF59610F4718}" })]
    public class SitecoreFormsPipelineConverter : PipelineConverter
    {
        public SitecoreFormsPipelineConverter(IItemModelRepository repository) : base(repository)
        {
        }
    }
}