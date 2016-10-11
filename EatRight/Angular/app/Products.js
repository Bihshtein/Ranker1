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
        $http.get('http://localhost:51612/Api/Products/Protein=1,100,2000').
        then(function (data) {
            $scope.dict = {
                "Protein": angular.fromJson(data),
            };
    });
    $http.get('http://localhost:51612/Api/Products/').
         then(function (data) {
             $scope.dict["All"] = angular.fromJson(data);

         });
    $http.get('http://localhost:51612/Api/Products/Animal=Chicken').
   then(function (data) {
       $scope.dict["Chicken"] = angular.fromJson(data);

   });
    $http.get('http://localhost:51612/Api/Products/Animal=Chicken').
   then(function (data) {
       $scope.dict["Aleg"] = angular.fromJson(data);

   });

}

})();