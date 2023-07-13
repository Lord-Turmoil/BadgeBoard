import api from "../api";
class User {
    static get() {
        var str = window.localStorage.getItem("user");
        var user = (str == null) ? null : JSON.parse(str);
        console.log("🚀 ~ file: user.jsx:6 ~ User ~ get ~ user:", user);
        return user;
    }

    static getPreference() {
        var user = this.get();
        return (user == null) ? null : user.preference;
    }

    static getInfo() {
        var user = this.get();
        return (user == null) ? null : user.info;
    }

    static save(user) {
        window.localStorage.setItem("uid", user.account.id);
        window.localStorage.setItem("user", JSON.stringify(user));
    }

    static savePreference(preference) {
        var user = this.get();
        if (user != null) {
            user.preference = preference;
            this.save(user);
        }
    }

    static saveInfo(info) {
        var user = this.get();
        if (user != null) {
            user.info = info;
            this.save(user);
        }
    }

    static drop() {
        window.localStorage.removeItem("uid");
        window.localStorage.removeItem("user");
    }

    static async fetch(uid) {
        console.log("🚀 ~ file: user.jsx:46 ~ User ~ fetch ~ uid:", uid)
        if (uid == null) {
            return null;
        }

        var dto = await api.get("user/user", { id: uid })
        if (dto.meta.status == 0) {
            return dto.data;
        } else {
            return null;
        }
    }
}

export default User;
