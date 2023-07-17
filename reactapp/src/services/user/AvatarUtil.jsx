const defaultAvatar = "/static/img/default.svg";
const defaultBaseUrl = "http://localhost:5168/"
// const defaultBaseUrl = "https://localhost:7075/";

class AvatarUtil {
    static getUrlFromUser(user = null) {
        return user ? this.getUrl(user.avatarUrl) : defaultAvatar;
    }
    
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
