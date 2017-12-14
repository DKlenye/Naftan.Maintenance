webix.protoUI({

    name: "view_specification",

    collection: 'specification',
    requireCollections: ["specificationType","reference"],
    editColumn: 'name',

    $init: function (config) {

        webix.extend(config.rows[1], {
            columns: [
                webix.column("id"),
                { id: 'name', header: ['Наименование', { content: "textFilter" }], sort: "string", editor: "text", fillspace: 2 },
                {
                    id: 'type',
                    header: ['Тип характеристики', { content: "selectFilter", options: webix.collection.options("specificationType","name",true)}],
                    template: webix.templates.collection("specificationType"),
                    sort: "int",
                    editor: "combo",
                    options: webix.collection.options("specificationType"),
                    fillspace: 1
                },
                {
                    id: 'referenceId',
                    header: ['Наименование справочника'],
                    template: webix.templates.collection("reference"),
                    sort: "int",
                    editor: "combo",
                    options: webix.collection.options("reference"),
                    fillspace: 1
                },
                webix.column("trash")
            ]
        }, true);

        this.$ready.push(this.initEvents)

    },

    initEvents: function () {
        var table = this.queryView({ view: "ui_datatable" });

        table.attachEvent("onBeforeEditStop", function (state, editor) {
            if (editor.column == "type" && state.value !="6") {
                var item = table.getItem(editor.row);
                item.referenceId = "";
            }
        });

        table.attachEvent("onBeforeEditStart", function (o) {
            var item = table.getItem(o.row);
            if (o.column == "referenceId" && item.type != "6") return false;
        });

    }

}, webix.ui.ui_editdatatable);