(function() {

	angular
		.module("app")
		.controller("CategoriesTableController", CategoriesTableController);
	
	/* recommended - but see next section */
	function CategoriesTableController() {
		this.name = {};
		this.sendMessage = function() { };
	}
})();

