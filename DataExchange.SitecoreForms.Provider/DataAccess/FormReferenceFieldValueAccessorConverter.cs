using System;
using DataExchange.SitecoreForms.Provider.ValueReaders;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.DataAccess
{
    [SupportedIds(DynamicListFieldValueAccessorTemplateId, StaticListFieldValueAccessorTemplateId)]
    public class FormReferenceFieldValueAccessorConverter : ValueAccessorConverter
    {
        public const string DynamicListFieldValueAccessorTemplateId = "{AFF8EEE9-C80D-4E29-936F-711CF6DF18A7}";
        public const string StaticListFieldValueAccessorTemplateId = "{E9087FBA-5CDB-4B32-BC6C-FB738DB6C67C}";
        public const string TemplateFieldFormField = "FieldToRead";
        public const string TemplateFieldFormTargetFieald = "TargetValueField";
        public FormReferenceFieldValueAccessorConverter(IItemModelRepository repository) : base(repository)
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
                var targetField = this.GetReferenceAsModel(source, TemplateFieldFormTargetFieald);
                reader = new SitecoreFormReferenceFieldValueReader(fieldId, targetField?[ItemModel.ItemID] is Guid guid ? guid : Guid.Empty);
            }
            return reader;
        }
    }
}
