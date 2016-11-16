angular.module("ERPModelApp").controller("AngularLoginController", LoginController);

LoginController.$inject = [
    "$scope", "$http", "$rootScope"
];

function LoginController($scope, $http, $rootScope) {
    $scope.model = {};
    $scope.registerStatus = [];

    $scope.Register = function () {
        $http({
            method: "POST",
            url: "/api/Account/Register",
            data: $scope.model,
            headers: { "Content-Type": "application/json" }
        })
            .then(function (response) {
                $rootScope.notification = "NOTIFICATION_REGISTERED";
                location.href = "/Home/Index";
            },
                function (response) {
                    $scope.error = true;
                    $scope.registerStatus = [];
                    for (var key in response.data.ModelState) {
                        if (response.data.ModelState.hasOwnProperty(key)) {
                            response.data.ModelState[key].forEach(
                                function (element) {
                                    $scope.registerStatus.push(element);
                                });
                        }
                    };
                });
    };

    $scope.LogIn = function () {
        $http({
            method: "POST",
            url: "/Token",
            data: "userName=" +
                $scope.loginData.username +
                "&password=" +
                $scope.loginData.password +
                "&grant_type=password"
        })
            .then(function (response) {
                localStorage.setItem("tokenKey", response.data.access_token);
                location.href = "/ProductManagement/Index";
            },
                function (response) {
                    $scope.error = true;
                    console.log(response);
                    $scope.loginStatus = response.data.error_description;
                });
    };
}