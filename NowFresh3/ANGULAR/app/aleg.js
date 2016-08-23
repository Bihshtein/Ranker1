(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hello', Hello);

    function Hello($scope, $http) {
    $http.get('http://localhost:51612/Api/Students/Sec=Citrus').
        then(function (data) {
            $scope.dict = {
                "Citrus": angular.fromJson(data)
            };
        });
    $http.get('http://localhost:51612/Api/Students/Sec=Tropic').
      then(function (data) {
          $scope.Tropic = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Students/Sec=Tree').
      then(function (data) {
          $scope.Tree = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Students/Sec=Bush').
      then(function (data) {
          $scope.Bush = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Students/Sec=Nut').
        then(function (data) {
            $scope.Nut = angular.fromJson(data);
        });
    $http.get('http://localhost:51612/Api/Students/Sec=Root').
      then(function (data) {
          $scope.Root = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Students/Sec=Flower').
      then(function (data) {
          $scope.Flower = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Students/Sec=Fruit').
      then(function (data) {
          $scope.SecFruit = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Students/Sec=Seed').
       then(function (data) {
           $scope.Seed = angular.fromJson(data);
       });
    $http.get('http://localhost:51612/Api/Students/Sec=Leaf').
      then(function (data) {
          $scope.Leaf = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Students/Sec=Tubur').
      then(function (data) {
          $scope.Tubur = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Students/Sec=Fungus').
      then(function (data) {
          $scope.Fungus = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Students/Main=Fruit').
     then(function (data) {
         $scope.MainFruit = angular.fromJson(data);
     });
    $http.get('http://localhost:51612/Api/Students/Main=Veg').
      then(function (data) {
          $scope.Veg = angular.fromJson(data);
      });

}

})();