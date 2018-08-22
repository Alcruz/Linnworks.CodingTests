(function () {
    "use strict";

    angular
        .module("app")
		.controller("DeleteCategoryModalController", DeleteCategoryModalController);

	DeleteCategoryModalController.$inject = ["categoriesService", "$uibModalInstance", "category"];

	function DeleteCategoryModalController(categoriesService, $uibModalInstance, category) {
		var viewModel = this;
		viewModel.ok = ok;
		viewModel.cancel = cancel;
		viewModel.category = category;

		function ok(){
			categoriesService.delete(viewModel.category.id)
				.then(function() {
					return categoriesService.getAll();
				}).then(function(data) {
					$uibModalInstance.close(data);
				});
		};

		function cancel() {
			$uibModalInstance.dismiss("cancel");
		};
    }
})();
