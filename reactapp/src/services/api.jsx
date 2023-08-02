import axios from 'axios';

class Api {
    constructor() {
        this._api = axios.create({
            withCredentials: true,
            baseURL: 'http://localhost:5168/api/'
            // baseURL: 'https://localhost:7075/api/'
        });
        this._api.interceptors.request.use(config => {
            config.headers.Authorization = window.localStorage.getItem('token');
            return config;
        });
    }

    axios() {
        return this._api;
    }

    _getDto(err) {
        if (!Object.hasOwn(err, 'response')) {
            return {
                meta: {
                    status: 101,
                    message: 'Connection error, try again later'
                },
                data: {
                    name: err.name,
                    message: err.message
                }
            };
        }
        const response = err.response;
        const ret = { meta: { status: response.status ?? 66, message: err.message }, data: null };

        // no data
        if (!response.data || response.data == '') {
            return ret;
        }

        const data = err.response.data;
        if (Object.hasOwn(data, 'meta')) {
            return data;
        } else {
            return {
                meta: {
                    status: data.status,
                    message: data.title
                },
                data: null
            };
        }
    }

    async refresh() {
        const dto = await this._post('auth/token/refresh');
        console.log("🚀 > Api > refresh > dto.meta.status:", dto.meta.status);
        if (dto.meta.status == 0) {
            this.saveToken(dto.data.token);
            return true;
        }

        return false;
    }

    // single post
    async _post(url, body) {
        return await this._api.post(url, body).then(res => {
            // 200 must be our custom data
            return res.data;
        }).catch(err => {
            return this._getDto(err);
        });
    }

    // post with retry
    async post(url, body) {
        const dto = await this._post(url, body);

        // no need for retry
        if (dto.meta.status != 401) {
            return dto;
        }

        if (!await this.refresh()) {
            // failed to refresh
            return dto;
        }

        return await this._post(url, body);
    }

    async _get(url, params) {
        return await this._api.get(url, { params: params }).then(res => {
            // 200 must be our custom data
            return res.data;
        }).catch(err => {
            return this._getDto(err);
        });
    }

    async get(url, params) {
        const dto = await this._get(url, params);
        if (dto.meta.status != 401) {
            return dto;
        }

        if (await this.refresh() == null) {
            return dto;
        }

        return await this._get(url, params);
    }

    saveToken(token) {
        window.localStorage.setItem('token', `bearer ${token}`);
    }

    dropToken() {
        window.localStorage.removeItem('token');
    }
}

var api = new Api();

export default api;