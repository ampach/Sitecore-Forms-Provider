using Sitecore.DataExchange;
using Sitecore.DataExchange.Attributes;
using Sitecore.DataExchange.Converters;
using Sitecore.DataExchange.DataAccess;
using Sitecore.DataExchange.DataAccess.Readers;
using Sitecore.DataExchange.Repositories;
using Sitecore.Services.Core.Model;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    
    [SupportedIds(new string[] { "{FBDC89C8-054C-4D92-B3E7-6738DC3E587B}" })]
    public class BooleanToCheckboxValueReaderConverter : BaseItemModelConverter<IValueReader>
    {
        public BooleanToCheckboxValueReaderConverter(IItemModelRepository repository)
            : base(repository)
        {
        }

        protected override ConvertResult<IValueReader> ConvertSupportedItem(ItemModel source)
        {
            return this.PositiveResult((IValueReader)new BooleanToCheckboxValueReader());
        }
    }
}
