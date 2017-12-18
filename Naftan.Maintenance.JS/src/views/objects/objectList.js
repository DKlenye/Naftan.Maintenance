webix.protoUI({

    name: 'view_objectlist',

    requireCollections: ["department", "plant", "environment", "manufacturer", "specification", "reference", "ObjectGroup", "object"],

    $init: function (cfg) {

        var me = this;

        webix.extend(cfg, {
            cols: [
                {
                    rows: [
                        {
                            view: "toolbar",
                            elements: [
                                {
                                    view: "icon", icon: "minus-square-o",
                                    click: webix.bind(function () { me.queryView({ name: "groups" }).closeAll() }, this)
                                },
                                {
                                    view: "icon", icon: "plus-square-o",
                                    click: webix.bind(function () { me.queryView({ name: "groups" }).openAll() }, this)
                                },
                                {
                                    view: "search",
                                    placeholder: "Поиск...",
                                    on: {
                                        onKeyPress: function (code, e) {
                                            //Esc
                                            if (code === 27) {
                                                this.$setValue('');
                                            }
                                            me.queryView({ name: "groups" }).filter("#name#", this.getValue());
                                        }
                                    }
                                }
                            ]
                        },
                        {
                            name: 'groups',
                            view: "tree",
                            template: "{common.icon()} {common.folder()}<span>#name#<span>",
                            select: true,
                            width: 400
                        }
                    ]
                },
                { view: "resizer" },
                {
                    animate: false,
                    cells: [
                        {
                            name: 'objectsView',
                            rows: [
                                {
                                    view: 'toolbar',
                                    elements: [
                                        {
                                            view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить", width: 100,
                                            click: function () {
                                                me.callEvent("onCreateView", [
                                                    'Новое оборудование',
                                                    {
                                                        view: "view_objecteditor",
                                                        mode: 'insert'
                                                    },
                                                    'plus-circle'
                                                ]);
                                            }
                                        },
                                        {
                                            view: "button", type: "iconButton", icon: "edit", label: "Редактировать", width: 135, click: function () {
                                                var item = me.queryView({ name: "objects" }).getSelectedItem() || {};
                                                if (item) {
                                                    me.callEvent("onCreateView", [
                                                        'Оборудование',
                                                        {
                                                            view: "view_objecteditor",
                                                            mode: 'update',
                                                            itemId: item.id
                                                        }
                                                    ]);
                                                }
                                            }
                                        },
                                        {
                                            view: "button", type: "iconButton", icon: "trash", label: "Удалить", width: 100, click: function () {
                                                var item = me.queryView({ name: "objects" }).getSelectedItem() || {};
                                                if (item) {
                                                    webix.confirm(
                                                        {
                                                            title: "Удаление записи",
                                                            text: "Вы действительно хотите удалить запись?",
                                                            ok: "Да",
                                                            cancel: "Нет",
                                                            callback: function (isOk) {
                                                                if (isOk) { }//todo delete}
                                                            }
                                                        });
                                                }
                                            }
                                        },
                                        {},
                                        {
                                            view: "button", type: "iconButton", icon: "check-square-o", label: "Характеристики", width: 150, click: function () {
                                                me.queryView({ view: 'view_addSpecification' }).show();
                                            }
                                        },
                                        {
                                            view: "button", type: "iconButton", label: "Экспорт", icon: "file-excel-o", width: 100, click: function () {
                                                webix.toExcel(me.queryView({ name: "objects" }));
                                            }
                                        }
                                    ]
                                },
                                {

                                    view: "datatable",
                                    footer: true,
                                    name: "objects",
                                    select: "row",
                                    navigation: true,
                                    resizeColumn: true,
                                    headermenu: { width: 200 },
                                    columns: this.getColumns(),
                                    on: {
                                        onItemDblClick: function () {
                                            var item = me.queryView({ name: "objects" }).getSelectedItem() || {};

                                            me.callEvent("onCreateView", [
                                                'Оборудование',
                                                {
                                                    view: "view_objecteditor",
                                                    mode: 'update',
                                                    itemId: item.id
                                                }
                                            ]);
                                        }
                                    }
                                }
                            ]
                        },
                        {
                            view: "view_addSpecification",
                            on: {
                                onSpecificationSelect: webix.bind(me.specificationSelectHandler, me)
                            }
                        }
                    ]
                }
            ]
        });

        this.$ready.push(this.initData);
        this.$ready.push(this.initEvents);
    },


    initData: function () {
        var groups = this.queryView({ name: "groups" });
        groups.parse(webix.collection("ObjectGroup"));

        var objects = this.queryView({ name: "objects" });
        objects.parse(webix.collection("object"));

    },

    initEvents: function () {
        var groups = this.queryView({ name: "groups" });
        var objects = this.queryView({ name: "objects" });

        groups.attachEvent("onSelectChange", webix.bind(this.onGroupSelect, this));

        this.objectsFilter = objects.filterByAll;

    },

    getColumns: function () {
        return [
            webix.column("id"),
            {
                id:
                'techIndex', header: ["Тех. индекс", { content: "textFilter" }], sort: 'text', width: 120,
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
                header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true) }],
                sort: "int",
                template: webix.templates.collection("plant"),
                width: 200
            },
            {
                id: 'environmentId',
                header: ["Рабочая среда", { content: "selectFilter", options: webix.collection.options("environment", "name", true) }],
                sort: 'int',
                template: webix.templates.collection("environment"),
                width: 200
            },
            {
                id: 'manufacturerId',
                header: ["Завод изготовитель", { content: "selectFilter", options: webix.collection.options("manufacturer", "name", true) }],
                template: webix.templates.collection("manufacturer"),
                sort: 'int',
                width: 250
            },
            { id: 'factoryNumber', header: ["Заводской №", { content: "textFilter" }], sort: 'text', width: 120 },
            { id: 'inventoryNumber', header: ["Инв. №", { content: "textFilter" }], sort: 'text', width: 100 },
            { id: 'currentOperatingState', header: ["Состояние", { content: "textFilter" }], sort: 'text', width: 150 }
        ]
    },

    onGroupSelect: function (selections) {
        var me = this;
        var groupId = selections[0];
        var groups = this.queryView({ name: "groups" });
        var objects = this.queryView({ name: "objects" });

        var map = {};
        map[groupId] = {}
        groups.data.eachSubItem(groupId, function (obj) {
            map[obj.id] = obj;
        });

        objects.filterByAll = function () {
            me.objectsFilter.call(objects);

            objects.filter(function (obj) {
                return !!map[obj.groupId];
            }, null, true);
        }

        objects.filterByAll();

    },

    specificationSelectHandler: function (selections) {
        var me = this;
        this.queryView({ name: 'objectsView' }).show();

        webix.ajax().headers({ "Content-Type": "application/json" })
            .post("api/objectSpecification", { data: selections.map(function (o) { return o.id }) })
            .then(function (data) {
                me.specificationsMap = data.json();

                var table = me.queryView({ name: "objects" });
                var columns = me.getColumns();
                if (selections) {
                    selections.forEach(function (sel) {
                        columns.push(me.buildColumn(sel));
                    });
                }

                table.define({ columns: columns });
                table.refreshColumns();
                table.filterByAll();

            }, this.onErrorHandler);

    },

    buildColumn: function (specification) {

        var me = this,
            specs = me.specificationsMap;

        //получение тех характеристики для объекта
        var getSpecification = function (objectId) {
            if (specs[objectId]) {
                var spec = specs[objectId][specification.id]
                return spec || null;;
            }
            return null;
        }

        var getColumn = function (filter, sort) {
            return {
                id: "specification_" + specification.id,
                header: [specification.name, { content: filter }],
                template: function (obj, common, value, config) {
                    return getSpecification(obj.id) || "";
                },
                sort: function (a, b) {
                    return webix.DataStore.prototype.sorting.as[sort](getSpecification(a.id), getSpecification(b.id));
                }
            }
        }
        
        switch (specification.type) {
            case 1: {
                return {
                    id: "specification_" + specification.id,
                    header: [specification.name, { content: "selectFilter", options:[{ id: "", value: "" }, { id: 0, value: "Нет" }, { id: 1, value: "Да" }] }],
                    sort: function (a, b) {
                        return webix.DataStore.prototype.sorting.as.int(getSpecification(a.id), getSpecification(b.id));
                    },
                    template: function (obj, common, value, config) {
                        var item = { "": "", "0": "Нет", "1": "Да" }[getSpecification(obj.id)];
                        if (!item) return "";
                        return item;
                    }
                };
            }
            case 2: {
                return getColumn('textFilter', 'string');
            }
            case 3: {
                return getColumn('numberFilter', 'int');
            }
            case 4: {
                return getColumn('numberFilter', 'int');
            }
            case 5: {
                return {
                    id: "specification_" + specification.id,
                    header: [specification.name, { content: 'dateFilter' }],
                    template: function (obj, common, value, config) {
                        return getSpecification(obj.id) || "";
                    },
                    sort: function (a, b) {
                        return webix.DataStore.prototype.sorting.as.date(
                            webix.i18n.parseFormatDate(getSpecification(a.id)),
                            webix.i18n.parseFormatDate(getSpecification(b.id))
                        );
                    }
                }
            }
            case 6: {
                var specCollection = webix.collection("specification");
                var references = webix.collection("reference");

                var spec = specCollection.getItem(specification.id);
                var options = [{ id: "", value: "" }].concat(references.getItem(spec.referenceId).values);
                var map = {};
                options.forEach(function (i) {
                    map[i.id] = i.value;
                })

                return {
                    id: "specification_" + specification.id,
                    header: [specification.name, { content: "selectFilter",options:options }],
                    sort: function (a, b) {
                        return webix.DataStore.prototype.sorting.as.int(getSpecification(a.id), getSpecification(b.id));
                    },
                    template: function (obj, common, value, config) {
                        var item = map[getSpecification(obj.id)];
                        if (!item) return "";
                        return item;
                    }
                }
            }
        }

    },
       
    mask: function () {

    },
    unmask: function () {

    },

    onErrorHandler: function () {

    }
           

}, webix.ui.layout);