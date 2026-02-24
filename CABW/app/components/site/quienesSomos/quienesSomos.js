angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'quienesSomos';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName,
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);