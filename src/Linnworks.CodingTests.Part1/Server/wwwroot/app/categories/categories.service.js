(function() {
	'use strict';

	angular
		.module("app")
		.factory("categoriesService", categoriesService);

	categoriesService.$inject = ["$http"];

	function categoriesService($http) {
		var service = {
			getAll: getAll,
		};

		return service;

		function getAll() {
			return $http.get("/api/categories")
					.then(response => response.data);
		};
	}
})();
