//入力チェック
function inputChk() {
    document.getElementById("txMsg").innerHTML = "";
    var outMsg;

    for (var i = 1; i < 3; i++) {
        outMsg = chkBytes("備考欄" + i + "行目", $('#txtNotes' + i).val(), 100);
        if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    }
    return true;

}