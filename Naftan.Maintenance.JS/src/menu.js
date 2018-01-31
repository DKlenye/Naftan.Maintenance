var menu_data = [
    {
        icon: "book", open: true, value: "Справочники", data: [
            { icon: "industry", value: "Цеха и производства", target: 'view_department' },
            { icon: "building-o", value: "Установки", target: 'view_plant' },
            { icon:"list-alt", value: "Тех. характеристики", target:'view_specification' },
            { icon: "cog", value: "Виды обслуживания", target: 'view_maintenancetype' },
            { icon: "tachometer", value: "Показатели наработки", target: 'view_measureunit', width: 200 },
            { icon: "warning", value: "Причины ремонта", target: 'view_maintenanceReason'},
            { icon: "address-book-o", value: "Пользовательские", target: 'view_reference' }
        ]
    },
    {
        icon: "table", open:true, value: "Объекты ремонта", data: [
            { icon: "sitemap", value: "Группы", target:"view_objectgroup" },
            { icon: "superpowers", value: "Оборудование", target: "view_objectlist" },
            { icon: "clock-o", value: "Интервалы ремонта", target: "view_groupinterval" },
            { icon: "cogs", value: "Наработка", target: "view_usage" }
        ]
    },
    {
        icon: "wrench", open: true, value: "ППР и ТО", data: [
            { icon: "file-text-o", value: "Оперативный отчёт", target: "view_operationalreport" },
            { icon: "print", value: "Печать оп. отчёта", target: "report_operationalreport" },
            { icon: "calendar", value: "График ППР", target: "view_plan" },
            { icon: "print", value: "Печать графика ППР", target: "report_plan" }
        ]
    },
    {
        icon: "server", open: true, value: "Настройки", data: [
            { icon: "user", value: "Пользователи", target: "view_userlist" }
        ]
    }
];