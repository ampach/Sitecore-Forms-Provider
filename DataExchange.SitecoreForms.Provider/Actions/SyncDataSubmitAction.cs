using System;
using System.Collections.Generic;
using System.Linq;
using DataExchange.SitecoreForms.Provider.Messaging.Models;
using Newtonsoft.Json;
using Sitecore.Analytics;
using Sitecore.Analytics.Tracking;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Data.Entities;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using Sitecore.Framework.Messaging;

namespace DataExchange.SitecoreForms.Provider.Actions
{
    public class SyncDataSubmitAction : SaveData
    {
        
        private Guid BatchId = Guid.Empty;

        public SyncDataSubmitAction(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull((object)formSubmitContext, nameof(formSubmitContext));
            Guid.TryParse(data, out BatchId);
            return this.SavePostedData(formSubmitContext.FormId, formSubmitContext.SessionId, formSubmitContext.Fields);
        }

        protected override bool TryParse(string value, out string target)
        {
            if (string.IsNullOrEmpty(value))
            {
                target = null;
                return false;
            }
            try
            {
                var model = JsonConvert.DeserializeObject<SyncDataSubmitModel>(value);
                target = model.ReferenceId;
            }
            catch (JsonException ex)
            {
                this.Logger.LogError(ex.Message, (Exception)ex, (object)this);
                target = null;
                return false;
            }
            return (object)target != null;
        }

        protected override bool SavePostedData(Guid formId, Guid sessionId, IList<IViewModel> postedFields)
        {
            var messageBus = (IMessageBus<SyncSubmissionDataMessageBus>)Sitecore.DependencyInjection.ServiceLocator.ServiceProvider.GetService(typeof(IMessageBus<SyncSubmissionDataMessageBus>));

            try
            {
                FormEntry formEntry = new FormEntry()
                {
                    Created = DateTime.UtcNow,
                    FormItemId = formId,
                    FormEntryId = sessionId,
                    Fields = new List<FieldData>()
                };
                if (postedFields != null)
                {
                    foreach (var postedField in postedFields)
                        AddFieldData(postedField, formEntry);
                }

                var outgoingModel = new SubmissionDataMessage {FormEntry = formEntry, BatchId = BatchId};

                if (Tracker.Current != null && Tracker.Current.Contact != null)
                {
                    outgoingModel.ContactId = Tracker.Current.Contact.ContactId.ToString("N");
                }


                messageBus.Send(outgoingModel);
                return true;
            }
            catch (Exception ex)
            {
                this.Logger.LogError(ex.Message, ex, this);
                return false;
            }
        }
    }
}