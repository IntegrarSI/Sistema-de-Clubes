angular.module('admin')
	.config(['$stateProvider', function ($stateProvider) {
	    var componentName = 'password_force_edit';
	    $stateProvider
			.state(componentName, {
			    url: '/Password/edit',
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	}]);