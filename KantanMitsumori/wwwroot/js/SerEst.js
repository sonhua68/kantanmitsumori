// JScript File
// Create Date 2022/09/13 by HoaiPhong
let Tday = new Date();
let month = parseInt(Tday.getMonth()) + 1;
GetListMaker();
GetDayOfMonth(1);
GetDayOfMonth(2);
setInitToDay();
LoadData(1);
function GetListMaker() {
    ; var result = Framework.GetObjectDataFromUrl("/SerEst/GetMakerNameAndModelName?makerName=");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;
        $("#ddlMaker").append(new Option("", ''));
        for (let i = 0; i < length; i++) {
            let value = result.data[i].makerName;
            $("#ddlMaker").append(new Option(value, value));
        }

    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            location.reload();
        }
    }

}

function setInitToDay() {

    let lastDay = parseInt(Tday.getDate());
    $("#ddlFromSelectD option[value='" + lastDay + "']").attr("selected", "selected");
    $("#ddlToSelectD option[value='" + lastDay + "']").attr("selected", "selected");
}
function setToDayChangeMonth(type) {
    let Tday = new Date();
    let lastDay = parseInt(Tday.getDate());
    let month = parseInt(Tday.getMonth()) + 1;
    if (type == 1) {
        let fromM = parseInt($('#ddlFromSelectM').val());
        if (fromM == month) {
            $("#ddlFromSelectD option[value='" + lastDay + "']").attr("selected", "selected");
        } else {
            $("#ddlFromSelectD option[value='1']").attr("selected", "selected");
        }
    } else {
        let toM = parseInt($('#ddlToSelectM').val());
        if (toM == month) {
            $("#ddlToSelectD option[value='" + lastDay + "']").attr("selected", "selected");
        } else {
            $("#ddlToSelectD option[value='1']").attr("selected", "selected");
        }

    }
}
function setIntData() {
    $("#ddlModel").append(new Option("", ""));
    $("#ddlModel option[value='']").attr("selected", "selected");

}
function GetListModel() {
    let vMark = $("#ddlMaker").val();
    if (vMark != '') {
        var result = Framework.GetObjectDataFromUrl("/SerEst/GetMakerNameAndModelName?makerName=" + vMark);
        if (result.resultStatus == 0 && result.messageCode === 'I0002') {
            $("#ddlModel").empty();
            setIntData();
            let length = result.data.length;
            for (let i = 0; i < length; i++) {
                let value = result.data[i].modelName;
                $("#ddlModel").append(new Option(value, value));
            }
        } else {
            let Items = result.data;
            if (typeof (Items) != "undefined") {
                location.reload();
            }
        }
    } else {
        $("#ddlModel").empty();
    }

}
function GetDayOfMonth(type) {
    if (type == 1) {
        let fromY = $('#ddlFromSelectY').val();
        let fromM = $('#ddlFromSelectM').val();
        var $this = $("#ddlFromSelectD");
        let day = new Date(fromY, fromM, 0);
        let lastDayOfMonth = parseInt(day.getDate());
        for (let i = 1; i <= lastDayOfMonth; i++) {
            $this.append(new Option(i, i));
        }
        setToDayChangeMonth(type);
    } else {
        let fromY = $('#ddlToSelectY').val();
        let fromM = $('#ddlToSelectM').val();
        var $this = $("#ddlToSelectD");
        let day = new Date(fromY, fromM, 0);
        let lastDayOfMonth = parseInt(day.getDate());
        for (let i = 1; i <= lastDayOfMonth; i++) {
            $this.append(new Option(i, i));
        }
        setToDayChangeMonth(type);
    }
}
function GoNextPage(pageNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    console.log(model);
    model.pageNumber = pageNumber
    var result = Framework.submitAjaxGetList(model, "/SerEst/LoadData");
    console.log(result);
    AddRowTable(result);
}
function GoNextPage_bk(pageNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    console.log(model);
    model.pageNumber = pageNumber
    Framework.SummitForm("/SerEst", model)
}

