webix.protoUI({

    name: 'view_objectlist',

    requireCollections: ["department", "plant", "specification", "reference", "ObjectGroup", "object","operatingState"],

    $init: function (cfg) {

        var me = this;

        webix.extend(cfg, {
            rows: [
                {
                    view: 'accordion',
                    cols: [
                        {
                            header: 'Группы оборудования',
                            body: {
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
                            }
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
                                            leftSplit: 2,
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
                }
            ]
        });

        this.$ready.push(this.initData);
        this.$ready.push(this.initEvents);
    },


    initData: function () {

        var groups = this.queryView({ name: "groups" });
        groups.sync(webix.collection("ObjectGroup"), function () { });

        var objects = this.queryView({ name: "objects" });
        objects.parse(webix.collection("object"));

        objects.data.attachEvent("onSyncApply", function () {
            objects.filterByAll();
        });

    },

    initEvents: function () {
        var me = this;
        var groups = this.queryView({ name: "groups" });
        var objects = this.queryView({ name: "objects" });

        groups.attachEvent("onSelectChange", webix.bind(this.onGroupSelect, this));

        var objectsFilter = objects.filterByAll;

        objects.filterByAll = function () {
            objectsFilter.call(objects);

            objects.filter(function (obj) {
                return !me.selectedGroups || me.selectedGroups[obj.groupId];
            }, null, true);
        }

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
                header: ['Установка', { content: "selectFilter", options: webix.collection.options("plant", "name", true,null,true) }],
                sort: "int",
                template: webix.templates.collection("plant"),
                width: 200
            },
            {
                id: 'period', header: ["Период", { content: "numberFilter" }], sort: 'int', width: 100,
                template: webix.templates.period()
            },
            {
                id: 'currentOperatingState',
                header: ['Состояние', { content: "selectFilter", options: webix.collection.options("operatingState", "name", true) }],
                sort: "int",
                template: webix.templates.collection("operatingState"),
                editor: "combo",
                options: webix.collection.options("operatingState"),
                width: 160
            }
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
        me.selectedGroups = map;

        objects.filterByAll();
    },

    specificationSelectHandler: function (selections) {

       

        var me = this;
        this.queryView({ name: 'objectsView' }).show();

        if (!selections) return;

        me.mask();

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
                me.unmask();

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

        var getColumn = function (type, extensions, compare) {
            return webix.extend({
                id: "specification_" + specification.id,
                adjust: "header",
                header: [specification.name, { content: 'textFilter', compare: filters[type] }],
                template: function (obj, common, value, config) {
                    return getSpecification(obj.id) || "";
                },
                sort: function (a, b) {
                    return webix.DataStore.prototype.sorting.as[type](getSpecification(a.id), getSpecification(b.id));
                }
            }, extensions || {}, true)
        }

        var filters = {
            int: (function () {
                var cache = {};
                var buildCache = function (filter) {
                    var equality = (filter.indexOf("=") != -1) ? 1 : 0;
                    var intvalue = filter.replace(/[^\-\.0-9]/g, "") * 1;
                    if (filter.indexOf(">") != -1){
                        cache[filter] = function (a) { return a * 1 > intvalue - equality }
                    }
                    else if (filter.indexOf("<") != -1) {
                        cache[filter] = function (a) {
                            return a !== null && a * 1 < (intvalue + equality);
                        }
                    }
                    else {
                        cache[filter] = function (a, b) { return a * 1 == intvalue }
                    }
                }
                
                return function (value, filter, object) {
                    if (!cache[filter]) {
                        buildCache(filter);
                    }
                    return cache[filter](getSpecification(object.id));                
                }

            })(),
            string: (function () {
                var cache = {};
                var buildCache = function (filter) {
                    cache[filter] = new RegExp(String(filter), 'ig');
                }
                return function (value, filter, object) {
                    if (!cache[filter]) {
                        buildCache(filter);
                    }
                    return cache[filter].test(getSpecification(object.id));
                }
            })(),
            date: (function () {
                var cache = {};
                var buildCache = function (filter) {
                    var equality = (filter.indexOf("=") != -1) ? 1 : 0;
                    var intvalue = webix.i18n.dateFormatDate(filter.replace(/^[>< =]+/, "")).valueOf();
                    if (filter.indexOf(">") != -1) {
                        cache[filter] = function (a) { return a * 1 > intvalue - equality }
                    }
                    else if (filter.indexOf("<") != -1) {
                        cache[filter] = function (a) {
                            return a !== "" && a * 1 < (intvalue + equality);
                        }
                    }
                    else {
                        cache[filter] = function (a, b) { return a * 1 == intvalue }
                    }
                }

                return function (value, filter, object) {
                    if (!cache[filter]) {
                        buildCache(filter);
                    }
                    return cache[filter](webix.i18n.dateFormatDate(getSpecification(object.id)).valueOf());
                }

            })(),

        }

        switch (specification.type) {
            case 1: {
                return getColumn('int', {
                    header: [specification.name, {
                        content: "selectFilter", options: [{ id: "", value: "" }, { id: 0, value: "Нет" }, { id: 1, value: "Да" }],
                        compare: function (value, filter, object) {
                            return filter == getSpecification(object.id)
                        }
                    }],
                    template: function (obj, common, value, config) {
                        var item = { "": "", "0": "Нет", "1": "Да" }[getSpecification(obj.id)];
                        if (!item) return "";
                        return item;
                    }
                });
            }
            case 2: {
                return getColumn('string');
            }
            case 3: {
                return getColumn('int');
            }
            case 4: {
                return getColumn('int');
            }
            case 5: {
                return getColumn('date', {
                    sort: function (a, b) {
                        return webix.DataStore.prototype.sorting.as.date(
                            webix.i18n.parseFormatDate(getSpecification(a.id)),
                            webix.i18n.parseFormatDate(getSpecification(b.id))
                        );
                    }
                });
            }
            case 6: {
                var specCollection = webix.collection("specification");
                var references = webix.collection("reference");

                var spec = specCollection.getItem(specification.id);
                var options = [{ id: "", value: "" }].concat(references.getItem(spec.referenceId).values);
                var map = {};
                options.forEach(function (i) {
                    map[i.id] = i.value;
                });

                return getColumn('int', {
                    header: [specification.name, {
                        content: "selectFilter", options: options, compare: function (value, filter, object) {
                            return filter == getSpecification(object.id)
                        }
                    }],
                    template: function (obj, common, value, config) {
                        var item = map[getSpecification(obj.id)];
                        return item || "";
                    }
                }); 
            }
        }

    },
       
    mask: function () {
        this.disable();
    },
    unmask: function () {
        this.enable();
    },

    onErrorHandler: function (e) {
        unmask();
        webix.message(e);
    }
           

}, webix.ui.layout);