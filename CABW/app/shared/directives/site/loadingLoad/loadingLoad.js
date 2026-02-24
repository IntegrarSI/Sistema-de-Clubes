'use strict';

angular.module('site')
	.directive('loadingload', function () {
	    return {
	        templateUrl: '/app/shared/directives/site/loadingLoad/loadingload.html',
	        restrict: 'E',
	        replace: true,
	    }
	});
