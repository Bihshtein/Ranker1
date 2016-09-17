(function () {
    'use strict';

    angular
        .module('app')
        .controller('Hello', Hello);

    function Hello($scope, $http) {
    $http.get('http://localhost:51612/Api/Products/Sec=Citrus').
        then(function (data) {
            $scope.dict = {
                "Citrus": angular.fromJson(data)
            };
        });
    $http.get('http://localhost:51612/Api/Products/').
then(function (data) {
    $scope.dict["All"] = angular.fromJson(data);
});

    $http.get('http://localhost:51612/Api/Products/Sec=Tropic').
      then(function (data) {
          $scope.dict["Tropic"] = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Products/Sec=Tree').
      then(function (data) {
          $scope.dict["Tree"] = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Products/Sec=Bush').
      then(function (data) {
          $scope.dict["Bush"] = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Products/Sec=Nut').
        then(function (data) {
            $scope.dict["Nut"] = angular.fromJson(data);
        });
    $http.get('http://localhost:51612/Api/Products/Sec=Root').
      then(function (data) {
          $scope.dict["Root"] = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Products/Sec=Flower').
      then(function (data) {
          $scope.dict["Flower"] = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Products/Sec=Fruit').
      then(function (data) {
          $scope.dict["SecFruit"] = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Products/Sec=Seed').
       then(function (data) {
           $scope.dict["Seed"] = angular.fromJson(data);
       });
    $http.get('http://localhost:51612/Api/Products/Sec=Leaf').
      then(function (data) {
          $scope.dict["Leaf"] = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Products/Sec=Tubur').
      then(function (data) {
          $scope.dict["Tubur"] = angular.fromJson(data);
      });
    $http.get('http://localhost:51612/Api/Products/Sec=Fungus').
      then(function (data) {
          $scope.dict["Fungus"] = angular.fromJson(data);
      });

    $http.get('http://localhost:51612/Api/Products/Main=Fruit').
     then(function (data) {
         $scope.dict["MainFruit"] = angular.fromJson(data);
     });
    $http.get('http://localhost:51612/Api/Products/Main=Veg').
      then(function (data) {
          $scope.dict["Veg"] = angular.fromJson(data);
      });

}

})();