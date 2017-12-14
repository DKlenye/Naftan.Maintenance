webix.rules.specification = {
    name: "isNotEmpty",
    type: "isNotEmpty",
    referenceId: function (val,obj) {
        return (obj.type != 6) || val;
    }
};