webix.protoUI({
    name: "treesuggest",
    defaults: {
        type: "tree",
        fitMaster: false,
        width: 0,
        body: {
            template: "{common.icon()} {common.folder()} #name#",
            navigation: true,
            select: true
        },
        filter: function (item, value) {
            var text = item[this.config.textValue];
            return text.toString().toLowerCase().indexOf(value.toLowerCase()) !== -1;
        }
    },
    $init: function (obj) {

        if (!obj.template)
            obj.template = webix.bind(this._getText, this);

        if (obj.collection) {
            this.defaults.collection = obj.collection;
        }

        this.attachEvent('onValueSuggest', function () {
            webix.delay(function () {
                webix.callEvent("onEditEnd", []);
            });
        });

        this.$ready.push(this._initData);                
    },

    _initData: function () {
        this.getBody().parse(webix.collection(this.defaults.collection));
    },

    _getText: function (item, common) {
        return webix.templates.tree(this.getBody(), item.id, this.config.textValue);
    }

}, webix.ui.suggest);