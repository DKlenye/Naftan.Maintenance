webix.protoUI({

    name: 'view_operationalreport',
    requireCollections: ["department", "plant", "operatingState", "maintenanceReason","maintenanceType"],

    $init: function (cfg) {

        var me = this;

        var intFormatter = function (value) {
            value = (value + "").replace(",", ".");
            var number = webix.Number.format(value, { decimalDelimiter: ".", decimalSize: 0 });
            return number == "NaN" ? null : number;
        };

        var colors = {
            usage: '#bbcbf1',
            maintenance: '#c8d0a7',
            offer: '#f6e2ad'
        };

        var getPrcn = function (obj) {
            var n = obj["nextUsageNorm"],
                f = obj["nextUsageFact"];
            var prcn = Math.round10((f / n) * 100);
            return Math.min(prcn, 100)
        }

        var sortByProgress =function(a, b) {
            a = getPrcn(a);
            b = getPrcn(b);
            return a > b ? 1 : (a < b ? -1 : 0);
        };


        webix.extend(cfg, {
            rows: [
                {
                    view: "toolbar", elements: [
                        { view: "button", type: "iconButton", icon: "refresh", label: "Обновить", width: 100, click: webix.bind(this.reload, this)},
                        {
                            view: "datepicker", name:"period", align: "right", label: "Период:", labelWidth: 70, type: "month", width: 220, format: '%F %Y',
                            value: new Date(),
                            on: {
                                onChange: webix.bind(this.reload, this)
                            }
                        },
                        //{ view: 'combo', options: webix.collection.options('department', "name", true), width: 200 },
                       // { view: 'combo', options: webix.collection.options('plant', "name", true), width: 200 },
                        {},
                        { view: "button", type: "iconButton", icon: "share-square-o", label: "Провести", width: 100, click: webix.bind(this.applyReport, this) },
                        /*{ view: "button", type: "iconButton", icon: "floppy-o", label: "Сохранить", width: 110 }*/
                    ]   
                },
                {
                    view: "datatable",
                    css: "center_columns",
                    footer: true,
                    select: 'cell',
                    leftSplit: 3,
                    navigation: true,
                    editable: true,
                    rules: webix.rules.operationalreport,
                    save: {
                        url: 'json->api/operationalReport',
                        updateFromResponse: true
                    },
                    columns: [
                        //webix.column("id"),
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        {
                            id: 'techIndex',
                            header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 120,
                            footer: { content: "countColumn" }
                        },
                        {
                            id: "nextPrcn",
                            header: 'Состояние',
                            template: webix.templates.progress("nextUsageNorm", "nextUsageFact", "nextMaintenance"),
                            sort: sortByProgress,
                            width:150
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
                            header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true,null,true) }],
                            sort: "int",
                            template: webix.templates.collection("plant"),
                            width: 200
                        },
                        {
                            id: 'state',
                            header: ['Состояние', { content: "selectFilter", options: webix.collection.options("operatingState", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("operatingState"),
                            editor: "combo",
                            options: webix.collection.options("operatingState"),
                            width: 160
                        },
                        {
                            id: 'usageBeforeMaintenance',
                            header: [{ text: "Наработка", colspan: 2, css: { "background": colors.usage } }, { text: "До", css: { "background": colors.usage } }],
                            sort: 'int',
                            editor: "text",
                            editFormat: intFormatter,
                            parseFormat: intFormatter,
                            width: 60
                        },
                        {
                            id: 'usageAfterMaintenance',
                            header: ['', { text: 'После', css: { "background": colors.usage } }],
                            sort: 'int',
                            editor: "text",
                            editFormat: intFormatter,
                            parseFormat: intFormatter,
                            width: 60
                        },
                        {
                            id: 'plannedMaintenanceType',
                            header: [{ text: "Ремонт/обслуживание", colspan: 5, css: { "background": colors.maintenance } }, { text: "План", css: { "background": colors.maintenance } }],
                            template: webix.templates.collection("maintenanceType"),
                            width: 120
                        },
                        {
                            id: 'actualMaintenanceType',
                            header: ["", { text: "Факт", css: { "background": colors.maintenance } }],
                            editor: 'combo',
                            template: webix.templates.collection("maintenanceType"),
                            options: webix.collection.options("maintenanceType", "name", true),
                            width: 120
                        },
                        {
                            id: 'unplannedReason',
                            header: ["", { text: "Причина", css: { "background": colors.maintenance } }],
                            editor: 'combo',
                            options: webix.collection.options("maintenanceReason"),
                            template: webix.templates.collection("maintenanceReason"),
                            width: 140
                        },
                        {
                            id: 'startMaintenance',
                            header: ["", { text: "Начало", css: { "background": colors.maintenance } }],
                            editFormat: webix.i18n.dateFormatStr,
                            editParse: webix.i18n.dateFormatStr,
                            editor: 'date',
                            width: 100
                        },
                        {
                            id: 'endMaintenance',
                            header: ["", { text: "Окончание", css: { "background": colors.maintenance } }],
                            editFormat: webix.i18n.dateFormatStr,
                            editParse: webix.i18n.dateFormatStr,
                            editor: 'date',
                            width: 100
                        },
                        {
                            id: 'offerForPlan',
                            header: [{ text: "Предложение к плану", colspan: 2, css: { "background": colors.offer } }, { text: "Вид", css: { "background": colors.offer } }],
                            editor: 'combo',
                            template: webix.templates.collection("maintenanceType"),
                            options: webix.collection.options("maintenanceType", "name", true),
                            width: 120
                        },
                        {
                            id: 'reasonForOffer',
                            header: ["", { text: "Причина", css: { "background": colors.offer } }],
                            editor: 'combo',
                            options: webix.collection.options("maintenanceReason"),
                            template: webix.templates.collection("maintenanceReason"),
                            width: 120
                        }
                    ],
                    onClick: {
                        "fa-edit": webix.bind(function (e, target) {

                            var item = me.reportTable.getItem(target.row);
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

        this.$ready.push(this.initData);
        this.$ready.push(this.initEvents);

    },

    initData: function () {
        webix.extend(this, {
            reportTable: this.queryView({ view: "datatable" })
        });
        
       // this.reportTable.parse(this.getCollection());
    },

    
    initEvents: function () {

        var me = this;
        var table = me.reportTable;


        webix.UIManager.addHotKey("enter", function (view) {
            var pos = view.getSelectedId();
            view.edit(pos);
        }, this.reportTable);

        table.attachEvent("onBeforeLoad", function () {
            me.mask("Загрузка...");
        });

        table.attachEvent("onAfterLoad", function () {
            me.unmask();
        });


        table.attachEvent("onBeforeEditStart", webix.bind(this.onEditStart, this));
        table.attachEvent("onAfterEditStart", webix.bind(this.onAfterEditStart, this));
        table.attachEvent("onBeforeEditStop", webix.bind(this.onEditStop, this));

        this.reload();
    },

    mask: function (text) {
        this.disable();
        this.reportTable.showOverlay(text + "<span class='webix_icon fa-spinner fa-spin'></span>");
    },

    unmask: function () {
        this.enable();
        this.reportTable.hideOverlay();
    },


    reload: function () {
        var table = this.reportTable;
        table.clearAll();
        table.load("json->api/operationalReport/" + this.getPeriod(), function () {
            table.filterByAll();
            table.sort("#techIndex#");
            table.markSorting("techIndex", "asc");
        });
    },

    getPeriod: function () {
        return webix.Date.dateToStr("%Y%m")(this.queryView({ name: "period" }).getValue());
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

    //Подсчёт часов за период
    getHours: function (period) {
        return this.getHoursBetweenDates(this.getStart(period), this.getEnd(period));
    },

    getHoursBetweenDates:function(date1, date2) {
        var parser = webix.Date.strToDate("%d.%m.%Y");
        return Math.round((parser(date2) - parser(date1)) / (1000 * 60 * 60 * 24)) * 24;
    },

    onEditStart: function (obj) {

        var item = this.reportTable.getItem(obj.row);
        var isRepair = item.state == 2;

        switch (obj.column) {

            case "usageBeforeMaintenance": {

                if (isRepair) return false;
                break;
            }
            case "usageAfterMaintenance": {

                if (!item.endMaintenance) return false;

                break;
            }
            case "actualMaintenanceType": {

                if (isRepair) return false;
                break;
            }
            case "unplannedReason": {

                if (isRepair) return false;
                if (!item.actualMaintenanceType) return false;

                break;
            }
            case "startMaintenance": {

                if (isRepair) return false;
                if (!item.actualMaintenanceType) return false;
                break;
            }
            case "endMaintenance": {

                if (!item.startMaintenance) return false;

                break;
            }
            case "offerForPlan": {

                break;
            }
            case "reasonForOffer": {

                if (!item.offerForPlan) return false;

                break;
            }
        }

    },

    onAfterEditStart: function (obj) {

        var item = this.reportTable.getItem(obj.row);
        var calendar;

        if (obj.column == 'startMaintenance') {
            calendar = this.reportTable.getEditor().getPopup().getBody();
            calendar.define("minDate", this.getStart(item.period));
            calendar.define("maxDate", this.getEnd(item.period));
            calendar.refresh();
        }

        if (obj.column == 'endMaintenance') {
            calendar = this.reportTable.getEditor().getPopup().getBody();

            if (item.state == 2) calendar.define("minDate", this.getStart(item.period));
            else calendar.define("minDate", item.startMaintenance);

            calendar.define("maxDate", this.getEnd(item.period));
            calendar.refresh();
        }

    },

    onEditStop: function (values, obj) {

        var item = this.reportTable.getItem(obj.row);
        var hours = this.getHours(item.period);
        var hoursBefore = this.getHoursBetweenDates(this.getStart(item.period), item.startMaintenance);
        var hoursAfter = this.getHoursBetweenDates(item.endMaintenance, this.getEnd(item.period));
        var value = values.value;
        var old = values.old;
        var parser = webix.Date.strToDate("%d.%m.%Y");

        switch (obj.column) {
            case "state": {
                item.state = value;
                break;
            }
            case "usageBeforeMaintenance": {

                if (item.startMaintenance) {
                    item.usageBeforeMaintenance = Math.min(value, hoursBefore)
                }
                //нет ремонтов
                else {
                    item.usageAfterMaintenance = 0;
                    item.usageBeforeMaintenance = Math.min(value, hours)
                }
                
                break;
            }
            case "usageAfterMaintenance": {
                item.usageAfterMaintenance = Math.min(value, hoursAfter)
                break;
            }
            case "actualMaintenanceType": {

                if (value > 1e10) {
                    item.startMaintenance = null;
                    item.endMaintenance = null;
                    item.usageBeforeMaintenance = hours;
                    item.usageAfterMaintenance = 0;
                    item.actualMaintenanceType = null;
                    item.unplannedReason = null;
                }
                else {
                    item.actualMaintenanceType = value;
                }
                break;
            }
            case "unplannedReason": {
                item.unplannedReason = value;
                break;
            }
            case "startMaintenance": {

                if (!value) {
                    item.usageBeforeMaintenance = hours;
                    item.startMaintenance = null;
                    item.usageAfterMaintenance = 0;
                    item.endMaintenance = null;
                }
                else {
                    hoursBefore = this.getHoursBetweenDates(this.getStart(item.period), value);
                    item.usageBeforeMaintenance = hoursBefore;
                    item.startMaintenance = value;
                }
                
                break;
            }
            case "endMaintenance": {

                if (!value) {
                    item.usageAfterMaintenance = 0;
                    item.endMaintenance = null;
                }
                else {
                    if (parser(item.startMaintenance) > parser(value)) {
                        value = parser(item.startMaintenance);
                    }
                    
                    hoursAfter = this.getHoursBetweenDates(value, this.getEnd(item.period));
                    item.usageAfterMaintenance = hoursAfter;
                    item.endMaintenance = value;
                }
                
                break;
            }
            case "offerForPlan": {

                if (value > 1e10 ) {
                    item.offerForPlan = null;
                    item.reasonForOffer = null;
                }
                else {
                    item.offerForPlan = value;
                }
                break;
            }
            case "reasonForOffer": {
                item.reasonForOffer = value;
                break;
            }
        }

        this.reportTable.updateItem(item.id, item);
    },
    
   /* getCollection: function () {
        return webix.collection('operationalReport');
    },*/

    applyReport: function () {

        var me = this, objects = [];
        this.reportTable.data.each(function (e) { objects.push(e.id) });

        if (objects.length > 0) {

            webix.confirm(
                {
                    title: "Проведение отчёта",
                    text: "Выбрано <b>" + objects.length +"</b> единиц для проведения.<br/> Вы действительно хотите провести отчёт?",
                    ok: "Да",
                    cancel: "Нет",
                    type: "confirm-warning",
                    callback: function (isOk) {
                        if (isOk) {

                            me.mask("Проведение отчёта...")

                            webix.ajax().headers({ "Content-Type": "application/json" })
                                .post("/api/operationalReport/applyReports", { data: objects })
                                .then(webix.bind(me.onApplyHandler, me))
                                .fail(webix.bind(me.onErrorHandler, me))
                        }
                    }
                });
        }
       
    },

    onApplyHandler: function (e) {
        this.unmask();
        var objects = e.json();
        var dp = webix.dp(this.reportTable);
        dp.off();
        this.reportTable.remove(objects);
        dp.on();
        
    },

    onErrorHandler: function (e) {
        this.unmask();
        webix.alert({
            title: "Произошла ошибка",
            width: 700,
            text: e.responseText,
            type: "alert-error"
        });
    }


}, webix.ui.layout);