(function() {
	'use strict';

	angular
		.module("app")
		.factory("categoriesService", categoriesService);

	categoriesService.$inject = ["$http"];

	function categoriesService($http) {
		var service = {
			getAll: getAll,
			delete: deleteCategory,
			create: create
		};

		return service;

		function getAll() {
			return $http.get("/api/categories")
					.then(response => response.data);
		};

		function deleteCategory(categoryId) {
			return $http.delete("/api/categories/" + categoryId);				;
		};

		function create(category) {
			return $http.post("/api/categories/", category)
				.then(function(response) {
					return response.data;
				}).catch(function(errorResponse) {
					return Promise.reject(errorResponse.data);
				});
		};
	}
})();
