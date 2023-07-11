import axios from 'axios';

const api = axios.create({
    baseURL: 'http://localhost:5168/api/'
});

export default api;
