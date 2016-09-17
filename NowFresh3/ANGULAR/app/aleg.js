(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hello', Hello);

    function Hello($scope, $http) {
    $http.get('http://localhost:51612/Api/Products/').
        then(function (data) {
            $scope.dict = {
                "Hem": angular.fromJson(data)
            };
        });
    $http.get('http://localhost:51612/Api/Products/').
        then(function (data) {
            $scope.dict["All"] = angular.fromJson(data);
        
    });

    $http.get('http://localhost:51612/Api/Products/').
      then(function (data) {
          $scope.dict["Balls"] = angular.fromJson(data);
      });
}

})();