function LoadData(pageNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    console.log(model);
    model.pageNumber = pageNumber
    var result = Framework.submitAjaxGetList(model, "/SerEst/LoadData");
    AddRowTable(result);

}
function DeleteEstimate(value) {
    var data = value.toString().split("-");
    let EstNo = data[0];
    let EstSubNo = data[1];
    var model = {};
    model.EstNo = EstNo;
    model.EstSubNo = EstSubNo;
    var result = Framework.submitAjaxFormUpdateAsync(model, "/SerEst/DeleteEstimate");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        LoadData(1)
    } else {
        LoadData(1)
    }
}
function Cleanform() {
    Resetddl();
    $("#EstNo").val("");
    $("#EstSubNo").val("");
    $("#CustKanaName").val("");
    $("#ChassisNo").val("");
    $("#ddlMaker").empty();
    $("#ddlModel").empty();
    $("#ddlToSelectD").empty();
    $("#ddlFromSelectD").empty();
    GetListMaker();
    GetDayOfMonth(1);
    GetDayOfMonth(2);
    setInitToDay();
}
function Resetddl() {
    let m = `${month}`;
    $("#ddlFromSelectM").val(m)
        .find("option[value=" + m + "]").attr('selected', true);
    $("#ddlToSelectM").val(m)
        .find("option[value=" + m + "]").attr('selected', true);

}
function AddRowTable(data) {
    if (data.length > 0) {
        $("#TableSerEst").css("display", "inline-table");
        $('#TablePage').remove();
        var tbody = $('#TableSerEst').children('tbody');
        var table = tbody.length ? tbody : $('#TableSerEst');
        var row = '<tr>' +
            '<td id="tbremote" align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"   type = "submit"  value = "選択"/>' + '</td>' +
            '<td id="tbremote" align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  type = "submit"  value = "再作成"' + '</td>' +
            '<td id="tbremote" align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:120px;">{{estNo}}</td>' +
            '<td id="tbremote" align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:90px;white-space:nowrap;">{{tradeDate}}</td>' +
            '<td id="tbremote" align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{custKName}} </td>' +
            '<td id="tbremote" align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{carName}}</td>' +
            '<td id="tbremote" align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style ="font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" type="submit" href="#" onclick="DeleteEstimate(`{{estNo}}`);return false"  value = "削除" ' + '</td>' +
            '</tr>';
        var pageTable = '<tr id="tbremote" align="center" style="color:White;background-color:#3C82ED;font-family:ＭＳ Ｐゴシック;font-size:14pt;font-weight:bold;white-space:nowrap;">' +
            '<td colspan = "7">' +
            '<table border="0" id=TablePage>' +
            '<tbody>' +
            '<tr id="trId"> ' +
            '</tr> ' +
            '</tbody>' +
            '</table>' +
            '</td>' +
            '</tr> '

        $('td#tbremote').remove();
        for (let i = 0; i < data.length; i++) {
            table.append(row.compose(data[i]));
        }
        let TotalPages = data[0].totalPages;
        table.append(pageTable)
        if (TotalPages > 1) {
            addPageding(data);
        }
    } else {
        $("#TableSerEst").css("display", "none");
    }

}
function addPageding(data) {
    let TotalPages = data[0].totalPages;
    let PageIndex = data[0].pageIndex;
    console.log(TotalPages);
    console.log(PageIndex);
    var pageding1 =
        '<td id="tbremote">' +
        '<span onclick="GoNextPage({{pageNumber}});return false" style="color:White;">{{vNumber}}</span>' +
        '</td>'
    var pageding =
        '<td id="tbremote">' +
        '<a href="#" onclick="GoNextPage({{pageNumber}});return false" style="color:White;">{{vNumber}}</a>' +
        '</td>'
    var tbodyPage = $('#TablePage').children('tbody');
    var tablePage = tbodyPage.length ? tbodyPage : $('#TablePage');
    let indexPage = PageIndex >= 10 ? PageIndex : 0;
    let TotalPagesNew = TotalPages > 10 ? 10 : TotalPages;

    for (let i = 1; i <= TotalPagesNew; i++) {
        if (PageIndex == i) {
            tablePage.find("#trId").append(pageding1.compose({
                'pageNumber': indexPage + i,
                'vNumber': +indexPage + i
            }));
        } else if (i == 10) {
            tablePage.find("#trId").append(pageding.compose({
                'pageNumber': (indexPage + i),
                'vNumber': '...'
            }));

            return;
        } else {
            if (TotalPages < indexPage + i) {
                return;
            } else {
                tablePage.find("#trId").append(pageding.compose({
                    'pageNumber': indexPage + i,
                    'vNumber': indexPage + i
                }));
            }

        }
    }
}

