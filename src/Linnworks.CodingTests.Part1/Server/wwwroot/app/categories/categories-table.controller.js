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
		viewModel.openCreateCategoryModal = openCreateCategoryModal;
		viewModel.openDeleteCategoryModal = openDeleteCategoryModal;

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

		function selectCategory(selectedCategory) {
			viewModel.selectedCategory = selectedCategory;
		};

		function openCreateCategoryModal() {
			var modalInstance = $uibModal.open({
				animation: true,
				ariaLabelledBy: "modal-title",
				ariaDescribedBy: "modal-body",
				templateUrl: "app/categories/create-category-modal.html",
				controller: "CreateCategoryModalController",
				controllerAs: "controller",
				size: "lg",
			});

			modalInstance.result.then(function(data){
				viewModel.selectedCategory = null;
				viewModel.tableParams = new NgTableParams({}, {
					dataset: data
				});
			});
		};

		function openDeleteCategoryModal() {
			var modalInstance = $uibModal.open({
				animation: true,
				ariaLabelledBy: "modal-title",
				ariaDescribedBy: "modal-body",
				templateUrl: "app/categories/delete-category-modal.html",
				controller: "DeleteCategoryModalController",
				controllerAs: "controller",
				size: "lg",
				resolve: {
					category: function() {
						return viewModel.selectedCategory;
					}
				}
			});

			modalInstance.result.then(function(data) {
				viewModel.selectedCategory = null;
				viewModel.tableParams = new NgTableParams({}, {
					dataset: data
				});
			});
		};
	};
})();

