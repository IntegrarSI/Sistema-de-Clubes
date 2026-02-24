'use strict';

angular.module('admin')
	.directive('loadingload', function () {
	    return {
	        templateUrl: '/app/shared/directives/admin/loadingLoad/loadingload.html',
	        restrict: 'E',
	        replace: true,
	    }
	});
