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
    }


};