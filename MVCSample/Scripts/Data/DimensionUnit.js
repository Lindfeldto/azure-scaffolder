/* *************************************** */
/* DimensionUnit api data access */
/* *************************************** */

var DimensionUnitData = DimensionUnitData || (function(){
    

	return {
		PublicId: 'NOTSET',
		Delete: function(e) {

			var that = this;

			e.preventDefault();

            $.ajax({
                url: '/api/DimensionUnit/' + that.PublicId,
                type: 'DELETE',
                dataType: 'json',
                success: function(data) {

                    window.location.href = '/DimensionUnit/';

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
				url: '/api/DimensionUnit/' + that.PublicId,
				type: 'PUT',
				dataType: 'json',
				data: {
					Name: that.Name,
					Description: that.Description,
					Order: that.Order,
					PublicId: this.PublicId
				},
				success: function(data) {

					window.location.href = '/DimensionUnit/Edit/' + that.PublicId;

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
				url: '/api/DimensionUnit',
				type: 'POST',
				dataType: 'json',
				data: {
					Name: that.Name,
					Description: that.Description,
					Order: that.Order,
					PublicId: that.PublicId
				},
				success: function(data) {

					window.location.href = '/DimensionUnit/Details/' + data;

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
				url: '/api/DimensionUnit/' + that.PublicId,
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
			that.ViewModel.Name = data.Name;
			that.ViewModel.Description = data.Description;
			that.ViewModel.Order = data.Order;
			that.ViewModel.PublicId = data.PublicId;


			that.ViewModel.loading = false;
			that.ViewModel.loaded = true;

			that.Bind(form);

		},
		ViewModel: kendo.observable({
			PublicId: '',
		
			Name: '',
			Description: '',
			Order: '',
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

			that.ViewModel.Name = '';
			that.ViewModel.Description = '';
			that.ViewModel.Order = '';
				
			that.ViewModel.update = that.Update;
			that.ViewModel.delete = that.Delete;
			that.ViewModel.create = that.Create;
		}
	};
}());
