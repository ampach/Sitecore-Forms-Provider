using System;
using System.Reflection;
using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.Common;
using Sitecore.DataExchange;
using Sitecore.DataExchange.DataAccess;
using Sitecore.ExperienceForms.Data.Entities;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    public class FormSubmissionContextPropertyValueReader : IValueReader
    {
        public string PropertyName { get; private set; }
        public IReflectionUtil ReflectionUtil { get; set; }

        public FormSubmissionContextPropertyValueReader(string propertyName)
        {
            if (string.IsNullOrWhiteSpace(propertyName))
                throw new ArgumentOutOfRangeException(nameof(propertyName), (object)propertyName, "Property name must be specified.");

            this.PropertyName = propertyName;
            this.ReflectionUtil = Sitecore.DataExchange.DataAccess.Reflection.ReflectionUtil.Instance;
        }

       
        protected virtual bool CanRead(object source, DataAccessContext context)
        {
            return source is FormSubmissionEntry;
        }

        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            if (!this.CanRead(source, context))
                return ReadResult.NegativeResult(DateTime.Now);

            var submissionContext = (FormSubmissionEntry)source;

            
            var wasValueRead = false;
            object obj = null;

            var property = ReflectionUtil.GetProperty(PropertyName, submissionContext.FormSubmissionContext);

            if (property != null && property.CanRead)
            {
                obj = property.GetValue(submissionContext.FormSubmissionContext);
                wasValueRead = true;
            }
            return new ReadResult(DateTime.UtcNow)
            {
                WasValueRead = wasValueRead,
                ReadValue = obj
            };
        }
    }
}