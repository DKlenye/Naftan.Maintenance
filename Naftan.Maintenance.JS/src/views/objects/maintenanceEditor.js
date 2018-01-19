webix.protoUI({
    name: "view_maintenanceEditor",

    $init: function (config) {

        webix.extend(config, {
            rows: [
                {
                    view: "datatable",
                    columns: [
                        { id: "maintenanceTypeId", header: "Дата начала", sort: 'date', fillspace: 1, template: webix.templates.collection("maintenanceType") },
                        { id: "startMaintenance", header: "Дата начала", sort:'date',fillspace:1 },
                        { id: "endMaintenance", header: "Дата окончания", sort: 'date', fillspace: 1 },
                        { id: "unplannedReasonId", header: "Причина внепланового ремонта", fillspace: 1, template: webix.templates.collection("maintenanceReason") },
                    ]
                }
            ]
        });

    },

    setData: function (data) {
        var table = this.queryView({ view: "datatable" });
        table.clearAll();
        table.parse(data.maintenance);
        table.refresh();
    }

}, webix.ui.layout);