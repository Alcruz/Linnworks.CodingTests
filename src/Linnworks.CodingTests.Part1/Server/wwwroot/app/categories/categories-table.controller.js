(function() {
	'use strict';

	angular
		.module("app")
		.component("categoriestable", {
			templateUrl: "app/categories/categories-table.html",
			controllerAs: "controller", 
			controller: CategoriesTableController
		});

	CategoriesTableController.$inject = ["categoriesService", "NgTableParams"];

	function CategoriesTableController(categoriesService, NgTableParams) {
		var viewModel = this;
		viewModel.select = selectCategory;
		viewModel.deleteSelectedCategory = deleteSelectedCategory;
		activate();

		function activate() {
			getAllCategories();
		};

		function getAllCategories() {
			categoriesService.getAll()
				.then(function(data) {
					viewModel.tableParams = new NgTableParams({}, {
						dataset: data
					});
				});
		};

		function deleteSelectedCategory() {
			categoriesService.delete(viewModel.selectedCategory.id)
				.then(function() {
					console.log("category deleted");
				}).catch(function() {
					console.log("category not deleted");
				});
		};

		function selectCategory(selectedCategory) {
			viewModel.selectedCategory = selectedCategory;
		};
	};
})();

