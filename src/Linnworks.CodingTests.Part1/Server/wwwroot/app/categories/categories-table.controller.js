(function() {
	"use strict";

	angular
		.module("app")
		.component("categoriestable", {
			templateUrl: "app/categories/categories-table.html",
			controllerAs: "controller", 
			controller: CategoriesTableController
		});

	CategoriesTableController.$inject = ["categoriesService", "NgTableParams", "$uibModal"];

	function CategoriesTableController(categoriesService, NgTableParams, $uibModal) {
		var viewModel = this;
		viewModel.select = selectCategory;
		viewModel.deleteSelectedCategory = deleteSelectedCategory;
		viewModel.openCategoryCreateModal = openCategoryCreateModal;

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

		function openCategoryCreateModal() {
			var modalInstance = $uibModal.open({
				animation: true,
				ariaLabelledBy: "modal-title",
				ariaDescribedBy: "modal-body",
				templateUrl: "app/categories/categories-create-modal.html",
				controller: "CategoryCreateModalController",
				controllerAs: "controller",
				size: "lg",
			});

			modalInstance.result.then(function(data) {
				viewModel.tableParams = new NgTableParams({}, {
					dataset: data
				});
			});
		}
	};
})();

