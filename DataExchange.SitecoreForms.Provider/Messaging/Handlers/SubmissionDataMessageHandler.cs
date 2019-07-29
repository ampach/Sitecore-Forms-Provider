using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataExchange.SitecoreForms.Provider.Messaging.Models;
using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.Abstractions;
using Sitecore.Data;
using Sitecore.DataExchange;
using Sitecore.Framework.Messaging;

namespace DataExchange.SitecoreForms.Provider.Messaging.Handlers
{
    public class SubmissionDataMessageHandler : IMessageHandler<SubmissionDataMessage>
    {
        
        private readonly IBatchRunner _batchRuunner;
        private readonly BaseLog _logger;

        public SubmissionDataMessageHandler(IBatchRunner batchRuunner, BaseLog logger)
        {
            _batchRuunner = batchRuunner;
            _logger = logger;
        }
        public Task Handle(SubmissionDataMessage message, IMessageReceiveContext receiveContext, IMessageReplyContext replyContext)
        {
            if (this.ValidateMessage(message))
            {
                var formData = new FormSyncData
                {
                    FormSubmissionEntry = new FormSubmissionEntry {
                        FormEntry = message.FormEntry,
                        FormSubmissionContext = new FormSubmissionContext
                        {
                            ContactId = message.ContactId
                        }
                    }
                };

                _batchRuunner.Run(
                    new ID(message.BatchId), new IPlugin[]{ formData });
            }

            return Task.CompletedTask;
        }

        private bool ValidateMessage(SubmissionDataMessage message)
        {
            if (message == null)
            {
                _logger.Error($"[DataExchange.SitecoreForms.Provider]: Message is null.", this);
                return false;
            }

            if (message.FormEntry == null)
            {
                _logger.Error($"[DataExchange.SitecoreForms.Provider]: FormEntry is null.", this);
                return false;
            }

            if (message.BatchId == Guid.Empty)
            {
                _logger.Error($"[DataExchange.SitecoreForms.Provider]: BatchId is wrong.", this);
                return false;
            }

            return true;
        }
    }
}