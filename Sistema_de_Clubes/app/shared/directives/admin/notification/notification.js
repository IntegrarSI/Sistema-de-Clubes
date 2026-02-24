'use strict';

angular.module('admin')
	.directive('notification', ['$timeout', function ($timeout) {
	    return {
	        templateUrl: '/app/shared/directives/admin/notification/notification.html',
	        restrict: 'E',
	        replace: true,
	        scope: {
	            'model': '=',
	            'type': '@',
	            'title': '@',
	            'content': '@',
	        }
	    }
	}]);
