/*
action: {
    type: "set",
    value: [
        ...categories
    ]
}

action: {
    type: "add",
    value: {
        ...category
    }
}

action: {
    type: "delete",
    value {
        id: category.id
    }
}

action: { 
    type: "edit",
    value: {
        id: category.id,
        name: "name",
        option: {
            isPublic: true,
            allowAnonymity: true,
            allowQuestion: true,
            allowMemory: true
        }
    }
}
*/

export default function categoryReducer(state, action) {
    if (action == null) {
        return state;
    }

    var value = action.value;
    switch (action.type) {
        case "set":
            return value;
        case "add":
            // currently, do not sort immediately
            return [...state, value]
        case "delete":
            return state.map((category) => {
                if (category.id == value.id) {
                    return null;
                } else {
                    return category;
                }
            }).filter(category => category != null);
        case "edit":
            return state.map((category) => {
                if (category.id == value.id) {
                    return { ...category, name: value.name, option: value.option };
                } else {
                    return category;
                }
            });
        default:
            return state;
    }
}