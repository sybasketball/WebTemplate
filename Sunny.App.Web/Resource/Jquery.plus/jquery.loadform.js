$.fn.populate = function (entity, attrName) {
    if (this.length > 0) {
        for (var i = 0; i < this.length; i++) {
            var tObj = $(this[i]);
            if (attrName == null) {
                attrName = "name";
            }

            var propertyVal = tObj.attr(attrName);
            if (entity[propertyVal] != undefined) {
                var enValue = entity[propertyVal];
                if ("radio" == tObj.attr["type"] || "checkbox" == tObj.attr["type"]) {
                    if (enValue == tObj.val()) {
                        tObj.attr("checked", "checked");
                    }
                }
                else {
                    tObj.val(enValue);
                }
            }
        }
    }
};