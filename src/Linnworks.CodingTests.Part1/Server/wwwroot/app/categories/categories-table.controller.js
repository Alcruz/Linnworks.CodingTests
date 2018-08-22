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
		viewModel.openCategoryCreateModal = openCategoryCreateModal;
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
		};

		function openDeleteCategoryModal() {
			var modalInstance = $uibModal.open({
				animation: true,
				ariaLabelledBy: "modal-title",
				ariaDescribedBy: "modal-body",
				templateUrl: "app/categories/categories-delete-modal.html",
				controller: "CategoryDeleteModalController",
				controllerAs: "controller",
				size: "lg",
				resolve: {
					category: function() {
						return viewModel.selectedCategory;
					}
				}
			});

			modalInstance.result.then(function(data) {
				viewModel.tableParams = new NgTableParams({}, {
					dataset: data
				});
			});
		};
	};
})();

