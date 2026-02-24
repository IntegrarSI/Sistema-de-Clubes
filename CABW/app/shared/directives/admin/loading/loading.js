angular.module('admin')
 .directive('loading', function () {
     return {
         templateUrl: '/app/shared/directives/admin/loading/loading.html',
         restrict: 'E',
         replace: true,
     }
 });