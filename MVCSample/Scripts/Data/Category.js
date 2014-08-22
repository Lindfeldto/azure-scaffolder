/* *************************************** */
/* Category api data access */
/* *************************************** */

var CategoryData = CategoryData || (function(){
    

	return {
		PublicId: 'NOTSET',
		Delete: function(e) {

			var that = this;

			e.preventDefault();

            $.ajax({
                url: '/api/Category/' + that.PublicId,
                type: 'DELETE',
                dataType: 'json',
                success: function(data) {

                    window.location.href = '/Category/';

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
				url: '/api/Category/' + that.PublicId,
				type: 'PUT',
				dataType: 'json',
				data: {
					ModelGuid: that.ModelGuid,
					Name: that.Name,
					Description: that.Description,
					Order: that.Order,
					ParentCategoryPublicId: that.ParentCategoryPublicId,
					PublicId: this.PublicId
				},
				success: function(data) {

					window.location.href = '/Category/Edit/' + that.PublicId;

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
				url: '/api/Category',
				type: 'POST',
				dataType: 'json',
				data: {
					ModelGuid: that.ModelGuid,
					Name: that.Name,
					Description: that.Description,
					Order: that.Order,
					ParentCategoryPublicId: that.ParentCategoryPublicId,
					PublicId: that.PublicId
				},
				success: function(data) {

					window.location.href = '/Category/Details/' + data;

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
				url: '/api/Category/' + that.PublicId,
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
			that.ViewModel.Name = data.Name;
			that.ViewModel.Description = data.Description;
			that.ViewModel.Order = data.Order;
			that.ViewModel.ParentCategoryPublicId = data.ParentCategoryPublicId;
			that.ViewModel.PublicId = data.PublicId;

			$.ajax({
				url: '/api/Category/' + that.ViewModel.ParentCategoryPublicId,
				type: 'GET',
				dataType: 'json',
				success: function (data) {

					that.ViewModel.ParentCategoryPublicId_Display = data.Name;

				},
				error: function (error) {

					that.ViewModel.ParentCategoryPublicId_Display = 'error reading data';

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
			CategoriesDataSource: null,
		
			ModelGuid: '',
			Name: '',
			Description: '',
			Order: '',
			ParentCategoryPublicId: '',
			ParentCategoryPublicId_Display: 'loading...',
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

			that.ViewModel.CategoriesDataSource = CategoriesData.CategoriesDataSource;
			that.ViewModel.ModelGuid = '';
			that.ViewModel.Name = '';
			that.ViewModel.Description = '';
			that.ViewModel.Order = '';
			that.ViewModel.ParentCategoryPublicId = '';
			that.ViewModel.ParentCategoryPublicId_Display = 'loading...';
				
			that.ViewModel.update = that.Update;
			that.ViewModel.delete = that.Delete;
			that.ViewModel.create = that.Create;
		}
	};
}());
