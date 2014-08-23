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
		#region Services

		#region Constants

		internal partial class Constants
		{
			internal partial class StorageTableNames
			{
				public const string WeightUnits = "weightunits"; 
			}

			internal partial class StoragePartitionNames
			{
				public const string WeightUnit = "weightunit"; 
			}
		}

		#endregion

		#region Initialize Table

		[InitializeTable]
        public void InitializeWeightUnitTables()
        {
            WeightUnits = CloudTableClient.GetTableReference(Constants.StorageTableNames.WeightUnits);
            WeightUnits.CreateIfNotExists();
        }

		public CloudTable WeightUnits { get; set; }

		#endregion

		#region Data Access Methods

		public IQueryable<WeightUnit> GetWeightUnits()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<WeightUnit>().Where(partitionKeyFilter);

            var collection = WeightUnits.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<WeightUnit>> GetWeightUnitsAsync()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<WeightUnit>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<WeightUnit>(query);

            return returnList.AsQueryable();
        }

		public IQueryable<WeightUnit> GetWeightUnits(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<WeightUnit>().Where(partitionKeyFilter);

            var collection = WeightUnits.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<WeightUnit>> GetWeightUnitsAsync(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<WeightUnit>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<WeightUnit>(query);

            return returnList.AsQueryable();
        }

        public WeightUnit GetWeightUnit(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<WeightUnit>().Where(queryFilter);

            var collection = WeightUnits.ExecuteQuery(query);

            return collection.FirstOrDefault();
        }

        public async Task<WeightUnit> GetWeightUnitAsync(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<WeightUnit>().Where(queryFilter);

            var returnList = await ExecuteSegmentedQueryAsync<WeightUnit>(query);

            return returnList.FirstOrDefault();
        }

        public void InsertWeightUnit(WeightUnit WeightUnit)
        {
            WeightUnit.PartitionKey = GetWeightUnitPartitionKey(WeightUnit);
			WeightUnit.RowKey = GetWeightUnitRowKey(WeightUnit);
			WeightUnit.PublicId = WeightUnit.GetPublicId();

            Insert<WeightUnit>(WeightUnit, WeightUnits);
        }

        public async Task InsertWeightUnitAsync(WeightUnit WeightUnit)
        {
            WeightUnit.PartitionKey = GetWeightUnitPartitionKey(WeightUnit);
			WeightUnit.RowKey = GetWeightUnitRowKey(WeightUnit);
			WeightUnit.PublicId = WeightUnit.GetPublicId();

            await InsertAsync<WeightUnit>(WeightUnit, WeightUnits);
        }

        public void InsertWeightUnitBatch(IEnumerable<WeightUnit> WeightUnits)
        {
            InsertBatch<WeightUnit>(WeightUnits, this.WeightUnits);
        }

        public async Task InsertWeightUnitBatchAsync(IEnumerable<WeightUnit> WeightUnits)
        {
            await InsertBatchAsync<WeightUnit>(WeightUnits, this.WeightUnits);
        }

        public void UpdateWeightUnit(WeightUnit WeightUnit)
        {
            Replace<WeightUnit>(WeightUnit, WeightUnits);
        }

        public async Task UpdateWeightUnitAsync(WeightUnit WeightUnit)
        {
            await ReplaceAsync<WeightUnit>(WeightUnit, WeightUnits);
        }

        public void DeleteWeightUnit(WeightUnit WeightUnit)
        {
            Delete<WeightUnit>(WeightUnit, WeightUnits);
        }

        public async Task DeleteWeightUnitAsync(WeightUnit WeightUnit)
        {
            await DeleteAsync<WeightUnit>(WeightUnit, WeightUnits);
        }

		#endregion

        #endregion
	}
}