(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hello', Hello);

function Hello($scope, $http) {
    $http.get('http://localhost:51612/Api/Students/Citrus').
        then(function (data) {
            $scope.Citrus =  angular.fromJson(data);
        });

    $http.get('http://localhost:51612/Api/Students/Tropic').
      then(function (data) {
          $scope.Tropic = angular.fromJson(data);
      });


    $http.get('http://localhost:51612/Api/Students/Tree').
      then(function (data) {
          $scope.Tree = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Students/Bush').
      then(function (data) {
          $scope.Bush = angular.fromJson(data);
      });
}

})();