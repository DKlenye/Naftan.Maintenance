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

        var planTpl = webix.templates.collection("maintenanceType");


        var cssFormat = function (value, obj) {
            if (obj.state == 4) return "row-silver"
            return "";
        }

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
                        {},
                        { css: "red-button", view: "button", type: "iconButton", icon: "ban", label: "Откатить", width: 100, click: webix.bind(this.discardReport, this) },
                        { css: "green-button", view: "button", type: "iconButton", icon: "share-square-o", label: "Провести", width: 100, click: webix.bind(this.applyReport, this) },
                    ]   
                },
                {
                    view: "datatable",
                   // dragColumn: true,
                    css: "center_columns",
                    footer: true,
                    select: 'cell',
                    leftSplit: 3,
                    rightSplit: 1,
                    navigation: true,
                    editable: true,
                    rules: webix.rules.operationalreport,
                    save: {
                        url: 'json->api/operationalReport',
                        updateFromResponse: true
                    },
                    columns: [
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        webix.column("id"),
                        {
                            id: "nextPrcn",
                            header: 'Износ',
                            template: webix.templates.progress("nextUsageNorm","nextUsageNormMax", "nextUsageFact", "nextMaintenance"),
                            sort: sortByProgress,
                            width:150
                        },
                        {
                            id: 'departmentId',
                            header: ['Цех\Производство', { content: "selectFilter", options: webix.collection.options("department", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("department"),
                            width: 140,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'plantId',
                            header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true,null,true) }],
                            sort: "int",
                            template: webix.templates.collection("plant"),
                            width: 180,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'state',
                            header: ['Состояние', { content: "selectFilter", options: webix.collection.options("operatingState", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("operatingState"),
                            editor: "combo",
                            options: webix.collection.options("operatingState", "name", false, function (i) { return [1,3,4].indexOf(i.id) != -1; }),
                            width: 170,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'techIndex',
                            header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 110,
                            footer: { content: "countColumn" },
                            cssFormat: cssFormat
                        },
                        {
                            id: 'usageParent',
                            header: [{ text: "Наработка", colspan: 3, css: { "background": colors.usage } }, { text: "Родитель", css: { "background": colors.usage } }],
                            sort: 'int',
                            parseFormat: intFormatter,
                            width: 80,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'usageBeforeMaintenance',
                            header: ['', { text: "До", css: { "background": colors.usage } }],
                            sort: 'int',
                            editor: "text",
                            editFormat: intFormatter,
                            parseFormat: intFormatter,
                            width: 60,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'usageAfterMaintenance',
                            header: ['', { text: 'После', css: { "background": colors.usage } }],
                            sort: 'int',
                            editor: "text",
                            editFormat: intFormatter,
                            parseFormat: intFormatter,
                            width: 60,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'plannedMaintenanceType',
                            header: [{ text: "Ремонт/обслуживание", colspan: 5, css: { "background": colors.maintenance } }, { text: "План", css: { "background": colors.maintenance } }],
                            sort: "int",
                            template: function (obj, common, value, config) {
                                var out = planTpl.apply(this,arguments);
                                if (obj.isTransfer) {
                                    out = "[П] " + out;
                                }
                                return out;
                            },
                            width: 120,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'actualMaintenanceType',
                            header: ["", { text: "Факт", css: { "background": colors.maintenance } }],
                            editor: 'combo',
                            sort:"int",
                            template: webix.templates.collection("maintenanceType"),
                            options: webix.collection.options("maintenanceType", "name", true),
                            width: 120,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'unplannedReason',
                            header: ["", { text: "Причина", css: { "background": colors.maintenance } }],
                            editor: 'combo',
                            options: webix.collection.options("maintenanceReason","name",true),
                            template: webix.templates.collection("maintenanceReason"),
                            width: 140,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'startMaintenance',
                            header: ["", { text: "Начало", css: { "background": colors.maintenance } }],
                            format: webix.i18n.dateFormatStr,
                            //editFormat: webix.i18n.dateFormatStr,
                            //editParse: webix.i18n.dateFormatStr,
                            editor: 'text',
                            width: 100,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'endMaintenance',
                            header: ["", { text: "Окончание", css: { "background": colors.maintenance } }],
                            format: webix.i18n.dateFormatStr,
                            //editFormat: webix.i18n.dateFormatStr,
                            //editParse: webix.i18n.dateFormatStr,
                            editor: 'text',
                            width: 100,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'offerForPlan',
                            header: [{ text: "Предложение к плану", colspan: 2, css: { "background": colors.offer } }, { text: "Вид", css: { "background": colors.offer } }],
                            editor: 'combo',
                            template: webix.templates.collection("maintenanceType"),
                            options: webix.collection.options("maintenanceType", "name", true),
                            width: 120,
                            cssFormat: cssFormat
                        },
                        {
                            id: 'reasonForOffer',
                            header: ["", { text: "Причина", css: { "background": colors.offer } }],
                            editor: 'combo',
                            options: webix.collection.options("maintenanceReason", "name", true),
                            template: webix.templates.collection("maintenanceReason"),
                            width: 120,
                            cssFormat: cssFormat
                        },
                        { id: "ban", header: "&nbsp;", align: "center", width: 35, css:"red-button", template: "<span  style='cursor:pointer;'  class='webix_icon fa-ban'></span>" },

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
                        }),
                        "fa-ban": webix.bind(function (e, target) {
                            var item = me.reportTable.getItem(target.row);

                            webix.confirm(
                                {
                                    title: "Откат отчёта",
                                    text: "Выбран тех. индекс <b>" + item.techIndex + "</b> для отката.<br/> Вы действительно хотите откатить отчёт?",
                                    ok: "Да",
                                    cancel: "Нет",
                                    type: "confirm-error",
                                    callback: function (isOk) {
                                        if (isOk) {

                                            me.mask("Откат отчёта...")

                                            webix.ajax().headers({ "Content-Type": "application/json" })
                                                .post("/api/operationalReport/discardReports/" + me.getPeriod(), { data: [item.id] })
                                                .then(webix.bind(me.onApplyHandler, me))
                                                .fail(webix.bind(me.onErrorHandler, me))
                                        }
                                    }
                                });

                        })
                    },
                    on: {
                        onKeyPress: function (code, e) {

                            var code = e.code == "NumpadEnter" ? "Enter" : e.code;
                            
                            switch (code) {
                                case "KeyJ": { me.setMaintenance(1); return false; }
                                case "KeyN": { me.setMaintenance(2); return false; }
                                case "KeyC": { me.setMaintenance(3); return false; }
                                case "KeyR": { me.setMaintenance(4); return false; }
                                case "KeyH": { me.setState(3); return false; }
                                case "Enter": {

                                    //Если в режиме edit
                                    if (e.srcElement.tagName == "INPUT") {
                                        var table = me.reportTable;
                                        var columnName = table.getSelectedId().column;

                                        table.editStop();
                                        var current =  table.getSelectedItem();

                                        var setCell = function (name, nextRow) {
                                            
                                            if (nextRow === true) {
                                                table.moveSelection("down");
                                            }
                                            var sel = table.getSelectedId();
                                            var item = table.getSelectedItem();

                                            if (nextRow && item.state == 2) name = "endMaintenance";

                                            table.select(sel.row, name, false);
                                            table.editStop();
                                            table.editCell(sel.row, name, false, true);
                                        }

                                        switch (columnName) {
                                            case "usageBeforeMaintenance": {
                                                webix.delay(setCell, this, ["offerForPlan"]);
                                                break;
                                            }
                                            case "usageAfterMaintenance": {
                                                webix.delay(setCell, this, ["offerForPlan"]);
                                                break;
                                            }
                                            case "startMaintenance": {
                                                webix.delay(setCell, this, ["endMaintenance"]);
                                                break;
                                            }
                                            case "endMaintenance": {

                                                current.endMaintenance ?
                                                    webix.delay(setCell, this, ["usageAfterMaintenance"]) :
                                                    webix.delay(setCell, this, ["offerForPlan"]);
                                                break;
                                            }
                                            case "unplannedReason": {
                                                webix.delay(setCell, this, ["startMaintenance"]);
                                                break;
                                            }
                                            case "offerForPlan": {
                                                current.offerForPlan && current.offerForPlan < 1e10 ?
                                                    webix.delay(setCell, this, ["reasonForOffer"]) :
                                                    webix.delay(setCell, this, ["usageBeforeMaintenance", true])
                                                break;
                                            }
                                            case "reasonForOffer": {
                                                webix.delay(setCell, this, ["usageBeforeMaintenance", true]);
                                                break;
                                            }

                                        }
                                    }
                                }
                            }
                        }
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
       // table.attachEvent("onAfterEditStart", webix.bind(this.onAfterEditStart, this));
        table.attachEvent("onBeforeEditStop", webix.bind(this.onEditStop, this));
       // table.attachEvent("onAfterEditStop", webix.bind(this.onAfterEditStop, this));

        var currentDepartment;

        table.attachEvent("onBeforeFilter", function (id, value, config) {

            if (id == "departmentId" && value != currentDepartment) {
                currentDepartment = value;
                var cfg = this.config.columns.filter(function (x) { return x.id == "plantId" })[0].header[1];

                if (value) {
                    var newOptions = webix.collection.options("plant", "name", true, function (i) {
                        return i.departmentId == value;
                    });
                    cfg.options = newOptions;
                }
                else {
                    cfg.value = "";
                    cfg.options = webix.collection.options("plant", "name", true, null, true)
                }

                this.refreshColumns();

            }

        });

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
            //table.sort("#techIndex#");
            //table.markSorting("techIndex", "asc");
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

    setMaintenance: function (type) {

        var table = this.reportTable;
        var item = table.getSelectedItem();
        var dp = webix.dp(table);

        dp.define({ autoupdate: false });

        item.actualMaintenanceType = type;



        table.updateItem(item.id, item);

        table.editStop();

        var nextCell = item.plannedMaintenanceType ? "startMaintenance":"unplannedReason";

        table.select(item.id, nextCell, false);
        table.editCell(item.id, nextCell, false, true);

        dp.define({ autoupdate: true });
    },

    setState: function (state) {
        var table = this.reportTable;
        var item = table.getSelectedItem();
        var dp = webix.dp(table);

        item.state = state;
        item.usageAfterMaintenance = 0;
        item.usageBeforeMaintenance = 0;
        table.updateItem(item.id, item);

        table.editStop();
        table.moveSelection("down");
        item = table.getSelectedItem();
        
        table.select(item.id, "usageBeforeMaintenance", false);
        table.editCell(item.id, "usageBeforeMaintenance", false, true);

    },

    onEditStart: function (obj) {

        var item = this.reportTable.getItem(obj.row);
        var isRepair = item.state == 2;

        switch (obj.column) {
            case "state": {
                if (isRepair) return false;
            }
            case "usageBeforeMaintenance": {

                if (isRepair) return false;
                break;
            }
            case "usageAfterMaintenance": {

                if (!item.endMaintenance) return false;

                break;
            }
            case "actualMaintenanceType": {

                //if (isRepair) return false;
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
/*
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
    */
    onEditStop: function (values, obj) {
        var me = this;
        var item = this.reportTable.getItem(obj.row);
        var hours = this.getHours(item.period);
        var hoursBefore = this.getHoursBetweenDates(this.getStart(item.period), item.startMaintenance);
        var hoursAfter = this.getHoursBetweenDates(item.endMaintenance, this.getEnd(item.period));
        var value = values.value;
        var old = values.old;
        var parser = webix.Date.strToDate("%d.%m.%Y");
        var isRepair = item.state == 2;

        if (old == value) return;


        /*var resetCalendar = function(){
            var calendar = me.reportTable.getEditor().getPopup().getBody();
            delete calendar.config.minDate;
            delete calendar.config.maxDate;
            calendar.refresh();
        }*/

        var buildDate = function (v) {

            var periodParser = webix.Date.dateToStr("%Y%m");

            v = (v + "").substring(0, 2);
            v = parseInt(v) || 0;
            var date = me.getStart(me.getPeriod());
            var period = periodParser(date)


            if (!v) return null;

             webix.Date.add(date, v-1, "day");

            if (period != periodParser(date)) return null;

            return date;
        }
        
        switch (obj.column) {
            case "state": {
                if (old != value) {
                    item.state = value;
                    if (value != 1) {
                        item.usageAfterMaintenance = 0;
                        item.usageBeforeMaintenance = 0;
                    }
                    else {
                        item.usageBeforeMaintenance = hours;
                        item.usageAfterMaintenance = 0;
                    }
                    break;
                }
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

                //Если вводится наработка, но у нас резерв, то устанавливаем состояние в эксплуатации
                if (item.usageBeforeMaintenance > 0 && item.state==3) {
                    item.state = 1;
                }

                break;
            }
            case "usageAfterMaintenance": {
                item.usageAfterMaintenance = Math.min(value, hoursAfter)
                break;
            }
            case "actualMaintenanceType": {

                if (value > 1e10) {
                    if (!isRepair) {
                        item.startMaintenance = null;
                        item.endMaintenance = null;
                        item.usageBeforeMaintenance = hours;
                        item.usageAfterMaintenance = 0;
                        item.actualMaintenanceType = null;
                        item.unplannedReason = null;
                    }
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

                value = buildDate(value);

                if (!value) {
                    item.usageBeforeMaintenance = hours;
                    item.startMaintenance = null;
                    item.usageAfterMaintenance = 0;
                    item.endMaintenance = null;
                }
                else {
                    hoursBefore = this.getHoursBetweenDates(this.getStart(item.period), value);
                    item.usageBeforeMaintenance = Math.min(hoursBefore, item.usageBeforeMaintenance);
                    item.startMaintenance = value;
                }

               // resetCalendar();

                break;
            }
            case "endMaintenance": {

                value = buildDate(value);

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

               // resetCalendar();

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

    onAfterEditStop: function (values, obj) {

        console.log(arguments);

        var table = this.reportTable;
        var item = table.getSelectedItem();

        switch (obj.column) {
            case "startMaintenance": {
                table.select(item.id, "endMaintenance", false);
                table.editCell(item.id, "endMaintenance", false, true);
            }
        }
    },
    
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
                                .post("/api/operationalReport/applyReports/" + me.getPeriod(), { data: objects })
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

    discardReport: function () {
        var me = this, objects = [];
        this.reportTable.data.each(function (e) { objects.push(e.id) });

        if (objects.length > 0) {

            webix.confirm(
                {
                    title: "Откат отчёта",
                    text: "Выбрано <b>" + objects.length + "</b> единиц для отката.<br/> Вы действительно хотите откатить отчёт?",
                    ok: "Да",
                    cancel: "Нет",
                    type: "confirm-error",
                    callback: function (isOk) {
                        if (isOk) {

                            me.mask("Откат отчёта...")

                            webix.ajax().headers({ "Content-Type": "application/json" })
                                .post("/api/operationalReport/discardReports/" + me.getPeriod(), { data: objects })
                                .then(webix.bind(me.onApplyHandler, me))
                                .fail(webix.bind(me.onErrorHandler, me))
                        }
                    }
                });
        }
    },

    onErrorHandler: function (e) {
        var me = this;
        this.unmask();
        webix.alert({
            title: "Произошла ошибка",
            width: 700,
            text: e.responseText,
            type: "alert-error",
            callback: function (result) {
                me.reload();
            }

        });
        
    }


}, webix.ui.layout);