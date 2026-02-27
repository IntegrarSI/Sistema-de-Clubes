'use strict';

angular.module('admin').filter("siNo", function () {
    return function (input, n) {
        if (input == false)
            input = "No";
        else if (input == true)
            input = "Si";
        else
            input = "";

        return input;
    };
});