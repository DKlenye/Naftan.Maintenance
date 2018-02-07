webix.protoUI({
    name: "view_childrenEditor",

    $init: function (config) {

        var me = this;
        var parent = config.parent;

        webix.extend(config, {
            rows: [
                {
                    view: "datatable",
                    columns: [
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        webix.column("id"),
                        {
                            id:
                            'techIndex', header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 120,
                            footer: { content: "countColumn" }
                        },
                        {
                            id: 'departmentId',
                            header: ['Цех\Производство', { content: "selectFilter", options: webix.collection.options("department", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("department"),
                            width: 150
                        },
                        {
                            id: 'plantId',
                            header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true, null, true) }],
                            sort: "int",
                            template: webix.templates.collection("plant"),
                            width: 200
                        },
                        {
                            id: 'period', header: ["Период", { content: "numberFilter" }], sort: 'int', width: 100,
                            template: webix.templates.period()
                        },
                        {
                            id: 'currentOperatingState',
                            header: ['Состояние', { content: "selectFilter", options: webix.collection.options("operatingState", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("operatingState"),
                            width: 160
                        }
                    ],
                    onClick: {
                        "fa-edit": webix.bind(function (e, target) {

                            var item = me.queryView({ view: "datatable" }).getItem(target.row);
                            parent.callEvent("onCreateView", [
                                'Оборудование',
                                {
                                    view: "view_objecteditor",
                                    mode: 'update',
                                    itemId: item.id
                                }
                            ]);

                            return false;
                        })
                    }
                }
            ]
        });
    },

    setData: function (data) {

        var children = data.children;
        var collection = webix.collection("object");

        var objects = children.map(function (i) {
            return collection.getItem(i);
        });

        if (data.parentId) {
            objects.push(collection.getItem(data.parentId));
        }
                
        var table = this.queryView({ view: "datatable" });
        table.clearAll();
        table.parse(objects);
        table.refresh();

    }

}, webix.ui.layout);