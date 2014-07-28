using EnvDTE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureTableStorageScaffolder.Extensions
{
    public static class AzureStorageContextExtensions
    {
        public static bool IsValidAzureStorageContext(this CodeType graph)
        {
            return recurseInheritence(graph).Contains(typeof(BlueMarble.Shared.Azure.Storage.Table.StorageContext).FullName);
        }

        public static bool IsValidAzureTableEntity(this CodeType graph)
        {
            return recurseInheritence(graph).Contains(typeof(BlueMarble.Shared.Azure.Storage.Table.Entity).FullName);
        }

        private static List<string> recurseInheritence(CodeType graph)
        {
            var results = new List<string>();

            foreach (CodeType inheritedBase in graph.Bases)
            {
                results.Add(inheritedBase.FullName);

                results.AddRange(recurseInheritence(inheritedBase));
            }

            return results;
        }
    }
}
