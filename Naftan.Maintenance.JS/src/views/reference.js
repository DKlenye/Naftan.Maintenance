webix.protoUI({

    name: "view_reference",

    $init: function (config) {

        var me = this;

        webix.extend(config, {
            cols: [
                {
                    view: "ui_editdatatable",
                    collection: "reference",
                    editColumn: "name",
                    columns: [
                        webix.column("id"),
                        { id: 'name', header: ['Наименование справочника', { content: "textFilter" }], sort: "string", editor: "text", fillspace: true },
                        webix.column("trash")
                    ]
                },
                { view: "resizer" },
                {
                    rows: [
                        {
                            view: "toolbar",
                            elements: [
                                {
                                    view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить", width: 100, click: webix.bind(function () {

                                        if (this.values.getEditState()) {
                                            this.values.editStop();
                                            return false;
                                        }

                                        var id = this.values.add({ id: 0 }, 0);
                                        this.values.edit({
                                            row: id,
                                            column: 'value'
                                        });

                                    }, this) }
                            ]
                        },
                        {
                            view: "ui_datatable",
                            name: "valuesTable",
                            columns: [
                                webix.column("id"),
                                { id: 'value', header: ['Значение справочника', { content: "textFilter" }], sort: "string", editor: "text", fillspace: 5 },
                                webix.column("trash")
                            ],
                            rules: {
                                "value": "isNotEmpty"
                            },
                            onClick: {
                                "fa-trash-o": function (e, target) {

                                    var table = this;

                                    webix.confirm(
                                        {
                                            title: "Удаление записи",
                                            text: "Вы действительно хотите удалить запись?",
                                            ok: "Да",
                                            cancel: "Нет",
                                            callback: function (isOk) {
                                                if (isOk) table.remove(target.row);
                                            }
                                        });
                                    
                                    return false;
                                }
                            }
                        }
                    ]
                }
            ]
        });

        this.$ready.push(this.initEvents);

    },

    findByName: function (name) {
        return this.queryView({ name: name });
    },

    initEvents: function () {

        var reference = this.queryView({ view: "ui_editdatatable" }).queryView({ view: "ui_datatable" });
        var referenceStore = webix.collection("reference");
        var values = this.findByName('valuesTable');

        webix.extend(this, {
            reference: reference,
            values: values
        });

              
        reference.attachEvent("onSelectChange", function () {
            var item = reference.getSelectedItem();
                        values.clearAll();
            if (item) {
                values.parse(item.values);
            }
        });

        webix.dp(referenceStore).attachEvent("onAfterSave", function () {
            var item = reference.getSelectedItem();
            values.clearAll();
            if (item) {
                values.parse(item.values);
            }
        });


        values.data.attachEvent("onStoreUpdated", function (id, obj, operation) {

            if (!operation || operation=="paint") return;

            if (operation!="delete" && !values.validate(id)) return;

            var item = reference.getSelectedItem();
            var valuesArray = [];

            var pull = values.data.pull;

            for (var pullId in pull) {
                if (values.validate(pullId)) {
                    valuesArray.push(pull[pullId]);
                }
            }

            /*
            values.data.each(function (r) {
                if (values.validate(r.id)) {
                    if(r.id>1e10) delete r.id;
                    valuesArray.push(r);
                }
            });*/

            item.values = valuesArray;
            referenceStore.updateItem(item.id, item);
          
        });
    }

}, webix.ui.layout);