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
                            header: 'Группы оборудования',
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


    print: function () {

        var params = {
            period: this.getPeriod(),
            groups: this.getGroups()
        };

        this.queryView({ view: 'iframe' }).load(webix.Reporter.getUrl("OperationalReport", params));
    },

    export: function () {
        var params = {
            period: this.getPeriod(),
            groups: this.getGroups()
        };
        var format = this.queryView({ name: "format" }).getValue();

        webix.Reporter.exportReport("OperationalReport", params, format);
    }

}, webix.ui.layout);