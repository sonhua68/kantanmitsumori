// JScript File

//（iPad 対応以降不使用のため、コメントアウト）
///**********************************************
///* 車両価格DB更新
///*********************************************/
//function upCarPrice(){
//
//	//入力チェック
//	if (inputChk() == false) {return;} 
//
//    //画面から値の取得
//    
//	var estno = $get('hidEstNo').value;
//	var estsubno = $get('hidEstSubNo').value;
//	
//	var Price = $get('txtPrice').value;
//	var RakuSatu = $get('txtRakuSatu').value;
//	var Rikusou = $get('txtRikusou').value;
//	var SonotaTitle = $get('txtSonotaTitle').value;
//	var Sonota = $get('txtSonota').value;
//	
//	var SyakenSeibi = $get('txtSyakenSeibi').value;
//	var CarSum = $get('txtCarSum').value;
//	
//	
//	//車検費用か納車費用か
//	var SyakenNewZok = $get('radSyakenY').checked;
//	
//	var carprice = new Array();
//	carprice[0] = Price;
//	carprice[1] = RakuSatu;
//	carprice[2] = Rikusou;
//	carprice[3] = SyakenSeibi;
//	carprice[4] = SyakenNewZok;
//	
//	carprice[5] = SonotaTitle;
//	carprice[6] = Sonota;
//	carprice[7] = CarSum;
//	
//    asest2.WebService.updateCarPrice(estno,estsubno,carprice)
//    window.returnValue=carprice;
//    window.close();
//}


//入力チェック
function inputChk() {

    //2012-03-31 エラーメッセージ出力をHiddenからLabalに変更
	document.getElementById("txMsg").innerHTML = "";
	var outMsg;
	
	outMsg = chkKingaku("車両本体",$get('txtPrice').value);
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

	var Price = chkNull($get('txtPrice').value);
	var RakuSatu = chkNull($get('txtRakuSatu').value);
	var Rikusou = chkNull($get('txtRikusou').value);
	
	
    frm.txtSonota.value = RakuSatu + Rikusou;
    
    var SyakenSeibi = chkNull($get('txtSyakenSeibi').value);
    frm.txtCarSum.value = Price + RakuSatu + Rikusou + SyakenSeibi;
	
}
//整備費用のボタン切り替え時の金額取得
function chgSeibi() {

	var SyakenNewZok = $get('radSyakenY').checked;
	var UserNo = $get('hidUserNo').value;
	var DispVol = $get('hidDispVol').value;
	
	//整備費用呼出
	var retSeibi = asest2.WebService.getSeibiPrice(UserNo,SyakenNewZok,DispVol,setSeibi)

}
function setSeibi(retSeibi) {
    if (retSeibi != 0) {
        frm.txtSyakenSeibi.value = retSeibi;
    }
    EstSum();
}
//初期表示時のラジオボタン制御
function setSyakenNewZok() {
	var SyakenNewZok = frm.hidSyakenNewZok.value;
	if (SyakenNewZok == "Syaken") {
        frm.radSyakenY.checked = true;
    } else {
        frm.radSyakenN.checked = true;
    }
}
