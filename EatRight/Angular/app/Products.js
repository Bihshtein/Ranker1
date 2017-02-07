(function () {
    'use strict';

    angular.module('app', []);

})();

(function () {
    'use strict';

    angular
        .module('app')
        .controller('Query', Hello);
   
   
    function Hello($scope, $http) {
        $scope.colours = [];
        for (var i = 0; i < 50*2.5; i++) {
            $scope.colours.push("LightCoral");
        }
        for (var i = 50 * 2.5; i < 75 * 2.5; i++) {
            $scope.colours.push("Khaki");
        }
      
        for (var i = 75 * 2.5; i < 101 * 2.5; i++) {
            $scope.colours.push("LightGreen");
        }
        $scope.initProducts = function (query) {
            $http.get('http://localhost:51612/Api/Products/' + query).
                then(function (data) {
                    if ($scope.dictProducts != null)
                        $scope.dictProducts[query] = angular.fromJson(data);
                    else 
                        $scope.dictProducts = {
                            query : angular.fromJson(data),
                        };
                });
        };

        $scope.initMeals = function (query) {
            $http.get('http://localhost:51612/Api/Meals/' + query).
                then(function (data) {
                    if ($scope.dictMeals != null)
                        $scope.dictMeals[query] = angular.fromJson(data);
                    else
                        $scope.dictMeals = {
                            query: angular.fromJson(data),
                        };
                });
        };


        $scope.initRecipes = function (query) {
            $http.get('http://localhost:51612/Api/Recipes/' + query).
                then(function (data) {
                    if ($scope.dictRecipes != null)
                        $scope.dictRecipes[query] = angular.fromJson(data);
                    else
                        $scope.dictRecipes = {
                            query: angular.fromJson(data),
                        };
                });
        };
}

})();