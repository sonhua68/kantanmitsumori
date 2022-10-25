//setInitialValue();
ShowColorChangeCarPrice();
DeleteBtnHanei()
setInitValueCookie();
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

    document.getElementById("Msg_CustNm").innerHTML = "";
    outMsg = chkBytes("お名前", $get('CustNm_forPrint').value, 40);
    if (outMsg) {
        document.getElementById("Msg_CustNm").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("Msg_CustZip").innerHTML = "";
    outMsg = chkBytes("郵便番号", $get('CustZip_forPrint').value, 8);
    if (outMsg) {
        document.getElementById("Msg_CustZip").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("Msg_CustAdr").innerHTML = "";
    outMsg = chkBytes("住所", $get('CustAdr_forPrint').value, 60);
    if (outMsg) {
        document.getElementById("Msg_CustAdr").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("Msg_CustTel").innerHTML = "";
    outMsg = chkBytes("", $get('CustTel_forPrint').value, 13);
    if (outMsg) {
        document.getElementById("Msg_CustTel").innerHTML = '<br>' + outMsg;
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
    $("#CustNm_forPrint").val(unescapeFromBase64(custNm_forPrint));
    $("#CustZip_forPrint").val(unescapeFromBase64(custZip_forPrint));
    $("#CustAdr_forPrint").val(unescapeFromBase64(custAdr_forPrint));
    $("#CustTel_forPrint").val(unescapeFromBase64(custTel_forPrint));
}
/*
 * setCookiePageMain
 *  Create By HoaiPhong
 *  Date 2022/09/22
 /*/
function decodedString(encodedString) {
    var decodedString = window.atob(encodedString);
    return decodedString;
}

function focusFunction(idName) {
    let value = $('#' + idName).val();
    var encodedString = updateByBase64(value);
    setCookie(idName, encodedString, 1);

}
function DeleteBtnHanei() {
    document.cookie = "btnHanei" + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
const ERR_MSG = "この車両はリース対象外です。\n リース対象車両は、国産車かつ初年度12年以内かつ走行距離15万km以内となります。"
const ERR_MSG_FirstRegYm = "先に初年度登録を行ってください。"
function CheckNowOdometer() {
    let hidFirstRegYm = $("#hidFirstRegYm").val();
    let hidNowOdometer = $("#hidNowOdometer").val();
    let hidMilUnit = $("#hidMilUnit").val();
    let makerName = $("#hidMakerName").val();
    if (hidFirstRegYm == "") {
        alert(ERR_MSG_FirstRegYm);
        return;
    };
    let nowOdometer = 0;
    if (hidMilUnit.includes("千km")) {
        nowOdometer = parseInt(hidNowOdometer) * 1000;
    }
    var pram = "?firstRegYm=" + hidFirstRegYm + "&makerName=" + makerName + "&nowOdometer=" + nowOdometer;
    var result = Framework.GetObjectDataFromUrl("/Estmain/CheckGoPageLease" + pram);
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPageUrl("/InpLeaseCalc")
    } else if (result.resultStatus == 0 && result.messageCode === 'I0003') {
        alert(ERR_MSG);
        return;
    } else {
        location.reload();
    }

}

function onReport(type) {
    if (inputChk() == true) {
        if (type == 1) {
            Framework.GoBackReloadPageUrl('/Report/DownloadEstimateReport');
        } else {
            Framework.GoBackReloadPageUrl('/Report/DownloadOrderReport')
        }

    }
    return false;
}
function upJiko(estno, estsubno, raJrk) {
    var model = {};
    model.EstNo = estno;
    model.EstSubNo = estsubno;
    model.raJrk = raJrk;
    var result = Framework.submitAjaxFormUpdateAsync(model, "/Estmain/UpdateJiko");
    if (result.resultStatus != 0) {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    } else {
        console.log(result.messageContent);
    }

}
function ShowColorChangeCarPrice() {
    let lblWarningRecalc = $('#lblWarningRecalc').text();
    if (lblWarningRecalc != "") {
        if (document.getElementById("lblWarningRecalc").style.display != "none") {
            document.getElementById("idDeposit").style.backgroundColor = "#FFFFCC";
            document.getElementById("idPartitionFee").style.backgroundColor = "#FFFFCC";
            document.getElementById("idKaisu").style.backgroundColor = "#FFFFCC";
            document.getElementById("idKikan").style.backgroundColor = "#FFFFCC";
            document.getElementById("idFirstPrice").style.backgroundColor = "#FFFFCC";
            document.getElementById("idSecoPrice").style.backgroundColor = "#FFFFCC";
            document.getElementById("idBonusMonth").style.backgroundColor = "#FFFFCC";
            document.getElementById("idBonusAdd").style.backgroundColor = "#FFFFCC";
        }
    }
    if (document.getElementById("lbl_GetTax").innerHTML == "") { document.getElementById("idGetTax").style.backgroundColor = "#FFFFCC"; }
    if (document.getElementById("lbl_WeightTax").innerHTML == "") { document.getElementById("idWeightTax").style.backgroundColor = "#FFFFCC"; }
    if (document.getElementById("lbl_JibaiHoken").innerHTML == "") { document.getElementById("idJibaiHoken").style.backgroundColor = "#FFFFCC"; }

}
function updateByBase64(str) {
    return escapeToBase64(str, "UTF-8")
}
function unescapeFromBase64(str) {
  return   unescapeFromBase64(str, "UTF-8");
}

