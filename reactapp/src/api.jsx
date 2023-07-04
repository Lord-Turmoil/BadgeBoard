import axios from 'axios';

const api = axios.create({
    baseURL: 'https://localhost:7075/'
});

export default api;
