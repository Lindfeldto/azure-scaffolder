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
		
        public string GetDimensionUnitPartitionKey(DimensionUnit DimensionUnit)
        {
            return GetPartitionKey(Constants.StorageTableNames.DimensionUnits);
        }

        public string GetDimensionUnitPartitionKey()
        {
            return GetPartitionKey(Constants.StorageTableNames.DimensionUnits);
        }

        public string GetDimensionUnitRowKey(DimensionUnit DimensionUnit)
        {
            return GetRowKey();
        }

        public string GetDimensionUnitRowKey()
        {
            return GetRowKey();
        }

		#endregion
	}
}