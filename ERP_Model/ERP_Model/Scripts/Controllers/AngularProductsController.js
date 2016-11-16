angular.module("ERPModelApp").controller("AngularProductsController", AngularProductsController);

AngularProductsController.$inject = [
    "$scope", "AngularProductsService", "$rootScope"
];

function AngularProductsController($scope, AngularProductsService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    $scope.newProduct = new AngularProductsService();

    $scope.ProductList = function() {
        $scope.products = AngularProductsService.GetProducts({
            },
            function() {
                console.log($scope.products);
            });
    };

    $scope.ProductDetails = function(guid) {
        $scope.product = AngularProductsService.GetProduct({id:guid});
    }; 

    $scope.CreateProduct = function() {
        $scope.newProduct.$PostProduct(
            function (response) {
                console.log("Success");
                location.href = "/ProductManagement/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.EditProduct = function() {
        $scope.product.$PutProduct({ id: $scope.product.ProductGuid },
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