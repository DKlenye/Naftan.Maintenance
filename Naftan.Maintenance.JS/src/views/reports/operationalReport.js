webix.protoUI({

    name: "report_operationalreport",
    requireCollections: ["ObjectGroup"],

    $init: function (cfg) {
        var me = this;

        webix.extend(cfg, {
            rows: [
                {
                    cols: [
                        {
                            header: 'Условия выборки данных',
                            body: {
                                rows: [
                                    {
                                        view: "property",
                                        width: 500, nameWidth: 200,
                                        elements: [
                                            { id: "departmentId", label: "Цех\Производство", type: "combo", options: webix.collection.options("department", "name", true) },
                                            { id: "plantId", label: "Установка", type: "combo", options: webix.collection.options("plant", "name", true, null, true) }
                                        ],
                                        height: 80,
                                        on: {
                                            onBeforeEditStart: function (id) {
                                                var prop = me.queryView({ view: "property" });
                                                var values = prop.getValues();

                                                if (id == "plantId") {
                                                    var newOptions = webix.collection.options("plant", "name", true, function (i) {
                                                        return i.departmentId == values.departmentId;
                                                    });
                                                    var collection = prop.config.elements.filter(function (i) { return i.id == "plantId" })[0].collection;
                                                    collection.clearAll();
                                                    collection.parse(newOptions);
                                                }
                                            },
                                            onAfterEditStop: function (state, editor, ignoreUpdate) {

                                                if (editor.id == "departmentId") {
                                                    if (state.value != state.old) {
                                                        var prop = me.queryView({ view: "property" });
                                                        var values = prop.getValues();
                                                        values.plantId = null;
                                                        prop.setValues(values);
                                                    }
                                                }

                                            }
                                        }
                                    },
                                    {
                                        view: "toolbar",
                                        elements: [
                                            {
                                                view: "icon", icon: "minus-square-o",
                                                click: webix.bind(function () { me.queryView({ name: "groups" }).closeAll() }, this)
                                            },
                                            {
                                                view: "icon", icon: "plus-square-o",
                                                click: webix.bind(function () { me.queryView({ name: "groups" }).openAll() }, this)
                                            },
                                            {
                                                view: "search",
                                                placeholder: "Поиск...",
                                                on: {
                                                    onKeyPress: function (code, e) {
                                                        //Esc
                                                        if (code === 27) {
                                                            this.$setValue('');
                                                        }
                                                        me.queryView({ name: "groups" }).filter("#name#", this.getValue());
                                                    }
                                                }
                                            }
                                        ]
                                    },
                                    {
                                        name: 'groups',
                                        view: "tree",
                                        threeState: true,
                                        template: "{common.icon()} {common.checkbox()} {common.folder()}<span>#name#<span>",
                                        select: true,
                                        width: 400
                                    }
                                ]
                            }
                        },
                        { view: "resizer" },
                        {
                            rows: [
                                {
                                    view: "toolbar", elements: [
                                        {
                                            view: "datepicker", name: "period", align: "right", label: "Период:", labelWidth: 70, type: "month", width: 220, format: '%F %Y',
                                            value: new Date()
                                        },
                                        {
                                            view: "button", type: "iconButton", icon: "file-text-o", label: "Открыть", width: 100, click: webix.bind(this.print, this)
                                        },
                                        { width: 30 },
                                        {
                                            view: "richselect",
                                            name: "format",
                                            width: 120,
                                            value: "Pdf",
                                            options: [
                                                { id: "Excel" },
                                                { id: "Word" },
                                                { id: "Pdf" }
                                            ]
                                        },
                                        {
                                            view: "button", type: "iconButton", icon: "download", label: "Экспорт", width: 100, click: webix.bind(this.export, this)
                                        }
                                    ]
                                },
                                {
                                    view: "iframe"
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
        var groups = this.queryView({ name: "groups" });
        groups.data.importData(webix.collection("ObjectGroup"), true);
        groups.uncheckAll();
    },

    getPeriod: function () {
        return webix.Date.dateToStr("%Y%m")(this.queryView({ name: "period" }).getValue());
    },

    getGroups: function () {
        return this.queryView({ name: "groups" }).getChecked();
    },

    getPlants: function () {

        var values = this.queryView({ view: "property" }).getValues();

        if (values.plantId && values.plantId < 1e10) return values.plantId + "";

        if (values.departmentId && values.departmentId < 1e10) {
            return webix.collection.options("plant", "name", false, function (i) {
                return i.departmentId == values.departmentId;
            }).map(function (i) { return i.id }).join(",");
        }

        return " ";

    },

    print: function () {

        var params = {
            period: this.getPeriod(),
            groups: this.getGroups(),
            plants: this.getPlants()
        };

        this.queryView({ view: 'iframe' }).load(webix.Reporter.getUrl("OperationalReport", params));
    },

    export: function () {
        var params = {
            period: this.getPeriod(),
            groups: this.getGroups(),
            plants: this.getPlants()
        };
        var format = this.queryView({ name: "format" }).getValue();

        webix.Reporter.exportReport("OperationalReport", params, format);
    }

}, webix.ui.layout);