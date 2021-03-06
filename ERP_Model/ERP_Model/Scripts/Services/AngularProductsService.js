﻿angular.module("ERPModelApp").factory("AngularProductsService", AngularProductsService);

AngularProductsService.$inject = ["$resource"];

function AngularProductsService($resource) {
    return $resource("/api/works/",
        null,
        {
            SearchProduct: {
                method: "GET",
                url: "/api/products/searchproduct",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetProducts: {
                method: "GET",
                url: "/api/products/getproducts",
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
                url: "/api/products/deleteproduct",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}