(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hello', Hello);

function Hello($scope, $http) {
    $http.get('http://localhost:51612/Api/Students/Fruit').
        then(function (data) {
            $scope.fruit =  angular.fromJson(data);
        });

    $http.get('http://localhost:51612/Api/Students/Veg').
      then(function (data) {
          $scope.veg = angular.fromJson(data);
      });
}

})();