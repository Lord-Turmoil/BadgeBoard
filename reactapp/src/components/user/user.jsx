import api from '../api';
class User {
    static get() {
        var str = window.localStorage.getItem('user');
        var user = (str == null) ? null : JSON.parse(str);
        console.log('🚀 > User > get > user:', user);
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
        window.localStorage.setItem('uid', user.account.id);
        window.localStorage.setItem('user', JSON.stringify(user));
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
        window.localStorage.removeItem('uid');
        window.localStorage.removeItem('user');
    }

    static async fetch(uid) {
        console.log('🚀 > User > fetch > uid:', uid);
        if (uid == null) {
            return null;
        }

        var dto = await api.get('user/user', { id: uid })
        if (dto.meta.status == 0) {
            return dto.data;
        } else {
            return null;
        }
    }

    // get flat user
    static flat(user) {
        return user ? {
            ...user.info,
            username: user.username,
            avatarUrl: user.avatarUrl
        } : null;
    }

    // detailed getter and setter
    static username(user) {
        return user ? user.username : null;
    }

    static motto(user) {
        return user ? user.info.motto : null;
    }

    static birthday(user) {
        return user ? user.info.birthday : null;
    }
}

export default User;
