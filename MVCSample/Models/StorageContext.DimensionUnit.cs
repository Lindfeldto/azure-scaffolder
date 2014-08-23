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
				public const string DimensionUnits = "dimensionunits"; 
			}

			internal partial class StoragePartitionNames
			{
				public const string DimensionUnit = "dimensionunit"; 
			}
		}

		#endregion

		#region Initialize Table

		[InitializeTable]
        public void InitializeDimensionUnitTables()
        {
            DimensionUnits = CloudTableClient.GetTableReference(Constants.StorageTableNames.DimensionUnits);
            DimensionUnits.CreateIfNotExists();
        }

		public CloudTable DimensionUnits { get; set; }

		#endregion

		#region Data Access Methods

		public IQueryable<DimensionUnit> GetDimensionUnits()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<DimensionUnit>().Where(partitionKeyFilter);

            var collection = DimensionUnits.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<DimensionUnit>> GetDimensionUnitsAsync()
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.NotEqual, string.Empty);

            var query = new TableQuery<DimensionUnit>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<DimensionUnit>(query);

            return returnList.AsQueryable();
        }

		public IQueryable<DimensionUnit> GetDimensionUnits(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<DimensionUnit>().Where(partitionKeyFilter);

            var collection = DimensionUnits.ExecuteQuery(query);

            return collection.AsQueryable();
        }

		public async Task<IQueryable<DimensionUnit>> GetDimensionUnitsAsync(string PartitionKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);

            var query = new TableQuery<DimensionUnit>().Where(partitionKeyFilter);

            var returnList = await ExecuteSegmentedQueryAsync<DimensionUnit>(query);

            return returnList.AsQueryable();
        }

        public DimensionUnit GetDimensionUnit(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<DimensionUnit>().Where(queryFilter);

            var collection = DimensionUnits.ExecuteQuery(query);

            return collection.FirstOrDefault();
        }

        public async Task<DimensionUnit> GetDimensionUnitAsync(string PartitionKey, string RowKey)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, PartitionKey);
			var rowKeyFilter = TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.Equal, RowKey);

			var queryFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, rowKeyFilter);

            var query = new TableQuery<DimensionUnit>().Where(queryFilter);

            var returnList = await ExecuteSegmentedQueryAsync<DimensionUnit>(query);

            return returnList.FirstOrDefault();
        }

        public void InsertDimensionUnit(DimensionUnit DimensionUnit)
        {
            DimensionUnit.PartitionKey = GetDimensionUnitPartitionKey(DimensionUnit);
			DimensionUnit.RowKey = GetDimensionUnitRowKey(DimensionUnit);
			DimensionUnit.PublicId = DimensionUnit.GetPublicId();

            Insert<DimensionUnit>(DimensionUnit, DimensionUnits);
        }

        public async Task InsertDimensionUnitAsync(DimensionUnit DimensionUnit)
        {
            DimensionUnit.PartitionKey = GetDimensionUnitPartitionKey(DimensionUnit);
			DimensionUnit.RowKey = GetDimensionUnitRowKey(DimensionUnit);
			DimensionUnit.PublicId = DimensionUnit.GetPublicId();

            await InsertAsync<DimensionUnit>(DimensionUnit, DimensionUnits);
        }

        public void InsertDimensionUnitBatch(IEnumerable<DimensionUnit> DimensionUnits)
        {
            InsertBatch<DimensionUnit>(DimensionUnits, this.DimensionUnits);
        }

        public async Task InsertDimensionUnitBatchAsync(IEnumerable<DimensionUnit> DimensionUnits)
        {
            await InsertBatchAsync<DimensionUnit>(DimensionUnits, this.DimensionUnits);
        }

        public void UpdateDimensionUnit(DimensionUnit DimensionUnit)
        {
            Replace<DimensionUnit>(DimensionUnit, DimensionUnits);
        }

        public async Task UpdateDimensionUnitAsync(DimensionUnit DimensionUnit)
        {
            await ReplaceAsync<DimensionUnit>(DimensionUnit, DimensionUnits);
        }

        public void DeleteDimensionUnit(DimensionUnit DimensionUnit)
        {
            Delete<DimensionUnit>(DimensionUnit, DimensionUnits);
        }

        public async Task DeleteDimensionUnitAsync(DimensionUnit DimensionUnit)
        {
            await DeleteAsync<DimensionUnit>(DimensionUnit, DimensionUnits);
        }

		#endregion

        #endregion
	}
}