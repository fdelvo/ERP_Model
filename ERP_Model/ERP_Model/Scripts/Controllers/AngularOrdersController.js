angular.module("ERPModelApp").controller("AngularOrdersController", AngularOrdersController);

AngularOrdersController.$inject = [
    "$scope", "AngularOrdersService", "AngularProductsService", "AngularAdminService", "$rootScope"
];

function AngularOrdersController($scope, AngularOrdersService, AngularProductsService, AngularAdminService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.orderItems = [];
    $scope.orderQuantity = {
        value: 0
    };
    $scope.newOrder = new AngularOrdersService();

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

    $scope.CreateOrder = function () {
        $scope.newOrder.OrderItems = $scope.orderItems;
        $scope.newOrder.OrderDeliveryDate = new Date($scope.newOrder.OrderDeliveryDate);
        $scope.newOrder.$PostOrder(
            function (response) {
                console.log("Success");
                location.href = "/Orders/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.AddProductToOrderItems = function (p) {
        p.OrderQuantity = $scope.orderQuantity.value;
        $scope.orderItems.push(p);
        console.log($scope.orderItems);
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

    $scope.ProductList = function () {
        $scope.products = AngularProductsService.GetProducts({
        },
            function () {
                console.log($scope.products);
            });
    };

    $scope.UserList = function () {
        $scope.users = AngularAdminService.GetUsers({
        },
            function () {
                console.log($scope.users);
            });
    };

    $scope.GetValueAtIndex = function (index) {
        var str = window.location.href;
        console.log(str.split("/")[index]);
        return str.split("/")[index];
    };
}