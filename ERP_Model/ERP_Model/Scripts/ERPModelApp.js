angular.module("ERPModelApp", ["ngResource"]);

angular.module("ERPModelApp").filter("strikeString", function () {
    return function (x) {
        x.strike();
        return x;
    };
});