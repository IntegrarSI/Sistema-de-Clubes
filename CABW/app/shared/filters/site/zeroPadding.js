'use strict';

angular.module('site').filter("zeroPadding", function () {
    return function (input, n) {
        if (input === undefined)
            input = ""
        if (input.length >= n)
            return input

        var zeros = "";
        for (var i = 0; i < n; i++) {
            zeros += "0";
        }
        //"0".repeat(n);

        return (zeros + input).slice(-1 * n);
    };
});