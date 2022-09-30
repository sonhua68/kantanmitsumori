var frm = document.getElementById('formInpCarPrice');
function inputChk() {

    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	document.getElementById("txMsg").innerHTML = "";
	var outMsg;
	
	outMsg = chkKingaku("車両本体",$get('txtCarPrice').value);
    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") {document.getElementById("txMsg").innerHTML = outMsg; return false;}
	
	outMsg = chkKingaku("落札料",$get('txtRakuSatu').value);
    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") {document.getElementById("txMsg").innerHTML = outMsg; return false;}
	
	outMsg = chkKingaku("陸送代",$get('txtRikusou').value);
    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") {document.getElementById("txMsg").innerHTML = outMsg; return false;}

	outMsg = chkKingaku("整備費用",$get('txtSyakenSeibi').value);
    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") {document.getElementById("txMsg").innerHTML = outMsg; return false;}

	outMsg = chkBytes("その他費用のタイトル",$get('txtSonotaTitle').value,20)
    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	if (outMsg != "") {document.getElementById("txMsg").innerHTML = outMsg; return false;}

    return true;
    
}

//その他費用のタイトル入力時
function chgSonotaSumTitle() {

    var SonotaTitle = $get('txtSonotaTitle').value;
    frm.txtSonotaSumTitle.value = SonotaTitle + "計";

}

//合計
function EstSum() {

	//入力チェック
	if (inputChk() == false) {return;} 

	var Price = chkNull($get('txtCarPrice').value);
	var RakuSatu = chkNull($get('txtRakuSatu').value);
	var Rikusou = chkNull($get('txtRikusou').value);
	
	
    frm.txtSonota.value = RakuSatu + Rikusou;
    
    var SyakenSeibi = chkNull($get('txtSyakenSeibi').value);
    frm.txtCarSum.value = Price + RakuSatu + Rikusou + SyakenSeibi;
	
}
//整備費用のボタン切り替え時の金額取得
function chgSeibi() {
	var isSyakenZok = $get('radSyakenY').checked;
	var userSyakenZok = $get('hidUserSyakenZok').value;
	var userSyakenNew = $get('hidUserSyakenNew').value;
	if(isSyakenZok)
		setSeibi(userSyakenZok);
	else
		setSeibi(userSyakenNew);
}

function setSeibi(retSeibi) {
    if (retSeibi != 0) {
        frm.txtSyakenSeibi.value = retSeibi;
    }
    EstSum();
}
