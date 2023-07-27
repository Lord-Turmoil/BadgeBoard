const defaultAvatar = '/static/img/default.svg';
const defaultBaseUrl = 'http://localhost:5168/';
// const defaultBaseUrl = "https://localhost:7075/";

class AvatarUtil {
    static getUrlFromUser(user = null) {
        return user ? this.getUrl(user.avatarUrl) : defaultAvatar;
    }

    static getUrl(url = null) {
        // console.log("🚀 > AvatarUtil > getUrl > url:", url);
        if (url == null || url == '') {
            return defaultAvatar;
        }
        if (url.startsWith('/') || url.startsWith('data')) {
            return url;
        }
        return defaultBaseUrl + url;
    }
}

export default AvatarUtil;