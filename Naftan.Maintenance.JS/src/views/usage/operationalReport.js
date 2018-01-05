webix.protoUI({

    name: 'view_operationalreport',
    requireCollections: ["department","plant"],

    $init: function (cfg) {

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
                            id: 'usageBeforeMaintenance',
                            header: ["Нар. до ремонта", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'usageAfterMaintenance',
                            header: ["Нар. после ремонта", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'plannedMaintenanceType',
                            header: ["Ремонт по плану", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'actualMaintenanceType',
                            header: ["Ремонт фактический", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'unplannedReason',
                            header: ["Причина внепланового ремонта", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'startMaintenance',
                            header: ["Начало ремонта", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'endMaintenance',
                            header: ["Окончание ремонта", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'offerForPlan',
                            header: ["Предложение к плану", { content: "textFilter" }], sort: 'text', width: 120
                        },
                        {
                            id: 'reasonForOffer',
                            header: ["Причина ремонта", { content: "textFilter" }], sort: 'text', width: 120
                        }
                    ]
                }
            ]
        });

    }

}, webix.ui.layout);