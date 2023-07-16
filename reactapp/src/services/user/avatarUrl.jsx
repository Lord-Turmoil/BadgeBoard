const defaultAvatar = "/static/img/default.svg"

class AvatarUrl {
    static get(url = null) {
        return (url == null || url == "") ? defaultAvatar : url;
    }
}

export default AvatarUrl;
