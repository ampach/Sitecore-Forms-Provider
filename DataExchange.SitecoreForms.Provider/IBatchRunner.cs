using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.Data;
using Sitecore.DataExchange;

namespace DataExchange.SitecoreForms.Provider
{
    public interface IBatchRunner
    {
        void Run(ID batchItemId, IPlugin[] plugins);
    }
}