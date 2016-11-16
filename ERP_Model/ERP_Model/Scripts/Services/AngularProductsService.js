angular.module("ERPModelApp").factory("AngularProductsService", AngularProductsService);

AngularProductsService.$inject = ["$resource"];

function AngularProductsService($resource) {
    return $resource("/api/works/",
        null,
        {
            GetProducts: {
                method: "GET",
                url: "/api/products/getproducts",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetProduct: {
                method: "GET",
                url: "/api/products/getproduct",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostProduct: {
                method: "POST",
                url: "/api/products/postproduct",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutProduct: {
                method: "PUT",
                url: "/api/products/putproduct",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteProduct: {
                method: "DELETE",
                url: "/api/products/deleteproduct/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}