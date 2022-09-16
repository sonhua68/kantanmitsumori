// JScript File
// Create Date 2022/09/16 by HoaiPhong
// JScript File


getUserDefData();
resetValue();
setTitle();
//入力チェック
function inputChk() {
    document.getElementById("txMsg").innerHTML = "";
    var outMsg;
    outMsg = chkNum("ローン計算標準金利", $('#Rate').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkMin("ローン計算標準金利", $('#Rate').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#YtiRiekiH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#SyakenNewH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#SyakenZokH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxFreeCheckH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxFreeGarageH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxFreeTradeInH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxCheckH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxGarageH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxTradeInH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxTradeInChkH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxRecycleH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxDeliveryH').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxSet1H').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxSet2H').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxSet3H').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#YtiRiekiK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#SyakenNewK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#SyakenZokK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxFreeCheckK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxFreeGarageK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxFreeTradeInK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxCheckK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxGarageK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxTradeInK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxTradeInChkK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxRecycleK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxDeliveryK').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxSet1K').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxSet2K').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkKingaku("金額欄", $('#TaxSet3K').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#TaxFreeSet1Title').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#TaxFreeSet2Title').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#TaxSet1Title').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#TaxSet2Title').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#TaxSet3Title').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("販売店", $('#ShopNm').val(), 100)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("住所", $('#ShopAdr').val(), 120)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("電話番号", $('#ShopTel').val(), 13)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("見積担当者", $('#EstTanName').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("責任者", $('#Sekinin').val(), 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("販売店URL", $('#MemberURL').val(), 80)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    return true;

}

function setTitle() {
    setTitleConTax("lblRiekiCap");
    setTitleConTax("lblSyaken");
    setTitleConTax("lblSyakenNew");
    setTitleConTax("lblSyakenZok");
    setTitleConTax("lblDaikoAll");
    setTitleConTax("lblTaxCheck");
    setTitleConTax("lblTaxGarage");
    setTitleConTax("lblTaxTradeIn");
    setTitleConTax("lblTaxTradeInChk");
    setTitleConTax("lblTaxRecycle");
    setTitleConTax("lblTaxDelivery");
    setTitleConTax("lblTaxSet1Title");
    setTitleConTax("lblTaxSet2Title");
    setTitleConTax("lblTaxSet3Title");
}

function setTitleConTax(element) {

    if (document.getElementById(element) != null) {
        if (document.getElementById("hidConTaxInputKb").value == "False") {
            textBefore = "（税込）";
            textAfter = "（税抜）";
        } else {
            textBefore = "（税抜）";
            textAfter = "（税込）";
        }
        if (document.getElementById(element).innerHTML != "") {
            if (document.getElementById(element).innerHTML.indexOf(textBefore) != -1) {
                document.getElementById(element).innerHTML = document.getElementById(element).innerHTML.replace(textBefore, textAfter);
            }
            if (document.getElementById(element).innerHTML.indexOf(textAfter) == -1) {
                document.getElementById(element).innerHTML = document.getElementById(element).innerHTML + textAfter;
            }
        } else {
            document.getElementById(element).innerHTML = textAfter;
        }
    }

}

function resetValue() {
    if ((document.getElementById("radInTax").checked == true) &&
        (document.getElementById("hidConTaxInputKb").value == "False")) {
        document.getElementById("hidConTaxInputKb").value = "True";
        setCalc();
    }
    if ((document.getElementById("radOutTax").checked == true) &&
        (document.getElementById("hidConTaxInputKb").value == "True")) {
        document.getElementById("hidConTaxInputKb").value = "False";
        setCalc();
    }
}

function setCalc() {
    setTitle();
    setNewValue("YtiRiekiH");
    setNewValue("SyakenNewH");
    setNewValue("SyakenZokH");
    setNewValue("TaxCheckH");
    setNewValue("TaxGarageH");
    setNewValue("TaxTradeInH");
    setNewValue("TaxTradeInChkH");
    setNewValue("TaxRecycleH");
    setNewValue("TaxDeliveryH");
    setNewValue("TaxSet1H");
    setNewValue("TaxSet2H");
    setNewValue("TaxSet3H");
    setNewValue("YtiRiekiK");
    setNewValue("SyakenNewK");
    setNewValue("SyakenZokK");
    setNewValue("TaxCheckK");
    setNewValue("TaxGarageK");
    setNewValue("TaxTradeInK");
    setNewValue("TaxTradeInChkK");
    setNewValue("TaxRecycleK");
    setNewValue("TaxDeliveryK");
    setNewValue("TaxSet1K");
    setNewValue("TaxSet2K");
    setNewValue("TaxSet3K");
}



function setNewValue(element) {
    var hundredfoldTax = 100 + (getTaxPercentValue() * 100);
    if (document.getElementById(element) != null) {
        if (document.getElementById(element).value != "") {
            if (document.getElementById("hidConTaxInputKb").value == "False") {
                if (isFinite(document.getElementById(element).value)) {
                    wkVal = (document.getElementById(element).value * 100) / hundredfoldTax;
                    document.getElementById(element).value = Math.floor(wkVal);
                }
            } else {
                if (isFinite(document.getElementById(element).value)) {
                    wkVal = document.getElementById(element).value * hundredfoldTax;
                    document.getElementById(element).value = Math.floor(wkVal / 100);
                }
            }
        }
    }

}

function getTaxPercentValue() {
    if (document.getElementById("TaxRatio1") != null) {
        if (document.getElementById("TaxRatio1").checked == true) return 0.05
    }
    if (document.getElementById("TaxRatio2") != null) {
        if (document.getElementById("TaxRatio2").checked == true) return 0.08
    }
    if (document.getElementById("TaxRatio3") != null) {
        if (document.getElementById("TaxRatio3").checked == true) return 0.1
    }
    return 0

}
function getUserDefData() {
    var result = Framework.GetObjectDataFromUrl("/InpInitVal/GetUserDefData");
    console.log(result)
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        SetInitData(result.data)
    } else {
        Framework.resetForm();
    }
}
function SetInitData(data) {    
    var ConTaxInputKb = $('#hidConTaxInputKb').val();
    if (ConTaxInputKb.toLowerCase() == "false") {
        $("#radOutTax").prop("checked", true);
    } else {
        $("#radInTax").prop("checked", true);
    }
    $('#YtiRiekiH').val(StringNull(data.ytiRiekiH));
    $('#YtiRiekiK').val(StringNull(data.ytiRiekiK));
    $('#SyakenNewH').val(StringNull(data.syakenNewH));
    $('#SyakenNewK').val(StringNull(data.syakenNewK));
    $('#SyakenZokH').val(StringNull(data.syakenZokH));
    $('#SyakenZokK').val(StringNull(data.syakenZokK));
    $('#TaxFreeCheckH').val(StringNull(data.taxFreeCheckH));
    $('#TaxFreeCheckK').val(StringNull(data.taxFreeCheckK));
    $('#TaxFreeGarageH').val(StringNull(data.taxFreeGarageH));
    $('#TaxFreeGarageK').val(StringNull(data.taxFreeGarageK));
    $('#TaxFreeTradeInH').val(StringNull(data.taxFreeTradeInH));
    $('#TaxFreeTradeInK').val(StringNull(data.taxFreeTradeInK));
    $('#TaxCheckH').val(StringNull(data.taxCheckH));
    $('#TaxCheckK').val(StringNull(data.taxCheckK));
    $('#TaxGarageH').val(StringNull(data.taxGarageH));
    $('#TaxGarageK').val(StringNull(data.taxGarageK));
    $('#TaxTradeInH').val(StringNull(data.taxTradeInH));
    $('#TaxTradeInK').val(StringNull(data.taxTradeInK));
    $('#TaxTradeInChkH').val(StringNull(data.taxTradeInChkH));
    $('#TaxTradeInChkK').val(StringNull(data.taxTradeInChkK));
    $('#TaxRecycleH').val(StringNull(data.taxRecycleH));
    $('#TaxRecycleK').val(StringNull(data.taxRecycleK));
    $('#TaxDeliveryH').val(StringNull(data.taxDeliveryH));
    $('#TaxDeliveryK').val(StringNull(data.taxDeliveryK));
    $('#TaxSet1H').val(StringNull(data.taxSet1H));
    $('#TaxSet1K').val(StringNull(data.taxSet1K));
    $('#TaxSet2H').val(StringNull(data.taxSet2H));
    $('#TaxSet2K').val(StringNull(data.taxSet2K));
    $('#TaxSet3H').val(StringNull(data.taxSet3H));
    $('#TaxSet3K').val(StringNull(data.taxSet3K));
    $('#TaxSet1Title').val(StringNull(data.taxSet1Title));
    $('#TaxSet2Title').val(StringNull(data.taxSet2Title));
    $('#TaxSet3Title').val(StringNull(data.taxSet3Title));
    $('#TaxFreeSet1Title').val(StringNull(data.taxFreeSet1Title));
    $('#TaxFreeSet2Title').val(StringNull(data.taxFreeSet2Title));
    $('#TaxFreeSet1H').val(StringNull(data.taxFreeSet1H));
    $('#TaxFreeSet1K').val(StringNull(data.taxFreeSet1K));
    $('#TaxFreeSet2H').val(StringNull(data.taxFreeSet2H));
    $('#TaxFreeSet2K').val(StringNull(data.taxFreeSet2K));
    $('#Rate').val(data.rate)
    $('#ShopNm').val(data.shopNm)
    $('#ShopAdr').val(data.shopAdr)
    $('#ShopTel').val(data.shopTel)
    $('#EstTanName').val(data.estTanName)
    $('#Sekinin').val(data.sekininName)
    $('#hidUserNo').val(data.userNo)
    $('#MemberURL').val(data.memberUrl)
    let asArticle = data.asArticle;
    console.log(asArticle)
    if (asArticle == 1) {
        $("#radUse").prop("checked", true);
    } else {
        $("#radDisuse").prop("checked", true);
    }
}