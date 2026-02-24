angular.module('admin')
    .controller('authCtrl', ['$scope', '$rootScope', 'auth', function ($scope, $rootScope, auth) {

        $scope.ticket = auth.me();

        $scope.logout = function () {
            auth.logout();
        }

        //se verifica el tamaño de la pantalla para activar que el menú se cierre luego de hacerle clic
        if (window.innerWidth < 768) {
            $rootScope.dataToggle = "collapse";
            $rootScope.isMobile = true;
        } else {
            $rootScope.dataToggle = "";
            $rootScope.isMobile = false;
        }

    }]);
