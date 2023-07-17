const defaultAvatar = "/static/img/default.svg"
const defaultBaseUrl = "http://localhost:5168/"

class AvatarUtil {
    static getUrl(url = null) {
        if (url == null || url == "") {
            return defaultAvatar;
        }
        if (url.startsWith("static")) {
            return defaultBaseUrl + url;
        }
        return url;
    }
}

export default AvatarUtil;
