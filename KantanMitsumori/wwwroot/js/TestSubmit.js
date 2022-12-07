const kbnSet = "KbnSet";
const caseSet = "CaseSet";
const sesMaker = "sesMaker";
const sesCarNM = "sesCarNM";
const btnHanei = "btnHanei";
const callKbn = "CallKbn";
CleanCookies();
function RemoveCookies(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
document.getElementById('esmain').setAttribute('action', getUrl());
function getUrl() {
    let fullpath = location.protocol + '//' + location.host;
    let pathname = location.pathname.split('/')[1];
    if (pathname == '') {
        return fullpath + "/Estmain";
    } else {
        return fullpath + "/" + pathname + "/Estmain";
    }
}

function CleanCookies() {    
    RemoveCookies(kbnSet);
    RemoveCookies(caseSet);
    RemoveCookies(sesMaker);
    RemoveCookies(sesCarNM);
    RemoveCookies(btnHanei);
    RemoveCookies(callKbn);
}