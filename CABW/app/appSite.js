'use strict';

angular.module('site', [
    'ngRoute',
    'ui.bootstrap',
    'ngAnimate',
    'ngMessages',
    'ngSanitize',
    'ui.router',
])
.config(function ($httpProvider) { })
.config(function ($stateProvider, $urlRouterProvider, $locationProvider) {
    $urlRouterProvider.otherwise('/home');
})
.run(function ($rootScope, $state, $window) {
    //$state.go('home');
    //window.location = '/Admin/#/login';
//    $rootScope.$on('$stateChangeStart',
//    function (event, toState, toParams, fromState, fromParams) {
//                (function (d, s, id) {
//            var js, fjs = d.getElementsByTagName(s)[0];
//            if (d.getElementById(id)) return;
//            js = d.createElement(s); js.id = id;
//            js.src = "//connect.facebook.net/es_LA/sdk.js#xfbml=1&version=v2.8";
//            fjs.parentNode.insertBefore(js, fjs);
//            }
//            (document, 'script', 'facebook-jssdk'))
//})
});