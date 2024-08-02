//import { Interop } from './js/interop'

window.getAntiForgeryToken = () => {
    var elements = document.getElementsByName('__RequestVerificationToken');
    if (elements.length > 0) {
        return elements[0].value
    }

    console.warn('no anti forgery token found!');
    return null;
}

window.cookieStorage = {
    get: (cname) => {
        var name = cname + "=";
        var decodedCookie = decodeURIComponent(document.cookie);
        var ca = decodedCookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    },
    set: (cookie) => {
        document.cookie = cookie;
    },
    delete: (cookieName) => {
        document.cookie = cookieName + '=; expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    }
};
