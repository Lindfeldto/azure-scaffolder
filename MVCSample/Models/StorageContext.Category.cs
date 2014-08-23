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
				public const string Categories = "categories"; 
			}

			internal partial class StoragePartitionNames
			{
				public const string Category = "category"; 
			}
		}

		#endregion

		#region Initialize Table

		[InitializeTable]
        public void InitializeCategoryTables()
        {
            Categories = CloudTableClient.GetTableReference(Constants.StorageTableNames.Categories);
            Categories.CreateIfNotExists();
        }

		public CloudTable Categories { get; set; }

		#endregion

		#region Data Access Methods

		public IQueryable<Category> GetCategories()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<Category>().Where(partitionKeyFilter);

            var collection = Categories.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<Category>> GetCategoriesAsync()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<Category>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<Category>(query);

            return returnList.AsQueryable();
        }

		public IQueryable<Category> GetCategories(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<Category>().Where(partitionKeyFilter);

            var collection = Categories.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<Category>> GetCategoriesAsync(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<Category>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<Category>(query);

            return returnList.AsQueryable();
        }

        public Category GetCategory(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<Category>().Where(queryFilter);

            var collection = Categories.ExecuteQuery(query);

            return collection.FirstOrDefault();
        }

        public async Task<Category> GetCategoryAsync(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<Category>().Where(queryFilter);

            var returnList = await ExecuteSegmentedQueryAsync<Category>(query);

            return returnList.FirstOrDefault();
        }

        public void InsertCategory(Category Category)
        {
            Category.PartitionKey = GetCategoryPartitionKey(Category);
			Category.RowKey = GetCategoryRowKey(Category);
			Category.PublicId = Category.GetPublicId();

            Insert<Category>(Category, Categories);
        }

        public async Task InsertCategoryAsync(Category Category)
        {
            Category.PartitionKey = GetCategoryPartitionKey(Category);
			Category.RowKey = GetCategoryRowKey(Category);
			Category.PublicId = Category.GetPublicId();

            await InsertAsync<Category>(Category, Categories);
        }

        public void InsertCategoryBatch(IEnumerable<Category> Categories)
        {
            InsertBatch<Category>(Categories, this.Categories);
        }

        public async Task InsertCategoryBatchAsync(IEnumerable<Category> Categories)
        {
            await InsertBatchAsync<Category>(Categories, this.Categories);
        }

        public void UpdateCategory(Category Category)
        {
            Replace<Category>(Category, Categories);
        }

        public async Task UpdateCategoryAsync(Category Category)
        {
            await ReplaceAsync<Category>(Category, Categories);
        }

        public void DeleteCategory(Category Category)
        {
            Delete<Category>(Category, Categories);
        }

        public async Task DeleteCategoryAsync(Category Category)
        {
            await DeleteAsync<Category>(Category, Categories);
        }

		#endregion

        #endregion
	}
}