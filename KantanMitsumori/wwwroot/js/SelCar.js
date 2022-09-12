// JScript File
// Create Date 2022/09/09 by HoaiPhong
const selectDefMaker = "メーカーを選択して下さい";
const selectDefCarName = "車種を選択して下さい"
const def_SelMakerMsg = "メーカーを選択して下さい"
const def_SelModelMsg = "車種を選択して下さい"
const def_SelTypeMsg = "型式指定は必ず入力して下さい"
const def_GradeNotFoundMsg = "該当するグレードが見つかりません"
setIntData();
GetListASOPMaker();
function GetListASOPMaker() {
    var result = Framework.GetObjectDataFromUrl("/SelCar/GetListASOPMaker");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        console.log(result.data)
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
function setIntData() {
    $("#ddlModel").append(new Option(selectDefCarName, "-1"));
    $("#ddlModel option[value='-1']").attr("selected", "selected");
    $('#ddlModel').attr("disabled", true);
    $('#btnNextGrade').attr("disabled", true);

}
function GetListASOPCarName() {
    $("#ddlModel").empty();
    let vMarkId = $("#ddlMaker").val();
    setIntData()
    console.log(vMarkId)
    var result = Framework.GetObjectDataFromUrl("/SelCar/GetListASOPCarName?markId=" + vMarkId);
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;
        for (let i = 0; i < length; i++) {
            let text = result.data[i].carmodelCode;
            let value = result.data[i].carmodelName;
            $("#ddlModel").append(new Option(value, text));
            $('#ddlModel').attr("disabled", false);
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
function onchangeCarName() {
    let vCarName = $("#ddlModel").val();
    if (vCarName == "-1") {
        $('#btnNextGrade').attr("disabled", true);
    } else {
        $('#btnNextGrade').attr("disabled", false);

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
    var result = Framework.submitAjaxGetList(model, "/SelCar/ChkModel");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
    } else {
        if (typeof (Items) != "undefined") {
            $("#lblErrMsg2").html(def_GradeNotFoundMsg)
        } else {         
            Framework.SummitForm("/SelGrd", result)
        }
    }

}
function btnNextGrade() {
    var model = {};
    model.sesMakID = $("#ddlMaker").val();
    model.sesMaker = $("#ddlMaker option:selected").text();
    model.sesCarNM = $("#ddlModel option:selected").text();
    var result = Framework.submitAjaxGetList(model, "/SelCar/NextGrade");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            $("#lblErrMsg1").html(def_GradeNotFoundMsg)
        } else {
            Framework.SummitForm("/SelGrd", result)
        }
    }
}