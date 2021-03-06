﻿angular.module("ERPModelApp").controller("AngularOrdersController", AngularOrdersController);

AngularOrdersController.$inject = [
    "$scope", "AngularOrdersService", "AngularProductsService", "AngularAdminService", "AngularDeliveryNotesService",
    "$rootScope"
];

function AngularOrdersController($scope,
    AngularOrdersService,
    AngularProductsService,
    AngularAdminService,
    AngularDeliveryNotesService,
    $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    var page = 0;
    $scope.pageSize = 20;
    $scope.orderItems = [];
    $scope.deliveryItems = [];
    $scope.orderQuantity = {
        value: 0
    };
    $scope.newOrder = new AngularOrdersService();
    $scope.newDeliveryNote = new AngularDeliveryNotesService();
    $scope.errorMessages = [];

    $scope.OrderSearch = function() {
        $scope.orders = AngularOrdersService.SearchOrder({
                page: page,
                pageSize: $scope.pageSize,
                searchString: $scope.searchString
            },
            function() {
                console.log("Success");
            });
    };

    $scope.DeliveryNoteSearch = function() {
        $scope.deliveryNotes = AngularDeliveryNotesService.SearchDeliveryNote({
                page: page,
                pageSize: $scope.pageSize,
                searchString: $scope.searchString
            },
            function() {
                console.log("Success");
            });
    };

    $scope.Next = function(currentPage, pageAmount, fnc) {
        if (currentPage + 1 === pageAmount) {
            page = currentPage;
        } else {
            page++;
        }

        fnc();
    };

    $scope.CustomerList = function(pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.customers = AngularAdminService.GetCustomers({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.customers);
            });
    };

    $scope.Previous = function(currentPage, pageAmount, fnc) {
        if (currentPage === 0) {
            page = currentPage;
        } else {
            page--;
        }

        fnc();
    };

    $scope.OrdersList = function() {
        $scope.orders = AngularOrdersService.GetOrders({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.orders);
            });
    };

    $scope.DeliveryNoteDetails = function(guid) {
        $scope.deliveryNote = AngularDeliveryNotesService.GetDeliveryNote({
                id: $scope.GetValueAtIndex(5)
            },
            function() {
                console.log($scope.deliveryNote);
            });
    };

    $scope.OrderDetails = function(guid) {
        $scope.order = AngularOrdersService.GetOrder({
                id: $scope.GetValueAtIndex(5)
            },
            function() {
                console.log($scope.order);
            });
    };

    $scope.EditDeliveryNote = function() {
        $scope.deliveryNote.$PutDeliveryNote({
                id: $scope.deliveryNote.DeliveryNote.DeliveryGuid
            },
            function(response) {
                console.log("Success");
                location.href = "/DeliveryNotes/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function(element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.EditOrder = function() {
        $scope.order.$PutOrder({
                id: $scope.order.Order.OrderGuid
            },
            function(response) {
                console.log("Success");
                location.href = "/Orders/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function(element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.DeliveryNotesList = function() {
        $scope.deliveryNotes = AngularDeliveryNotesService.GetDeliveryNotes({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.deliveryNotes);
            });
    };

    $scope.OrderItemsList = function(guid, pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.orderItems = AngularOrdersService.GetOrderItems({
                id: $scope.GetValueAtIndex(5),
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.orderItems);
            });
    };

    $scope.CreateOrder = function() {
        $scope.newOrder.OrderItems = $scope.orderItems;
        //$scope.newOrder.OrderDeliveryDate = new Date($scope.newOrder.OrderDeliveryDate);
        $scope.newOrder.$PostOrder(
            function(response) {
                console.log("Success");
                location.href = "/Orders/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function(element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.AddProductToOrderItems = function(p, index) {
        p.OrderQuantity = $scope.orderQuantity[index].value;
        $scope.orderItems.push(p);
        console.log($scope.orderItems);
    };

    $scope.RemoveProductFromOrderItems = function(index) {
        $scope.orderItems.splice(index, 1);
        console.log($scope.orderItems);
    };

    $scope.CreateDeliveryNote = function() {
        $scope.newDeliveryNote.DeliveryItems = $scope.deliveryItems;
        $scope.newDeliveryNote.$PostDeliveryNote(
            function(response) {
                console.log("Success");
                location.href = "/DeliveryNotes/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function(element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.AddOrderItemToDeliveryItems = function(element) {
        $scope.deliveryItems.push(element);
        console.log($scope.deliveryItems);
    };

    $scope.EditStock = function() {
        $scope.stock.$PutStock({ id: $scope.stock.StockGuid },
            function(response) {
                console.log("Success");
                location.href = "/Stock/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function(element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.RemoveOrder = function(guid) {
        AngularOrdersService.DeleteOrder({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/Orders/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.ProductList = function() {
        $scope.products = AngularProductsService.GetProducts({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.products);
            });
    };

    $scope.UserList = function() {
        $scope.users = AngularAdminService.GetUsers({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.users);
            });
    };

    $scope.GetValueAtIndex = function(index) {
        var str = window.location.href;
        console.log(str.split("/")[index]);
        return str.split("/")[index];
    };
}