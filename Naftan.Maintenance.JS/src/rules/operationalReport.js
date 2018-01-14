webix.rules.operationalreport = {
    startMaintenance: function (val, obj) {
        var maintenance = obj.actualMaintenanceType > 1e10 ? null : obj.actualMaintenanceType;
        return !(maintenance && !obj.startMaintenance);
    }
};