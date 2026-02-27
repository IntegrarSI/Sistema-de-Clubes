'use strict';

angular.module('admin')
	.directive('header',function(){
		return {
		    templateUrl: '/app/shared/directives/admin/header/header.html',
            restrict: 'E',
            replace: true,
    	}
	});


