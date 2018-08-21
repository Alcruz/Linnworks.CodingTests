(function() {
	angular
		.module("app")
		.controller("CategoryCreateModalController", CategoryCreateModalController);

	CategoryCreateModalController.$inject = ["categoriesService", "$uibModalInstance"];

	function CategoryCreateModalController(categoryService, $uibModalInstance) {
		viewModel = this;
		viewModel.categoryName = "";
		viewModel.ok = ok;
		viewModel.cancel = cancel;

		function ok() {
			categoryService.create({
				categoryName: viewModel.categoryName
			}).then(function(createdItem) {
				$uibModalInstance.close(createdItem);
			});
		};

		function cancel() {
			$uibModalInstance.dismiss('cancel');
		};
	};
})();