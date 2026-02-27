angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'calendario_detalle';
	    $stateProvider
			.state(componentName, {
			    url: '/calendario/detalle/:id',
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);