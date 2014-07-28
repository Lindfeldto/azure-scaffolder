using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageScaffolder.Extensions
{
    public static class CodeAttributeExtensions
    {
        public static object GetValueFromAttribute<T>(this CodeAttribute graph, string PropertyName)
        {
            var formattedValue = graph.Value.Replace("\r", "").Replace("\n", "").Replace("\t", "");
            var valueStrings = formattedValue.Split(',');

            foreach (var value in valueStrings)
            {
                var parameterStrings = value.Split('=');

                if (parameterStrings[0].Trim() == PropertyName)
                {
                    var parameterValue = parameterStrings[1].Trim();

                    if (PropertyName.Trim() == "Type")
                    {
                        var typeName = parameterValue.Replace("typeof(", "").Replace(")", "").Trim();

                        return Type.GetType(typeName);
                    }
                }
            }

            throw new Exception(string.Format("Property '{0}' not found", PropertyName));
        }
    }
}
