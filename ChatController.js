var routerApp = angular.module('routerApp');

routerApp.controller('chatController', ['$scope', '$http', '$stateParams', function ($scope, $http, $stateParams) {

    $scope.password = 'Guest';

    $scope.groupName = 'Guest';
    $scope.message = '';

    $scope.messages = [];

    $scope.chatHub = $.connection.chatHub;
    $scope.chatHub.client.broadcastMessage = function (name, message) {
        var newMessage = name + ':' + message;

        $scope.messages.push(newMessage);
        $scope.$apply();
    };
    $scope.newMessage = function () {
        $scope.chatHub.server.newMessage(window.userId, $stateParams.id, $scope.message);
        $scope.message = '';
    };

    $scope.getConversations = function () {
        if (!window.userId) return;

        $http({
            method: 'GET',
            url: 'api/Messages/' + $stateParams.id
        }).then(function succesCallback(response) {
            response.data.forEach(function (d) {
                $scope.messages.push(d.user + ':' + d.text);
            });

            $.connection.hub.start().done(function () {
                $scope.chatHub.server.connect(window.userId, $stateParams.id);
            });
            
        }, function errorCallback(response) {
            debugger;
        });
    }
    $scope.getConversations();
}]);