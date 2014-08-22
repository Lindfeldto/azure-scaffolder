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
		
        public string GetStockItemPartitionKey(StockItem StockItem)
        {
            return GetPartitionKey(Constants.StorageTableNames.StockItems);
        }

        public string GetStockItemPartitionKey()
        {
            return GetPartitionKey(Constants.StorageTableNames.StockItems);
        }

        public string GetStockItemRowKey(StockItem StockItem)
        {
            return GetRowKey();
        }

        public string GetStockItemRowKey()
        {
            return GetRowKey();
        }

		#endregion
	}
}