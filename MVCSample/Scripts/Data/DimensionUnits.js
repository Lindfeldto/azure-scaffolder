/* *************************************** */
/* DimensionUnit api data access */
/* *************************************** */

var DimensionUnitsData = DimensionUnitsData || (function(){

	return {
		DimensionUnitsDataSource: new kendo.data.DataSource({
			transport: {

				read: {
					url: '/api/DimensionUnit',
					dataType: 'json'
				}
			
			},
			schema: {

				model: {
					Name: 'Name',
					Description: 'Description',
					Order: 'Order',
				}

			}
		})

	};
}());
