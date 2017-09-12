var routerApp = angular.module('routerApp');

routerApp.controller('homeController', ['$scope', '$http', '$location', function ($scope, $http, $location) {

    $scope.userName = 'Guest';
    $scope.password = 'Guest';

    $scope.newConversation = 'Guest';
    $scope.newConversationMembers = 'test';

    $scope.conversations = [];


    $scope.login = function () {
        var data = {
            userName: $scope.userName,
            password: $scope.password
        };
        $http({
            method: 'POST',
            url: 'api/Login',
            headers: {
                'Content-Type': 'application/json', 
                'Accept': 'application/json' 
            },
            data: data
        }).then(function successCallback(response) {
            window.userId = response.data;
            $scope.getConversations();

        }, function errorCallback(response) {
            alert("Error")
        });
    }
    $scope.createConversation = function () {
        if (!window.userId) return;

        var data = {
            userId: window.userId,
            name: $scope.newConversation,
            members: $scope.newConversationMembers
        };
        $http({
            method: 'POST',
            url: 'api/Conversation',
            data: data
        }).then(function successCallback(response) {
            window.conversationId = response.data;

            $scope.conversations.push({ id: response.data, name: $scope.newConversation });
            $scope.openConversation(response.data);
        }, function errorCallback(response) {
            alert("Error")
        });
    }

    $scope.openConversation = function (id) {
        $location.path('/write/'+id);
    }

    $scope.getConversations = function () {
        if (!window.userId) return;

        $http({
            method: 'GET',
            url: 'api/Conversations/' + window.userId
        }).then(function succesCallback(response) {
            $scope.conversations = $scope.conversations.concat(response.data);
        }, function errorCallback(response) {
            debugger;
        });
    }
}]);