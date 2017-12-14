webix.protoUI({

    name: 'view_objectlist',

    requireCollections: ["department", "plant", "environment", "manufacturer","specification","reference", "ObjectGroup","object"],

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
                            name:'objectsView',
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
                                onSpecificationSelect:webix.bind(me.specificationSelectHandler,me)
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
        groups.data.eachSubItem(groupId,function (obj) {
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
        console.log(selections);
        var me = this;
        this.queryView({ name: 'objectsView' }).show();

        var table = this.queryView({ name: "objects" });
        var columns = this.getColumns();
        if (selections) {
            selections.forEach(function (sel) {
                columns.push(me.buildColumn(sel));
            });
        }

        table.define({ columns: columns });
        table.refreshColumns();
        table.filterByAll();

    },

    buildColumn: function (specification) {

        switch (specification.type) {
            case 1:{
                return {
                    header: [specification.name, { content: "textFilter" }],
                    sort: 'text'
                }
            }
            case 2: {
                return {
                    header: [specification.name, { content: "textFilter" }],
                    sort: 'text'
                }
            }
            case 3: {
                return {
                    header: [specification.name, { content: "numberFilter" }],
                    sort: 'int'
                }
            }
            case 4: {
                return {
                    header: [specification.name, { content: "numberFilter" }],
                    sort: 'int'
                }
            }
            case 5: {
                return {
                    header: [specification.name, { content: "dateFilter" }],
                    sort: 'date',
                    format: webix.Date.dateToStr("%d.%m.%Y")
                }
            }
            case 6: {
                var specs = webix.collection("specification");
                var references = webix.collection("reference");

                var spec = specs.getItem(specification.id);
                var options = references.getItem(spec.referenceId).values;
                var map = {};
                options.forEach(function (i) {
                    map[i.id] = i.value;
                })

                return {
                    header: [specification.name, { content: "selectFilter",options:options }],
                    sort: 'date',
                    template: function (obj, common, value, config) {
                        var item = map[value];
                        if (!item) return "";
                        return item.value;
                    }
                }
            }
        }
        

    },

    loadSpecifications: function () {

    },

    onSpecificationsLoad: function () {

    },

    mask: function () {

    },
    unmask: function () {

    }

        

}, webix.ui.layout);