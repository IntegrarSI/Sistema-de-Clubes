angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'calendario_resultados';
	    $stateProvider
			.state(componentName, {
			    url: '/calendario/resultados/:id',
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);