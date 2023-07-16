import { useEffect, useState } from 'react';
import api from '../../services/api';

// User
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


// useUser
export const useLocalUser = (update = null, callback = null) => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        let didCancel = false;

        setError(null);
        (async () => {
            try {
                setLoading(true);
                var dto = await api.get('user/current');
                if (dto.meta.status != 0) {
                    throw new Error(dto.meta.message);
                }
                setData(dto.data);
                console.log('🚀 > dto.data:', dto.data);
            } catch (err) {
                setError(err);
                callback && callback();
            } finally {
                setLoading(false);
            }
        })();
        return () => {
            didCancel = true;
        }
    }, [update]);

    return { data, loading, error };
}

export const useUser = (uid, callback = null) => {
    const [data, setData] = useState(null);
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    useEffect(() => {
        let didCancel = false;

        setError(null);
        if (uid) {
            (async () => {
                try {
                    setLoading(true);
                    var dto = await api.get('user/user', { id: uid });
                    if (dto.meta.status != 0) {
                        throw new Error(dto.meta.message);
                    }
                    setData(dto.data);
                    console.log('🚀 > dto.data:', dto.data);
                } catch (err) {
                    setError(err);
                    callback && callback();
                } finally {
                    setLoading(false);
                }
            })();
        } else {
            setData(null);
            setLoading(false);
            callback && callback();
        }
        return () => {
            didCancel = true;
        }
    }, [uid]);

    return { data, loading, error };
}
