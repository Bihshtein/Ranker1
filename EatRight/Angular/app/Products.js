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