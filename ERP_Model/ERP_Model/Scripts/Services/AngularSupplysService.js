angular.module("ERPModelApp").factory("AngularSupplysService", AngularSupplysService);

AngularSupplysService.$inject = ["$resource"];

function AngularSupplysService($resource) {
    return $resource("/api/supplys/",
        null,
        {
            GetSupplys: {
                method: "GET",
                url: "/api/supplys/getsupplys",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetSupply: {
                method: "GET",
                url: "/api/supplys/getsupply",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetSupplyItems: {
                method: "GET",
                url: "/api/supplys/getsupplyitems",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostSupply: {
                method: "POST",
                url: "/api/supplys/postsupply",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutSupply: {
                method: "PUT",
                url: "/api/supplys/putsupply",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteSupply: {
                method: "DELETE",
                url: "/api/supplys/deletesupply/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}