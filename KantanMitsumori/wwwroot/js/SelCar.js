// JScript File
// Create Date 2022/09/09 by HoaiPhong
const selectDefMaker = "メーカーを選択して下さい";
const selectDefCarName = "車種を選択して下さい"
const def_SelMakerMsg = "メーカーを選択して下さい"
const def_SelModelMsg = "車種を選択して下さい"
const def_SelTypeMsg = "型式指定は必ず入力して下さい"
const def_GradeNotFoundMsg = "該当するグレードが見つかりません"
const def_DataNotFoundMsg_NULL = "該当するデータがありませんでした<br>[車名を直接入力する >>] ボタンから作成してください"
GetListASOPMaker();
SetIntData();
SetInitCarSet();
setCookie("btnHanei", "1", 1);
function GetListASOPMaker() {
    var result = Framework.GetObjectDataFromUrl("/SelCar/GetListASOPMaker");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;
        $("#ddlMaker").append(new Option(selectDefMaker, '-1'));
        $("#ddlMaker option[value='" + selectDefMaker + "']").attr("selected", "selected");
        for (let i = 0; i < length; i++) {
            let text = result.data[i].makerCode;
            let value = result.data[i].makerName;
            $("#ddlMaker").append(new Option(value, text));
        }

    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            $("#lblErrMsg2").html(Items)
        } else {
            location.reload();
        }
    }
}
function SetIntData() {
    let vMarkId = getCookie("sesMaker");
    if (vMarkId != "") {
        $("#ddlMaker").val(vMarkId)
        GetListASOPCarName();
        let sesCarNM = getCookie("sesCarNM");
        Framework.SetSelectedNumber("ddlModel", sesCarNM)
        $('#btnNextGrade').attr("disabled", false);
    } else {
        $("#ddlModel").append(new Option(selectDefCarName, "-1"));
        $("#ddlModel option[value='-1']").attr("selected", "selected");
        $('#ddlModel').attr("disabled", true);
        $('#btnNextGrade').attr("disabled", true);
    }

}
function ReSetddlMaker() {
    $("#ddlModel").append(new Option(selectDefCarName, "-1"));
    $("#ddlModel option[value='-1']").attr("selected", "selected");
    $('#ddlModel').attr("disabled", true);
    $('#btnNextGrade').attr("disabled", true);
}
function GetListASOPCarName() {
    $("#lblErrMsg1").html("");
    DeleteValue();
    let vMarkId = $("#ddlMaker").val();
    var result = Framework.GetObjectDataFromUrl("/SelCar/GetListASOPCarName?markId=" + vMarkId);
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        $("#ddlModel").empty();
        ReSetddlMaker();
        let length = result.data.length;
        for (let i = 0; i < length; i++) {
            let text = result.data[i].carmodelCode;
            let value = result.data[i].carmodelName;
            $("#ddlModel").append(new Option(value, text));
            $('#ddlModel').attr("disabled", false);
        }
    } else if (result.resultStatus == 0 && result.messageCode == 'I0003') {
        $("#lblErrMsg1").html(def_DataNotFoundMsg_NULL);
        ReSetddlMaker();
    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            $("#lblErrMsg2").html(Items)
        } else {
            location.reload();
        }
    }

}
function onchangeCarName() {
    let vCarName = $("#ddlModel").val();
    if (vCarName == "-1") {
        $('#btnNextGrade').attr("disabled", true);
    } else {
        $('#btnNextGrade').attr("disabled", false);
        CleanCarSet();
    }
}
function btnChkModel() {
    let caseSet = $("#CaseSet").val();
    if (caseSet == "") {
        $("#lblErrMsg2").text(def_SelTypeMsg);
        return;
    }
    var model = {};
    model.CaseSet = caseSet;
    model.KbnSet = $("#KbnSet").val();
    var result = Framework.submitAjaxLoadData(model, "/SelCar/ChkModel");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
    } else if (result.resultStatus == 0 && result.messageCode != 'I0003') {

    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            $("#lblErrMsg2").html(def_GradeNotFoundMsg)
        } else {
            DeleteValue();
            Framework.SummitForm("/SelGrd", result)
            setCookie("CaseSet", model.CaseSet, 1);
            setCookie("KbnSet", model.KbnSet, 1);
        }
    }

}
function btnNextGrade() {
    var model = {};
    model.sesMakID = $("#ddlMaker").val();
    model.sesMaker = $("#ddlMaker option:selected").text();
    model.sesCarNM = $("#ddlModel option:selected").text();
    var result = Framework.submitAjaxLoadData(model, "/SelCar/NextGrade");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
    } else if (result.resultStatus == 0 && result.messageCode != 'I0003') {

    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            $("#lblErrMsg1").html(def_GradeNotFoundMsg)
        } else {
            Framework.SummitForm("/SelGrd", result)
            setValue()

        }
    }
}

function setValue() {
    let sesMaker = $('#ddlMaker').find(":selected").val();
    let sesCarNM = $('#ddlModel').find(":selected").val();
    setCookie("sesMaker", sesMaker, 1);
    setCookie("sesCarNM", sesCarNM, 1);
}
function DeleteValue() {
    document.cookie = "sesMaker" + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = "sesCarNM" + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
function SetInitCarSet() {
    let vCaseSet = getCookie("CaseSet");
    let vKbnSet = getCookie("KbnSet");
    if (vCaseSet != "") {
        $("#ddlMaker option[value='-1']").attr("selected", "selected");
        ReSetddlMaker();
        $("#CaseSet").val(vCaseSet);
        $("#KbnSet").val(vKbnSet);
    }
  
}
function CleanCarSet() {
    $("#CaseSet").val("");
    $("#KbnSet").val("");
    DeleteValueCarSet();
}
function DeleteValueCarSet() {
    document.cookie = "KbnSet" + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
    document.cookie = "CaseSet" + '=; Path=/; Expires=Thu, 01 Jan 1970 00:00:01 GMT;';
}
function OnclickSelCar() {
    DeleteValue();
    CleanCarSet();
    Framework.GoBackReloadPageUrl('/SerEst');
}
function AddEstimate() {    
    var model = {};   
    var result = Framework.submitAjaxFormUpdateAsync(model, "/SelCar/SetFreeEst");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPage();
    }
}