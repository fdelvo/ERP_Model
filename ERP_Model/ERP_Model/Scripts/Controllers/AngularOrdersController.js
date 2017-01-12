angular.module("ERPModelApp").controller("AngularOrdersController", AngularOrdersController);

AngularOrdersController.$inject = [
    "$scope", "AngularOrdersService", "AngularProductsService", "AngularAdminService", "AngularDeliveryNotesService", "$rootScope"
];

function AngularOrdersController($scope, AngularOrdersService, AngularProductsService, AngularAdminService, AngularDeliveryNotesService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.orderItems = [];
    $scope.deliveryItems = [];
    $scope.orderQuantity = {
        value: 0
    };
    $scope.newOrder = new AngularOrdersService();
    $scope.newDeliveryNote = new AngularDeliveryNotesService();

    $scope.OrdersList = function () {
        $scope.orders = AngularOrdersService.GetOrders({
        },
            function () {
                console.log($scope.orders);
            });
    };

    $scope.DeliveryNotesList = function () {
        $scope.deliveryNotes = AngularDeliveryNotesService.GetDeliveryNotes({
        },
            function () {
                console.log($scope.deliveryNotes);
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

    $scope.DeliveryItemsList = function (guid) {
        $scope.deliveryItems = AngularDeliveryNotesService.GetDeliveryItems({
            id: guid
        },
            function () {
                console.log($scope.deliveryItems);
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

    $scope.CreateDeliveryNote = function () {
        $scope.newDeliveryNote.DeliveryItems = $scope.deliveryItems;
        $scope.newDeliveryNote.$PostDeliveryNote(
            function (response) {
                console.log("Success");
                location.href = "/DeliveryNotes/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.AddOrderItemToDeliveryItems = function (element) {
        $scope.deliveryItems.push(element);
        console.log($scope.deliveryItems);
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