webix.protoUI({

    name: "view_usereditor",
    requireCollections: ["ObjectGroup","department","plant"],

    $init: function (cfg) {

        var me = this;

        webix.extend(cfg, {

            rows: [
                {
                    view: 'toolbar',
                    elements: [{
                        view: "button", type: "iconButton", icon: "floppy-o", label: "Сохранить", width: 110, click: webix.bind(me.save, me)
                    }]
                },
                {
                    cols: [
                        {
                            view: 'property',
                            width:400,
                            elements: [
                                { label: "Код", id: "id" },
                                { label: "Логин", id: "login" },
                                { label: "Фио", type: "text", id: "name" },
                                { label: "Телефон", type: "text", id: "phone" }, 
                                { label: "Эл.почта", type: "text", id: "email" },
                                { label: "Участок", type: "combo", options: [1, 2, 3], id: "site" }
                            ]
                        },
                        { view: 'resizer' },
                        {
                            rows: [
                                {
                                    view: 'toolbar',
                                    elements: [
                                        {
                                            view: "icon", icon: "minus-square-o",
                                            click: function () {
                                                var name = me.queryView({ view: "segmented" }).getValue();
                                                me.queryView({ name: name }).closeAll();
                                            }
                                        },
                                        {
                                            view: "icon", icon: "plus-square-o",
                                            click:function () {
                                                var name = me.queryView({ view: "segmented" }).getValue();
                                                me.queryView({ name: name }).openAll();
                                            }
                                        },
                                        {
                                            view: "segmented", options: [
                                                { id: "objectGroups", value: "Группы" },
                                                { id: "plants", value: "Установки" }
                                            ],
                                            width: 300,
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
                                            name: 'objectGroups',
                                            view: "tree",
                                            threeState: true,
                                            template: "{common.icon()} {common.checkbox()} {common.folder()}<span>#name#<span>"
                                        },
                                        {
                                            name: 'plants',
                                            view: "tree",
                                            threeState: true,
                                            template: "{common.icon()} {common.checkbox()} {common.folder()}<span>#name#<span>"
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        this.$ready.push(this.initData)

    },

    initData: function () {

        webix.extend(this, webix.ProgressBar);

        webix.extend(this, {
            groups: this.queryView({ name: "objectGroups" }),
            plants: this.queryView({ name: "plants" }),
            property: this.queryView({ view: 'property' })
        });

        /*this.groups.sync(webix.collection("ObjectGroup"), function () { })
        this.groups.data.unsync();*/

        this.groups.data.importData(webix.collection("ObjectGroup"), true);
        this.groups.uncheckAll();

        this.plants.parse(this.buildPlants());

        var id = this.config.itemId;
        this.load(id);
    },

    buildPlants: function () {

        var data = [];
        var plants = webix.collection('plant').data;

        webix.collection("department").data.each(function (d) {
            var _department = { name: d.name };
            _department.data = plants.find(function (e) { return e.departmentId == d.id }).map(function (e) { return { id: e.id, name: e.name } });
            data.push(_department);
        });

        return data;
    },

    load: function (id) {

        this.mask();

        webix.ajax()
            .get("/api/user/" + id)
            .then(webix.bind(this.onLoadHandler, this))
            .fail(this.onErrorHandler);
    },

    onLoadHandler: function (data) {
        this.setData(data.json());
        this.unmask();
    },

    save: function () {

        this.mask();

        this.property.editStop();

        var data = this.getData();
        webix.ajax()
            .headers({ "Content-Type": "application/json" })
            .put("/api/user/" + data.id, data)
            .then(webix.bind(this.onLoadHandler, this), this.onErrorHandler);
    },

    setData: function (json) {
        var me = this;
        this.property.setValues(json);
        json.groups.forEach(function (g) {
            me.groups.checkItem(g);
        });
        json.plants.forEach(function (p) {
            me.plants.checkItem(p);
        })
    },

    getData: function () {
        var me = this;
        var item = this.property.getValues();
        item.groups = this.groups.getChecked();

        item.plants = me.plants.getChecked()
            .filter(function (id) { return me.plants.getItem(id).$level > 1 });

        return item;
    },

    onErrorHandler: function (e) {
        this.unmask();
        webix.alert({
            title: "Произошла ошибка",
            width: 700,
            text: e.responseText,
            type: "alert-error"
        });
    },

    mask: function () {
        this.disable();
        this.showProgress({
            type: "icon",
            icon: "refresh"
        });
    },

    unmask: function () {
        this.hideProgress();
        this.enable();
    }

}, webix.ui.layout);