angular.module('admin').service('notificationService',
		['$rootScope', '$timeout',
		 function ($rootScope, $timeout) {

		     this.show = function (type, title, content) {
		         $rootScope.message = {
		             "type": type,
		             "title": title,
		             "content": content
		         };

		         $timeout(function () {
		             $rootScope.message = {};
		         }, 5000);
		     };
		     
		 }]);