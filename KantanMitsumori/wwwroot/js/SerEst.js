﻿// JScript File
// Create Date 2022/09/13 by HoaiPhong
let Tday = new Date();
let month = parseInt(Tday.getMonth()) + 1;
GetListMaker();
GetDayOfMonth(1);
GetDayOfMonth(2);
SetInitToDay();
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
function SetInitToDay() {
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
        $this.empty();
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
        $this.empty();
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
    model.pageNumber = pageNumber
    var result = Framework.submitAjaxLoadData(model, "/SerEst/LoadData");
    ReloadListData(result);
}
function GoNextPage_bk(pageNumber) {
    var model = Framework.getFormData($("#FormSerEst")); 
    model.pageNumber = pageNumber
    Framework.SummitForm("/SerEst", model)
}
function LoadData(pageNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    model.pageNumber = pageNumber
    var result = Framework.submitAjaxLoadData(model, "/SerEst/LoadData");
    if (result.length > 0) {
        AddRowTable(result);
        let TotalPages = result[0].totalPages;
        AddPagination(TotalPages);
    } else {
        $("#TableSerEst").css("display", "none");
    }

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
    SetInitToDay();
    LoadData(1);
}
function Resetddl() {
    let m = `${month}`;
    $("#ddlFromSelectM").val(m)
        .find("option[value=" + m + "]").attr('selected', true);
    $("#ddlToSelectM").val(m)
        .find("option[value=" + m + "]").attr('selected', true);

}
function AddRowTable(data) {
    $("#TableSerEst").css("display", "inline-table");
    $('#TablePage').remove();
    var tbody = $('#TableSerEst').children('tbody');
    var table = tbody.length ? tbody : $('#TableSerEst');
    var row = '<tr id="tbremote">' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"   type = "submit"  value = "選択"/>' + '</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  type = "submit"  value = "再作成"' + '</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:120px;">{{estNo}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:90px;white-space:nowrap;">{{tradeDate}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{custKName}} </td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{carName}}</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style ="font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" type="submit" href="#" onclick="DeleteEstimate(`{{estNo}}`);return false"  value = "削除" ' + '</td>' +
        '</tr>';
    var pageTable = '<tr id="pagination" align="center"  style="color:White;background-color:#3C82ED;font-family:ＭＳ Ｐゴシック;font-size:14pt;font-weight:bold;white-space:nowrap;">' +
        '<td colspan  = "7">' +
        '<table border="0" id=TablePage>' +
        '<tbody>' +
        '<tr id="trId"> ' +
        '</tr> ' +
        '</tbody>' +
        '</table>' +
        '</td>' +
        '</tr> '

    $('tr#tbremote').remove();
    for (let i = 0; i < data.length; i++) {
        table.append(row.compose(data[i]));
    }
    let TotalPages = data[0].totalPages;
    if (TotalPages > 1) {
        table.append(pageTable);
    }

}
function AddPagination(totalPages) {
    $('#trId').twbsPagination({
        totalPages: totalPages,
        visiblePages: 10,
        next: '次',
        prev: '前',
        onPageClick: function (event, page) {         
            GoNextPage(page)
        }
    });
}
function ReloadListData(data) {
    $("#TableSerEst").css("display", "inline-table");
    var row = '<tr id="tbremote">' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"   type = "submit"  value = "選択"/>' + '</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  type = "submit"  value = "再作成"' + '</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:120px;">{{estNo}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:90px;white-space:nowrap;">{{tradeDate}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{custKName}} </td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{carName}}</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style ="font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" type="submit" href="#" onclick="DeleteEstimate(`{{estNo}}`);return false"  value = "削除" ' + '</td>' +
        '</tr>';
    $('tr#tbremote').remove();
    var itemsArr = [];
    for (let i = 0; i < data.length; i++) {
        itemsArr.push(row.compose(data[i]));
    };
    SortPagination(itemsArr);
}
function AddHeaderName() {
    $("#TableSerEst").css("display", "inline-table");
    var tbody = $('#TableSerEst').children('tbody');
    var table = tbody.length ? tbody : $('#TableSerEst');
    var header = '<tr id="tbremote" align="center" id="tbremote" valign="middle" style="position: static;color:White;background-color:#3C82ED;border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:11pt;height:20px;white-space:nowrap;">' +
        '<th scope = "col"> 選択' + '</th >' +
        '<th scope="col">再作成' + '</th>' +
        '<th id="SortEstNo" scope="col">' +
        '<a href = "#" onclick = "return Framework.SortDataTable("TableSerEst","SortEstNo")" style = "color:White;" > 見積書番号' +
        '</a> ' + '</th> ' +
        '<th id="SortTradeDate" scope="col">' +
        '<a href = "#" onclick ="return Framework.SortDataTable("TableSerEst","SortTradeDate")" style = "color:White;" > 見積日' +
        '</a>' + '</th> ' +
        '<th id="SortCustKName" scope="col">' +
        '<a href = "#" onclick ="return Framework.SortDataTable("TableSerEst","SortCustKName")" style = "color:White;" > カナ名' +
        '</a>' + '</th> ' +
        '<th id="SortCarName" scope="col">' +
        '<a href = "#" onclick = "return Framework.SortDataTable("TableSerEst","SortCarName")" style="color: White; ">車両 ' + '</a>' + '</th>' +
        '<th scope="col">削除' + '</th>' +
        '</tr > ';
    table.prepend(header);
}
function SortPagination(itemsArr) {
    let p = 0;
    var tbody = $('#TableSerEst').children('tbody');
    var items = $('#TableSerEst').children('tbody')[0].childNodes; 
    for (i = 0; i < items.length; ++i) {
        if (i > 0 && (items[i].id) == "pagination") {
            p = i; 
        } 
    }
    itemsArr.push(items[p]);
    for (i = 0; i < itemsArr.length; ++i) {
        tbody.append(itemsArr[i]);
    }
}