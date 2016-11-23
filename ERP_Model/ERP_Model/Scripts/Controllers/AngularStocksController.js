angular.module("ERPModelApp").controller("AngularStocksController", AngularStocksController);

AngularStocksController.$inject = [
    "$scope", "AngularStocksService", "AngularAdminService", "$rootScope"
];

function AngularStocksController($scope, AngularStocksService, AngularAdminService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.AddressList = function () {
        $scope.addresses = AngularAdminService.GetAddresses({
        },
            function () {
                console.log($scope.addresses);
            });
    };

    $scope.newStock = new AngularStocksService();

    $scope.StockList = function() {
        $scope.stocks = AngularStocksService.GetStocks({
            },
            function() {
                console.log($scope.stocks);
            });
    };

    $scope.StockDetails = function (guid) {
        $scope.stock = AngularStocksService.GetStock({id: guid
        },
            function () {
                console.log($scope.stock);
            });
    };

    $scope.StockItemList = function (guid) {
        $scope.stockItems = AngularStocksService.GetStockItems({id: guid
        },
            function () {
                console.log($scope.stockItems);
            });
    };

    $scope.StockTransactionList = function (guid) {
        $scope.stockTransactions = AngularStocksService.GetStockTransactions({
            id: guid
        },
            function () {
                console.log($scope.stockTransactions);
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
        AngularStocksService.DeleteStock({ id: guid },
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