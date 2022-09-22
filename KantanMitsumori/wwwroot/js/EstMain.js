setInitialValue();
//setInitValueCookie();
function setInitialValue() {
    var leaseFlag = $("#hidLeaseFlag").val();

    if (leaseFlag == "1") {
        ShowInpLeaseCalc();
    }
    else {
        ShowInpLoan();
    }
}

function ShowInpLeaseCalc() {
    document.getElementById("formTblInpLoan").setAttribute("style", "display:none");
    document.getElementById("tblSaleAll").setAttribute("style", "display:none");
}

function ShowInpLoan() {
    document.getElementById("formTblInpLease").setAttribute("style", "display:none");
    document.getElementById("tblLeaseTotal").setAttribute("style", "display:none");
}

//入力チェック
function inputChk() {

    var outMsg = "";
    var flgErr = false;

    document.getElementById("txtMsg_CustNm").innerHTML = "";
    outMsg = chkBytes("お名前", $get('CustNm_forPrint').value, 40);
    if (outMsg) {
        document.getElementById("txtMsg_CustNm").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("txtMsg_CustZip").innerHTML = "";
    outMsg = chkBytes("郵便番号", $get('CustZip_forPrint').value, 8);
    if (outMsg) {
        document.getElementById("txtMsg_CustZip").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("txtMsg_CustAdr").innerHTML = "";
    outMsg = chkBytes("住所", $get('CustAdr_forPrint').value, 60);
    if (outMsg) {
        document.getElementById("txtMsg_CustAdr").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("txtMsg_CustTel").innerHTML = "";
    outMsg = chkBytes("", $get('CustTel_forPrint').value, 13);
    if (outMsg) {
        document.getElementById("txtMsg_CustTel").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    if (flgErr) { return false; }

    return true;
}
/*
 * setInitValueCookie
 *  Create By HoaiPhong
 *  Date 2022/09/22
 /*/
function setInitValueCookie() {
    var custNm_forPrint = getCookie("CustNm_forPrint")
    var custZip_forPrint = getCookie("CustZip_forPrint")
    var custAdr_forPrint = getCookie("CustAdr_forPrint")
    var custTel_forPrint = getCookie("CustTel_forPrint")
    console.log(custNm_forPrint);
    $("#CustNm_forPrint").val(custNm_forPrint);
    $("#CustZip_forPrint").val(custZip_forPrint);
    $("#CustAdr_forPrint").val(custAdr_forPrint);
    $("#CustTel_forPrint").val(custTel_forPrint);
}
/*
 * setCookiePageMain
 *  Create By HoaiPhong
 *  Date 2022/09/22
 /*/
(function () {
    var oldVal;
    var idName = "CustNm_forPrint";
    $('#' + idName).on('change textInput input', function () {
        var val = this.value;
        if (val !== oldVal) {
            oldVal = val;
            setCookie(idName, val, 1);
        }
    });
}());
(function () {
    var oldVal;
    var idName = "CustZip_forPrint";
    $('#' + idName).on('change textInput input', function () {
        var val = this.value;
        if (val !== oldVal) {
            oldVal = val;
            setCookie(idName, val, 1);
        }
    });
}());
(function () {
    var oldVal;
    var idName = "CustAdr_forPrint";
    $('#' + idName).on('change textInput input', function () {
        var val = this.value;
        if (val !== oldVal) {
            oldVal = val;
            setCookie(idName, val, 1);
        }
    });
}());
(function () {
    var oldVal;
    var idName = "CustTel_forPrint";
    $('#' + idName).on('change textInput input', function () {
        var val = this.value;
        if (val !== oldVal) {
            oldVal = val;
            setCookie(idName, val, 1);
        }
    });
}());