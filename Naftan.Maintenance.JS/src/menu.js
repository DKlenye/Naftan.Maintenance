var menu_data = [
    {
        icon: "book", open: true, value: "Справочники", data: [
            { icon: "industry", value: "Цеха и производства", target: 'view_department' },
            { icon: "building-o", value: "Установки", target: 'view_plant' },
            { icon:"list-alt", value: "Тех. характеристики", target:'view_specification' },
            { icon: "cog", value: "Виды обслуживания", target: 'view_maintenancetype' },
            { icon: "tachometer", value: "Показатели наработки", target: 'view_measureunit', width:200 },
            { icon: "address-book-o", value: "Пользовательские", target: 'view_reference' }
        ]
    },
    {
        icon: "table", open:true, value: "Объекты ремонта", data: [
            { icon: "sitemap", value: "Группы", target:"view_objectgroup" },
            { icon: "superpowers", value: "Оборудование", target: "view_objectlist" }
        ]
    }
    /*,
    {
        id: "uis", icon: "puzzle-piece", value: "UI Components", data: [
            { id: "dataview", value: "DataView" },
            { id: "list", value: "List" },
            { id: "menu", value: "Menu" },
            { id: "tree", value: "Tree" }
        ]
    },
    {
        id: "tools", icon: "calendar-o", value: "Tools", data: [
            { id: "kanban", value: "Kanban Board" },
            { id: "pivot", value: "Pivot Chart" },
            { id: "scheduler", value: "Calendar" }
        ]
    },
    {
        id: "forms", icon: "pencil-square-o", value: "Forms", data: [
            { id: "buttons", value: "Buttons" },
            { id: "selects", value: "Select boxes" },
            { id: "inputs", value: "Inputs" }
        ]
    }*/
];