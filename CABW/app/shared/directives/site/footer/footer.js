'use strict';

angular.module('site')
	.directive('footer',function(){
		return {
		    templateUrl: '/app/shared/directives/site/footer/footer.html',
            restrict: 'E',
            replace: true,
    	}
	});


