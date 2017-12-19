webix.protoUI({

    name: "view_objecteditor",

    requireCollections: ["reference","maintenanceType", "specification", "plant", "environment","manufacturer","objectGroup"],

    $init: function (cfg) {

        var me = this;

        webix.extend(cfg, {

            rows: [
                {
                    view: "toolbar", elements: [
                        { view: "button", type: "iconButton", icon: "floppy-o", label: "Сохранить", width: 110, click: webix.bind(me.save,me) }
                    ]
                },
                {
                    view: "accordion",
                    cols: [
                        {
                            header: "Основные данные",
                            body:
                            {
                                rows: [
                                    {
                                        view: "property", name: "common", width: 500, nameWidth: 200,
                                        elements: [
                                            { id: "id", label: "Код" },
                                            { id: "techIndex", label: "Тех. индекс", type: "text" },
                                            { id: "departmentId", label: "Цех\Производство", type: "combo", options: webix.collection.options("department", "name", true) },
                                            { id: "plantId", label: "Установка", type: "combo", options: webix.collection.options("plant", "name", true) },
                                            { id: "environmentId", label: "Рабочая среда", type: "combo", options: webix.collection.options("environment", "name", true) },
                                            { id: "manufacturerId", label: "Производитель", type: "combo", options: webix.collection.options("manufacturer", "name", true) },
                                            { id: "factoryNumber", label: "Заводской №", type: "text" },
                                            { id: "inventoryNumber", label: "Инв. №", type: "text" },
                                            {
                                                id: "groupId",
                                                label: "Входит в группу",
                                                type: "combo",
                                                popup: webix.ui({
                                                    view: "treesuggest",
                                                    textValue: "name",
                                                    width: 900,
                                                    height: 500,
                                                    collection: "ObjectGroup"
                                                }),
                                                template: function (col, id) {
                                                    return webix.templates.tree(col.collection, id);
                                                },
                                                collection: webix.collection("ObjectGroup")
                                            },
                                            {
                                                id: "parentId",
                                                label: "Входит в состав объекта",
                                                type: "combo",
                                                popup: webix.ui({
                                                    view: "gridsuggest",
                                                    width: 800,
                                                    body: {
                                                        height: 400,
                                                        columns: [
                                                            webix.column('id'),
                                                            { id: 'techIndex', header: ["Тех. индекс", { content: "textFilter" }] },
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
                                                            { id: 'factoryNumber', header: ["Заводской №", { content: "textFilter" }], sort: 'text', width: 120 },
                                                            { id: 'inventoryNumber', header: ["Инв. №", { content: "textFilter" }], sort: 'text', width: 100 }
                                                        ],
                                                        data: webix.collection("object"),
                                                        getText: function (id) {
                                                            if (!id) return "";
                                                            var item = webix.collection("object").getItem(id);
                                                            if (!item) return "";
                                                            return "[" + item.id + "] " + item.techIndex;
                                                        }
                                                    }
                                                }),
                                                template: function (col, id) {
                                                    return col.popup.getBody().config.getText(id);
                                                },
                                                collection:webix.collection("object")
                                            },
                                            { id: "currentState", label: "Текущее состояние" },
                                            { id: "startOperating", label: "Ввод в эксплуатацию" },
                                            { id: "usageFromStart", label: "Наработка с начала экспл." }
                                        ]
                                    },
                                    {view:'resizer'},
                                    {
                                        header: "Последнее обслуживание",
                                        body: {
                                            view: 'datatable', 
                                            name:'lastMaintenance',
                                            columns: [
                                                { id: 'maintenanceTypeId', header: 'Вид обслуживания', template: webix.templates.collection('maintenanceType'), fillspace: 2 },
                                                {
                                                    id: 'lastMaintenanceDate', header: 'Дата',
                                                    format: function (value) {
                                                        if (!value) return "";
                                                        var format = webix.Date.dateToStr(webix.i18n.dateFormat);
                                                        var parser = webix.Date.strToDate(webix.i18n.dateFormat);
                                                        return format(parser(value));
                                                    },
                                                    fillspace: 1
                                                },
                                                { id: 'usageFromLastMaintenance', header: 'Наработка', fillspace: 1 }
                                            ]
                                        }
                                    }
                                ]
                                
                            }
                        },
                        { view: 'resizer' },
                        {
                            name:'details',
                            rows: [
                                {
                                    view: 'toolbar',
                                    elements: [
                                        {
                                            view: "segmented", options: [
                                                { id: "specifications", value: "Характеристики" },
                                                { id: "child", value: "Узлы" },
                                                { id: "usage", value: "Наработка" },
                                                { id: "repairs", value: "Ремонты" },
                                                { id: "states", value: "Состояния" },
                                                { id: "intervals", value: "Интервалы" }
                                            ],
                                            on: {
                                                onAfterTabClick: function (id) {
                                                    var view = me.queryView({ name: id });
                                                    if (view) view.show();
                                                }
                                            }
                                        }
                                    ]
                                },
                                {
                                    animate: false,
                                    cells: [
                                        {
                                            name: "specifications",
                                            view: 'view_specificationsEditor'
                                        },
                                        {
                                            name:"child"
                                        },
                                        {
                                            name: "usage",
                                            view: "view_usageEditor"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        
        this.$ready.push(this.initEvents);
        this.$ready.push(this.initData);
    },

    //Поиск виджетов по имени и установка их в this
    findViews: function (names) {
        var me = this;
        names = names || [];
        names.forEach(function (name) {
            me[name] = me.queryView({ name: name });
        });
    },
        
    initEvents: function () {
        var me = this;

        this.findViews(["viewsSegment", "specifications", "common", "details", "lastMaintenance"]);

        this.common.attachEvent('onBeforeEditStart', function (id) {

            var values = me.common.getValues();
            var parent = values.parentId;

            switch (id) {
                case "groupId": {
                    if (me.config.mode == "update") return false; //нельзя править группу когда объект уже сохранён
                    break;
                }
                case "departmentId": {
                    if (parent) return false;
                    break;
                }
                case "plantId": {
                    if(parent) return false;

                    var newOptions = webix.collection.options("plant", "name", true, function (i) {
                        return i.departmentId == values.departmentId;
                    });

                    var collection = me.common.config.elements.filter(function (i) { return i.id == "plantId" })[0].collection;
                    collection.clearAll();
                    collection.parse(newOptions);

                    break;
                }
                case "techIndex": {
                    if (parent) return false;
                    break;
                }
            }

        });


        this.common.attachEvent('onAfterEditStop', function (state, editor, ignoreUpdate) {

            switch (editor.id) {
                //когда изменяется родительский объект устонавливаем тех индекс и местоположение родителя
                case "parentId": {

                    if (state.value && state.value != state.old) {
                        var values = me.common.getValues();
                        var collection = webix.collection('object');
                        var item = collection.getItem(state.value);

                        values.departmentId = item.departmentId;
                        values.plantId = item.plantId;
                        values.techIndex = item.techIndex;
                        me.common.setValues(values);
                    }

                    break;
                }
                case "groupId": {
                    /*if (state.value != state.old) {
                        if (me.config.mode == "update") {
                            me.specifications.load(me.config.itemId, state.value);
                        }
                    }*/
                    break;
                }
                case "departmentId": {
                    if (state.value != state.old) {
                        var values = me.common.getValues();
                        values.plantId = null;
                        me.common.setValues(values);
                    }
                    break;
                }
            }
            
        });
    },

    initData: function () {
        var mode = this.config.mode;
        var itemId = this.config.itemId;

        this.setMode(mode);

        if (mode == "update") {
            this.loadObject(itemId);
            this.specifications.load(itemId);
        }

    },

    loadObject: function (id) {
        webix.ajax()
            .get("/api/object/" + id)
            .then(webix.bind(this.onObjectLoad, this), this.onErrorHandler)
    },

    onObjectLoad: function (data) {
        var json = data.json();
        this.define({ itemId: json.id });
        var mode = this.config.mode;

        if (mode == "insert") {
            this.specifications.load(json.id);
        }

        this.setMode("update");
        this.common.setValues(json);
        this.lastMaintenance.parse(json.lastMaintenance);
    },

    save: function () {
        this.common.editStop();
        this.disable();
        var data = this.common.getValues();
        this[this.config.mode](data);
    },

    update: function (data) {

        var promises = [
            webix.ajax().headers({ "Content-Type": "application/json" }).put("/api/object/" + data.id, data),
            this.specifications.save()
        ];

        webix.promise.all(promises)
            .then(webix.bind(this.onSaveHandler, this), this.onErrorHandler)
    },

    insert: function (data) {
        webix.ajax().headers({ "Content-Type": "application/json" })
            .post("/api/object/", data)
            .then(webix.bind(this.onSaveHandler, this), this.onErrorHandler)
    },

    onSaveHandler: function (e) {
        if (!e.json) e = e[0];
        this.onObjectLoad(e);

        var collection = webix.collection('object');
        var data = e.json();


        var dp = webix.dp(collection);
        dp.off();
        var item = collection.getItem(data.id);
        if (!item) {
            collection.add(data);
        }
        else collection.updateItem(data.id, data);
        dp.on();

        this.enable();
    },

    setMode: function (mode) {
        var fn = this["setMode_" + mode];
        if (fn) fn.call(this);
    },

    setMode_insert: function () {
        this.define({ mode: 'insert' });
        this.details.disable();
        this.callEvent('onChangeTitle', [this.config.id, "Новое оборудование", "edit"]);
    },

    setMode_update: function () {

        var id = this.config.itemId;

        this.define({ mode: 'update' });
        this.callEvent('onChangeTitle', [this.config.id, "Оборудование №" + id, "edit"]);
        this.details.enable();
    },

    onErrorHandler: function () {

    }

    
}, webix.ui.layout);