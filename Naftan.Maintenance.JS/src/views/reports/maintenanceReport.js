webix.protoUI({

    name: "report_maintenancereport",
    requireCollections: ["ObjectGroup","department","plant"],

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
                                            label: "Вид :",
                                            labelWidth: 50,
                                            view: "richselect",
                                            name: "reportType",
                                            width: 350,
                                            value: "Word",
                                            options: [
                                                { id: "1", value: "" },
                                                { id: "2", value: "насосно-компрессорного оборудования" },
                                                { id: "3", value: "нефте-химического оборудования" },
                                                { id: "4", value: "вентиляционного оборудования" }
                                            ],
                                            on: {
                                                onChange: function (val) {
                                                    var groups = me.queryView({ name: "groups" });
                                                    groups.uncheckAll();
                                                    groups.open(1,false);

                                                    var items = {
                                                        "1": [],
                                                        "2": [4, 5],
                                                        "3":[],
                                                        "4": [6,7,8]
                                                    };

                                                    var i = 9;
                                                    while (i < 24) items["3"].push(i++);

                                                    items[val].forEach(function (i) { groups.checkItem(i) });

                                                }
                                            }
                                        },
                                        {
                                            view: "button", type: "iconButton", icon: "file-text-o", label: "Открыть", width: 100, click: webix.bind(this.print, this)
                                        },
                                        { width: 30 },
                                        {
                                            view: "richselect",
                                            name: "format",
                                            width: 120,
                                            value: "Word",
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

    getHeader: function () {
        var items = {
            "1": "",
            "2": "насосно-компрессорного оборудования",
            "3": "нефте-химического оборудования",
            "4": "вентиляционного оборудования"
        };
        return items[this.queryView({ name: "reportType" }).getValue()] || " ";
    },


    getStart: function (period) {
        var parser = webix.Date.strToDate("%Y.%m");
        var _p = (period + '').split('');
        _p = (_p.slice(0, 4).concat(['.']).concat(_p.slice(4, 6))).join('');
        return parser(_p);
    },

    getEnd: function (period) {
        var end = this.getStart(period);
        webix.Date.add(end, 1, "month");
        webix.Date.add(end, -1, 'minute');
        return end;
    },

    print: function () {

        var params = {
            start: webix.i18n.dateFormatStr(this.getStart(this.getPeriod())),
            end: webix.i18n.dateFormatStr(this.getEnd(this.getPeriod())),
            groups: this.getGroups(),
            header: this.getHeader()
        };

        this.queryView({ view: 'iframe' }).load(webix.Reporter.getUrl("MaintenanceReport", params));
    },

    export: function () {
        var params = {
            start: webix.i18n.dateFormatStr(this.getStart(this.getPeriod())),
            end: webix.i18n.dateFormatStr(this.getEnd(this.getPeriod())),
            groups: this.getGroups(),
            header: this.getHeader()
        };
        var format = this.queryView({ name: "format" }).getValue();

        webix.Reporter.exportReport("MaintenanceReport", params, format);
    }

}, webix.ui.layout);