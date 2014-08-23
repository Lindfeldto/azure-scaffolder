/* *************************************** */
/* Category api data access */
/* *************************************** */

var CategoriesData = CategoriesData || (function(){

	return {
		CategoriesDataSource: new kendo.data.DataSource({
			transport: {

				read: {
					url: '/api/Category',
					dataType: 'json'
				}
			
			},
			schema: {

				model: {
					Name: 'Name',
					Description: 'Description',
					Order: 'Order',
					ParentCategoryPublicId: 'ParentCategoryPublicId',
				}

			}
		})

	};
}());
