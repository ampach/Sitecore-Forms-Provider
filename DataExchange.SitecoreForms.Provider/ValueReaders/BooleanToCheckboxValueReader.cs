using System;
using Sitecore.DataExchange.DataAccess;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{
    public class BooleanToCheckboxValueReader : IValueReader
    {
        public virtual ReadResult Read(object source, DataAccessContext context)
        {
            if (source == null)
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }
            try
            {
                if (source is string)
                {
                    bool convertedValue;
                    if (bool.TryParse((string)source, out convertedValue))
                    {
                        return ReadResult.PositiveResult(convertedValue ? "1" : "0", DateTime.Now);
                    }
                }

                if (source is bool)
                {
                    return ReadResult.PositiveResult((bool)source ? "1" : "0", DateTime.Now);
                }

                return ReadResult.NegativeResult(DateTime.Now);

            }
            catch (FormatException ex)
            {
                return ReadResult.NegativeResult(DateTime.Now);
            }
        }
    }
}
