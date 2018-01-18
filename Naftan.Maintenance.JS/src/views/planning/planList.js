webix.protoUI({

    name: 'view_planList',
    requireCollections:["objectGroup"],


    $init: function (cfg) {
        webix.extend(cfg, {

            rows: [
                {
                    animate: false,
                    cells: [
                        {
                            name: "list",
                            rows: [
                                {
                                    view: "toolbar",
                                    elements: [
                                        {
                                            view: "button", type: "iconButton", icon: "refresh", label: "Обновить", width: 100, click: webix.bind(function () {
                                                this.reload();
                                            }, this)
                                        },
                                        {
                                            view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить", width: 100, click: webix.bind(function () {
                                                this.queryView({ name: "addForm" }).show();
                                            }, this)
                                        }
                                    ]
                                },
                                {
                                    view: "datatable",
                                    columns: [
                                        webix.column("id"),
                                        {
                                            id: 'userId',
                                            header: ['Пользователь', { content: "selectFilter", options: webix.collection.options("user", "name", true) }],
                                            sort: "int",
                                            template: webix.templates.collection("user"),
                                            width: 200
                                        },
                                        {
                                            id: 'period',
                                            header: 'Период',
                                            template: function (obj, common, value, config) {
                                                var parser = webix.Date.strToDate("%Y.%m");
                                                var _p = (value + '').split('');
                                                _p = (_p.slice(0, 4).concat(['.']).concat(_p.slice(4, 6))).join('');
                                                return webix.Date.dateToStr("%F %Y")(parser(_p)); 
                                            },
                                            width: 150
                                        }
                                    ]
                                }

                            ]
                        },
                        {
                            name: "addForm",
                            rows: [
                                {

                                    view: "toolbar", elements: [
                                        {
                                            view: "datepicker", name: "period", align: "right", label: "Период:", labelWidth: 70, type: "month", width: 220, format: '%F %Y',
                                            value: new Date(),

                                        },
                                        { view: "button", type: "iconButton", icon: "share-square-o", label: "Рассчитать график ППР", width: 200, click: webix.bind(this.addPlan, this) },
                                        {
                                            view: "button", type: "iconButton", icon: "ban", label: "Отмена", width: 100, click: webix.bind( function () {
                                                this.queryView({ name: "list" }).show();
                                            },this)
                                        }
                                    ]

                                },
                                {
                                    name: 'objectGroups',
                                    view: "tree",
                                    threeState: true,
                                    template: "{common.icon()} {common.checkbox()} {common.folder()}<span>#name#<span>"
                                }
                            ]
                        }
                    ]
                }
            ]

        });

        this.$ready.push(this.initData)

    },

    initData: function () {

        this.table = this.queryView({ view: 'datatable' });

        this.reload();
        this.queryView({ name: 'objectGroups' }).data.importData(webix.collection("ObjectGroup"), true);
    },

    reload: function () {
        this.table.clearAll();
        this.table.load("json->api/maintenancePlan");
    },

    getPeriod: function () {
        return webix.Date.dateToStr("%Y%m")(this.queryView({ name: "period" }).getValue());
    },

    getGroups: function () {
        return this.queryView({ name:"objectGroups"}).getChecked();
    },

    addPlan: function () {
       
        this.mask();

        var data = {
            period : this.getPeriod(),
            groups : this.getGroups()
        };

        webix.ajax().headers({ "Content-Type": "application/json" })
            .post("/api/maintenancePlan", data)
            .then(webix.bind(this.onAddHandler, this), this.onErrorHandler)

    },

    onAddHandler: function (e) {
        
        this.queryView({ name: "list" }).show();
        this.unmask();

        var data = e.json();
        this.table.parse(data);
    },

    onErrorHandler: function () {
        this.unmask();
    },

    mask: function () {
        this.disable();
    },
    unmask: function () {
        this.enable();
    }


}, webix.ui.layout);