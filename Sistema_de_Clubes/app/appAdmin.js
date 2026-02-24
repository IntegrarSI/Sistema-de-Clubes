'use strict';

angular.module('admin', [
    'ngRoute',
    'ui.bootstrap',
    'ngAnimate',
    'ngMessages',
    'ngCookies',
    'ui.router',
    'vcRecaptcha',
    'angularFileUpload',
    'textAngular',
])
.constant('ROOT_SERVER_CONTROLLERS', '/admin/')
.config(function ($httpProvider) { })
.config(function ($urlRouterProvider) {
    $urlRouterProvider.otherwise(function ($injector, $location) {
        window.location.href = '/admin/#/login';
    });
})
.config(function ($httpProvider) {
    $httpProvider.interceptors.push('authInterceptor');
})
.run(function ($rootScope, $state, $window, auth) {
    auth.init();
});