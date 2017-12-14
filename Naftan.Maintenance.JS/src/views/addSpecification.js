webix.protoUI({

    name: 'view_addSpecification',

    $init: function (config) {

        var me = this;

        var status = function (value, obj) {
            if (obj.ch) return "row-marked";
            return "";
        }
    

        webix.extend(config, {

            rows: [
                {
                    view: 'toolbar',
                    elements: [
                        {
                            view: "button", type: "iconButton", icon: "plus-circle", label: "Выбрать", width: 100, click: function () {
                                me.callEvent("onSpecificationSelect", [me.getSelections()])
                            }
                        },
                        {
                            view: "button", type: "iconButton", icon: "ban", label: "Отмена", width: 100, click: function () {
                                me.callEvent("onSpecificationSelect",[])
                            }
                        }
                    ]
                },
                {
                    view: "datatable",
                    checkboxRefresh: true,
                    columns: [
                        {
                            id: "ch", header: { content: "masterCheckbox" }, template: "{common.checkbox()}", width: 40, cssFormat: status
                        },
                        { id: 'name', header: ['Наименование', { content: "textFilter" }], sort: "string", fillspace: 2, cssFormat: status },
                        {
                            id: 'type',
                            header: ['Тип характеристики'],
                            template: webix.templates.collection("specificationType"),
                            sort: "string",
                            fillspace: 1,
                            cssFormat: status
                        },
                        {
                            id: 'referenceId',
                            header: ['Наименование справочника'],
                            template: webix.templates.collection("reference"),
                            sort: "string",
                            fillspace: 1,
                            cssFormat: status
                        }
                    ]
                }
            ]

        });

        this.$ready.push(this.initData);

    },

    initData: function () {
        var table = this.queryView({ view: "datatable" });
        table.parse(webix.collection("specification"));
        table.data.unsync();
    },

    getSelections: function () {
        var table = this.queryView({ view: "datatable" });
        var selections = [];

        table.data.each(function (i) { if (i.ch) selections.push(i) });

        return selections;
    },

    filterExists: function (idArray) {
        idArray = idArray || [];
        var table = this.queryView({ view: "datatable" });
        table.data.filter(function (i) {
            return idArray.indexOf(i.id) == -1;
        });
    },

    reset: function () {
        var table = this.queryView({ view: "datatable" });
        table.data.filter(function () { return true });
        table.data.each(function (i) {
            i.ch = 0;
        });
        table.refresh();
    }


},webix.ui.layout)