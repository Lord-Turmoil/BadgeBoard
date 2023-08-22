import api from "../api";

// c-categoryId-initiatorId
function _getCategoryKey(category, initiator) {
    var key = "c-" + category.id;
    if (initiator) {
        key += "-" + initiator.account.id;
    }
    return key;
}

// save all badges to session storage
export const saveBadges = (category, badges, initiator = null) => {
    if (badges == null) {
        return;
    }
    var key = _getCategoryKey(category, initiator);
    var value = JSON.stringify(badges);

    window.sessionStorage.setItem(key, value);
}

const getCache = (key) => {
    var value = window.sessionStorage.getItem(key);
    if (value) {
        return JSON.parse(value);
    } else {
        return null;
    }
}

export const getBadges = async (category, timestamp, initiator = null) => {
    var key = _getCategoryKey(category, initiator);
    var cache = getCache(key);
    var badges = cache;
    // if (!cache || cache.timestamp == null) {
    {
        var [data, error] = await fetchBadges(category, timestamp, initiator);
        if (error) {
            return [null, error];
        }
        badges = data;
    }
    if (badges) {
        console.log("🚀 > getBadges > badges:", badges);
        saveBadges(category, badges, initiator);
        return [badges, null];
    } else {
        return [null, "Badges lost"];
    }
}

export const fetchBadges = async (category, timestamp, initiator = null) => {
    var data = null;
    var error = null;

    var url = initiator ? "badge/browse/identified/category/" : "badge/browse/anonymous/category/";
    url += category.id;

    try {
        var dto;
        if (timestamp) {
            dto = await api.get(url, { timestamp: timestamp });
        } else {
            dto = await api.get(url);
        }
        data = dto.data;
    } catch (err) {
        error = err;
    }

    return [data, error];
}
