angular.module('admin')
	.config(function ($stateProvider) {
	    var componentName = 'login';
	    $stateProvider
			.state(componentName, {
			    url: '/' + componentName,
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			})
	        .state(componentName + '2', {
	            url: '/' + componentName + '/:returnUrl',
	            templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
	            controller: componentName + 'Ctrl'
	    });

	});
