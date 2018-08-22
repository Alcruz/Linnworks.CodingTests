(function() {
	angular
		.module("app")
		.controller("CreateCategoryModalController", CreateCategoryModalController);

	CreateCategoryModalController.$inject = ["categoriesService", "$uibModalInstance"];

	function CreateCategoryModalController(categoriesService, $uibModalInstance) {
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