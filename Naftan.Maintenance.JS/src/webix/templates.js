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
    },

    progress: function (norm,fact,type) {

        var getColor = function (value) {
            var hue = (100 - value).toString(10);
            return "hsl(" + hue + ", 90%,44%)";
        }
        
        return function (obj, common, value, config) {
            
            var n = obj[norm],
                f = obj[fact],
                t = obj[type],
                diff = n - f;

            if (f === null) return "";

            var prcn = Math.min(
                Math.round10((f / n) * 100),
                100
            );
            
            return '<div class="progress">' +
                '<header>' + f + '<span>' + diff +'&nbsp;&nbsp;'+t+'</span></header>' +
                '<div class="bar"><div title="'+prcn+'%" class="percent" style="background:' + getColor(prcn) + '; width: '+prcn+'%;">&nbsp;</div></div></div>'
        }
    }

};