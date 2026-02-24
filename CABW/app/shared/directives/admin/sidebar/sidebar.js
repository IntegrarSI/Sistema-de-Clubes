'use strict';

angular.module('admin')
  .directive('sidebar', ['$location', function () {
      return {
          templateUrl: '/areas/empresas/app/app/shared/directives/sidebar/sidebar.html',
          templateUrl: '/app/shared/directives/admin/sidebar/sidebar.html',
          restrict: 'E',
          replace: true
      }
  }]);
