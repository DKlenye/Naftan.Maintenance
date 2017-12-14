var menu_data = [
    {
        icon: "book", value: "Справочники", data: [
            { value: "Цеха и производства", target: 'view_department' },
            { value: "Установки", target: 'view_plant' },
            { value: "Рабочие среды", target: 'view_environment'},
            { value: "Производители", target: 'view_manufacturer' },
            { value: "Тех. характеристики", target:'view_specification' },
            { value: "Виды обслуживания", target: 'view_maintenancetype' },
            { value: "Показатели наработки", target: 'view_measureunit' },
            { value: "Пользовательские", target: 'view_reference' }
        ]
    },
    {
        icon: "table", value: "Объекты ремонта", data: [
            { value: "Группы", target:"view_objectgroup" },
            { value: "Список оборудования", target: "view_objectlist" }
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