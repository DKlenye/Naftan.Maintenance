webix.ready(function () {

     window.chat = $.connection.chatHub;
     window.chat.client.showMessage = function (message) {
         webix.message(message);
     };

     webix.i18n.setLocale("ru-RU");
     webix.i18n.parseFormatStr = webix.Date.dateToStr("%d.%m.%Y");
     webix.i18n.parseFormatDate = webix.Date.strToDate("%d.%m.%Y");


    
    // Хак, который предотвращает появление ошибки после динамической замены эдитора
    
     webix.editors.date.getInputNode = function () {
         try {
             return this.getPopup().getChildViews()[0];
         }
         catch (ex) {
             return {
                 isVisible: function () { return false}
             }
         }
     }

    //--


     webix.collection.tree(["objectGroup"]);

     $.connection.hub.start();

     webix.ajax()
         .get("/api/user/current")
         .then(function (user) {


             webix.ui({
                 rows: [
                     {
                         view: "toolbar", css: "app_header", padding: 3, elements: [
                             {
                                 view: "button", type: "icon", icon: "bars",
                                 width: 30, align: "center", css: "app_button", click: function () {
                                     $$("$sidebar1").toggle();
                                 }
                             },
                             { width: 20 },
                             {
                                 view: "template",
                                 borderless: !0,
                                 css: "logo",
                                 template: "<img src='Content/images/1-50-32.png' />",
                                 width: 36,
                                 height: 34
                             },
                             {
                                 view: "label",
                                 width: 600,
                                 label: "Система управления техническим обслуживанием и ремонтом",
                                 css: "header_label"
                             },
                             {},
                             webix.debug ?
                                 {
                                     view: "icon", icon: "file", click: function () {
                                         webix.ajax()
                                             .post("/api/database/updateScript").
                                             then(function (e) {

                                                 var win = webix.ui({
                                                     view: 'window',
                                                     move: true,
                                                     resize: true,
                                                     position: "center",
                                                     width: 1000,
                                                     height:800,
                                                     head: {
                                                         view: "toolbar", margin: -4, cols: [
                                                             { view: "label", label: "Скрипт для изменения базы данных" },
                                                             {
                                                                 view: "icon", icon: "times-circle",
                                                                 click: function () { win.close(); }
                                                             }
                                                         ]
                                                     },
                                                     body: {
                                                         view: "form",
                                                         borderless: true,
                                                         elements: [
                                                             {
                                                                 view: "textarea",
                                                                 value: eval(e.text())
                                                             }
                                                         ]
                                                     }
                                                 });
                                                 win.show();

                                             }, function (e) { webix.message(e.response, 'error') })
                                     }
                                 } :
                                 {},
                             webix.debug ?
                                 {
                                     view: "icon", icon: "database", click: function () {

                                         webix.confirm(
                                             {
                                                 title: "Обновление базы данных",
                                                 text: "Будет выполнен скрипт, вносящий изменение в базу данных. Применить изменения?",
                                                 ok: "Да",
                                                 cancel: "Нет",
                                                 type: "alert-error",
                                                 callback: function (isOk) {
                                                     if (isOk) {
                                                         webix.ajax()
                                                             .post("/api/database/update").
                                                             then(function () { webix.message('База обновлена') }, function (e) { webix.message(e.response, 'error') })
                                                     }
                                                 }
                                             });
                                     }
                                 } :
                                 {}


                         ]
                     },
                     {
                         cols: [
                             {
                                 view: "sidebar",
                                 multipleOpen: true,
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
                                             addView(item.value, config, item.icon, item.width);
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


             var addView = function (header, cfg, icon, width, ignoreLoadCollections) {

                 var viewName = cfg.view;
                 if (!ignoreLoadCollections && viewName) {

                     var proto = webix.ui[viewName];
                     if (proto.$protoWait) proto.call(webix, viewName);
                     proto = webix.ui[viewName].prototype;

                     var collections = proto.requireCollections || [];
                     if (proto.collection) {
                         collections.push(proto.collection);
                     }

                     if (collections.length > 0) {
                         mask();
                         webix.collection.require(collections, function () {
                             unmask();
                             addView(header, cfg, icon, width, true);
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
                     body: cfg,
                     width: width
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

});