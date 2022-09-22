swichUnit('SitaMilUnit');
resetValue();

//入力チェック
function inputChk() {
    document.getElementById("txMsg").innerHTML = "";
    var outMsg;

    //金額形式チェック
    outMsg = chkKingaku("下取車価格", $('#txtSitaPri').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("下取車残債", $('#txtSitaZan').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("走行距離", $('#txtSitaNowRun').val());
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    //日本語バイト数チェック
    outMsg = chkBytes("車名", $('#txtSitaCarName').val(), 100)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("車台番号", $('#txtSitaCarNO').val(), 30)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("走行距離単位", $('#txtSitaMilUnit').val(), 10)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("色", $('#txtSitaColor').val(), 40)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    // 入力項目を活性化
    document.getElementById("txtSitaPri").disabled = false;
    document.getElementById("txtSitaZan").disabled = false;

    return true;
}

function resetValue() {
    if (document.getElementById("radSitaM").checked == true) {
        document.getElementById("txtSitaPri").value = "";
        document.getElementById("txtSitaZan").value = "";
        document.getElementById("txtTaxFreeTradeIn").value = "0";
        document.getElementById("txtTaxTradeIn").value = "0";
        document.getElementById("txtTaxTradeInSatei").value = "0";
    }

    if (document.getElementById("radSitaU").checked == true) {
        document.getElementById("txtTaxFreeTradeIn").value = document.getElementById("hidTaxFreeTradeIn").value;
        document.getElementById("txtTaxTradeIn").value = document.getElementById("hidTaxTradeIn").value;
        document.getElementById("txtTaxTradeInSatei").value = document.getElementById("hidTaxTradeInSatei").value;
    }

    resetProp();
}

function resetProp() {
    var bkColor = "#fff";

    if (document.getElementById("radSitaM").checked == true) {
        document.getElementById("txtSitaPri").disabled = true;
        document.getElementById("txtSitaZan").disabled = true;
        document.getElementById("lblInfo1").style.display = "block";
        document.getElementById("lblInfo2").style.display = "block";
        bkColor = "#ccc"
    }

    if (document.getElementById("radSitaU").checked == true) {
        document.getElementById("txtSitaPri").disabled = false;
        document.getElementById("txtSitaZan").disabled = false;
        document.getElementById("lblInfo1").style.display = "none";
        document.getElementById("lblInfo2").style.display = "none";
        bkColor = "#d2e0fb"
    }

    for (var i = 1; i <= 14; i++) {
        document.getElementById("detail_" + i).style.backgroundColor = bkColor;
    }
}


