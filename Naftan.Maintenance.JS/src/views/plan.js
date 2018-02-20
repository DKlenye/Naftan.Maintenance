webix.protoUI({

    name: "view_plan",

    requireCollections: ["plant", "department", "maintenanceType","maintenanceReason","objectGroup"],

    $init: function (cfg) {

        var me = this;

        var getPrcn = function (obj) {
            var n = obj["nextUsageNorm"],
                f = obj["nextUsageFact"];
            var prcn = Math.round10((f / n) * 100);
            return Math.min(prcn, 100)
        }

        var sortByProgress = function (a, b) {
            a = getPrcn(a);
            b = getPrcn(b);
            return a > b ? 1 : (a < b ? -1 : 0);
        };

        var groups = webix.collection('objectgroup');
        var groupOptions = groups.find(function (e) { return e.id != 1 && (e.$parent == 0 || e.$parent == 1) })
            .map(function (e) { return { id: e.id, value: e.name } });

        groupOptions.unshift({ id: 0, value: '' });

        webix.extend(cfg, {
            rows: [
                {
                    view: "toolbar", elements: [
                        { view: "button", type: "iconButton", icon: "refresh", label: "Обновить", width: 100, click: webix.bind(this.reload, this) },
                        {
                            view: "datepicker", name: "period", align: "right", label: "Период:", labelWidth: 70, type: "month", width: 220, format: '%F %Y',
                            value: new Date(),
                            on: {
                                onChange: webix.bind(this.reload, this)
                            }
                        }
                    ]
                },
                {
                    view: 'datatable',
                    save:"json->api/maintenancePlan/",
                    columns: [
                        { id: "object", header: "&nbsp;", align: "center", width: 35, template: "<span  style='cursor:pointer;'  class='webix_icon fa-edit'></span>" },
                        { id: 'objectId', header: ["Код", { content: "numberFilter" }], width: 60, sort: "int" },
                        {
                            id: "nextPrcn",
                            header: 'Износ',
                            template: webix.templates.progress("nextUsageNorm", "nextUsageNormMax", "nextUsageFact", "nextMaintenance"),
                            sort: sortByProgress,
                            width: 150
                        },
                        {
                            id: 'groupId',
                            header: ["Группа", {
                                content: "selectFilter", options: groupOptions, compare: function (value, filter, object) {

                                    if (filter == 0) return true;

                                    var group = groups.getItem(value);
                                    if (group.$parent) {
                                        return groups.getItem(group.$parent).id == filter;
                                    }
                                    return false;
                                }
                            }], sort: 'int', width: 200,

                            template: function (obj, common, value, config) {
                                if (!value) return null;

                                var item = groups.getItem(value);

                                if (item.$parent) {
                                    return groups.getItem(item.$parent).name;
                                }
                                return null;
                            }
                        },
                        {
                            id:'techIndex', header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 120,
                            footer: { content: "countColumn" }
                        },
                        {
                            id: 'departmentId',
                            header: ['Цех\Производство', { content: "selectFilter", options: webix.collection.options("department", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("department"),
                            width: 150
                        },
                        {
                            id: 'plantId',
                            header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true, null, true) }],
                            sort: "int",
                            template: webix.templates.collection("plant"),
                            width: 200
                        },
                        {
                            id: 'maintenanceDate',
                            header: ['Дата ремонта', { content: "dateFilter" }],
                        },
                        {
                            id: 'maintenanceTypeId',
                            header: ['Вид ремонта', { content: "selectFilter", options: webix.collection.options("maintenanceType", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("maintenanceType"),
                            width: 150
                        },
                        {
                            id: 'isTransfer',
                            header: ['Перенос', 'Предложение'],
                            template: function (obj) { return !obj.isTransfer ? (!obj.isOffer?'':'Предложение') : 'Перенос'; },
                            width:110
                        },
                        {
                            id: 'usageForPlan', header: ["Наработка", { content: "textFilter" }], sort: 'int', width: 120,
                        },
                        {
                            id: 'maintenanceReasonId',
                            header: ['Причина ремонта', { content: "selectFilter", options: webix.collection.options("maintenanceReason", "name", true) }],
                            sort: "int",
                            template: webix.templates.collection("maintenanceReason"),
                            width: 150
                        },
                        {
                            id: 'previousDate',
                            header: [{ text: 'Предыдущий ремонт', colspan: 3 }, 'Дата']
                        },
                        {
                            id: 'previousMaintenanceType',
                            header: ['', 'Вид ремонта'],
                            sort: "int",
                            template: webix.templates.collection("maintenanceType"),
                            width: 150
                        },
                        {
                            id: 'previousUsage',
                            header: ['', 'Наработка']
                        },
                        webix.column('trash')
                    ],
                    onClick: {
                        "fa-edit": webix.bind(function (e, target) {

                            var item = me.table.getItem(target.row);
                            me.callEvent("onCreateView", [
                                'Оборудование',
                                {
                                    view: "view_objecteditor",
                                    mode: 'update',
                                    itemId: item.objectId
                                }
                            ]);

                            return false;
                        }),
                        "fa-trash-o": webix.bind(function (e, target) {

                            webix.confirm(
                                {
                                    title: "Удаление записи",
                                    text: "Вы действительно хотите удалить запись?",
                                    ok: "Да",
                                    cancel: "Нет",
                                    callback: function (isOk) {
                                        if (isOk) me.table.remove(target.row);
                                    }
                                });

                            return false;
                        })
                    }
                }
            ]
        });

        this.$ready.push(this.initData);

    },

    initData: function () {

        var me = this;

        this.table = this.queryView({ view: 'datatable' });

        this.table.attachEvent("onBeforeLoad", function () {
            me.mask("Загрузка...");
        });

        this.table.attachEvent("onAfterLoad", function () {
            me.unmask();
        });

        var currentDepartment;
        this.table.attachEvent("onBeforeFilter", function (id, value, config) {

            if (id == "departmentId" && value != currentDepartment) {
                currentDepartment = value;
                var cfg = this.config.columns.filter(function (x) { return x.id == "plantId" })[0].header[1];

                if (value) {
                    var newOptions = webix.collection.options("plant", "name", true, function (i) {
                        return i.departmentId == value;
                    });
                    cfg.options = newOptions;
                }
                else {
                    cfg.value = "";
                    cfg.options = webix.collection.options("plant", "name", true, null, true)
                }

                this.refreshColumns();

            }

        });


        this.reload();

    },

    reload: function () {

        var table = this.table;

        table.clearAll();
        table.load("json->api/maintenancePlan/" + this.getPeriod());

    },

    getPeriod: function () {
        return webix.Date.dateToStr("%Y%m")(this.queryView({ name: "period" }).getValue());
    },
    mask: function (text) {
        this.disable();
        this.table.showOverlay(text+ "<span class='webix_icon fa-spinner fa-spin'></span>");
    },
    unmask: function () {
        this.enable();
        this.table.hideOverlay();
    }

}, webix.ui.layout);