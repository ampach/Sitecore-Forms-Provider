using System;
using DataExchange.SitecoreForms.Provider.Messaging.Models;
using Sitecore.Framework.Messaging;
using Sitecore.Pipelines;

namespace DataExchange.SitecoreForms.Provider
{
    public class Initialize
    {
        private readonly IServiceProvider _serviceProvider;

        public Initialize(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public void Process(PipelineArgs args)
        {
            this._serviceProvider.StartMessageBus<SyncSubmissionDataMessageBus>();
        }
    }
}