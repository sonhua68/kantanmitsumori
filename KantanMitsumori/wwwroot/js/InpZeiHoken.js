// JScript File
// Create Date 2022/09/06 by HoaiPhong
// JScript File

// JScript File


setInitialValue();
function setInitialValue() {
    if (document.getElementById("MeiCarTaxEquivalent").value != "") {
        document.getElementById("chkMeiCarTaxEquivalent").checked = true;
        changeProps("MeiCarTax");
    } else {
        document.getElementById("chkMeiCarTaxEquivalent").checked = false;
        changeProps("MeiCarTax");
    }
    if (document.getElementById("MeiJibaiHokenEquivalent").value != "") {
        document.getElementById("chkMeiJibaiHokenEquivalent").checked = true;
        changeProps("MeiJibaiHoken");
    } else {
        document.getElementById("chkMeiJibaiHokenEquivalent").checked = false;
        changeProps("MeiJibaiHoken");
    }
}

function disabledMeiNiniHoken() {
    document.getElementById('MeiNiniHoken').setAttribute('disabled', true);
    document.getElementById('MeiNiniHoken').style.backgroundColor = "rgb(224, 224, 224)";
}

function inputChk() {
    document.getElementById("Msg").innerHTML = "";
    var outMsg;

    if ((document.getElementById("MeiCarTax").value != "") &&
        (document.getElementById("MeiCarTaxEquivalent").value != "")) {
        outMsg = "自動車税と自動車税相当額は、同時に設定できません。"
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    if ((document.getElementById("MeiCarTax").value != "") &&
        (document.getElementById("ddlCarTaxMonth").value == "")) {
        outMsg = "自動車税基準月を選択してください。"
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    if (document.getElementById("MeiCarTax").value != "") {
        outMsg = chkKingaku("自動車税", $('#MeiCarTax').val());
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    if (document.getElementById("MeiCarTaxEquivalent").value != "") {
        outMsg = chkKingaku("自動車税相当額", $('#MeiCarTaxEquivalent').val());
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    outMsg = chkKingaku("環境性能割", $('#MeiGetTax').val());
    if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("重量税", $('#MeiWeightTax').val());
    if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }

    if ((document.getElementById("MeiJibaiHoken").value != "") &&
        (document.getElementById("MeiJibaiHokenEquivalent").value != "")) {
        outMsg = "自賠責保険料と自賠責保険料相当額は、同時に設定できません。"
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    if ((document.getElementById("MeiJibaiHoken").value != "") &&
        (document.getElementById("ddlJibaiHokenMonth").value == "")) {
        outMsg = "自賠責保険基準月数を選択してください。"
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    if (document.getElementById("MeiJibaiHoken").value != "") {
        outMsg = chkKingaku("自賠責保険料", $('#MeiJibaiHoken').val());
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    if (document.getElementById("MeiJibaiHokenEquivalent").value != "") {
        outMsg = chkKingaku("自賠責保険料相当額", $('#MeiJibaiHokenEquivalent').val());
        if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }
    }

    outMsg = chkKingaku("任意保険料", $('#MeiNiniHoken').val());
    if (outMsg != "") { document.getElementById("Msg").innerHTML = outMsg; return false; }

    return true;

}

function EstSum() {

    //入力チェック
    if (inputChk() == false) { return; }

    var CarTax = chkNull($('#MeiCarTax').val());
    var GetTax = chkNull($('#MeiGetTax').val());
    var WeightTax = chkNull($('#MeiWeightTax').val());
    var JibaiHoken = chkNull($('#MeiJibaiHoken').val());
    var NiniHoken = chkNull($('#MeiNiniHoken').val());
    let valueTaxInsAll = Number(CarTax) + Number(GetTax) + Number(WeightTax) + Number(JibaiHoken) + Number(NiniHoken);
    $("#TaxInsAll").val(valueTaxInsAll);
    var CarTaxEquivalent = chkNull($('#MeiCarTaxEquivalent').val());
    var JibaiHokenEquivalent = chkNull($('#MeiJibaiHokenEquivalent').val());
    let valueTaxInsEquivalentAll = Number(CarTaxEquivalent) + Number(JibaiHokenEquivalent);
    $("#TaxInsEquivalentAll").val(valueTaxInsEquivalentAll);
}

function getCarTax() {
    var CarTaxMonth = chkNull($('#ddlCarTaxMonth').val());
    var DispVol = chkNull($('#DispVol').val());

    if (document.getElementById("AutoCalcFlg").value == false) { setCarTax(""); return; }
    if (CarTaxMonth == "") { setCarTax(""); return; }
    if (DispVol == "") { setCarTax(""); return; }

    ////自動車税計算
    //var retCarTax = asest2.WebService.calcCarTax(DispVol, CarTaxMonth, setCarTax)
    var model = {};
    model.CarTaxMonth = CarTaxMonth;
    model.DispVol = DispVol;
    var result = Framework.submitAjaxLoadData(model, "/InpZeiHoken/calcCarTax");
    console.log(result)
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        setCarTax(result.data)
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}

function setCarTax(retCarTax) {
    if (parseInt(retCarTax) == -1 || parseInt(retCarTax) == 0) {
        $("#MeiCarTax").val("");
        if (document.getElementById("ddlCarTaxMonth").value != "") {
            alert("自動計算はできませんでした");
        }
    } else {
        $("#MeiCarTax").val(retCarTax);
    }
    document.getElementById("chkMeiCarTaxEquivalent").checked = false;
    changeProps("MeiCarTax");
}

function getJibai() {
    var DispVol = chkNull($('#DispVol').val());
    var JibaiMonth = chkNull($('#ddlJibaiHokenMonth').val());

    if (document.getElementById("AutoCalcFlg").value == false) { setJibai(""); return; }
    if (JibaiMonth == "") { setJibai(""); return; }
    if (DispVol == "") { setJibai(""); return; }

    ////自動車税計算
    //var retJibai = asest2.WebService.calcJibai(JibaiMonth, DispVol, setJibai)	
    var model = {};
    model.JibaiMonth = JibaiMonth;
    model.DispVol = DispVol;
    var result = Framework.submitAjaxLoadData(model, "/InpZeiHoken/calcJibai");
    console.log(result)
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        setJibai(result.data)
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}

function setJibai(retJibai) {
    if (retJibai != "") {
        $("#MeiJibaiHoken").val(retJibai)

    } else {
        $("#MeiJibaiHoken").val("");
        if (document.getElementById("ddlJibaiHokenMonth").value != "") {
            alert("自動計算はできませんでした");
        }
    }
    document.getElementById("chkMeiJibaiHokenEquivalent").checked = false;
    changeProps("MeiJibaiHoken");
}

function clickBox(element) {

    if (document.getElementById("chk" + element + "Equivalent").checked == true) {

        if ((document.getElementById("ConTaxInputKb").value == "True") && isFinite(document.getElementById("TaxPercent").value)) {
            document.getElementById(element + "Equivalent").value = Math.floor(document.getElementById(element).value * (Number(1) + Number(document.getElementById("TaxPercent").value)));
        } else {
            document.getElementById(element + "Equivalent").value = Math.floor(document.getElementById(element).value);
        }

        if (document.getElementById(element + "Equivalent").value == "0") { document.getElementById(element + "Equivalent").value = "" }
        changeProps(element);

    } else {

        if (element == "MeiCarTax") {
            getCarTax();
        } else {
            getJibai();
        }
    }

}

function changeProps(element) {

    // 相当額の活性／非活性制御
    if (document.getElementById("chk" + element + "Equivalent").checked == true) {

        document.getElementById(element + "Equivalent").style.backgroundColor = "#FFF";
        document.getElementById(element + "Equivalent").readOnly = false;

        document.getElementById(element).value = "";
        document.getElementById(element).style.background = "#E0E0E0";
        document.getElementById(element).readOnly = true;

    } else {

        document.getElementById(element + "Equivalent").value = "";
        document.getElementById(element + "Equivalent").style.backgroundColor = "#E0E0E0";
        document.getElementById(element + "Equivalent").readOnly = true;

        document.getElementById(element).style.background = "#FFF";
        document.getElementById(element).readOnly = false;
    }

    EstSum();

}
