angular.module("ERPModelApp").controller("AngularProductsController", AngularProductsController);

AngularProductsController.$inject = [
    "$scope", "AngularProductsService", "AngularStocksService", "$rootScope"
];

function AngularProductsController($scope, AngularProductsService, AngularStocksService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.newProduct = new AngularProductsService();

    var page = 0;
    $scope.pageSize = 20;  

    $scope.Next = function(currentPage, pageAmount, fnc) {
        if (currentPage+1 === pageAmount) {
            page = currentPage;
        } else {
            page++;
        }

        fnc();
    };

    $scope.Previous = function (currentPage, pageAmount, fnc) {
        if (currentPage === 0) {
            page = currentPage;
        } else {
            page--;
        }

        fnc();
    };

    $scope.StockList = function () {
        $scope.stocks = AngularStocksService.GetStocks({
        },
            function () {
                console.log($scope.stocks);
            });
    };

    $scope.ProductList = function() {
        $scope.products = AngularProductsService.GetProducts({
            page: page, pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.products);
            });
    };

    $scope.ProductDetails = function(guid) {
        $scope.product = AngularProductsService.GetProduct({id:guid});
    }; 

    $scope.CreateProduct = function() {
        $scope.newProduct.$PostProduct({stockGuid: $scope.stock.StockGuid, maxQuantity: $scope.maxQuantity, minQuantity: $scope.minQuantity},
            function (response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.EditProduct = function() {
        $scope.product.$PutProduct({ id: $scope.product.ProductGuid, stockGuid: $scope.stock.StockGuid },
            function (response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.RemoveProduct = function(guid) {
        AngularProductsService.DeleteProduct({id:guid},
            function (response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
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