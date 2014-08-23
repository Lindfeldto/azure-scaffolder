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
				public const string StockItems = "stockitems"; 
			}

			internal partial class StoragePartitionNames
			{
				public const string StockItem = "stockitem"; 
			}
		}

		#endregion

		#region Initialize Table

		[InitializeTable]
        public void InitializeStockItemTables()
        {
            StockItems = CloudTableClient.GetTableReference(Constants.StorageTableNames.StockItems);
            StockItems.CreateIfNotExists();
        }

		public CloudTable StockItems { get; set; }

		#endregion

		#region Data Access Methods

		public IQueryable<StockItem> GetStockItems()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<StockItem>().Where(partitionKeyFilter);

            var collection = StockItems.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<StockItem>> GetStockItemsAsync()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<StockItem>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<StockItem>(query);

            return returnList.AsQueryable();
        }

		public IQueryable<StockItem> GetStockItems(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<StockItem>().Where(partitionKeyFilter);

            var collection = StockItems.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<StockItem>> GetStockItemsAsync(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<StockItem>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<StockItem>(query);

            return returnList.AsQueryable();
        }

        public StockItem GetStockItem(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<StockItem>().Where(queryFilter);

            var collection = StockItems.ExecuteQuery(query);

            return collection.FirstOrDefault();
        }

        public async Task<StockItem> GetStockItemAsync(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<StockItem>().Where(queryFilter);

            var returnList = await ExecuteSegmentedQueryAsync<StockItem>(query);

            return returnList.FirstOrDefault();
        }

        public void InsertStockItem(StockItem StockItem)
        {
            StockItem.PartitionKey = GetStockItemPartitionKey(StockItem);
			StockItem.RowKey = GetStockItemRowKey(StockItem);
			StockItem.PublicId = StockItem.GetPublicId();

            Insert<StockItem>(StockItem, StockItems);
        }

        public async Task InsertStockItemAsync(StockItem StockItem)
        {
            StockItem.PartitionKey = GetStockItemPartitionKey(StockItem);
			StockItem.RowKey = GetStockItemRowKey(StockItem);
			StockItem.PublicId = StockItem.GetPublicId();

            await InsertAsync<StockItem>(StockItem, StockItems);
        }

        public void InsertStockItemBatch(IEnumerable<StockItem> StockItems)
        {
            InsertBatch<StockItem>(StockItems, this.StockItems);
        }

        public async Task InsertStockItemBatchAsync(IEnumerable<StockItem> StockItems)
        {
            await InsertBatchAsync<StockItem>(StockItems, this.StockItems);
        }

        public void UpdateStockItem(StockItem StockItem)
        {
            Replace<StockItem>(StockItem, StockItems);
        }

        public async Task UpdateStockItemAsync(StockItem StockItem)
        {
            await ReplaceAsync<StockItem>(StockItem, StockItems);
        }

        public void DeleteStockItem(StockItem StockItem)
        {
            Delete<StockItem>(StockItem, StockItems);
        }

        public async Task DeleteStockItemAsync(StockItem StockItem)
        {
            await DeleteAsync<StockItem>(StockItem, StockItems);
        }

		#endregion

        #endregion
	}
}