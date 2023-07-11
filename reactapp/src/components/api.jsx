import axios from 'axios';

// auth
axios.interceptors.request.use(config => {
    config.headers.Authorization = window.localStorage.getItem('token');
    return config
})

class API {
    constructor() {
        this._api = axios.create({
            baseURL: 'http://localhost:5168/api/'
        });
    }

    axios() {
        return this._api;
    }

    _getDto(err) {
        var data = err.response.data;
        if (Object.hasOwn(data, 'meta')) {
            return data;
        } else {
            return {
                meta: {
                    status: data.status,
                    message: data.title
                },
                data: null
            }
        }
    }

    async refresh() {
        var dto = await this._post("auth/token/refresh");
        if (dto.meta.status == 0) {
            window.localStorage.setItem("token", dto.data.token);
            window.localStorage.setItem("uid", dto.data.id);
            return dto.data;
        }

        return null;
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
        var dto = await this._post(url, body);

        // no need for retry
        if (dto.meta.status != 401) {
            return dto;
        }

        if (await this.refresh() == null) {
            // failed to refresh
            return dto
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
        var dto = this._get(url, params);
        if (dto.meta.status != 401) {
            return dto;
        }

        if (await this.refresh() == null) {
            return dto;
        }

        return await this._get(url, params);
    }
}

var api = new API();

export default api;
