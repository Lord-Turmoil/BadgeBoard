/*
action: {
    type: "set",
    value: [ 
        ...state 
    ]
}

action: {
    type: "add",
    value: { 
        ...badge
    }
}

action: {
    type: "update",
    value: {
        id: badge.id,
        badge: {
            ...badge
        }
    }
}

action: {
    type: "delete",
    value: {
        id: badge.id
    }
}

action: {
    type: "edit",
    value: {
        id: badge.id
        style: "style",
        isPublic: true
    }
}

action: {
    type: "editAnswer",
    value: {
        id: badge.id,
        answer: "answer"
    }
}

action: {
    type: "editMemory",
    value: {
        id: badge.id,
        memory: "memory"
    }
}
*/

export default function badgeReducer(state, action) {
    if (action == null) {
        return state;
    }
    console.log("🚀 > badgeReducer > state:", state);
    console.log("🚀 > badgeReducer > action:", action);

    var value = action.value;
    switch (action.type) {
        case "set":
            return value;
        case "add":
            return [...state, value];
        case "update":
            return state.map((badge) => {
                if (badge.id == value.id) {
                    return value.badge;
                } else {
                    return badge;
                }
            });
        case "delete":
            return state.map((badge) => {
                if (badge.id == value.id) {
                    return null;
                } else {
                    return badge;
                }
            }).filter(badge => badge != null);
        case "edit":
            return state.map((badge) => {
                if (badge.id == value.id) {
                    return { ...badge, style: value.style, isPublic: value.isPublic };
                } else {
                    return badge;
                }
            });
        case "editAnswer":
            return state.map((badge) => {
                if (badge.type == 1 && badge.id == value.id) {
                    return { ...badge, payload: { ...badge.payload, answer: value.answer } };
                } else {
                    return badge;
                }
            });
        case "editMemory":
            return state.map((badge) => {
                if (badge.type == 2 && badge.id == value.id) {
                    return { ...badge, payload: { ...badge.payload, memory: value.memory } };
                } else {
                    return badge;
                }
            });
        default:
            return state;
    }
}
