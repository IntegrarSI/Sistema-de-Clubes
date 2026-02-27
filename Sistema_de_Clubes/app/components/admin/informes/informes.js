angular.module('admin')
	.config(['$stateProvider', function ($stateProvider) {
	    $stateProvider
			.state('informes_carnetsNoEntregados', {
			    url: '/informes/carnetsNoEntregados',
			    templateUrl: '/app/components/admin/informes/carnetsNoEntregados.html',
			    controller: 'informes_carnetsNoEntregadosCtrl'
			})
	        .state('informes_jugadoresHabilitados', {
	            url: '/informes/jugadoresHabilitados',
	            templateUrl: '/app/components/admin/informes/jugadoresHabilitados.html',
	            controller: 'informes_jugadoresHabilitadosCtrl'
            })
            .state('informes_pases', {
                url: '/informes/pases',
                templateUrl: '/app/components/admin/informes/pases.html',
                controller: 'informes_pasesCtrl'
            })
            .state('informes_jugadoresCategorias', {
                url: '/informes/cambiosDeCategoria',
                templateUrl: '/app/components/admin/informes/jugadoresCategorias.html',
                controller: 'informes_jugadoresCategoriasCtrl'
            })
	        .state('informes_totalJugadores', {
	            url: '/informes/totalJugadores',
	            templateUrl: '/app/components/admin/informes/totalJugadores.html',
	            controller: 'informes_totalJugadoresCtrl'
	        });

	}]);