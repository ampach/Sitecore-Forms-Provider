using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Sitecore.Common;
using Sitecore.Data;
using Sitecore.DataExchange.DataAccess;
using Sitecore.ExperienceForms.Data.Entities;

namespace DataExchange.SitecoreForms.Provider.ValueReaders
{

    public class SitecoreFormReferenceFieldValueReader : SitecoreFormFieldValueReader
    {
        public Guid TargetFieldId { get; protected set; }
        public SitecoreFormReferenceFieldValueReader(Guid fieldId, Guid targetFieldId) : base(fieldId)
        {
            TargetFieldId = targetFieldId;
        }

        protected override string ReadFieldValue(FieldData data)
        {
            var type = Type.GetType(data.ValueType);
            if (type != null && !type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IList<>)))
            {
                return base.ReadFieldValue(data);
            }

            var values = data.Value.Split(',');
            var outputList = new List<string>();
            foreach (var value in values)
            {
                ID id;
                if (ID.TryParse(value, out id))
                {
                    var item = Sitecore.Context.ContentDatabase.GetItem(id);
                    if(item == null) { 
                        outputList.Add(value);
                        continue;
                    }
                    if (TargetFieldId != Guid.Empty) { 
                        var targetFiled = item.Fields[TargetFieldId.ToID()];
                        if (targetFiled != null && targetFiled.HasValue)
                        {
                            outputList.Add(targetFiled.Value);
                        }
                        continue;
                    }

                    outputList.Add(item.DisplayName);
                    continue;
                }

                outputList.Add(value);
            }

            return outputList.Aggregate((q, w) => q + "," + w);
        }
    }
}