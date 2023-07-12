class User {
    static get() {
        var str = window.localStorage.getItem("user");
        return (str == null) ? null : JSON.parse(str);
    }

    static getPreference() {
        var user = get();
        return (user == null) ? null : user.preference;
    }

    static getInfo() {
        var user = get();
        return (user == null) ? null : user.info;
    }

    static save(user) {
        window.localStorage.setItem("uid", user.account.id);
        window.localStorage.setItem("user", JSON.stringify(user));
    }

    static savePreference(preference) {
        var user = get();
        if (user != null) {
            user.preference = preference;
            this.save(user);
        }
    }

    static saveInfo(info) {
        var user = get();
        if (user != null) {
            user.info = info;
            this.save(user);
        }
    }

    static drop() {
        window.localStorage.removeItem("uid");
        window.localStorage.removeItem("user");
    }
}

export default User;
