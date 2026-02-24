angular.module('admin')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'dashboard';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName,
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);