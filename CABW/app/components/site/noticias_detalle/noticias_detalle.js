angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'noticias_detalle';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName + '/:id',
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);