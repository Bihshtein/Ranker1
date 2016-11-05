(function () {
    'use strict';

    angular.module('app', []);

})();


(function () {
    'use strict';

    angular
        .module('app')
        .controller('Query', Hello);


    function Hello2($scope) {
        $scope.aleg = "aleg";
    }
    function Hello($rootScope, $scope, $http) {
        $rootScope.dict = {
            "last": {}
        };
        $scope.init = function (query, min, products) {
            $http.get('http://localhost:51612/Api/Products/' + query + '=' + min + ',' + products).
           then(function (data) {
               $rootScope.dict['last']= angular.fromJson(data);
           });
 

        };

        $http.get('http://localhost:51612/Api/Products/').
        then(function (data) {
            $rootScope.dict.last = angular.fromJson(data);
        });

        $http.get('http://localhost:51612/Api/Values/').
      then(function (data) {
          $scope.availableOptions = angular.fromJson(data);
          $scope.availableOptions.selected = $scope.availableOptions.data[0].id;
      });

        window.scope = $scope;

}

})();