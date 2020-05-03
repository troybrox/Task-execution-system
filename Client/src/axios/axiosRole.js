// Компонент запроса с основным адресом и заголовком

import axios from 'axios'
import cookie from 'react-cookies'

const api = axios.create({
    baseURL: 'https://localhost:44303/'
})
api.interceptors.request.use(request => requestInterceptor(request))

const requestInterceptor = (request) => {
   request.headers['Authorization'] = 'Bearer '.concat(cookie.load('.AspNetCore.Application.Id'))
   return request
}

export default api