using System;
using Sitecore.DataExchange;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.ReadData
{
    public class ReadDataSettings : IPlugin
    {
        public ReadDataSettings()
        {
        }

        public Guid FormID { get; set; }
        public DateTime From { get; set; }

        public DateTime To { get; set; }
    }
}