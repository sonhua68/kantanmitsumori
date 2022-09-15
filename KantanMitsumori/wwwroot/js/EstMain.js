setInitialValue();
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
    outMsg = chkBytes("お名前", $get('txtCustNm_forPrint').value, 40);
    if (outMsg) {
        document.getElementById("txtMsg_CustNm").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("txtMsg_CustZip").innerHTML = "";
    outMsg = chkBytes("郵便番号", $get('txtCustZip_forPrint').value, 8);
    if (outMsg) {
        document.getElementById("txtMsg_CustZip").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("txtMsg_CustAdr").innerHTML = "";
    outMsg = chkBytes("住所", $get('txtCustAdr_forPrint').value, 60);
    if (outMsg) {
        document.getElementById("txtMsg_CustAdr").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    document.getElementById("txtMsg_CustTel").innerHTML = "";
    outMsg = chkBytes("", $get('txtCustTel_forPrint').value, 13);
    if (outMsg) {
        document.getElementById("txtMsg_CustTel").innerHTML = '<br>' + outMsg;
        flgErr = true;
    }

    if (flgErr) { return false; }

    return true;
}
