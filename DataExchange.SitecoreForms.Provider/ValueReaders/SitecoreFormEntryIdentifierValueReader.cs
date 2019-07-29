using System;
using System.Linq;
using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.Common;
using Sitecore.DataExchange.DataAccess;
using Sitecore.ExperienceForms.Data.Entities;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    public class SitecoreFormEntryIdentifierValueReader : IValueReader
    {
        protected virtual bool CanRead(object source, DataAccessContext context)
        {
            return source is FormSubmissionEntry;
        }

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (!this.CanRead(source, context))
                return ReadResult.NegativeResult(DateTime.Now);

            var entityModel = (FormSubmissionEntry)source;

            return ReadResult.PositiveResult(entityModel.FormEntry.FormEntryId.ToID().ToShortID().ToString(), DateTime.Now);
        }
    }
}