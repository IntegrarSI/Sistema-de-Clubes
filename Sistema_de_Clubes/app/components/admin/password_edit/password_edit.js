angular.module('admin')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'password_edit';
	    $stateProvider
			.state(componentName, {
			    url: '/Password',
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);