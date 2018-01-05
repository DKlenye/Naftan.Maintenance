webix.protoUI({

    name: "view_maintenanceReason",
    collection: 'maintenanceReason',
    editColumn: 'name',

    $init: function (config) {

        webix.extend(config.rows[1], {
            columns: [
                webix.column("id"),
                { id: 'name', header: ['Наименование', { content: "textFilter" }], sort: "string", editor: "text", fillspace: true },
                { id: 'designation', header: ['Обозначение', { content: "textFilter" }], sort: "string", editor: "text", fillspace: true },
                webix.column("trash")
            ]
        }, true);
    }

},webix.ui.ui_editdatatable)