webix.protoUI({

    name: "view_usage",
    requireCollections:["department","plant"],

    $init: function (config) {

        var me = this;

        webix.extend(config, {
            rows: [
                {
                    view: "toolbar", elements: [
                        {
                            view: "button", type: "iconButton", icon: "refresh", label: "Обновить", width: 100, click: webix.bind(function () {

                                var table = this.queryView({ view: "ui_datatable" });
                                table.editStop();

                                var store = this._getStore();
                                store.clearAll()
                                store.load(store.config.url);

                            }, this)
                        },
                        {},
                        {
                            view: "button", type: "iconButton", label: "Экспорт", icon: "file-excel-o", width: 100, click: function () {
                                webix.toExcel(
                                    me.queryView({ view: "ui_datatable" }),
                                    {
                                        ignore: { "trash": true }
                                    }
                                );
                            }
                        }
                    ]
                },
                {
                    view: "ui_datatable",
                    css: "center_columns",
                    select: "row",
                    leftSplit: 3,
                    footer:true,
                    navigation: true,
                    columns: [
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        webix.column("id"),
                        {
                            id: 'techIndex', header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 120,
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
                        { id: "o_date", header: [{ text: "Осмотр", colspan: 2 }, "Дата"] },
                        { id: "o_usage", header: ["", "Наработка"] },
                        { id: "t_date", header: [{ text: "Текущий", colspan: 2 }, "Дата"] },
                        { id: "t_usage", header: ["", "Наработка"] },
                        { id: "s_date", header: [{ text: "Средний", colspan: 2 }, "Дата"] },
                        { id: "s_usage", header: ["", "Наработка"] },
                        { id: "k_date", header: [{ text: "Капитальный", colspan: 2 }, "Дата"] },
                        { id: "k_usage", header: ["", "Наработка"] }

                    ],
                    onClick: {
                        "fa-edit": webix.bind(function (e, target) {

                            var item = me.queryView({ view: "ui_datatable" }).getItem(target.row);
                            me.callEvent("onCreateView", [
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

        this.$ready.push(this._initData);

    },

    _initData: function () {

        var me = this;
        var table = this.queryView({ view: "ui_datatable" });
        var store = this._getStore();

        var events = [];

        events.push(store.attachEvent("onBeforeLoad", function () {
            me.disable();
            table.showOverlay("Загрузка..." + "<span class='webix_icon fa-spinner fa-spin'></span>");
        }));

        events.push(store.attachEvent("onAfterLoad", function () {
            me.enable();
            table.hideOverlay();
        }));

        this.attachEvent("onDestruct", function () {
            events.forEach(function (e) { store.detachEvent(e) });
        });

        table.parse(this._getStore());
    },

    _getStore: function () {
        return webix.collection("usage");
    }

}, webix.ui.layout)