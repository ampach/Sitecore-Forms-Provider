using DataExchange.SitecoreForms.Provider.Endpoints;
using Sitecore.DataExchange.Models;

namespace DataExchange.SitecoreForms.Provider.Extensions
{
    public static class EndpointExtensions
    {
        public static FormsSettings GetFormsSettings(this Endpoint endpoint)
        {
            return endpoint.GetPlugin<FormsSettings>();
        }
        public static bool HasFormsSettings(this Endpoint endpoint)
        {
            return (GetFormsSettings(endpoint) != null);
        }
    }
}