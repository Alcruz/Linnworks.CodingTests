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

		activate()

		function activate() {
			categoriesService.getAll()
				.then(function(data) {
					viewModel.tableParams = new NgTableParams({}, { dataset: data });
				});
		};
	}
})();

