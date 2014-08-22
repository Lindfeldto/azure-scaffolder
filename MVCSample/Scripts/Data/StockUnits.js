/* *************************************** */
/* StockUnit api data access */
/* *************************************** */

var StockUnitsData = StockUnitsData || (function(){

	return {
		StockUnitsDataSource: new kendo.data.DataSource({
			transport: {

				read: {
					url: '/api/StockUnit',
					dataType: 'json'
				}
			
			},
			schema: {

				model: {
					ModelGuid: 'ModelGuid',
					StockItemPublicId: 'StockItemPublicId',
					TransactionDateTime: 'TransactionDateTime',
					StockIn: 'StockIn',
					ReceiptNumber: 'ReceiptNumber',
					StockOut: 'StockOut',
					InvoiceNumber: 'InvoiceNumber',
				}

			}
		})

	};
}());
