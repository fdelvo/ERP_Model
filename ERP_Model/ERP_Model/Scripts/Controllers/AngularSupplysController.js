angular.module("ERPModelApp").controller("AngularSupplysController", AngularSupplysController);

AngularSupplysController.$inject = [
    "$scope", "AngularSupplysService", "AngularProductsService", "AngularAdminService", "AngularGoodsReceiptsService",
    "$rootScope"
];

function AngularSupplysController($scope,
    AngularSupplysService,
    AngularProductsService,
    AngularAdminService,
    AngularGoodsReceiptsService,
    $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    var page = 0;
    $scope.pageSize = 20;
    $scope.supplyItems = [];
    $scope.goodsReceiptItems = [];
    $scope.supplyQuantity = {
        value: 0
    };
    $scope.newSupply = new AngularSupplysService();
    $scope.newGoodsReceipt = new AngularGoodsReceiptsService();
    $scope.errorMessages = [];

    $scope.SupplySearch = function() {
        $scope.supplys = AngularSupplysService.SearchSupply({
                page: page,
                pageSize: $scope.pageSize,
                searchString: $scope.searchString
            },
            function() {
                console.log("Success");
            });
    };

    $scope.GoodsReceiptSearch = function() {
        $scope.goodsReceipts = AngularGoodsReceiptsService.SearchGoodsReceipt({
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

    $scope.Previous = function(currentPage, pageAmount, fnc) {
        if (currentPage === 0) {
            page = currentPage;
        } else {
            page--;
        }

        fnc();
    };

    $scope.SupplierList = function(pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.suppliers = AngularAdminService.GetSuppliers({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.suppliers);
            });
    };

    $scope.SupplyList = function() {
        $scope.supplys = AngularSupplysService.GetSupplys({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.supplys);
            });
    };

    $scope.GoodsReceiptDetails = function(guid) {
        $scope.goodsReceipt = AngularGoodsReceiptsService.GetGoodsReceipt({
                id: $scope.GetValueAtIndex(5)
            },
            function() {
                console.log($scope.goodsReceipt);
            });
    };

    $scope.SupplyDetails = function(guid) {
        $scope.supply = AngularSupplysService.GetSupply({
                id: $scope.GetValueAtIndex(5)
            },
            function() {
                console.log($scope.supply);
            });
    };

    $scope.EditGoodsReceipt = function() {
        $scope.goodsReceipt.$PutGoodsReceipt({
                id: $scope.goodsReceipt.GoodsReceipt.GoodsReceiptGuid
            },
            function(response) {
                console.log("Success");
                location.href = "/GoodsReceipts/Index";
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

    $scope.EditSupply = function() {
        $scope.supply.$PutSupply({
                id: $scope.supply.Supply.SupplyGuid
            },
            function(response) {
                console.log("Success");
                location.href = "/Supplys/Index";
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

    $scope.GoodsReceiptList = function() {
        $scope.goodsReceipts = AngularGoodsReceiptsService.GetGoodsReceipts({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.goodsReceipts);
            });
    };

    $scope.SupplyItemsList = function(guid, pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.supplyItems = AngularSupplysService.GetSupplyItems({
                id: $scope.GetValueAtIndex(5),
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.supplyItems);
            });
    };

    $scope.CreateSupply = function() {
        $scope.newSupply.SupplyItems = $scope.supplyItems;
        //$scope.newSupply.SupplyDeliveryDate = new Date($scope.newSupply.SupplyDeliveryDate);
        $scope.newSupply.$PostSupply(
            function(response) {
                console.log("Success");
                location.href = "/Supplys/Index";
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

    $scope.AddProductToSupplyItems = function(p, index) {
        p.SupplyQuantity = $scope.supplyQuantity[index].value;
        $scope.supplyItems.push(p);
        console.log($scope.supplyItems);
    };

    $scope.RemoveProductFromSupplyItems = function(index) {
        $scope.supplyItems.splice(index, 1);
        console.log($scope.supplyItems);
    };

    $scope.CreateGoodsReceipt = function() {
        $scope.newGoodsReceipt.GoodsReceiptItems = $scope.goodsReceiptItems;
        $scope.newGoodsReceipt.$PostGoodsReceipt(
            function(response) {
                console.log("Success");
                location.href = "/GoodsReceipts/Index";
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

    $scope.AddSupplyItemToGoodsReceiptItems = function(element) {
        $scope.goodsReceiptItems.push(element);
        console.log($scope.goodsReceiptItems);
    };

    $scope.RemoveSupply = function(guid) {
        AngularSupplysService.DeleteSupply({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/Supplys/Index";
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