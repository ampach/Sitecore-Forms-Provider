using System;
using System.Linq;
using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.Common;
using Sitecore.DataExchange.DataAccess;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    public class SitecoreFormEntryIdentifierValueReader : IValueReader
    {
        protected virtual bool CanRead(object source, DataAccessContext context)
        {
            return source is FormSubmissionEntry || source is Sitecore.ExperienceForms.Data.Entities.FormEntry;
        }

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (!this.CanRead(source, context))
                return ReadResult.NegativeResult(DateTime.Now);

            var entityModel = source is FormSubmissionEntry
                ? ((FormSubmissionEntry) source).FormEntry
                : (Sitecore.ExperienceForms.Data.Entities.FormEntry) source;

            return ReadResult.PositiveResult(entityModel.FormEntryId.ToID().ToShortID().ToString(), DateTime.Now);
        }
    }
}