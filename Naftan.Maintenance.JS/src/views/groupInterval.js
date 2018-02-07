webix.protoUI({

    name: "view_groupinterval",

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
                                /*webix.toExcel(
                                    me.queryView({ view: "ui_datatable" }),
                                    {
                                        ignore: { "trash": true }
                                    }
                                );*/
                                webix.Reporter.exportReport("Intervals");
                            }
                        }
                    ]
                },
                {
                    view: "ui_datatable",
                    css: "center_columns",
                    select: "row",
                    navigation: true,
                    columns: [
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        webix.column("id"),
                        { id: 'parentName', header: ['Наименование родителя', { content: 'textFilter' }], fillspace: 1 },
                        { id: 'groupName', header: ['Наименование групппы', { content: 'textFilter' }], fillspace: 2 },
                        { id: 'o_min', header: [{ text: 'Осмотр', colspan: 3 }, 'мин'], sort:'int', width: 60 },
                        { id: 'o_max', header: ['', 'макс'], sort: 'int', width: 60 },
                        { id: 'o_qt', header: ['', 'кол-во'], sort: 'int', width: 70 },
                        { id: 't_min', header: [{ text: 'Текущий', colspan: 3 }, 'мин'], sort: 'int', width: 60 },
                        { id: 't_max', header: ['', 'макс'], sort: 'int', width: 60 },
                        { id: 't_qt', header: ['', 'кол-во'], sort: 'int', width: 70 },
                        { id: 's_min', header: [{ text: 'Средний', colspan: 3 }, 'мин'], sort: 'int', width: 60 },
                        { id: 's_max', header: ['', 'макс'], sort: 'int', width: 60 },
                        { id: 's_qt', header: ['', 'кол-во'], sort: 'int', width: 70 },
                        { id: 'k_min', header: [{ text: 'Капитальный', colspan: 3 }, 'мин'], sort: 'int', width: 60 },
                        { id: 'k_max', header: ['', 'макс'], sort: 'int', width: 60 },
                        { id: 'k_qt', header: ['', 'кол-во'], sort: 'int', width: 70 }
                    ],
                    onClick: {
                        "fa-edit": webix.bind(function (e, target) {

                            var item = me.queryView({ view: "ui_datatable" }).getItem(target.row);
                            me.callEvent("onCreateView", [
                                'Группа',
                                {
                                    view: "view_objectgroupeditor",
                                    mode: 'update',
                                    itemId: item.id,
                                    segmentId:'intervalEditor'
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
        return webix.collection("groupinterval");
    }

}, webix.ui.layout)