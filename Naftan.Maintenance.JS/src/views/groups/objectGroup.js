webix.protoUI({
    name: "view_objectgroup",

    collection:'ObjectGroup',

    $init: function (config) {

        var me = this;

        webix.extend(config, {

            rows: [
                {
                    view: 'toolbar',
                    elements: [
                        {
                            view: "button", type: "iconButton", icon: "refresh", label: "Обновить", width: 100,
                            click: webix.bind(function () {
                                var store = this.getStore();
                                store.clearAll()
                                store.load(store.config.url);
                            }, this)
                        },
                        {
                            view: "button", type: "iconButton", icon: "minus-square-o", label: "Свернуть все", width: 130,
                            click: webix.bind(function () { this.queryView({ view: "treetable"}).closeAll()},this)
                        },
                        {
                            view: "button", type: "iconButton", icon: "plus-square-o", label: "Развернуть все", width: 140,
                            click: webix.bind(function () { this.queryView({ view: "treetable" }).openAll() }, this)
                        },
                        {
                            view: "button", type: "iconButton", icon: "plus-circle", label: "Добавить группу", width: 150,
                            click: function () {
                                
                                var item = me.queryView({ view: "treetable" }).getSelectedItem() || {};
                                
                                me.callEvent("onCreateView", [
                                    'Новая группа',
                                    {
                                        view: "view_objectgroupeditor",
                                        mode: 'insert',
                                        parentGroupId: item.id
                                    },
                                    'plus-circle'
                                ]);
                            }
                        }
                    ]
                },
                {
                    view: "treetable",
                    select: "row",
                    columns: [
                        webix.column("id"),
                        {
                            id: "name",
                            header: ["Наименование группы", { content: "textFilter" }],
                            template: "{common.treetable()} #name#",
                            fillspace: 1
                        }
                    ],
                    on: {
                        onItemDblClick: function () {
                            var item = me.queryView({ view: "treetable" }).getSelectedItem() || {};

                            me.callEvent("onCreateView", [
                                'Группа',
                                {
                                    view: "view_objectgroupeditor",
                                    mode: 'update',
                                    itemId: item.id
                                }
                            ]);
                        }
                    }
                }
            ]

        });

        this.$ready.push(this._initData);

    },

    _initData: function () {
        var table = this.queryView({ view: "treetable" });
        table.parse(this.getStore());
    },

    getStore: function () {
        return webix.collection(this.collection);
    }

}, webix.ui.layout);