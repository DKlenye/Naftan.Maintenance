webix.protoUI({

    name: 'view_operationalreport',
    requireCollections: ["department", "plant", "operatingState", "maintenanceReason","maintenanceType"],

    $init: function (cfg) {

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


        webix.extend(cfg, {
            rows: [
                {
                    view: "toolbar", elements: [
                        { view: "datepicker", align: "right", label: "Период:", labelWidth: 70, type: "month", width: 220, format:'%F %Y' },
                        /*{ view: "button", type: "iconButton", icon: "floppy-o", label: "Сохранить", width: 110 }*/
                    ]   
                },
                {
                    view: "datatable",
                    css: "center_columns",
                    select:'cell',
                    navigation: true,
                    editable: true,
                    columns: [
                        webix.column("id"),
                        {
                            id: 'techIndex',
                            header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 120,
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
                            header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true) }],
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
                    ]
                }
            ]
        });

        this.$ready.push(this.initData);
        this.$ready.push(this.initEvents);

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
    
    initData: function () {
        webix.extend(this,{
            reportTable: this.queryView({ view: "datatable" })
        });

        this.reportTable.parse(this.getCollection());
    },

    initEvents: function () {

        webix.UIManager.addHotKey("enter", function (view) {
            var pos = view.getSelectedId();
            view.edit(pos);
        }, this.reportTable);

        this.reportTable.attachEvent("onBeforeEditStart", webix.bind(this.onEditStart, this));
        this.reportTable.attachEvent("onAfterEditStart", webix.bind(this.onAfterEditStart, this));
        this.reportTable.attachEvent("onBeforeEditStop", webix.bind(this.onEditStop, this));
    },

    onEditStart: function (obj) {

        var item = this.reportTable.getItem(obj.row);

        switch (obj.column) {

            case "usageBeforeMaintenance": {

                break;
            }
            case "usageAfterMaintenance": {

                if (!item.endMaintenance) return false;

                break;
            }
            case "actualMaintenanceType": {

                break;
            }
            case "unplannedReason": {

                if (!item.actualMaintenanceType) return false;

                break;
            }
            case "startMaintenance": {

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

        if (obj.column == 'startMaintenance') {
            var calendar = this.reportTable.getEditor().getPopup().getBody();
            calendar.define("minDate", this.getStart(item.period));
            calendar.define("maxDate", this.getEnd(item.period));
            calendar.refresh();
        }

        if (obj.column == 'endMaintenance') {
            var calendar = this.reportTable.getEditor().getPopup().getBody();
            calendar.define("minDate", item.startMaintenance);
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

        switch (obj.column) {
            case "state": {
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

                break;
            }
        }

        this.reportTable.updateItem(item.id, item);

    },
    
    getCollection: function () {
        return webix.collection('operationalReport');
    }

}, webix.ui.layout);