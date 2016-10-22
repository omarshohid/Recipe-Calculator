angular.module('myApp').controller('HomeController', function($scope, $window, $http) {
    $scope.getTodayTaskStatus = function() {
        $scope.workList = {};
        $http.get('/Home/getTodayTaskStatus')
            .success(function(data) {
                if (data.success) {
                    $scope.workList = [].concat(data.data);
                }
            }).
            error(function(XMLHttpRequest, textStatus, errorThrown) {
                toastr.error(XMLHttpRequest + ": " + textStatus + ": " + errorThrown, 'Error!!!');
            });
    };
});