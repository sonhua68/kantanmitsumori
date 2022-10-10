const custZip_forPrint = "CustZip_forPrint";
const custNm_forPrint = "CustNm_forPrint";
const custAdr_forPrint = "CustAdr_forPrint";
const custTel_forPrint = "CustTel_forPrint";
const kbnSet = "KbnSet";
const caseSet = "CaseSet";
const sesMaker = "sesMaker";
const sesCarNM = "sesCarNM";
const btnHanei = "btnHanei";
const cookiesASEST = "CookiesASEST";
RemoveCookies(custZip_forPrint);
RemoveCookies(custNm_forPrint);
RemoveCookies(custAdr_forPrint);
RemoveCookies(custTel_forPrint);
RemoveCookies(cookiesASEST);
RemoveCookies(kbnSet);
RemoveCookies(caseSet);
RemoveCookies(sesMaker);
RemoveCookies(sesCarNM);
RemoveCookies(btnHanei);
function RemoveCookies(name) {
    document.cookie = name + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
document.getElementById('esmain').setAttribute('action', getUrl());
function getUrl() {
    let fullpath = location.protocol + '//' + location.host;
    let pathname = location.pathname.split('/')[1];
    if (pathname != null) {
        return fullpath + "/Estmain";
    } else {
        return fullpath + "/" + pathname + "/Estmain";
    }

}