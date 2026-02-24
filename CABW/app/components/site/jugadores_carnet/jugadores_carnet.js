angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'jugadores_carnet';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName.replace('_','/') + '/:id',
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);