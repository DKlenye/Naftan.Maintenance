webix.protoUI({
    name: "gridsuggest",
    defaults: {
        type: "datatable",
        fitMaster: false,
        width: 0,
        body: {
            navigation: true,
            select: true
        },
        filter: function (item, value) {
            var text = this.config.template(item);
            if (text.toString().toLowerCase().indexOf(value.toLowerCase()) != -1) return true;
            return false;
        }
    },
    $init: function (obj) {
        if (!obj.body.columns)
            obj.body.autoConfig = true;
        if (!obj.template)
            obj.template = webix.bind(this._getText, this);

        this.attachEvent('onValueSuggest', function () {
            webix.delay(function () {
                webix.callEvent("onEditEnd", []);
            });
        });
    },

    _getText: function (item, common) {
        var grid = this.getBody();
        var value = this.config.textValue || grid.config.columns[0].id;
        if (grid.config.getText) return grid.config.getText(item.id);
        return grid.getText(item.id, value);
    }

}, webix.ui.suggest);