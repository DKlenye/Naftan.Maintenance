webix.ready(function () {

     window.chat = $.connection.chatHub;
     window.chat.client.showMessage = function (message) {
         webix.message(message);
     };

     webix.i18n.setLocale("ru-RU");

     webix.collection.tree(["objectGroup"]);
     //webix.debug_bind = true;

     $.connection.hub.start();

    webix.ui({
        rows: [
            {
                view: "toolbar", css:"app_header", padding: 3, elements: [
                    {
                        view: "button", type: "icon", icon: "bars",
                        width: 30, align: "center", css: "app_button", click: function () {
                            $$("$sidebar1").toggle();
                        }
                    },
                    {width:20},
                    {
                        view: "template",
                        borderless: !0,
                        css: "logo",
                        template: "<img src='Content/images/1-50-32.png' />",
                        width: 36,
                        height:34
                    },
                    {
                        view: "label",
                        width:600,
                        label: "Система управления техническим обслуживанием и ремонтом",
                        css: "header_label"
                    },
                    {},
                    {
                        view: "icon", icon: "retweet", click: function () {
                            webix.ajax()
                                .post("/api/database/update").
                                then(function () { webix.message('База обновлена') }, function (e) { webix.message(e.response, 'error') })}
                    },
                    {
                        view: "icon", icon: "database", click: function () {
                            webix.ajax()
                                .post("/api/database/create").
                                then(function () {webix.message('База построена') }, function (e) { webix.message(e.response, 'error') })
                        }
                    },

                ]
            },
            {
                cols: [
                    {
                        view: "sidebar",
                        width: 250,
                        data: menu_data,
                        on: {
                            onAfterSelect: function (id) {

                                var me = this,
                                    item = me.getItem(id),
                                    tab = $$("tab");

                                var viewId, config = {};

                                if (typeof (item.target) == "string") {
                                    viewId = item.target;
                                    config = {
                                        id: viewId,
                                        view: item.target
                                    }
                                }
                                else {
                                    viewId = item.target.multiple ? webix.uid() : item.target.view;
                                    webix.extend(config, item.target);
                                    webix.extend(config, {
                                        id: viewId
                                    });
                                }
                                                                                                   
                                var view = $$(viewId);

                                if (!view) {
                                    addView(item.value, config, item.icon);
                                }
                                else {
                                    tab.setValue(viewId);
                                }
                                
                                me.unselect(id);
                            }
                        }
                    },
                    {
                        id: "tab",
                        view: "ui_tabview",
                        cells: [{ id: 'tabHidden', hidden: true, body: { template: '' } }],
                        value: 'tabHidden',
                        tabbar: {
                            close: true,
                            optionWidth: 180
                        }
                    }
                ]
            }
        ]
     });


    var addView = function (header, cfg, icon, ignoreLoadCollections) {

        var viewName = cfg.view;
        if (!ignoreLoadCollections && viewName) {

            var proto = webix.ui[viewName];
            if (proto.$protoWait) proto.call(webix, viewName);
            proto = webix.ui[viewName].prototype;

            var collections = proto.requireCollections || [];
            if (proto.collection) {
                collections.push(proto.collection);
            }
            
            if (collections.length>0) {
                mask();
                webix.collection.require(collections, function () {
                    unmask();
                    addView(header, cfg, icon, true);
                });
                return;
            }
        }

        var getTitle = function (title, icon) {
            if (icon) {
                return title = "<span class='webix_icon fa-" + icon + "'></span>" + title;
            }
            return title;
        }

        var tab = $$("tab");

        var id = tab.addView({
            header: getTitle(header, icon),
            close: true,
            body: cfg
        });

        view = $$(id);

        view.attachEvent("onCreateView", addView);

        view.attachEvent("onChangeTitle", function (id, title, icon) {

            var bar = tab.getTabbar();
            var i = bar.optionIndex(id);

            if (i > 0) {
                bar.config.options[i].value = getTitle(title, icon);
                bar.refresh();
            }

        });

        tab.setValue(id);
    }

    var mask = function (text) {
        var tab = $$("tab");
        tab.disable();
        tab.showProgress({
            type: "icon",
            icon: "refresh"
        });
    };

    var unmask = function () {
        var tab = $$("tab");
        tab.hideProgress();
        tab.enable();
    };

    webix.extend($$("tab"), webix.ProgressBar);

});