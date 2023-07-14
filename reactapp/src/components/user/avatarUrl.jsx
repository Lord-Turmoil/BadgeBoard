const defaultAvatar = "/static/img/default.svg"

class AvatarUrl {
    static get(url = null) {
        return (url == null) ? defaultAvatar : url;
    }
}

export default AvatarUrl;
