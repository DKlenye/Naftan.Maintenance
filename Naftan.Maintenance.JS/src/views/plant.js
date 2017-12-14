webix.protoUI({
    name: "view_plant",

    requireCollections: ["department"],
    collection: "plant",
    editColumn: "name",

    $init: function (config) {
        webix.extend(config.rows[1], {
            columns: [
                webix.column("id"),
                {
                    id: 'departmentId',
                    header: ['Цех\Производство', { content: 'selectFilter', options: webix.collection.options("department", "name", true) }],
                    sort: "int",
                    template: webix.templates.collection("department"),
                    editor: "combo",
                    options: webix.collection.options("department"),
                    fillspace: 1
                },
                { id: 'name', header: ['Наименование установки', { content: "textFilter" }], sort: "string", editor: "text", fillspace: 1 },
                webix.column("trash")
            ]
        }, true);
    }

}, webix.ui.ui_editdatatable);