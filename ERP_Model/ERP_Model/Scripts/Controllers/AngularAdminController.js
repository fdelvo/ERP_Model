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
    $scope.errorMessages = [];
    $scope.newAddress = new AngularAdminService();
    $scope.newCustomer = new AngularAdminService();
    $scope.newSupplier = new AngularAdminService();
    $scope.model = new AngularAdminService();

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
        $scope.address = AngularAdminService.GetAddress({ id: $scope.GetValueAtIndex(5) });
    };

    $scope.CustomerDetails = function (guid) {
        $scope.customer = AngularAdminService.GetCustomer({ id: $scope.GetValueAtIndex(5) });
    };

    $scope.SupplierDetails = function (guid) {
        $scope.supplier = AngularAdminService.GetSupplier({ id: $scope.GetValueAtIndex(5) });
    };

    $scope.CreateAddress = function() {
        $scope.newAddress.$PostAddress(
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.CreateCustomer = function () {
        $scope.newCustomer.$PostCustomer(
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.CreateSupplier = function () {
        $scope.newSupplier.$PostSupplier(
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.EditAddress = function() {
        $scope.address.$PutAddress({ id: $scope.address.AddressGuid },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.EditCustomer = function () {
        $scope.customer.$PutCustomer({ id: $scope.customer.CustomerGuid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.EditSupplier = function () {
        $scope.supplier.$PutSupplier({ id: $scope.supplier.SupplierGuid },
            function (response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function (response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
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
        $scope.user = AngularAdminService.GetUser({ id: $scope.GetValueAtIndex(5) });
    };

    $scope.CreateUser = function() {
        $scope.model.$PostUser(
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.EditUser = function() {
        $scope.user.$PutUser({ id: $scope.user.Id },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
            });
    };

    $scope.ChangeUserPassword = function(guid) {
        $scope.model.$ChangePasswordForUser({ id: $scope.GetValueAtIndex(5) },
            function(response) {
                console.log("Success");
                location.href = "/Administration/Index";
            },
            function(response) {
                $scope.error = true;
                $scope.errorMessages = [];
                for (var key in response.data.ModelState) {
                    if (response.data.ModelState.hasOwnProperty(key)) {
                        response.data.ModelState[key].forEach(
                            function (element) {
                                $scope.errorMessages.push(element);
                            });
                    }
                };
                if (response.data.Message) {
                    $scope.errorMessages.push(response.data.Message);
                }
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