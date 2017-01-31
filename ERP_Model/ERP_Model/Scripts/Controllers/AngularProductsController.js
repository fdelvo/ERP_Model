angular.module("ERPModelApp").controller("AngularProductsController", AngularProductsController);

AngularProductsController.$inject = [
    "$scope", "AngularProductsService", "AngularStocksService", "$timeout"
];

function AngularProductsController($scope, AngularProductsService, AngularStocksService, $timeout) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.newProduct = new AngularProductsService();
    var page = 0;
    $scope.pageSize = 20;
    $scope.errorMessages = [];
    $scope.stocks = {};

    $scope.ProductSearch = function() {
        $scope.products = AngularProductsService.SearchProduct({
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

    $scope.StockList = function() {
        $scope.stocksTemp = AngularStocksService.GetStocks({
            
            },
            function() {
                if (Object.keys($scope.stocks).length === 0 && $scope.stocks.constructor === Object ||
                    $scope.stocks.DataObject.length !== $scope.stocksTemp.DataObject.length) {
                    $scope.stocks = $scope.stocksTemp;
                }
                $timeout($scope.StockList, 1000);
                console.log($scope.stocks);
            });
    };

    $scope.poll = function(fnc) {
        $timeout(function() {
                $scope.value++;
                poll();
            },
            1000);
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

    $scope.ProductDetails = function(guid) {
        $scope.product = AngularProductsService.GetProduct({ id: $scope.GetValueAtIndex(5) });
    };

    $scope.CreateProduct = function() {
        if ($scope.stock === undefined) {
            $scope.stock = {
                StockGuid: "00000000-0000-0000-0000-000000000000"
            };
        }
        $scope.newProduct.$PostProduct({
                stockGuid: $scope.stock.StockGuid,
                maxQuantity: $scope.maxQuantity,
                minQuantity: $scope.minQuantity
            },
            function(response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
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

    $scope.EditProduct = function() {
        $scope.product.$PutProduct({ id: $scope.product.Product.ProductGuid },
            function(response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
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

    $scope.RemoveProduct = function(guid) {
        AngularProductsService.DeleteProduct({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.GetValueAtIndex = function(index) {
        var str = window.location.href;
        console.log(str.split("/")[index]);
        return str.split("/")[index];
    };
}