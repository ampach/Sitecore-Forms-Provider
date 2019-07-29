using DataExchange.SitecoreForms.Provider.Models;
using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    [SupportedIds(new string[] { "{9AEDFE99-2EB4-4AB6-98E2-8E1561BF08A4}" })]
    public class FormSubmissionContextPropertyValueReaderConverter : BaseItemModelConverter<IValueReader>
    {
        public const string FieldNamePropertyName = "PropertyName";

        public FormSubmissionContextPropertyValueReaderConverter(IItemModelRepository repository)
            : base(repository)
        {
        }

        protected override ConvertResult<IValueReader> ConvertSupportedItem(ItemModel source)
        {
            string stringValue = this.GetStringValue(source, FieldNamePropertyName);

            if (!string.IsNullOrWhiteSpace(stringValue))
                return this.PositiveResult(new FormSubmissionContextPropertyValueReader(stringValue));

            return this.NegativeResult(source, "The property name field must have a value specified.", string.Format("field: {0}", (object)"PropertyName"));
        }
    }
}