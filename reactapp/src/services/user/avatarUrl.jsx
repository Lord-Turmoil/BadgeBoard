const defaultAvatar = "/static/img/default.svg"
const defaultBaseUrl = "http://localhost:5168/static"

class AvatarUrl {
    static get(url = null) {
        if (url == null || url == "") {
            return defaultAvatar;
        }
        if (url[0] == "/") {
            return defaultBaseUrl + url;
        }
        return url;
    }
}

export default AvatarUrl;
