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