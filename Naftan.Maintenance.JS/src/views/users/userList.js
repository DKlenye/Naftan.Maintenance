webix.protoUI({

    name: "view_userlist",
    collection: "user",

    $init: function (cfg) {

        var me = this;

        webix.extend(cfg, {

            rows: [
                {
                    view: "toolbar", elements: [
                        {
                            view: "button", type: "iconButton", icon: "edit", label: "Редактировать", width: 135,
                            click: function () {
                                var item = me.queryView({ view: "datatable" }).getSelectedItem() || {};
                                if (!item) return;
                                me.callEvent("onCreateView", [
                                    'Пользователь',
                                    {
                                        view: "view_usereditor",
                                        mode: 'update',
                                        itemId: item.id
                                    }
                                ]);
                            }
                        }
                    ]
                },
                {
                    view: "datatable",
                    select: "row",
                    navigation: true,
                    columns: [
                        webix.column("id"),
                        { id: 'name', header: ['Фио', { content: "textFilter" }], sort: "string", fillspace: true },
                        { id: 'login', header: ['Логин', { content: "textFilter" }], sort: "string", fillspace: true },
                        { id: 'phone', header: ['Телефон', { content: "textFilter" }], sort: "string", fillspace: true },
                        { id: 'email', header: ['Эл.почта', { content: "textFilter" }], sort: "string", fillspace: true }
                    ],
                    on: {
                        onItemDblClick: function () {
                            var item = me.queryView({ view: "datatable" }).getSelectedItem() || {};

                            me.callEvent("onCreateView", [
                                'Пользователь',
                                {
                                    view: "view_usereditor",
                                    mode: 'update',
                                    itemId: item.id
                                }
                            ]);
                        }
                    }
                }
            ]

        });

        this.$ready.push(this.initData);

    },

    initData: function () {
        var table = this.queryView({ view: "datatable" });
        table.parse(this._getStore());
    },

    _getStore: function () {
        return webix.collection(this.collection);
    }

}, webix.ui.layout)