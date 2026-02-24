angular.module('admin')
    .controller('usuarios_editCtrl',
        ['$scope', '$rootScope', '$state', 'focus', '$stateParams', 'usuariosService', 'notificationService', 'rolesService', 'federacionesService', 'asociacionesService', 'auth'
        , function ($scope, $rootScope, $state, focus, $stateParams, usuariosService, notificationService, rolesService, federacionesService, asociacionesService, auth) {

            $scope.user = auth.me();

            $scope.esAlta = false;
            if ($stateParams.id == '') {
                $scope.title = 'Nuevo usuario';
                $scope.esAlta = true;
            }
            else
                $scope.title = 'Modificación de usuario';

            $scope.init = function () {

                $scope.instancia = {};

                $scope.federacion = null;
                $scope.instancia.UsuariosFederaciones = [];
                $scope.asociacion = null;
                $scope.instancia.UsuariosAsociaciones = [];
                $scope.instancia.UsuariosRoles = [];
                $scope.rolesFilter = function () {
                    if ($scope.user.roles.length == 1 && $scope.user.roles[0] == "federacion")
                        return "federacion";
                    else return '';
                }
                $scope.isLoading = false;

                if ($stateParams.id != '') {

                    focus('nombre');

                    //es una modificacion
                    $scope.isLoading = true;
                    $rootScope.loadingLoad = true;
                    rolesService.obtenerLista($scope.rolesFilter()).then(
                        function (result) {
                            $scope.roles = result;

                            usuariosService.obtener($stateParams.id).then(
                                function (result) {
                                    $scope.instancia = result;

                                    if ($scope.rolesFilter() != 'federacion') {
                                        var indexRolConfederacion = $scope.roles.indexOfByProperty('codigoInterno', 'confederacion');
                                        var indexRolFederacion = $scope.roles.indexOfByProperty('codigoInterno', 'federacion');
                                        var indexRolAsociacion = $scope.roles.indexOfByProperty('codigoInterno', 'asociacion');
                                        var indexRolSeguridad = $scope.roles.indexOfByProperty('codigoInterno', 'seguridad');
                                        var indexRolAdministrador = $scope.roles.indexOfByProperty('codigoInterno', 'administrador');

                                        for (var i = 0; i < $scope.roles.length; i++) {
                                            $scope.roles[i].check = $scope.instancia.UsuariosRoles.indexOfByProperty('idRol', $scope.roles[i].id) > -1;
                                            if ($scope.roles[i].check && $scope.roles[i].codigoInterno == 'federacion') {
                                                $scope.poseeRolFederacion = true;
                                                $scope.roles[indexRolConfederacion].disabled = true;
                                                $scope.roles[indexRolAsociacion].disabled = true;
                                            }
                                            else if ($scope.roles[i].check && $scope.roles[i].codigoInterno == 'asociacion') {
                                                $scope.poseeRolAsociacion = true;
                                                $scope.roles[indexRolConfederacion].disabled = true;
                                                $scope.roles[indexRolFederacion].disabled = true;
                                            }
                                            else if ($scope.roles[i].check && $scope.roles[i].codigoInterno == 'confederacion') {
                                                $scope.roles[indexRolAsociacion].disabled = true;
                                                $scope.roles[indexRolFederacion].disabled = true;
                                            }
                                        }
                                    }
                                    $scope.isLoading = false;
                                    $rootScope.loadingLoad = false;
                                });

                        });
                }
                else {
                    focus('nombreUsuario');

                    $scope.instancia.activo = true;

                    rolesService.obtenerLista($scope.rolesFilter()).then(
                        function (result) {
                            $scope.roles = result;
                        });
                }
            }
            $scope.init();

            $scope.federaciones = function (filter) {
                $scope.federacion = null;
                return federacionesService.obtenerListaAutocompletar(filter).then(
                    function (data) {
                        return data;
                    }
                );
            };

            $scope.federacionesSelect = function ($item) {
                $scope.instancia.UsuariosFederaciones.push({ Federaciones: $item, idFederacion: $item.id });
                $scope.federacion = null;
            };

            $scope.eliminarFederacion = function (item) {
                $scope.instancia.UsuariosFederaciones.splice($scope.instancia.UsuariosFederaciones.indexOf(item), 1);
            };

            $scope.asociaciones = function (filter) {
                $scope.asociacion = null;
                if ($scope.rolesFilter()=='federacion')
                    return asociacionesService.obtenerListaPorUsuario(filter).then(
                        function (data) {
                            return data;
                        }
                    );
                else 
                    return asociacionesService.obtenerListaAutocompletar(filter).then(
                       function (data) {
                           return data;
                       }
                   );
            };

            $scope.asociacionesSelect = function ($item) {
                $scope.instancia.UsuariosAsociaciones.push({ Asociaciones: $item, idAsociacion: $item.id });
                $scope.asociacion = null;
            };

            $scope.eliminarAsociacion = function (item) {
                $scope.instancia.UsuariosAsociaciones.splice($scope.instancia.UsuariosAsociaciones.indexOf(item), 1);
            };

            $scope.toggleRol = function (checked, item) {
                //solo se permite seleccionar confederacion, federación o asociacion
                var indexRolConfederacion = $scope.roles.indexOfByProperty('codigoInterno', 'confederacion');
                var indexRolFederacion = $scope.roles.indexOfByProperty('codigoInterno', 'federacion');
                var indexRolAsociacion = $scope.roles.indexOfByProperty('codigoInterno', 'asociacion');

                if ($scope.rolesFilter() != 'federacion') {
                    if (item.codigoInterno == 'confederacion' || item.codigoInterno == 'federacion' || item.codigoInterno == 'asociacion') {

                        $scope.poseeRolFederacion = null;
                        $scope.poseeRolAsociacion = null;

                        $scope.instancia.UsuariosFederaciones = [];
                        $scope.instancia.UsuariosAsociaciones = [];

                        if (checked) {

                            //se deshabilitan todos los checks y se habilita el seleccionado
                            $scope.roles[indexRolConfederacion].disabled = true;
                            $scope.roles[indexRolFederacion].disabled = true;
                            $scope.roles[indexRolAsociacion].disabled = true;

                            if (item.codigoInterno == 'confederacion') {
                                $scope.roles[indexRolConfederacion].disabled = false;
                                $scope.roles[indexRolFederacion].check = false;
                                $scope.roles[indexRolAsociacion].check = false;
                            }
                            else if (item.codigoInterno == 'federacion') {
                                $scope.roles[indexRolFederacion].disabled = false;
                                $scope.poseeRolFederacion = true;
                                $scope.roles[indexRolConfederacion].check = false;
                                $scope.roles[indexRolAsociacion].check = false;
                            }
                            else {
                                $scope.roles[indexRolAsociacion].disabled = false;
                                $scope.poseeRolAsociacion = true;
                                $scope.roles[indexRolConfederacion].check = false;
                                $scope.roles[indexRolFederacion].check = false;
                            }
                        }
                        else {
                            $scope.roles[indexRolConfederacion].disabled = false;
                            $scope.roles[indexRolFederacion].disabled = false;
                            $scope.roles[indexRolAsociacion].disabled = false;
                        }
                    }
                   

                } else $scope.poseeRolAsociacion = true;
                //se verifica si ya existe el rol
                var existe = $scope.instancia.UsuariosRoles.indexOfByProperty('idRol', item.id);

                if (existe == -1) {
                    $scope.instancia.UsuariosRoles.push({ idRol: item.id, Roles: item });
                } else {
                    $scope.instancia.UsuariosRoles.splice(existe, 1);
                }

            };

            $scope.submit = function () {
                if ($scope.instancia.UsuariosRoles == null || $scope.instancia.UsuariosRoles.length == 0)
                    notificationService.show('danger', 'Atención', 'Debe seleccionar al menos un rol');
                else {
                    $scope.isLoading = true;
                    usuariosService.modificar($scope.instancia).then(
                        function (result) {
                            if (result.success == 'true') {
                                notificationService.show('green', 'Perfecto!', 'Los datos fueron guardados correctamente');
                                $state.go('usuarios');
                            }
                            else
                                notificationService.show('red', 'Ups!', result.message);

                            $scope.isLoading = false;
                        }
                    );
                }

                return false;
            };


    }]);

