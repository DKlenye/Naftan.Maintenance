(function () {

    var serverName = 'db2.lan.naftan.by',
        folderName = 'Maintenance',
        defaultParams = {
            "rs:Command": "Render",
            "rc:Toolbar": false,
            "rs:ClearSession": true
        },
        defaultExportFormat = 'excel';

    webix.Reporter = {
        getUrl: function (reportName, params) {

            var str = "", obj = webix.extend(params || {}, defaultParams);
            for (var key in obj) str += "&"+ key + "=" + encodeURIComponent(obj[key]);

            var template = [
                "http://",
                serverName,
                "/ReportServer/Pages/ReportViewer.aspx?/",
                folderName,
                "/",
                reportName,
                str
            ];

            return template.join("");
        },

        exportReport: function (reportName, params, format) {

            var formatParams = {
                "rs:Format": format || defaultExportFormat
            };

            params = params || {};
            webix.extend(params, formatParams);
            location.href = this.getUrl(reportName, params);
        }

    };
})();