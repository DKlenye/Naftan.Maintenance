webix.DataCollectionPull = function (config) {
    
    var hub = $.connection.dataHub;
    hub.client.dataChangeHandler = webix.bind(this._onServerDataChange,this);
      
    webix.extend(this, {
        trees:[],
        pull: {},
        templateUrl: "json->api/",
        hub : hub
    });

};

webix.DataCollectionPull.prototype = {
    
    getCollection: function (name) {
        name = name.toLowerCase();
        var store = this.pull[name];
        if (!store)
            store = this._initCollection(name);
        return store
    },

    initCollections: function (names, loadCallback, errorCallback) {

        var me = this, counter = 0;
        names = names || [];

        var afterLoad = function () {
            if (++counter == names.length && loadCallback) loadCallback();
        };

        var loadError = function () {
            if (errorCallback) errorCallback();
        };

        var collections = names.map(function (name) {
            name = name.toLowerCase();
            if (me.pull[name]) {
                afterLoad();
            }
            else {
                me._initCollection(name, webix.once(afterLoad), loadError)
            }

        });
                
    },

    _getUrl: function (name){
        return {
            url: this.templateUrl + name
        }
    },

    _initCollection: function (name, loadCallback, errorCallback) {

        var me = this, store;
        var isTree = this.trees.indexOf(name) != -1;

        if (!isTree) {
            store = new webix.DataCollection({
                save: {
                    url: this.templateUrl + name,
                    updateFromResponse: true
                },
                rules: webix.rules[name]
            });

            webix.dp(store).attachEvent("onAfterSync", function (state, text, data, loader) {
                var data = data.json() || {};
                var id = state.id > 1e10 ? 0 : state.id;
                me.hub.server.dataChange(name, id, state.status, data);
            });
        }
        else {
            store = new webix.TreeCollection()
        }

        if (loadCallback) store.attachEvent("onAfterLoad", loadCallback);
        if (errorCallback) store.attachEvent("onLoadError", errorCallback);

        store.define(this._getUrl(name));
        me.pull[name] = store;
        return store;
    },


    _onServerDataChange: function (collection, id, operation, data) {

        var store = this.pull[collection]; 

        if (store) {

            var dp = webix.dp(store);
            dp.off();

            switch (operation) {
                case "insert": {
                    store.add(data);
                    break;
                }
                case "update": {
                    store.updateItem(id, data);
                    break;
                }
                case "delete":{
                    store.remove(id);
                    break;
                }
            }

            dp.on();

        }
    }
};

webix.ready(function () {

    webix.collection = (function () {

        var dataCollectionPull = new webix.DataCollectionPull();

        var fn = function (name) {
            return dataCollectionPull.getCollection(name);
        }

        fn.tree = function (names) {
            names = names || [];
            dataCollectionPull.trees = dataCollectionPull.trees.concat(names.map(function (i) { return i.toLowerCase(); }));
        }

        fn.options = function (name, textColumn, emptyValueEnable, filter) {
            var store = dataCollectionPull.getCollection(name);
            var options = [];

            if (emptyValueEnable === true) {
                options.push({ id: "", value: "" });
            }

            store.data.each(function (i) {
                if (filter && !filter(i)) {
                }
                else {
                    options.push({
                        id: i.id,
                        value: i[textColumn || "name"]
                    });
                }
                
            });
            
            return options;
        }

        fn.require = function (names, loadCallback, errorCallback) {
            dataCollectionPull.initCollections(names, loadCallback, errorCallback);
        }

        fn.pull = dataCollectionPull;
        
        return fn;

    })();

});
