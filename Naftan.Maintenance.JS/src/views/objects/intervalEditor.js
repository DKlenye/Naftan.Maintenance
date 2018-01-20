webix.protoUI({
    name: "view_intervalEditor",

    $init: function (config) {

        var me = this;
        var parent = config.parent;

        webix.extend(config, {
            rows: [
                {
                    view: "datatable",
                    editable: true,
                    css: "center_columns",
                    columns: [
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        {
                            id: 'maintenanceTypeId',
                            template: webix.templates.collection("maintenanceType"),
                            header: [{ text: 'Вид обслуживания', rowspan: 2 }, '', { content: "textFilter" }],
                            fillspace: 2
                        },
                        {
                            id: 'measureUnitId',
                            template: webix.templates.collection("measureUnit"),
                            header: [{ text: 'Норма наработки', colspan: 3 }, 'Ед. изм.', { content: "textFilter" }],
                            fillspace: 1
                        },
                        {
                            id: 'minUsage',
                            header: ['', 'Минимальная', { content: "textFilter" }], fillspace: 1
                        },
                        {
                            id: 'maxUsage',
                            header: ['', 'Максимальная', { content: "textFilter" }], fillspace: 1
                        },
                        {
                            id: 'timePeriod',
                            header: [{ text: 'Интервал между обслуживанием', colspan: 2 }, 'Временной интервал', { content: "textFilter" }], fillspace: 1,
                            template: webix.templates.collection("timePeriod")
                        },
                        {
                            id: 'periodQuantity',
                            header: ['', 'Количество', { content: "textFilter" }],  fillspace: 1
                        },
                        {
                            id: 'quantityInCycle',
                            header: [{ text: 'Количество в структуре МРЦ', rowspan: 2 }, '', { content: "textFilter" }], fillspace: 1
                        }
                    ],
                    onClick: {
                        "fa-edit": webix.bind(function (e, target) {

                            var item = me.queryView({ view: "datatable" }).getItem(target.row);
                            parent.callEvent("onCreateView", [
                                'Группа',
                                {
                                    view: "view_objectgroupeditor",
                                    mode: 'update',
                                    itemId: item.groupId,
                                    segmentId: 'intervalEditor'
                                }
                            ]);

                            return false;
                        })
                    }
                }
            ]
        });

    },

    setData: function (data) {
        var table = this.queryView({ view: "datatable" });
        table.clearAll();
        table.parse(data.intervals);
        table.refresh();
    }

}, webix.ui.layout);