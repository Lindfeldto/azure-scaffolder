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
				public const string StockUnits = "stockunits"; 
			}

			internal partial class StoragePartitionNames
			{
				public const string StockUnit = "stockunit"; 
			}
		}

		#endregion

		#region Initialize Table

		[InitializeTable]
        public void InitializeStockUnitTables()
        {
            StockUnits = CloudTableClient.GetTableReference(Constants.StorageTableNames.StockUnits);
            StockUnits.CreateIfNotExists();
        }

		public CloudTable StockUnits { get; set; }

		#endregion

		#region Data Access Methods

		public IQueryable<StockUnit> GetStockUnits()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<StockUnit>().Where(partitionKeyFilter);

            var collection = StockUnits.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<StockUnit>> GetStockUnitsAsync()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<StockUnit>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<StockUnit>(query);

            return returnList.AsQueryable();
        }

		public IQueryable<StockUnit> GetStockUnits(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<StockUnit>().Where(partitionKeyFilter);

            var collection = StockUnits.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<StockUnit>> GetStockUnitsAsync(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<StockUnit>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<StockUnit>(query);

            return returnList.AsQueryable();
        }

        public StockUnit GetStockUnit(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<StockUnit>().Where(queryFilter);

            var collection = StockUnits.ExecuteQuery(query);

            return collection.FirstOrDefault();
        }

        public async Task<StockUnit> GetStockUnitAsync(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<StockUnit>().Where(queryFilter);

            var returnList = await ExecuteSegmentedQueryAsync<StockUnit>(query);

            return returnList.FirstOrDefault();
        }

        public void InsertStockUnit(StockUnit StockUnit)
        {
            StockUnit.PartitionKey = GetStockUnitPartitionKey(StockUnit);
			StockUnit.RowKey = GetStockUnitRowKey(StockUnit);
			StockUnit.PublicId = StockUnit.GetPublicId();

            Insert<StockUnit>(StockUnit, StockUnits);
        }

        public async Task InsertStockUnitAsync(StockUnit StockUnit)
        {
            StockUnit.PartitionKey = GetStockUnitPartitionKey(StockUnit);
			StockUnit.RowKey = GetStockUnitRowKey(StockUnit);
			StockUnit.PublicId = StockUnit.GetPublicId();

            await InsertAsync<StockUnit>(StockUnit, StockUnits);
        }

        public void InsertStockUnitBatch(IEnumerable<StockUnit> StockUnits)
        {
            InsertBatch<StockUnit>(StockUnits, this.StockUnits);
        }

        public async Task InsertStockUnitBatchAsync(IEnumerable<StockUnit> StockUnits)
        {
            await InsertBatchAsync<StockUnit>(StockUnits, this.StockUnits);
        }

        public void UpdateStockUnit(StockUnit StockUnit)
        {
            Replace<StockUnit>(StockUnit, StockUnits);
        }

        public async Task UpdateStockUnitAsync(StockUnit StockUnit)
        {
            await ReplaceAsync<StockUnit>(StockUnit, StockUnits);
        }

        public void DeleteStockUnit(StockUnit StockUnit)
        {
            Delete<StockUnit>(StockUnit, StockUnits);
        }

        public async Task DeleteStockUnitAsync(StockUnit StockUnit)
        {
            await DeleteAsync<StockUnit>(StockUnit, StockUnits);
        }

		#endregion

        #endregion
	}
}