angular.module("ERPModelApp").controller("AngularOrdersController", AngularOrdersController);

AngularOrdersController.$inject = [
    "$scope", "AngularOrdersService", "$rootScope"
];

function AngularOrdersController($scope, AngularOrdersService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.newStock = new AngularOrdersService();

    $scope.OrdersList = function () {
        $scope.orders = AngularOrdersService.GetOrders({
        },
            function () {
                console.log($scope.orders);
            });
    };

    $scope.OrderItemsList = function (guid) {
        $scope.orderItems = AngularOrdersService.GetOrderItems({
            id: guid
        },
            function () {
                console.log($scope.orderItems);
            });
    };

    $scope.CreateStock = function () {
        $scope.newStock.$PostStock(
            function (response) {
                console.log("Success");
                location.href = "/Stock/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.EditStock = function () {
        $scope.stock.$PutStock({ id: $scope.stock.StockGuid },
            function (response) {
                console.log("Success");
                location.href = "/Stock/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.RemoveStock = function (guid) {
        AngularOrdersService.DeleteStock({ id: guid },
            function (response) {
                console.log("Success");
                location.href = "/Stock/Index";
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