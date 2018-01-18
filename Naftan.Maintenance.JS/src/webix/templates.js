webix.templates = {


    //prop

    collection: function (name, property) {

        var collection = webix.collection(name);

        var fn = function (obj, common, value, config) {
            var item = collection.getItem(value);

            if (!item) return "";

            if (typeof property === "function") {
                return property(item);
            }

            return item[property || "name"] || "";
        }

        return fn;
    },

    tree: function (tree, id, property) {

        if (!id) return "";

        var items = [];

        while (id) {
            items.push(tree.getItem(id)[property || "name"])
            id = tree.getParentId(id);
        }

        items.reverse();
        return items.join(' -> ')
    },

    period: function (format) {
        format = format || "%M %Y";
        return function (obj, common, value, config) {
            var parser = webix.Date.strToDate("%Y.%m");
            var _p = (value + '').split('');
            _p = (_p.slice(0, 4).concat(['.']).concat(_p.slice(4, 6))).join('');
            return webix.Date.dateToStr(format)(parser(_p));
        }
    }

};