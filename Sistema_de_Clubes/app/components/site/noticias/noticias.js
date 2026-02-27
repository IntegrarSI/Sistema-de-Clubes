angular.module('site')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'noticias';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName,
			    templateUrl: '/app/components/site/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);