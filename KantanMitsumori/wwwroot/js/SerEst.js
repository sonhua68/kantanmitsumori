// JScript File
// Create Date 2022/09/13 by HoaiPhong
let Tday = new Date();
let month = parseInt(Tday.getMonth()) + 1;
let _conNumberSort = true;
let _conNumber = 0;
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
    let d = 1;
    let Tday = new Date();
    let lastDay = parseInt(Tday.getDate());
    let lastMonth = parseInt(Tday.getMonth()) + 1;
    if (type == 1) {

        let fromY = $('#ddlFromSelectY').val();
        let fromM = $('#ddlFromSelectM').val();
        var $this = $("#ddlFromSelectD");
        if (parseInt(fromM) == (lastMonth - 3)) {
            d = lastDay + 1;
        }
        $this.empty();
        let day = new Date(fromY, fromM, 0);
        let lastDayOfMonth = parseInt(day.getDate());
        for (let i = d; i <= lastDayOfMonth; i++) {
            $this.append(new Option(i, i));
        }
        setToDayChangeMonth(type);
    } else {
        let fromY = $('#ddlToSelectY').val();
        let fromM = $('#ddlToSelectM').val();
        var $this = $("#ddlToSelectD");
        if (parseInt(fromM) == (lastMonth - 3)) {
            d = lastDay + 1;
        }
        $this.empty();
        let day = new Date(fromY, fromM, 0);
        let lastDayOfMonth = parseInt(day.getDate());
        for (let i = d; i <= lastDayOfMonth; i++) {
            $this.append(new Option(i, i));
        }
        setToDayChangeMonth(type);
    }
}
function GoNextPage(pageNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    model.pageNumber = pageNumber
    model.colSort = _conNumber;
    var result = Framework.submitAjaxLoadData(model, "/SerEst/LoadData");
    ReloadListData(result);
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
function SortData(colNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    model.pageNumber = 1;
    let number = !_conNumberSort ? getNumberSort(colNumber) : colNumber;
    model.colSort = number;
    var result = Framework.submitAjaxLoadData(model, "/SerEst/LoadData");
    $('tr#pagination').remove();
    $('#trId').twbsPagination('destroy');
    UiPagination(result[0].totalPages)
    AddPagination(result[0].totalPages);
    ReloadListData(result);
    _conNumberSort = !_conNumberSort;
    _conNumber = number;
    return false;
}
function getNumberSort(number) {
    if (number == 3) {
        return 5;
    } else if (number == 4) {
        return 6;
    } else {
        return 0;
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
function AddEstimate(value) {
    var data = value.toString().split("-");
    let EstNo = data[0];
    let EstSubNo = data[1];
    var model = {};
    model.EstNo = EstNo;
    model.EstSubNo = EstSubNo;
    var result = Framework.submitAjaxFormUpdateAsync(model, "/SerEst/AddEstimate");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPage();
    }
}
function CalcSum(value) {
    var data = value.toString().split("-");
    let EstNo = data[0];
    let EstSubNo = data[1];
    var model = {};
    model.EstNo = EstNo;
    model.EstSubNo = EstSubNo;
    var result = Framework.submitAjaxFormUpdateAsync(model, "/SerEst/CalcSum");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPage();
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
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  href="#" onclick="CalcSum(`{{estNo}}`);return false"  type = "submit"  value = "選択"/>' + '</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" href="#" onclick="AddEstimate(`{{estNo}}`);return false" type = "submit"  value = "再作成"' + '</td>' +
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
    $('tr#pagination').remove();
    for (let i = 0; i < data.length; i++) {
        table.append(row.compose(data[i]));
    }
    let TotalPages = data[0].totalPages;
    UiPagination(TotalPages);

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
function UiPagination(totalPages) {
    if (totalPages > 1) {
        $("#TableSerEst").css("display", "inline-table");
        $('#TablePage').remove();
        var tbody = $('#TableSerEst').children('tbody');
        var table = tbody.length ? tbody : $('#TableSerEst');
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
        table.append(pageTable);
    }
}
function ReloadListData(data) {
    $("#TableSerEst").css("display", "inline-table");
    var row = '<tr id="tbremote">' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" href="#" onclick="CalcSum(`{{estNo}}`);return false"   type = "submit"  value = "選択"/>' + '</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  type = "submit"  href="#" onclick="AddEstimate(`{{estNo}}`);return false"  value = "再作成"' + '</td>' +
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