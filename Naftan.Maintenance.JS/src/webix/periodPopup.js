webix.editors.$popup.period = {
    view: "popup", width: 250, height: 250, padding: 0,
    body: {
        view: "calendar",
        icons: true,
        borderless: true,
        type:"month"
    }
};

webix.editors.period = webix.extend({
    focus: function () { },
    popupType: "period",
    setValue: function (value) {

        var parser = webix.Date.strToDate("%Y.%m");

        var _p = (value + '').split('');
        _p = (_p.slice(0, 4).concat(['.']).concat(_p.slice(4, 6))).join('');

        webix.editors.popup.setValue.call(this, parser(_p));
    },
    getValue: function () {
        var date = this.getInputNode().getValue(this._is_string ? webix.i18n.parseFormatStr : "") || "";
        return webix.Date.dateToStr("%Y%m")(date);
    },
    popupInit: function (popup) {
        popup.getChildViews()[0].attachEvent("onDateSelect", function (value) {
            webix.callEvent("onEditEnd", [value]);
        });
    }
}, webix.editors.popup);