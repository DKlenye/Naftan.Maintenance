webix.column = function (name,cfg) {
    var column = webix.copy(webix.columns[name]);
    webix.extend(column, cfg || {});
    return column;
};

webix.columns = {
    id: { id: 'id', header: ["Код", { content: "numberFilter" }], template: function (o, c, value) { return (value < 1e10) ? value : "" }, width: 60, sort: "int" },
    trash : { id: "trash", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-trash-o'></span>" }
};

webix.ui.datafilter.countColumn = webix.extend({
    refresh: function (master, node, value) {
        var result = 0;
        master.mapCells(null, value.columnId, null, 1, function (value) {
            result++;
            return value;
        });

        node.firstChild.innerHTML = "Кол-во = "+result;
    }
}, webix.ui.datafilter.summColumn);