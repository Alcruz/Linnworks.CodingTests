(function() {
	angular
		.module("app")
		.controller("CategoryCreateModalController", CategoryCreateModalController);

	CategoryCreateModalController.$inject = ["categoriesService", "$uibModalInstance"];

	function CategoryCreateModalController(categoriesService, $uibModalInstance) {
		viewModel = this;
		viewModel.categoryName = "";
		viewModel.ok = ok;
		viewModel.cancel = cancel;

		function ok() {
			categoriesService.create({
				categoryName: viewModel.categoryName
			}).then(function(){
				return categoriesService.getAll();
			}).then(function(data) {
				$uibModalInstance.close(data);
			}).catch(function(error) {
				viewModel.error = error.message;
			});
		};

		function cancel() {
			$uibModalInstance.dismiss("cancel");
		};
	};
})();