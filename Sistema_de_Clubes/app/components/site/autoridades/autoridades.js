angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'autoridades';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName,
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);