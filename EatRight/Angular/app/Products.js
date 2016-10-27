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
        $scope.init = function (query, min, products) {
            $http.get('http://localhost:51612/Api/Products/' + query + '=' + min + ',' + products).
                then(function (data) {
                    $scope.dict[query] = angular.fromJson(data);
                });
        };

        $scope.data = {
            
            availableOptions: [
              { id: 'Vegeterian', name: 'Vegeterian' },
              { id: 'Normal', name: 'Normal' },
              { id: 'BodyBuilder', name: 'Body Builder' },
              { id: 'VitaminFreak', name: 'Vitamin Freak' }
            ],
            selectedOption: { id: 'Normal', name: 'Normal' }
        };
     

        $http.get('http://localhost:51612/Api/Products/').
        then(function (data) {
            $scope.dict = {
                "All": angular.fromJson(data),
            };
        });

}

})();