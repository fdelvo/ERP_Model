angular.module("ERPModelApp").controller("AngularStocksController", AngularStocksController);

AngularStocksController.$inject = [
    "$scope", "AngularStocksService", "AngularAdminService", "$timeout"
];

function AngularStocksController($scope, AngularStocksService, AngularAdminService, $timeout) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    var page = 0;
    $scope.pageSize = 20;
    $scope.newStock = new AngularStocksService();
    $scope.newStockTransaction = [];
    $scope.errorMessages = [];
    $scope.addresses = {};

    $scope.StockSearch = function() {
        $scope.stocks = AngularStocksService.SearchStock({
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

    $scope.AddressList = function() {
        $scope.addressesTemp = AngularAdminService.GetAddresses({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                if (Object.keys($scope.addresses).length === 0 && $scope.addresses.constructor === Object ||
                    $scope.addresses.DataObject.length !== $scope.addressesTemp.DataObject.length) {
                    $scope.addresses = $scope.addressesTemp;
                }
                $timeout($scope.AddressList, 1000);
                console.log($scope.addresses);
            });
    };

    $scope.StockList = function() {
        $scope.stocks = AngularStocksService.GetStocks({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.stocks);
            });
    };

    $scope.StockDetails = function(guid) {
        $scope.stock = AngularStocksService.GetStock({
                id: $scope.GetValueAtIndex(5)
            },
            function() {
                console.log($scope.stock);
            });
    };

    $scope.StockItemList = function(guid) {
        $scope.stockItems = AngularStocksService.GetStockItems({
                id: $scope.GetValueAtIndex(5),
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.stockItems);
            });
    };

    $scope.StockTransactionList = function(guid) {
        $scope.stockTransactions = AngularStocksService.GetStockTransactions({
                id: $scope.GetValueAtIndex(5),
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.stockTransactions);
            });
    };

    $scope.CreateStock = function() {
        $scope.newStock.$PostStock(
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

    $scope.RemoveStock = function(guid) {
        AngularStocksService.DeleteStock({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/Stock/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.StockTransaction = function(index) {
        $scope.newStockTransaction[index] = new AngularStocksService($scope.newStockTransaction[index]);
        $scope.newStockTransaction[index].$CreateStockTransaction(
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

    $scope.GetValueAtIndex = function(index) {
        var str = window.location.href;
        console.log(str.split("/")[index]);
        return str.split("/")[index];
    };
}