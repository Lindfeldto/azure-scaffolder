/* *************************************** */
/* StockItem api data access */
/* *************************************** */

var StockItemData = StockItemData || (function(){
    

	return {
		PublicId: 'NOTSET',
		Delete: function(e) {

			var that = this;

			e.preventDefault();

            $.ajax({
                url: '/api/StockItem/' + that.PublicId,
                type: 'DELETE',
                dataType: 'json',
                success: function(data) {

                    window.location.href = '/StockItem/';

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
				url: '/api/StockItem/' + that.PublicId,
				type: 'PUT',
				dataType: 'json',
				data: {
					StockKeepingUnit: that.StockKeepingUnit,
					Name: that.Name,
					Description: that.Description,
					CostPrice: that.CostPrice,
					ListPrice: that.ListPrice,
					Width: that.Width,
					Length: that.Length,
					Height: that.Height,
					DimensionUnitPublicId: that.DimensionUnitPublicId,
					Weight: that.Weight,
					WeightUnitPublicId: that.WeightUnitPublicId,
					CategoryPublicId: that.CategoryPublicId,
					PublicId: this.PublicId
				},
				success: function(data) {

					window.location.href = '/StockItem/Edit/' + that.PublicId;

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
				url: '/api/StockItem',
				type: 'POST',
				dataType: 'json',
				data: {
					StockKeepingUnit: that.StockKeepingUnit,
					Name: that.Name,
					Description: that.Description,
					CostPrice: that.CostPrice,
					ListPrice: that.ListPrice,
					Width: that.Width,
					Length: that.Length,
					Height: that.Height,
					DimensionUnitPublicId: that.DimensionUnitPublicId,
					Weight: that.Weight,
					WeightUnitPublicId: that.WeightUnitPublicId,
					CategoryPublicId: that.CategoryPublicId,
					PublicId: that.PublicId
				},
				success: function(data) {

					window.location.href = '/StockItem/Details/' + data;

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
				url: '/api/StockItem/' + that.PublicId,
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
			that.ViewModel.StockKeepingUnit = data.StockKeepingUnit;
			that.ViewModel.Name = data.Name;
			that.ViewModel.Description = data.Description;
			that.ViewModel.CostPrice = data.CostPrice;
			that.ViewModel.ListPrice = data.ListPrice;
			that.ViewModel.Width = data.Width;
			that.ViewModel.Length = data.Length;
			that.ViewModel.Height = data.Height;
			that.ViewModel.DimensionUnitPublicId = data.DimensionUnitPublicId;
			that.ViewModel.Weight = data.Weight;
			that.ViewModel.WeightUnitPublicId = data.WeightUnitPublicId;
			that.ViewModel.CategoryPublicId = data.CategoryPublicId;
			that.ViewModel.PublicId = data.PublicId;

			$.ajax({
				url: '/api/DimensionUnit/' + that.ViewModel.DimensionUnitPublicId,
				type: 'GET',
				dataType: 'json',
				success: function (data) {

					that.ViewModel.DimensionUnitPublicId_Display = data.Name;

				},
				error: function (error) {

					that.ViewModel.DimensionUnitPublicId_Display = 'error reading data';

					console.log(error);
					    
				}
			}).done(function (data) {

		        that.Bind(form);

		    });
			$.ajax({
				url: '/api/WeightUnit/' + that.ViewModel.WeightUnitPublicId,
				type: 'GET',
				dataType: 'json',
				success: function (data) {

					that.ViewModel.WeightUnitPublicId_Display = data.Name;

				},
				error: function (error) {

					that.ViewModel.WeightUnitPublicId_Display = 'error reading data';

					console.log(error);
					    
				}
			}).done(function (data) {

		        that.Bind(form);

		    });
			$.ajax({
				url: '/api/Category/' + that.ViewModel.CategoryPublicId,
				type: 'GET',
				dataType: 'json',
				success: function (data) {

					that.ViewModel.CategoryPublicId_Display = data.Name;

				},
				error: function (error) {

					that.ViewModel.CategoryPublicId_Display = 'error reading data';

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
			DimensionUnitsDataSource: null,
			WeightUnitsDataSource: null,
			CategoriesDataSource: null,
		
			StockKeepingUnit: '',
			Name: '',
			Description: '',
			CostPrice: '',
			ListPrice: '',
			Width: '',
			Length: '',
			Height: '',
			DimensionUnitPublicId: '',
			DimensionUnitPublicId_Display: 'loading...',
			Weight: '',
			WeightUnitPublicId: '',
			WeightUnitPublicId_Display: 'loading...',
			CategoryPublicId: '',
			CategoryPublicId_Display: 'loading...',
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

			that.ViewModel.DimensionUnitsDataSource = DimensionUnitsData.DimensionUnitsDataSource;
			that.ViewModel.WeightUnitsDataSource = WeightUnitsData.WeightUnitsDataSource;
			that.ViewModel.CategoriesDataSource = CategoriesData.CategoriesDataSource;
			that.ViewModel.StockKeepingUnit = '';
			that.ViewModel.Name = '';
			that.ViewModel.Description = '';
			that.ViewModel.CostPrice = '';
			that.ViewModel.ListPrice = '';
			that.ViewModel.Width = '';
			that.ViewModel.Length = '';
			that.ViewModel.Height = '';
			that.ViewModel.DimensionUnitPublicId = '';
			that.ViewModel.DimensionUnitPublicId_Display = 'loading...';
			that.ViewModel.Weight = '';
			that.ViewModel.WeightUnitPublicId = '';
			that.ViewModel.WeightUnitPublicId_Display = 'loading...';
			that.ViewModel.CategoryPublicId = '';
			that.ViewModel.CategoryPublicId_Display = 'loading...';
				
			that.ViewModel.update = that.Update;
			that.ViewModel.delete = that.Delete;
			that.ViewModel.create = that.Create;
		}
	};
}());
