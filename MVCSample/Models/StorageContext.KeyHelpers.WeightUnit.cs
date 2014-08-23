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
		
        public string GetWeightUnitPartitionKey(WeightUnit WeightUnit)
        {
            return GetPartitionKey(Constants.StorageTableNames.WeightUnits);
        }

        public string GetWeightUnitPartitionKey()
        {
            return GetPartitionKey(Constants.StorageTableNames.WeightUnits);
        }

        public string GetWeightUnitRowKey(WeightUnit WeightUnit)
        {
            return GetWeightUnitRowKey(WeightUnit.ModelGuid);
        }

        public string GetWeightUnitRowKey(Guid ModelGuid)
        {
            return GetRowKey(ModelGuid.ToString());
        }

		#endregion
	}
}