webix.protoUI({
    name: "view_stateEditor",

    $init: function (config) {

        webix.extend(config, {
            rows: [
                {
                    view: "datatable",
                    columns: [
                        { id: "startDate", header: "Дата", sort: 'date', fillspace: 1 },
                        { id: "state", header: "Состояние", fillspace:2, template: webix.templates.collection('operatingState') }
                    ]
                }
            ]
        });

    },

    setData: function (data) {
        var table = this.queryView({ view: "datatable" });
        table.clearAll();
        table.parse(data.states);
        table.refresh();
    }

}, webix.ui.layout);
