angular.module("ERPModelApp").controller("AngularAdminController", AngularAdminController);

AngularAdminController.$inject = [
    "$scope", "AngularAdminService", "$rootScope"
];

function AngularAdminController($scope, AngularAdminService, $rootScope) {
    if (localStorage.getItem("tokenKey") === null) {
        location.href = "/Home/Index";
    }

    var page = 0;
    $scope.pageSize = 20;

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

    $scope.newAddress = new AngularAdminService();
    $scope.newCustomer = new AngularAdminService();
    $scope.newSupplier = new AngularAdminService();
    $scope.model = new AngularAdminService();

    $scope.AddressList = function (pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.addresses = AngularAdminService.GetAddresses({
                page: page,
                pageSize: $scope.pageSize
            },
            function() {
                console.log($scope.addresses);
            });
    };

    $scope.CustomerList = function (pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.customers = AngularAdminService.GetCustomers({
            page: page,
            pageSize: $scope.pageSize
        },
            function () {
                console.log($scope.customers);
            });
    };

    $scope.SupplierList = function (pageSize) {
        if (pageSize) {
            $scope.pageSize = pageSize;
        }
        $scope.suppliers = AngularAdminService.GetSuppliers({
            page: page,
            pageSize: $scope.pageSize
        },
            function () {
                console.log($scope.suppliers);
            });
    };

    $scope.AddressDetails = function(guid) {
        $scope.address = AngularAdminService.GetAddress({ id: guid });
    };

    $scope.CustomerDetails = function (guid) {
        $scope.customer = AngularAdminService.GetCustomer({ id: guid });
    };

    $scope.SupplierDetails = function (guid) {
        $scope.supplier = AngularAdminService.GetSupplier({ id: guid });
    };

    $scope.CreateAddress = function() {
        $scope.newAddress.$PostAddress(
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.CreateCustomer = function () {
        $scope.newCustomer.$PostCustomer(
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.CreateSupplier = function () {
        $scope.newSupplier.$PostSupplier(
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.EditAddress = function() {
        $scope.address.$PutAddress({ id: $scope.address.AddressGuid },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.EditCustomer = function () {
        $scope.customer.$PutCustomer({ id: $scope.customer.CustomerGuid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.EditSupplier = function () {
        $scope.supplier.$PutSupplier({ id: $scope.supplier.SupplierGuid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.RemoveAddress = function(guid) {
        AngularAdminService.DeleteAddress({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.RemoveCustomer = function (guid) {
        AngularAdminService.DeleteCustomer({ id: guid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
            });
    };

    $scope.RemoveSupplier = function (guid) {
        AngularAdminService.DeleteSupplier({ id: guid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (error) {
                console.log("Fail");
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

    $scope.UserDetails = function(guid) {
        $scope.user = AngularAdminService.GetUser({ id: guid });
    };

    $scope.CreateUser = function() {
        $scope.model.$PostUser(
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.EditUser = function() {
        $scope.user.$PutUser({ id: $scope.user.Id },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.ChangeUserPassword = function(guid) {
        $scope.model.$ChangePasswordForUser({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(error) {
                console.log("Fail");
            });
    };

    $scope.RemoveUser = function(guid) {
        AngularAdminService.DeleteUser({ id: guid },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
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