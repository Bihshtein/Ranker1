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
    function Hello($scope, $http) {
        $scope.init = function (query) {
            $http.get('http://localhost:51612/Api/Products/' + query).
                then(function (data) {
                     $scope.dict[query] = angular.fromJson(data);
            });
        };

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
    

}

})();