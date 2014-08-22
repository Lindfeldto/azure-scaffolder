using MVCSample.Models;
using MVCSample.Models;
using Microsoft.WindowsAzure.Storage.Table;
using BlueMarble.Shared.Azure.Storage.Table;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using System;

namespace MVCSample.Models
{
	public partial class StorageContext
	{
		#region Key Helpers
		
        public string GetCategoryPartitionKey(Category Category)
        {
            return GetPartitionKey(Constants.StorageTableNames.Categories);
        }

        public string GetCategoryPartitionKey()
        {
            return GetPartitionKey(Constants.StorageTableNames.Categories);
        }

        public string GetCategoryRowKey(Category Category)
        {
            return GetRowKey();
        }

        public string GetCategoryRowKey()
        {
            return GetRowKey();
        }

		#endregion
	}
}