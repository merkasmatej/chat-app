//app.js

var routerApp = angular.module('routerApp', ['ui.router']);


routerApp.config(function ($stateProvider, $urlRouterProvider) {

    $urlRouterProvider.otherwise('/home');

    $stateProvider
		.state('home', {
		    url: '/home',
		    templateUrl: './Templates/partial-prva.html',
		    controller: 'homeController'
		});

    $stateProvider
        .state('write', {
            url: '/write/:id',
            templateUrl: './Templates/partial-druga.html',
            controller: 'chatController'
        });

    $stateProvider
    .state('msg', {
        url: '/msg',
        templateUrl: '/Templates/partial-treca.html'
    });

});