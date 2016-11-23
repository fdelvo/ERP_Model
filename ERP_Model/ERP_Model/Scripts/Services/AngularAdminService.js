angular.module("ERPModelApp").factory("AngularAdminService", AngularAdminService);

AngularAdminService.$inject = ["$resource"];

function AngularAdminService($resource) {
    return $resource("/api/admin/",
        null,
        {
            GetAddresses: {
                method: "GET",
                url: "/api/admin/getaddresses",
                isArray: true,
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
            }
        });
}