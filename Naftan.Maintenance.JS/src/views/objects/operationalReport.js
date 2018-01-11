﻿webix.protoUI({

    name: 'view_operationalreport',
    requireCollections: ["department", "plant", "operatingState", "maintenanceReason","maintenanceType"],

    $init: function (cfg) {

        var intFormatter = function (value) {
            value = (value + "").replace(",", ".");
            var number = webix.Number.format(value, { decimalDelimiter: ".", decimalSize: 0 });
            return number == "NaN" ? null : number;
        }

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
                    select: "row",
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
                            header: [{ text: "Наработка", colspan: 2, css: { "background": "#bbcbf1" } }, { text: "До", css: { "background": "#bbcbf1" } }],
                            sort: 'int',
                            editor: "text",
                            editFormat: intFormatter,
                            parseFormat: intFormatter,
                            width: 70
                        },
                        {
                            id: 'usageAfterMaintenance',
                            header: ['', { text: 'После', css: { "background": "#bbcbf1" } }],
                            sort: 'int',
                            editor: "text",
                            editFormat: intFormatter,
                            parseFormat: intFormatter,
                            width: 70
                        },
                        {
                            id: 'plannedMaintenanceType',
                            header: [{ text: "Ремонт/обслуживание", colspan: 5, css: { "background": "#c8d0a7" } }, { text: "План", css: { "background": "#c8d0a7" } }],
                            template: webix.templates.collection("maintenanceType"),
                            width: 120
                        },
                        {
                            id: 'actualMaintenanceType',
                            header: ["", { text: "Факт", css: { "background": "#c8d0a7" } }],
                            editor: 'combo',
                            template: webix.templates.collection("maintenanceType"),
                            options: webix.collection.options("maintenanceType"),
                            width: 120
                        },
                        {
                            id: 'unplannedReason',
                            header: ["", { text: "Причина", css: { "background": "#c8d0a7" } }],
                            editor: 'combo',
                            options: webix.collection.options("maintenanceReason"),
                            template: webix.templates.collection("maintenanceReason"),
                            width: 140
                        },
                        {
                            id: 'startMaintenance',
                            header: ["", { text: "Начало", css: { "background": "#c8d0a7" } }],
                            editFormat: webix.i18n.dateFormatStr,
                            editParse: webix.i18n.dateFormatStr,
                            editor:'date',
                            width: 100
                        },
                        {
                            id: 'endMaintenance',
                            header: ["", { text: "Окончание", css: { "background": "#c8d0a7" } }],
                            editFormat : webix.i18n.dateFormatStr,
                            editParse : webix.i18n.dateFormatStr,
                            editor:'date',
                            width: 100
                        },
                        {
                            id: 'offerForPlan',
                            header: [{ text: "Предложение к плану", colspan: 2, css: { "background": "#f6e2ad" } }, { text: "Вид", css: { "background": "#f6e2ad" }}],
                            editor: 'combo',
                            template: webix.templates.collection("maintenanceType"),
                            options: webix.collection.options("maintenanceType"),
                            width: 120
                        },
                        {
                            id: 'reasonForOffer',
                            header: ["", { text: "Причина", css: { "background": "#f6e2ad" } }],
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

    initData: function () {
        webix.extend(this,{
            reportTable: this.queryView({ view: "datatable" })
        });

        this.reportTable.parse(this.getCollection());
    },

    initEvents: function () {
        this.reportTable.attachEvent("onBeforeEditStart", webix.bind(this.onEditStart, this));
        this.reportTable.attachEvent("onBeforeEditStop", webix.bind(this.onEditStop, this));
    },

    onEditStart: function (obj) {

        switch (obj.column) {

            case "usageBeforeMaintenance": {

                break;
            }
            case "usageAfterMaintenance": {

                break;
            }
            case "actualMaintenanceType": {

                break;
            }
            case "unplannedReason": {

                break;
            }
            case "startMaintenance": {

                break;
            }
            case "endMaintenance": {

                break;
            }
            case "offerForPlan": {

                break;
            }
            case "reasonForOffer": {

                break;
            }
        }


    },

    onEditStop: function (values, obj) {

        switch (obj.column) {
            case "state": {

                break;
            }
            case "usageBeforeMaintenance": {

                break;
            }
            case "usageAfterMaintenance": {

                break;
            }
            case "actualMaintenanceType": {

                break;
            }
            case "unplannedReason": {

                break;
            }
            case "startMaintenance": {

                break;
            }
            case "endMaintenance": {

                break;
            }
            case "offerForPlan": {

                break;
            }
            case "reasonForOffer": {

                break;
            }
        }

    },
    
    getCollection: function () {
        return webix.collection('operationalReport');
    }

}, webix.ui.layout);