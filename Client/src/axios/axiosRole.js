import axios from 'axios'
import cookie from 'react-cookies'

console.log(cookie.load('.AspNetCore.Application.Id'))

// function getCookie(name) {
//     let matches = document.cookie.match(new RegExp(
//       "(?:^|; )" + name.replace(/([\.$?*|{}\(\)\[\]\\\/\+^])/g, '\\$1') + "=([^;]*)"
//     ));
//     return matches ? decodeURIComponent(matches[1]) : undefined;
// }

export default axios.create({
    baseURL: 'https://localhost:44303/',
    headers: {
        Authorization:'Bearer '.concat(cookie.load('.AspNetCore.Application.Id'))
    }
})