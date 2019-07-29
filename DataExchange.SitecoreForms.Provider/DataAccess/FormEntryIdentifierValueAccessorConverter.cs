using System;
using DataExchange.SitecoreForms.Provider.ValueReaders;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters.DataAccess.ValueAccessors;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.DataAccess
{
    [SupportedIds(FormEntryIdentifierValueAccessorTemplateId)]
    public class FormEntryIdentifierValueAccessorConverter : ValueAccessorConverter
    {
        public const string FormEntryIdentifierValueAccessorTemplateId = "{4D2909D5-FA22-4A74-8C53-0BF16C3F48E1}";
        public FormEntryIdentifierValueAccessorConverter(IItemModelRepository repository) : base(repository)
        {
        }

        protected override IValueReader GetValueReader(ItemModel source)
        {
            var reader = base.GetValueReader(source);
            if (reader == null)
            {
                reader = new SitecoreFormEntryIdentifierValueReader();
            }
            return reader;
        }
    }
}