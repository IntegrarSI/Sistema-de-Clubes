angular.module('admin')
    .controller('noticias_editCtrl',
        ['$scope', '$rootScope', '$state', '$stateParams', 'noticiasService', 'notificationService', 'FileUploader'
        , function ($scope, $rootScope, $state, $stateParams, noticiasService, notificationService, FileUploader) {



            $scope.esAlta = false;
            if ($stateParams.id == '') {
                $scope.title = 'Nueva Noticia';
                $scope.esAlta = true;
            }
            else
                $scope.title = 'Modificación de Noticia';


            /* Uploader */
            var uploader = $scope.uploader = new FileUploader({
                url: '/noticias/UploadFile',
                autoUpload: true
            });

            uploader.isHTML5 = true;

            // FILTERS

            uploader.filters.push({
                name: 'imageFilter',
                fn: function (item, options) {
                    var type = '|' + item.type.slice(item.type.lastIndexOf('/') + 1) + '|';
                    return '|jpg|png|jpeg|'.indexOf(type) !== -1;
                }
            });

            uploader.onProgressItem = function (fileItem, progress) {
                $scope.isLoading = true;
            };
            uploader.onSuccessItem = function (fileItem, response, status, headers) {
                if (response.success)
                    $scope.instancia.fotoPeque = response.fileName;

                $scope.isLoading = false;
            };
            uploader.onErrorItem = function (fileItem, response, status, headers) {
                $scope.isLoading = false;
            };


            $scope.instancia = {
                fecha: new Date()
            };

            $scope.isLoading = false;

            if ($stateParams.id != '') {

                $rootScope.loadingLoad = true;

                //es una modificacion
                $scope.isLoading = true;
                noticiasService.obtener($stateParams.id).then(
                    function (result) {
                        console.log($scope.instancia)
                        $scope.instancia = result;
                        console.log($scope.instancia)
                        $scope.isLoading = false;
                        $rootScope.loadingLoad = false;
                    });
                }
            else 
            {

                $scope.instancia.destacada = true;

                
            }
            

            $scope.submit = function () {
                $scope.isLoading = true;
                noticiasService.modificar($scope.instancia).then(
                    function (result) {
                        if (result.success == 'true') {
                        
                            notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                            $state.go('noticias');
                        }
                        else
                            notificationService.show('red', 'Ups!', result.message);

                        $scope.isLoading = false;
                    }
                );

                return false;
            };



        }]);