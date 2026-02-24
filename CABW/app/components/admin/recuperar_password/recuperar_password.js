angular.module('admin')
	.config(function ($stateProvider) {
	    var componentName = 'recuperar_password';
	    $stateProvider
			.state(componentName, {
			    url: '/recuperar_password',
			    templateUrl: '/app/components/admin/' + componentName + '/' + componentName + '.html',
			    controller: componentName + 'Ctrl'
			});

	});
