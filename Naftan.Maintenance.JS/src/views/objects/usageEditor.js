webix.protoUI({
    name: "view_usageEditor",

    $init: function (config) {

        webix.extend(config, {
            rows: [
                {
                    view: "datatable",
                    columns: [
                        { id: "startUsage", header: "Дата начала", sort:'date',fillspace:2 },
                        { id: "endUsage", header: "Дата окончания", sort: 'date', fillspace: 2 },
                        { id: "usage", header: "Наработка", fillspace: 1 },
                    ]
                }
            ]
        });

    },

    setData: function (data) {
        var table = this.queryView({ view: "datatable" });
        table.clearAll();
        table.parse(data.usage);
        table.refresh();
    }

}, webix.ui.layout);