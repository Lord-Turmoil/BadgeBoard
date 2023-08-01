import { useEffect, useState } from 'react';
import api from '~/services/api'

// User
class UserUtil {
    static getUid() {
        return window.localStorage.getItem('uid');
    }

    static getPreference() {
        const user = this.get();
        return (user == null) ? null : user.preference;
    }

    static getInfo() {
        const user = this.get();
        return (user == null) ? null : user.info;
    }

    static saveUid(uid) {
        window.localStorage.setItem('uid', uid);
    }

    static savePreference(preference) {
        const user = this.get();
        if (user != null) {
            user.preference = preference;
            this.save(user);
        }
    }

    static saveInfo(info) {
        const user = this.get();
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

        const dto = await api.get('user/user', { id: uid });
        if (dto.meta.status == 0) {
            return dto.data;
        } else {
            return null;
        }
    }

    // get flat user
    static flat(user) {
        return user
            ? {
                ...user.info,
                username: user.username,
                avatarUrl: user.avatarUrl
            }
            : null;
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

    // data format
    static getSexText(no) {
        switch (no) {
        case 1:
            return 'Male';
        case 2:
            return 'Female';
        default:
            return 'Unknown';
        }
    }

    static getSexNo(text) {
        if (text == null) {
            return 0;
        }
        switch (text.toLowerCase()) {
        case 'male':
            return 1;
        case 'female':
            return 2;
        default:
            return 0;
        }
    }
}

export default UserUtil;


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
                    const dto = await api.get('user/current');
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
            };
        },
        [update]);

    return { data, setData, loading, error };
};

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
                        const dto = await api.get('user/user', { id: uid });
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
            };
        },
        [uid]);

    return { data, setData, loading, error };
};

export const fetchUser = async (uid) => {
    var data = null;
    var error = null;

    try {
        const dto = await api.get('user/user', { id: uid });
        if (dto.meta.status != 0) {
            throw new Error(dto.meta.message);
        }
        data = dto.data;
    } catch (err) {
        error = err;
    }

    return [data, error];
}