(function () {
    if (!Array.prototype.indexOfByProperty) {
        Array.prototype.indexOfByProperty = function (prop, value) {
            var finded = -1;
            for (var index = 0; index < this.length && finded == -1; index++) {
                if (this[index][prop]) {
                    if (this[index][prop] == value) {
                        finded = index;
                    }
                }
            }
            return finded;
        }
    }
})();

(function () {
    if (!Array.prototype.indexOfBySubproperty) {
        Array.prototype.indexOfBySubproperty = function (prop, subprop, value) {
            var finded = -1;
            for (var index = 0; index < this.length && finded == -1; index++) {
                if (this[index][prop][subprop]) {
                    if (this[index][prop][subprop] == value) {
                        finded = index;
                    }
                }
            }
            return finded;
        }
    }
})();