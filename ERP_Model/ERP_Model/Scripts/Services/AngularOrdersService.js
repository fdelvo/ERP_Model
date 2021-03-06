﻿angular.module("ERPModelApp").factory("AngularOrdersService", AngularOrdersService);

AngularOrdersService.$inject = ["$resource"];

function AngularOrdersService($resource) {
    return $resource("/api/orders/",
        null,
        {
            SearchOrder: {
                method: "GET",
                url: "/api/orders/searchorder",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetOrders: {
                method: "GET",
                url: "/api/orders/getorders",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetOrder: {
                method: "GET",
                url: "/api/orders/getorder",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetOrderItems: {
                method: "GET",
                url: "/api/orders/getorderitems",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostOrder: {
                method: "POST",
                url: "/api/orders/postorder",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutOrder: {
                method: "PUT",
                url: "/api/orders/putorder",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteOrder: {
                method: "DELETE",
                url: "/api/orders/deleteorder/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}