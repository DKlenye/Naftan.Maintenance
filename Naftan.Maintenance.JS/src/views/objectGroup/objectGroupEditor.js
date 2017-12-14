webix.protoUI({

    name: "view_objectgroupeditor",

    requireCollections: ["maintenanceType","measureUnit","timePeriod","specification","reference"],

    $init: function (config) {

        var me = this;

        var status = function (value, obj) {
            if (obj.inherited) return "row-inherited";
            return "";
        }

        webix.extend(config, {

            rows: [
                {
                    view: 'toolbar',
                    elements: [{
                        view: "button", type: "iconButton", icon: "floppy-o", label: "Сохранить", width: 110, click: webix.bind(me.save, me)
                    }]
                },
                {
                    view: "property", name:'common', nameWidth: 200, height: 80,
                    elements: [
                        { label: "Код", id: "id" },
                        { label: "Наименование", type: "text", id: "name" },
                        {
                            label: "Входит в группу",
                            id:"parentGroupId",
                            type: "combo",
                            popup: webix.ui({
                                view: "treesuggest",
                                textValue: "name",
                                width: 900,
                                height: 500,
                                collection:"ObjectGroup"
                            }),
                            template: function (col, id) {
                                return webix.templates.tree(col.collection, id);
                            },
                            collection: webix.collection("ObjectGroup")
                        }
                    ]
                },
                {
                    animate: false,
                    cells: [
                        {
                            name:'editor',
                            rows: [
                                {
                                    view: 'toolbar',
                                    elements: [{
                                        view: "segmented", width: 400, options: [
                                            { id: "specificationEditor", value: "Тех. характеристики" },
                                            { id: "intervalEditor", value: "Межремонтные интервалы" }
                                        ],
                                        on: {
                                            onAfterTabClick: function (id) {
                                                me.queryView({ name: id }).show();
                                            }
                                        }
                                    }]
                                },
                                {
                                    animate: false,
                                    cells: [
                                        {
                                            name:'specificationEditor',
                                            rows: [
                                                {
                                                    view: 'toolbar',
                                                    elements: [
                                                        {
                                                            view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить", width: 100, click: webix.bind(this.addSpecificationClick, this)
                                                        }
                                                    ]
                                                },
                                                {
                                                    view: "datatable",
                                                    name: "specifications",
                                                    select: "row",
                                                    navigation: true,
                                                    editable: true,
                                                    columns: [
                                                        webix.column("id", { cssFormat: status}),
                                                        { id: 'name', header: ['Наименование характеристики', { content: "textFilter" }], sort: "string", fillspace: 2, cssFormat: status },
                                                        {
                                                            id: 'defaultValue',
                                                            header: ['Значение по умолчанию', { content: "textFilter" }],
                                                            template: function (obj, common, value, config) {

                                                                if (!value) return "";

                                                                if (obj.options) {
                                                                    return obj.map[value];
                                                                }

                                                                if (obj.format) {
                                                                    return webix.Date.dateToStr(obj.format)(value);
                                                                }

                                                                return value;
                                                            },
                                                            sort: "string", editor: "text", fillspace: 1, cssFormat: status
                                                        },
                                                        webix.column("trash")
                                                    ],
                                                    onClick: {
                                                        "fa-trash-o": function (e, target) {
                                                            this.remove(target.row);
                                                            return false;
                                                        }
                                                    }
                                                }
                                            ]
                                        },
                                        {
                                            name: 'intervalEditor',
                                            rows: [
                                                { view: 'toolbar', elements: [{ view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить", width: 100, click: webix.bind(this.addIntervalClick, this)}]},
                                                {
                                                    view: 'datatable',
                                                    select: "row",
                                                    navigation: true,
                                                    editable: true,
                                                    name: "intervalTable",
                                                    css: "center_columns",
                                                    columns: [
                                                        { id: "id", header: [{ text: "Код", rowspan: 2 }, '', { content: "textFilter" }], template: function (o, c, value) { return (value < 1e10) ? value : "" }, width: 100, sort: "int"},
                                                        {
                                                            id: 'maintenanceTypeId',
                                                            template: webix.templates.collection("maintenanceType"),
                                                            editor: "combo",
                                                            options: webix.collection.options("maintenanceType"),
                                                            header: [{ text: 'Вид обслуживания', rowspan: 2 }, '', { content: "textFilter" }],
                                                            fillspace: 2
                                                        },
                                                        {
                                                            id: 'measureUnitId',
                                                            template: webix.templates.collection("measureUnit"),
                                                            editor: "combo",
                                                            options: webix.collection.options("measureUnit"),
                                                            header: [{ text: 'Норма наработки', colspan: 3 }, 'Ед. изм.', { content: "textFilter" }],
                                                            fillspace: 1
                                                        },
                                                        {
                                                            id: 'maxUsage',
                                                            header: ['', 'Минимальная', { content: "textFilter" }], editor:'text', fillspace: 1
                                                        },
                                                        {
                                                            id: 'minUsage',
                                                            header: ['', 'Максимальная', { content: "textFilter" }], editor: 'text', fillspace: 1
                                                        },
                                                        {
                                                            id: 'timePeriod',
                                                            header: [{ text: 'Интервал между обслуживанием', colspan: 2 }, 'Временной интервал', { content: "textFilter" }], fillspace: 1,
                                                            template: webix.templates.collection("timePeriod"),
                                                            editor: "combo",
                                                            options: webix.collection.options("timePeriod"),
                                                        },
                                                        {
                                                            id: 'periodQuantity',
                                                            header: ['', 'Количество', { content: "textFilter" }], editor: 'text', fillspace: 1
                                                        },
                                                        {
                                                            id: 'quantityInCycle',
                                                            header: [{ text: 'Количество в структуре МРЦ', rowspan: 2 }, '', { content: "textFilter" }], editor: 'text', fillspace: 1
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            name:'addSpecification',
                            view:"view_addSpecification"
                        }
                    ]
                }
            ]

        });

        this.$ready.push(this.initEvents);
        this.$ready.push(this.initData);

    },

    initEvents: function () {
        var me = this;

        var table = this.getSpecifications();

        table.attachEvent('onBeforeEditStart', function (id) {

            if (id.column != "defaultValue") return false;

            var config = this.getColumnConfig(id.column);
            var item = this.getItem(id.row);

            config.editor = item.editor;
            config.options = item.options;
        });

        var addView = this.queryView({ name: "addSpecification" });

        addView.attachEvent("onSpecificationSelect", function (selections) {

            if (selections) {
                var specifications = me.buildSpecifications(
                    selections.map(function (s) {
                        return {
                            specificationId:s.id,
                            inherited: false,
                            specificationType:s.type
                        }
                    })
                );
                me.getSpecifications().parse(specifications);
            }

            me.queryView({ name: 'editor' }).show();
        });

        var common = this.getCommon();
        common.attachEvent('onAfterEditStop', function (state, editor, ignoreUpdate) {
            if (editor.id != "parentGroupId") return;
            if (state.value != state.old) {
                me.changeParent(state.value);
            } 
        });
    },

    changeParent: function (value) {
        var me = this;

        this.loadGroup(value, function (e) {
            var data = e.json();
            var specificationsMap = [];

            specificationsMap = data.specifications.map(function (s) { return s.specificationId });

            var specifications = me.getSpecifications();
            var remove = [];
            specifications.data.each(function (s) {
                if (s.inherited || specificationsMap.indexOf(s.specificationId)!=-1) remove.push(s.id);
            });
            specifications.remove(remove);
            specifications.parse(
                me.buildSpecifications(data.specifications, true)
            );

        });
    },

    initData: function () {
        
        var mode = this.config.mode;
        var id = this.config.itemId;
        var parentGroupId = this.config.parentGroupId;

        if (mode == "update") {
            this.loadGroup(id, webix.bind(this.setData,this));
        }
        else {
            if (parentGroupId) {
                var common = this.getCommon();
                common.setValues({
                    parentGroupId: parentGroupId
                });
                this.changeParent(parentGroupId);
            }
        }
    },
    
    getData: function () {

        var o = {};
        var common = this.getCommon();
        webix.extend(o, common.getValues());


        var specifications = this.getSpecifications();
        var specArray = [];
        specifications.data.each(function (i) {

            specArray.push({
                id: i.id > 1e10 ? 0 : i.id,
                objectGroupId: i.objectGroupId,
                inherited: i.inherited,
                specificationId: i.specificationId,
                defaultValue: i.defaultValue
            });

        })

        o.specifications = specArray;

        o.intervals = [];
        
        var intervals = this.queryView({ name: "intervalTable" });
        intervals.data.each(function (i) {
            var copy = webix.copy(i);
            copy.id = copy.id > 1e10 ? 0 : copy.id;
            o.intervals.push(copy);
        });
        
        return o;
    },

    setData: function (e) {
        var data = e.json();
        var dataSpecs = this.buildSpecifications(data.specifications);

        var common = this.getCommon();
        common.setValues(data);

        var specifications = this.getSpecifications();

        specifications.clearAll();
        specifications.parse(dataSpecs);
        specifications.refresh();

        var intervals = this.queryView({ name: "intervalTable" });
        intervals.clearAll();
        intervals.parse(data.intervals);
        intervals.refresh();

        this.setMode("update");
    },

    getSpecifications: function () {
        return this.queryView({ name: 'specifications' });
    },
    getCommon: function () {
        return this.queryView({ name: 'common' });
    },

    setMode: function (mode) {
        var fn = this["setMode_" + mode];
        if (fn) fn.call(this);
    },

    setMode_insert: function () {

    },

    setMode_update: function () {

        var id = this.getCommon().getValues().id;

        this.define({ mode: 'update' });
        this.callEvent('onChangeTitle', [this.config.id, "Группа № "+id ,"edit"]);
    },

    clear: function () {

    },

    loadGroup: function (id, handler) {
        webix.ajax()
            .get("/api/objectgroup/"+id)
            .then( handler, this.onErrorHandler)
    },   

    buildSpecifications: function (specifications, inherited) {

        //specification(id,specificationId,objectGroupId,inherited, specificationType, defaultValue)

        var me = this;
        var groups = webix.collection("objectgroup");
        var specification = webix.collection("specification");
        var references = webix.collection("reference");

        return specifications.map(function (i) {

            var spec = specification.getItem(i.specificationId);
            var specInherited = inherited || i.inherited;

            var o = {
                name: spec.name,
                specificationId: i.specificationId,
                objectGroupId: i.objectGroupId,
                inherited: specInherited,
                editor: specInherited ? null : me.getSpecificationType(i.specificationType),
                id: i.id === 0 ? webix.uid() : i.id,
                defaultValue: i.defaultValue
            };

            if (i.specificationType == 5) {
                o.format = "%d.%M.%Y"
            }

            if (i.specificationType == 6) {
                o.options = references.getItem(spec.referenceId).values;
                o.map = {}; 
                o.options.forEach(function (i) {
                    o.map[i.id] = i.value;
                })
            }

            return o;


        });

    },

    addSpecificationClick: function () {
        var view = this.queryView({ name: "addSpecification" });
        var specifications = this.getSpecifications();

        var exist = [];
        specifications.data.each(function (i) { exist.push(i.specificationId); });

        view.reset();
        view.filterExists(exist);
        view.show();
    },

    getSpecificationType: function (type) {
        return [
            "",
            "checkbox",
            "text",
            "text",
            "text",
            "date",
            "combo"
        ][type];
    },

    addIntervalClick: function () {
        var view = this.queryView({ name: "intervalTable" });
        view.add({});
    },

    save: function () {
        var data = this.getData();
        this[this.config.mode](data);
    },

    update: function (data) {
        webix.ajax().headers({ "Content-Type": "application/json" })
            .put("/api/objectgroup/"+data.id, data)
            .then(webix.bind(this.onSaveHandler, this), this.onErrorHandler)
    },

    insert: function (data) {
        webix.ajax().headers({ "Content-Type": "application/json" })
            .post("/api/objectgroup/", data)
            .then(webix.bind(this.onSaveHandler, this), this.onErrorHandler)
    },

    onSaveHandler: function (e) {
        this.setData(e);

        var data = e.json();
        var collection = webix.collection("objectgroup");

        var item = collection.getItem(data.id);
        if (item) {

            //todo переделать, потому что parent меняется, но в дерево не перестраивается

            item.name = data.name;
            item.$parent = data.parentGroupId;
            
            collection.updateItem(item.id, item);;
        }
        else {
            collection.add({
                id: data.id,
                name: data.name
            }, -1, data.parentGroupId || 0);
        }
    },

    onErrorHandler: function (e) {
        webix.message(e.response, 'error')
    }
    

}, webix.ui.layout);