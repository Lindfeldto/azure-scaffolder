/* *************************************** */
/* StockItem api data access */
/* *************************************** */

var StockItemsData = StockItemsData || (function(){

	return {
		StockItemsDataSource: new kendo.data.DataSource({
			transport: {

				read: {
					url: '/api/StockItem',
					dataType: 'json'
				}
			
			},
			schema: {

				model: {
					StockKeepingUnit: 'StockKeepingUnit',
					Name: 'Name',
					Description: 'Description',
					CostPrice: 'CostPrice',
					ListPrice: 'ListPrice',
					Width: 'Width',
					Length: 'Length',
					Height: 'Height',
					DimensionUnitPublicId: 'DimensionUnitPublicId',
					Weight: 'Weight',
					WeightUnitPublicId: 'WeightUnitPublicId',
					CategoryPublicId: 'CategoryPublicId',
				}

			}
		})

	};
}());
