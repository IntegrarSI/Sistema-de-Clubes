angular.module('admin')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'jugadores_carnet';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName.replace('_','/') + '/:id',
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);