angular.module('site')
    .controller('noticias_detalleCtrl', ['$scope', '$rootScope', '$stateParams', 'noticiasService', function ($scope, $rootScope, $stateParams, noticiasService) {
        $scope.url = "sadsadd";
        //se busacan los eventos activos
        $rootScope.user = {};
        $scope.isLoading = true;
        $scope.filter = '';

        if ($stateParams.id != '')
        {
            $scope.isLoading = true;
            noticiasService.obtenerNoticia($stateParams.id).then(
                function (result) {
                    console.log($scope.instancia)
                    $scope.instancia = result;
                    $scope.remplazar();
                    $scope.isLoading = false;
                    $rootScope.loadingLoad = false;
                });

            
        }
        noticiasService.obtenerUrl().then(
            function (result) {
                $scope.url = result;
            });

        $scope.cargarFB = function () {
            window.fbAsyncInit = function () {
                FB.init({
                    appId: '1861248747424624',
                    status: true,
                    cookie: true,
                    xfbml: true,
                    oauth: true
                });

                FB.Event.subscribe('comment.create',
                    function (response) {
                        console.log('create', response);
                    });
                FB.Event.subscribe('comment.remove',
                    function (response) {
                        console.log('remove', response);
                    });
            };
        };

        $scope.remplazar = function () {
            $scope.instancia.texto = $scope.instancia.texto.split("\n").join("<br/>");
        };
       
        $scope.cargarFB();

        $scope.fbComments = 'http://developers.facebook.com/docs/plugins/comments/';

        
        }]);
                
