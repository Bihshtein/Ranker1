(function () {
    'use strict';

    angular
        .module('app')
        .controller('Main', Main);

    function Main($scope,$http) {
        $scope.event = {
            img: 'MMMMYeeeeees',
            img2: '/img/Red_Apple.png'
           
        }

    }


})();