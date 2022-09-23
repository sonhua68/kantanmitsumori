//入力チェック
function inputChk() {
    //エラーメッセージ出力をHiddenからLabalに変更
    document.getElementById("txMsg").innerHTML = "";
    var outMsg;

    outMsg = chkKingaku("金額欄", $('#txtTaxCheck').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxGarage').val());
    // エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxTradeIn').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxTradeInSatei').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxRecycle').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxDelivery').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxSet1').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxSet2').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxSet3').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxOther').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeCheck').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeGarage').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeTradeIn').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeRecycle').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeOther').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeSet1').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#txtTaxFreeSet2').val());
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    //日本語バイト数チェック
    outMsg = chkBytes("項目名", $('#txtTaxFreeSet1Title').val(), 20)
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#txtTaxFreeSet2Title').val(), 20)
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("項目名", $('#txtTaxSet1Title').val(), 20)
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#txtTaxSet2Title').val(), 20)
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    outMsg = chkBytes("項目名", $('#txtTaxSet3Title').val(), 20)
    //エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    return true;
}

//手続代行費用計
function TaxCostSum() {
    //入力チェック
    if (inputChk() == false) { return; }

    var TaxCheck = chkNull($('#txtTaxCheck').val());
    var TaxGarage = chkNull($('#txtTaxGarage').val());
    var TaxTradeIn = chkNull($('#txtTaxTradeIn').val());
    var TaxTradeInSatei = chkNull($('#txtTaxTradeInSatei').val());
    var TaxRecycle = chkNull($('#txtTaxRecycle').val());
    var TaxDelivery = chkNull($('#txtTaxDelivery').val());
    var TaxSet1 = chkNull($('#txtTaxSet1').val());
    var TaxSet2 = chkNull($('#txtTaxSet2').val());
    var TaxSet3 = chkNull($('#txtTaxSet3').val());
    var TaxOther = chkNull($('#txtTaxOther').val());
    formInputSyohiyo.txtTaxCostAll.value = TaxCheck + TaxGarage + TaxTradeIn + TaxTradeInSatei +
        TaxRecycle + TaxDelivery + TaxSet1 + TaxSet2 + TaxSet3 + TaxOther;
}

//預り法定費用計
function TaxFreeSum() {
    //入力チェック
    if (inputChk() == false) { return; }

    var TaxFreeCheck = chkNull($('#txtTaxFreeCheck').val());
    var TaxFreeGarage = chkNull($('#txtTaxFreeGarage').val());
    var TaxFreeTradeIn = chkNull($('#txtTaxFreeTradeIn').val());
    var TaxFreeRecycle = chkNull($('#txtTaxFreeRecycle').val());
    var TaxFreeOther = chkNull($('#txtTaxFreeOther').val());
    var TaxFreeSet1 = chkNull($('#txtTaxFreeSet1').val());
    var TaxFreeSet2 = chkNull($('#txtTaxFreeSet2').val());
    formInputSyohiyo.txtTaxFreeAll.value = TaxFreeCheck + TaxFreeGarage + TaxFreeTradeIn + TaxFreeRecycle +
        TaxFreeOther + TaxFreeSet1 + TaxFreeSet2;
}