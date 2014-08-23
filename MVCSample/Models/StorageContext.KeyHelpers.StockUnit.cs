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
		
        public string GetStockUnitPartitionKey(StockUnit StockUnit)
        {
            return GetPartitionKey(Constants.StorageTableNames.StockUnits);
        }

        public string GetStockUnitPartitionKey()
        {
            return GetPartitionKey(Constants.StorageTableNames.StockUnits);
        }

        public string GetStockUnitRowKey(StockUnit StockUnit)
        {
            return GetStockUnitRowKey(StockUnit.ModelGuid);
        }

        public string GetStockUnitRowKey(Guid ModelGuid)
        {
            return GetRowKey(ModelGuid.ToString());
        }

		#endregion
	}
}