angular.module('admin')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'federaciones';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName,
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);