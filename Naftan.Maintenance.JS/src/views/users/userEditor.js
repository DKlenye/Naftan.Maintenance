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
                                { label: "Логин", type: "text", id: "login" },
                                { label: "Фио", type: "text", id: "name" },
                                { label: "Телефон", type: "text", id: "phone" }, 
                                { label: "Эл.почта", type: "text", id: "email" }
                            ]
                        },
                        { view: 'resizer' },
                        {
                            rows: [
                                {
                                    view: 'toolbar',
                                    elements: [
                                        {
                                            view: "segmented", options: [
                                                { id: "objectGroups", value: "Группы" },
                                                { id: "plants", value: "Установки" }
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
        var groups = this.queryView({ name: "objectGroups" });
        groups.sync(webix.collection("ObjectGroup"), function () { })

        //groups.data.unsync();

        var plants = this.queryView({ name: "plants" });
        plants.parse(this.buildPlants());
        

    },

    buildPlants: function () {

        var data = [];
        var plants = webix.collection('plant').data;

        webix.collection("department").data.each(function (d) {
            var _department = webix.copy(d);
            _department.data = plants.find(function (e) { return e.departmentId == d.id }).map(function (e) { return { plantId: e.id, name: e.name } });
            data.push(_department);
        });

        return data;
    },

    save: function () {

    }

}, webix.ui.layout);