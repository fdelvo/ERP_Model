angular.module("ERPModelApp").factory("AngularStocksService", AngularStocksService);

AngularStocksService.$inject = ["$resource"];

function AngularStocksService($resource) {
    return $resource("/api/stocks/",
        null,
        {
            GetStocks: {
                method: "GET",
                url: "/api/stocks/getstocks",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetStock: {
                method: "GET",
                url: "/api/stocks/getstock",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetStockItems: {
                method: "GET",
                url: "/api/stocks/getstockitems",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetStockTransactions: {
                method: "GET",
                url: "/api/stocks/getstocktransactions",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostStock: {
                method: "POST",
                url: "/api/stocks/poststock",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutStock: {
                method: "PUT",
                url: "/api/stocks/putstock",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteStock: {
                method: "DELETE",
                url: "/api/stocks/deletestock/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}