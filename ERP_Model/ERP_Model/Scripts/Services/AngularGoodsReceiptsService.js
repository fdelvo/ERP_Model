angular.module("ERPModelApp").factory("AngularGoodsReceiptsService", AngularGoodsReceiptsService);

AngularGoodsReceiptsService.$inject = ["$resource"];

function AngularGoodsReceiptsService($resource) {
    return $resource("/api/goodsreceipts/",
        null,
        {
            SearchGoodsReceipt: {
                method: "GET",
                url: "/api/goodsreceipts/searchgoodsreceipt",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetGoodsReceipts: {
                method: "GET",
                url: "/api/goodsreceipts/getgoodsreceipts",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetGoodsReceipt: {
                method: "GET",
                url: "/api/goodsreceipts/getgoodsreceipt",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetGoodsReceiptItems: {
                method: "GET",
                url: "/api/goodsreceipts/getgoodsreceiptitems",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostGoodsReceipt: {
                method: "POST",
                url: "/api/goodsreceipts/postgoodsreceipt",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutGoodsReceipt: {
                method: "PUT",
                url: "/api/goodsreceipts/putgoodsreceipt",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteGoodsReceipt: {
                method: "DELETE",
                url: "/api/goodsreceipts/deletegoodsreceipt/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}