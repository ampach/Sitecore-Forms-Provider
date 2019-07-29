using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.Endpoints;
using Sitecore.DataExchange.Models;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.Endpoints
{
    [SupportedIds(TextFileEndpointTemplateId)]
    public class FormsEndpointConverter : BaseConnectionStringEndpointConverter
    {
        public const string TextFileEndpointTemplateId = "{A61205E4-858D-4555-BD0D-421C1286C787}";

        public FormsEndpointConverter(IItemModelRepository repository) 
            : base(repository)
        {
        }

        protected override void AddPlugins(ItemModel source, Endpoint endpoint)
        {
            //string connectionStringValue = this.GetConnectionStringValue(source, "ConnectionStringName");
            //FormsSettings newPlugin = new FormsSettings(connectionStringValue);
            FormsSettings newPlugin = new FormsSettings();
            endpoint.AddPlugin<FormsSettings>(newPlugin);
        }
    }
}