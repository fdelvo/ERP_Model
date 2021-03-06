﻿angular.module("ERPModelApp").factory("AngularDeliveryNotesService", AngularDeliveryNotesService);

AngularDeliveryNotesService.$inject = ["$resource"];

function AngularDeliveryNotesService($resource) {
    return $resource("/api/deliverynotes/",
        null,
        {
            SearchDeliveryNote: {
                method: "GET",
                url: "/api/deliverynotes/searchdeliverynote",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetDeliveryNotes: {
                method: "GET",
                url: "/api/deliverynotes/getdeliverynotes",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetDeliveryNote: {
                method: "GET",
                url: "/api/deliverynotes/getdeliverynote",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            GetDeliveryNoteItems: {
                method: "GET",
                url: "/api/deliverynotes/getdeliverynoteitems",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PostDeliveryNote: {
                method: "POST",
                url: "/api/deliverynotes/postdeliverynote",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            PutDeliveryNote: {
                method: "PUT",
                url: "/api/deliverynotes/putdeliverynote",
                isArray: true,
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            },
            DeleteDeliveryNote: {
                method: "DELETE",
                url: "/api/deliverynotes/deletedeliverynote/:id",
                headers: { "Authorization": "Bearer " + localStorage.getItem("tokenKey") }
            }
        });
}