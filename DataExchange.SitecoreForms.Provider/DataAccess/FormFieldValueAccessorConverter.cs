using System;
using DataExchange.SitecoreForms.Provider.ValueReaders;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.DataAccess.Writers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.DataAccess
{
    [SupportedIds(FormFieldValueAccessorTemplateId)]
    public class FormFieldValueAccessorConverter : ValueAccessorConverter
    {
        public const string FormFieldValueAccessorTemplateId = "{FF1EEDF5-584F-4D6A-BA1A-AE99B7690DCD}";
        public const string TemplateFieldFormField = "FieldToRead";
        public FormFieldValueAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }

        protected override IValueReader GetValueReader(ItemModel source)
        {
            var reader = base.GetValueReader(source);
            if (reader == null)
            {
                var fieldId = this.GetGuidValue(source, TemplateFieldFormField);
                if (fieldId == Guid.Empty)
                {
                    return null;
                }
                reader = new SitecoreFormFieldValueReader(fieldId);
            }
            return reader;
        }
    }
}