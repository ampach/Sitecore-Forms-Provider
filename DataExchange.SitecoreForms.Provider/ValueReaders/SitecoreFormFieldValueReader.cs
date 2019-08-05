using System;
using System.Linq;
using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.DataExchange.DataAccess;
using Sitecore.ExperienceForms.Data.Entities;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    public class SitecoreFormFieldValueReader : IValueReader
    {
        public Guid FieldId { get; protected set; }
        public Sitecore.ExperienceForms.Data.Entities.FormEntry FormEntry { get; protected set; }

        public SitecoreFormFieldValueReader(Guid fieldId)
        {
            this.FieldId = fieldId;
        }

        protected virtual bool CanRead(object source, DataAccessContext context)
        {
            if (FieldId == Guid.Empty)
                return false;

            var isTypeOk = source is FormSubmissionEntry || source is FormEntry;
            if(!isTypeOk)
                return false;

            FormEntry = source is FormSubmissionEntry
                ? ((FormSubmissionEntry)source).FormEntry
                : (FormEntry)source;

            return FormEntry != null && FormEntry.Fields.Any(q => q.FieldItemId == FieldId);
        }

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (!this.CanRead(source, context))
                return ReadResult.NegativeResult(DateTime.Now);

            var field = FormEntry.Fields.First(q => q.FieldItemId == FieldId);

            var result = ReadFieldValue(field);

            return ReadResult.PositiveResult(result, DateTime.Now);
        }

        protected virtual string ReadFieldValue(FieldData data)
        {
            return data.Value;
        }
    }
}