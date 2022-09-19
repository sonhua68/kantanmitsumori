// 入力チェック
function inputChk() {
    // エラーメッセージ出力をHiddenからLabalに変更
    document.getElementById("txMsg").innerHTML = "";
    var outMsg;

    var name = $('#txtCustKanaName').val();

    if (isZenKana(name) == false) {
        outMsg = "全角カナのみで入力して下さい。";
    } else {
        outMsg = "";
    }
    // エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    //日本語バイト数チェック
    outMsg = chkBytes("メモ欄", $('#txtCustMemo').val(), 60)
    // エラーメッセージ出力をHiddenからLabalに変更
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    return true;
}
