import axios from 'axios'
import cookie from 'react-cookies'

console.log(cookie.load('.AspNetCore.Application.Id'))

// function getCookie(name) {
//     let matches = document.cookie.match(new RegExp(
//       "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
//     ));
//     return matches ? decodeURIComponent(matches[1]) : undefined;
// }

const api = axios.create({
    baseURL: 'https://localhost:44303/'
})
api.interceptors.request.use(request => requestInterceptor(request))

const requestInterceptor = (request) => {
   request.headers['Authorization'] = 'Bearer '.concat(cookie.load('.AspNetCore.Application.Id'))
   return request;
}


export default api