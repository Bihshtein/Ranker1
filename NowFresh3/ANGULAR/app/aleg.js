(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hello', Hello);

function Hello($scope, $http) {
    $http.get('http://localhost:51612/Api/Students/Apple').
        then(function (data) {
            $scope.greeting = data;
        });
}

})();