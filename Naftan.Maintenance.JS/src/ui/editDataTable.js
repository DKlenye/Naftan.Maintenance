webix.protoUI({

    name: "ui_editdatatable",

    $init: function (config) {


        webix.extend(this, {
            editColumn: config.editColumn || "",
            collection: config.collection || ""
        });

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
                        {
                            view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить", width: 100, click: webix.bind(function () {
                                var table = this.queryView({ view: "ui_datatable" });
                                table.editStop();
                                var store = this._getStore();
                                var id = store.add({}, 0);
                                table.edit({
                                    row: id,
                                    column: this.editColumn
                                });

                            }, this)
                        }
                    ]
                },
                {
                    view: "ui_datatable",
                    select: "row",
                    navigation: true,
                    editable: true,
                    editaction: "dblclick",
                    onClick: {
                        "fa-trash-o": webix.bind(function (e, target) {

                            webix.confirm(
                                {
                                    title: "Удаление записи",
                                    text: "Вы действительно хотите удалить запись?",
                                    ok: "Да",
                                    cancel: "Нет",
                                    callback: function (isOk) {
                                        if(isOk) me._getStore().remove(target.row);
                                    }
                                });
                                
                            return false;
                        })
                    },       
                    /*
                    on: {
                        "onItemClick": function (id) {
                            this.editRow(id);
                        }
                    },
                    */
                    columns:config.columns,
                    rules: webix.rules[this.collection]
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
        return webix.collection(this.collection);
    }

}, webix.ui.layout)