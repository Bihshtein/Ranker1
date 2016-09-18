﻿(function () {
    'use strict';

    angular.module('app', []);

})();

(function () {
    'use strict';

    angular
        .module('app')
        .controller('Query', Hello);

    function Hello($scope, $http) {
        $http.get('http://localhost:51612/Api/Products/Protein=2').
        then(function (data) {
            $scope.dict = {
                "Protein": angular.fromJson(data),
                "Protein2": angular.fromJson(data),
            };
    });
        $http.get('http://localhost:51612/Api/Products/').
         then(function (data) {
             $scope.dict["All"] = angular.fromJson(data);

         });

}

})();