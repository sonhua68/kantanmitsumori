// JScript File
//入力チェック
function inputChk() {

    document.getElementById("txMsg").innerHTML = "";
    var outMsg;

    //金額形式チェック
    outMsg = chkKingaku("走行距離", $get('Run').value);
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("排気量", $get('Haiki').value);
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    //日本語バイト数チェック
    outMsg = chkBytes("メーカー名", $get('Maker').value, 20)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("車名", $get('CarNM').value, 100)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("グレード名", $get('Grade').value, 100)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("型式", $get('Kata').value, 30)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("車台番号", $get('CarNo').value, 30)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("走行距離単位", $get('MilUnit').value, 10)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("排気量単位", $get('DispVolUnit').value, 10)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("シフト", $get('Sft').value, 5)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("色", $get('Color').value, 50)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkBytes("装備", $get('Option').value, 150)
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    return true;

}

function SetValue(value) {
    if (value == 1) {
        $("#hidRaJrk").val(value);
    } else {
        $("#hidRaJrk").val(value);
    }

}