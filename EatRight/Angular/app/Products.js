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
        $scope.init = function (query) {
            $http.get('http://localhost:51612/Api/Products/' + query).
                then(function (data) {
                    if ($scope.dict != null)
                        $scope.dict[query] = angular.fromJson(data);
                    else 
                        $scope.dict = {
                            query : angular.fromJson(data),
                        };
                });
        };
}

})();