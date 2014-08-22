/* *************************************** */
/* StockUnit api data access */
/* *************************************** */

var StockUnitData = StockUnitData || (function(){
    

	return {
		PublicId: 'NOTSET',
		Delete: function(e) {

			var that = this;

			e.preventDefault();

            $.ajax({
                url: '/api/StockUnit/' + that.PublicId,
                type: 'DELETE',
                dataType: 'json',
                success: function(data) {

                    window.location.href = '/StockUnit/';

                },
                error: function(error) {

                    that.LogError(error);

                }
            });

		},
		Update: function(e) {

			var that = this;

		    e.preventDefault();

			$.ajax({
				url: '/api/StockUnit/' + that.PublicId,
				type: 'PUT',
				dataType: 'json',
				data: {
					ModelGuid: that.ModelGuid,
					StockItemPublicId: that.StockItemPublicId,
					TransactionDateTime: kendo.toString(this.TransactionDateTime, 'F'),
					StockIn: that.StockIn,
					ReceiptNumber: that.ReceiptNumber,
					StockOut: that.StockOut,
					InvoiceNumber: that.InvoiceNumber,
					PublicId: this.PublicId
				},
				success: function(data) {

					window.location.href = '/StockUnit/Edit/' + that.PublicId;

				},
				error: function(error) {

					that.LogError(error);

				}
			});

		},
		Create: function(e) {

			var that = this;

			e.preventDefault();

			$.ajax({
				url: '/api/StockUnit',
				type: 'POST',
				dataType: 'json',
				data: {
					ModelGuid: that.ModelGuid,
					StockItemPublicId: that.StockItemPublicId,
					TransactionDateTime: kendo.toString(that.TransactionDateTime, 'F'),
					StockIn: that.StockIn,
					ReceiptNumber: that.ReceiptNumber,
					StockOut: that.StockOut,
					InvoiceNumber: that.InvoiceNumber,
					PublicId: that.PublicId
				},
				success: function(data) {

					window.location.href = '/StockUnit/Details/' + data;

				},
				error: function(error) {

					that.LogError(error);

				}
			});

		},
		Load: function (form) {

			var that = this;

			that.ViewModel.loading = true;
			that.ViewModel.loaded = false;

			that.Bind(form);

			$.ajax({
				url: '/api/StockUnit/' + that.PublicId,
				type: 'GET',
				dataType: 'json',
				success: function (data) {

					that.LoadEntity(data, form);

				},
				error: function (error) {

					that.LogError(error);

				}
			});

		},
		LoadEntity: function(data, form){

			var that = this;
			that.ViewModel.ModelGuid = data.ModelGuid;
			that.ViewModel.StockItemPublicId = data.StockItemPublicId;
			that.ViewModel.TransactionDateTime = data.TransactionDateTime;
			that.ViewModel.StockIn = data.StockIn;
			that.ViewModel.ReceiptNumber = data.ReceiptNumber;
			that.ViewModel.StockOut = data.StockOut;
			that.ViewModel.InvoiceNumber = data.InvoiceNumber;
			that.ViewModel.PublicId = data.PublicId;

			$.ajax({
				url: '/api/StockItem/' + that.ViewModel.StockItemPublicId,
				type: 'GET',
				dataType: 'json',
				success: function (data) {

					that.ViewModel.StockItemPublicId_Display = data.Name;

				},
				error: function (error) {

					that.ViewModel.StockItemPublicId_Display = 'error reading data';

					console.log(error);
					    
				}
			}).done(function (data) {

		        that.Bind(form);

		    });

			that.ViewModel.loading = false;
			that.ViewModel.loaded = true;

			that.Bind(form);

		},
		ViewModel: kendo.observable({
			PublicId: '',
			StockItemsDataSource: null,
		
			ModelGuid: '',
			StockItemPublicId: '',
			StockItemPublicId_Display: 'loading...',
			TransactionDateTime: '',
			StockIn: '',
			ReceiptNumber: '',
			StockOut: '',
			InvoiceNumber: '',
			hasChanges: false,
			saving: false,
			saved: false,
			creating: false,
			created: false,
			deleting: false,
			deleted: false,
			loading: true,
			loaded: false,
			error: false,
			errorMessage: '',
			update: undefined,
			delete: undefined,
			create: undefined
				
		}),
		Bind: function(form) {

		    kendo.bind(form, this.ViewModel);

		},
		LogError: function(error) {

			var that = this;
			
			that.ViewModel.error = true;
			that.ViewModel.errorMessage = error.responseJSON.ExceptionMessage;

			console.log(error);

		},
		Init: function(publicId) {
			
			var that = this;

		    that.PublicId = publicId;

			that.ViewModel.PublicId = publicId;

			that.ViewModel.StockItemsDataSource = StockItemsData.StockItemsDataSource;
			that.ViewModel.ModelGuid = '';
			that.ViewModel.StockItemPublicId = '';
			that.ViewModel.StockItemPublicId_Display = 'loading...';
			that.ViewModel.TransactionDateTime = '';
			that.ViewModel.StockIn = '';
			that.ViewModel.ReceiptNumber = '';
			that.ViewModel.StockOut = '';
			that.ViewModel.InvoiceNumber = '';
				
			that.ViewModel.update = that.Update;
			that.ViewModel.delete = that.Delete;
			that.ViewModel.create = that.Create;
		}
	};
}());
