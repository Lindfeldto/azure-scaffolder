/* *************************************** */
/* WeightUnit api data access */
/* *************************************** */

var WeightUnitsData = WeightUnitsData || (function(){

	return {
		WeightUnitsDataSource: new kendo.data.DataSource({
			transport: {

				read: {
					url: '/api/WeightUnit',
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
