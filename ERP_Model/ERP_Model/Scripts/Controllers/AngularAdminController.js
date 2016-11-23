angular.module("ERPModelApp").controller("AngularAdminController", AngularAdminController);

AngularAdminController.$inject = [
    "$scope", "AngularAdminService", "$rootScope"
];

function AngularAdminController($scope, AngularAdminService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.newAddress = new AngularAdminService();

    $scope.AddressList = function () {
        $scope.addresses = AngularAdminService.GetAddresses({
        },
            function () {
                console.log($scope.addresses);
            });
    };

    $scope.AddressDetails = function (guid) {
        $scope.address = AngularAdminService.GetAddress({ id: guid });
    };

    $scope.CreateAddress = function () {
        $scope.newAddress.$PostAddress(
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.EditAddress = function () {
        $scope.address.$PutAddress({ id: $scope.address.AddressGuid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.RemoveAddress = function (guid) {
        AngularAdminService.DeleteAddress({ id: guid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.GetValueAtIndex = function (index) {
        var str = window.location.href;
        console.log(str.split("/")[index]);
        return str.split("/")[index];
    };
}