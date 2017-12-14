webix.protoUI({

    name: "view_environment",
    collection: "environment",
    editColumn: "name",

    $init: function (config) {

        webix.extend(config.rows[1], {
            columns: [
                webix.column("id"),
                { id: 'name', header: ['Наименование среды', { content: "textFilter" }], sort: "string", editor: "text", fillspace: true },
                webix.column("trash")
            ]
        }, true);

    }


}, webix.ui.ui_editdatatable);