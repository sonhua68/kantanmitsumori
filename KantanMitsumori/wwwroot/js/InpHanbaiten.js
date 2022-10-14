// JScript File
// Create Date 2022/09/06 by HoaiPhong
// JScript File
SetTextArea();
function inputChk() {	
	document.getElementById("txMsg").innerHTML = "";
	var outMsg;
	//日本語バイト数チェック
	outMsg = chkBytes("販売店",$('#HanName').val(), 100)
	//2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
	outMsg = chkBytes("住所",$('#HanAdd').val(), 120)
	//2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
	outMsg = chkBytes("電話番号",$('#Tel').val(), 13)
	//2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
	outMsg = chkBytes("見積担当者",$('#TantoName').val(), 20)
	//2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
	outMsg = chkBytes("責任者",$('#Sekinin').val(), 20)
	//#2067 エラーメッセージ出力をHiddenからLabalに変更 2022/06/16 by Huy
	if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
	return true;
}

function SetTextArea() {
	var $this = $("#HanName");
		$this.css({ resize: "both" });	
}