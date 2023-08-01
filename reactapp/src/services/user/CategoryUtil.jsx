import api from "~/services/api"

export const fetchCategories = async (uid, initiator) => {
    var data = null;
    var error = null;
    var defaultCategory = null;

    if (uid == null) {
        error = "Who are you requesting for?"
        return [data, error];
    }

    try {
        var dto;
        if (initiator != null) {
            dto = await api.get('category/identified/get', { id: uid });
        } else {
            dto = await api.get('category/anonymous/get', { id: uid });
        }
        if (dto.meta.status != 0) {
            throw new Error(dto.meta.message);
        }
        console.log("🚀 > fetchCategories > dto.data:", dto.data);
        var categories = dto.data.categories;
        for (var i = 0; i < categories.length; i++) {
            if (categories[i].isDefault) {
                defaultCategory = i;
                var t = categories[0];
                categories[0] = categories[i];
                categories[i] = t;
                break;
            }
        }
        if (defaultCategory == null) {
            throw new Error("Corrupted default category");
        }
        data = categories;
    } catch (err) {
        error = err;
    }

    return [data, defaultCategory, error];
}