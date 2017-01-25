angular.module("ERPModelApp").factory("AngularAdminService", AngularAdminService);

AngularAdminService.$inject = ["$resource"];

function AngularAdminService($resource) {
    return $resource("/api/admin/",
        null,
        {
            GetAddresses: {
                method: "GET",
                url: "/api/admin/getaddresses",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetAddress: {
                method: "GET",
                url: "/api/admin/getaddress",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostAddress: {
                method: "POST",
                url: "/api/admin/postaddress",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutAddress: {
                method: "PUT",
                url: "/api/admin/putaddress",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteAddress: {
                method: "DELETE",
                url: "/api/admin/deleteaddress/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetSuppliers: {
                method: "GET",
                url: "/api/admin/getsuppliers",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetSupplier: {
                method: "GET",
                url: "/api/admin/getsupplier",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostSupplier: {
                method: "POST",
                url: "/api/admin/postsupplier",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutSupplier: {
                method: "PUT",
                url: "/api/admin/putsupplier",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteSupplier: {
                method: "DELETE",
                url: "/api/admin/deletesupplier/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetCustomers: {
                method: "GET",
                url: "/api/admin/getcustomers",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetCustomer: {
                method: "GET",
                url: "/api/admin/getcustomer",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostCustomer: {
                method: "POST",
                url: "/api/admin/postcustomer",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutCustomer: {
                method: "PUT",
                url: "/api/admin/putcustomer",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteCustomer: {
                method: "DELETE",
                url: "/api/admin/deletecustomer/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetUsers: {
                method: "GET",
                url: "/api/admin/getusers",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetUser: {
                method: "GET",
                url: "/api/admin/getuser",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostUser: {
                method: "POST",
                url: "/api/admin/postuser",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutUser: {
                method: "PUT",
                url: "/api/admin/putuser",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteUser: {
                method: "DELETE",
                url: "/api/admin/deleteuser/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            ChangePasswordForUser: {
                method: "POST",
                url: "/api/admin/changepasswordforuser",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}