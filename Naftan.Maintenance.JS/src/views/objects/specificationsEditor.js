webix.protoUI({
    name: 'view_specificationsEditor',

    $init: function (config) {

        webix.extend(config, {

            rows: [
                {
                    view: "datatable",
                    name: "specifications",
                    select: "row",
                    navigation: true,
                    editable: true,
                    columns: [
                        { id: 'name', header: ['Наименование характеристики', { content: "textFilter" }], sort: "string", fillspace: 1, cssFormat: status },
                        {
                            id: 'value',
                            header: 'Значение',
                            template: function (obj, common, value, config) {

                                if (!value) return "";

                                if (obj.options) {
                                    return obj.map[value];
                                }

                                if (obj.format) {
                                    return webix.Date.dateToStr(obj.format)(value);
                                }

                                return value;
                            },
                            sort: "string", editor: "text", fillspace: 1, cssFormat: status
                        }
                    ]
                }
            ]
        });

        this.$ready.push(this.initEvents);

    },


    initEvents: function () {

        this.specifications = this.queryView({ name: "specifications" });

        this.specifications.attachEvent('onBeforeEditStart', function (id) {

            if (id.column != "value") return false;

            var config = this.getColumnConfig(id.column);
            var item = this.getItem(id.row);

            config.editor = item.editor;
            config.options = item.options;
        });
    },

    load: function (objectId) {

        this.mask("Загрузка данных...")

        this.define({ objectId: objectId });

        var property = this.queryView({ view: 'property' });

        webix.ajax()
            .get("/api/object/specifications/" + objectId)
            .then(webix.bind(this.onDataLoad, this), this.onErrorHandler)

    },

    save: function () {

        this.mask("Сохранение...")

        var data = this.getData();

       return  webix.ajax().headers({ "Content-Type": "application/json" })
            .post("/api/object/specifications", {data:data})
            .then(webix.bind(this.onDataLoad, this), this.onErrorHandler)

    },

    onDataLoad: function (data) {

        this.setData(data.json());
        this.unmask();
    },

    setData: function (data) {

        var dataSpecs = this.buildSpecifications(data);
        var specifications = this.specifications

        specifications.clearAll();
        specifications.parse(dataSpecs);
        specifications.refresh();

    },

    getData: function () {
        var me = this;
        var objectId = this.config.objectId;
        var specifications = this.specifications
        var specArray = [];
        specifications.data.each(function (i) {

            specArray.push({
                id: i.id > 1e10 ? 0 : i.id,
                objectId: objectId,
                specificationId: i.specificationId,
                specificationType: i.specificationType,
                value: i.value
            });
        });

        return specArray;
    },

    buildSpecifications: function (specifications) {

        var me = this;
        var specification = webix.collection("specification");
        var references = webix.collection("reference");

        return specifications.map(function (i) {

            var spec = specification.getItem(i.specificationId);

            var o = {
                name: spec.name,
                specificationId: i.specificationId,
                specificationType:i.specificationType,
                editor: me.getSpecificationType(i.specificationType),
                value: i.value
            };

            if (i.specificationType == 5) {
                o.format = "%d.%M.%Y"
            }

            if (i.specificationType == 6) {
                o.options = references.getItem(spec.referenceId).values;
                o.map = {};
                o.options.forEach(function (i) {
                    o.map[i.id] = i.value;
                })
            }
            return o;
        });

    },

    getSpecificationType: function (type) {
        return [
            "",
            "checkbox",
            "text",
            "text",
            "text",
            "date",
            "combo"
        ][type];
    },

    onErrorHandler: function () {
        this.umask();
    },

    mask: function (text) {
        this.specifications.showOverlay(text + " <span class='webix_icon fa-spinner fa-spin'></span>");
    },

    unmask: function () {
        this.specifications.hideOverlay();
    }

}, webix.ui.layout)