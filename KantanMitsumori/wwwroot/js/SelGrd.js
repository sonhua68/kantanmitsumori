// JScript File
// Create Date 2022/09/09 by HoaiPhong
let vCaseSet = getCookie("CaseSet");
let vKbnSet = getCookie("KbnSet");
let _conNumberSort = true;
let _conNumber = 0;
InitPage();
function GoNextPage(pageNumber) {
    var model = {};
    model.sesMakID = $("#sesMakID").val();
    model.sesMaker = $("#sesMaker").val();
    model.sesCarNM = $("#sesCarNM").val();
    model.CaseSet = vCaseSet;
    model.KbnSet = vKbnSet;
    model.colSort = _conNumber;
    model.pageNumber = pageNumber
    var result = Framework.submitAjaxLoadData(model, "/SelGrd/LoadData");
    ReloadListData(result);
}

function SortData(colNumber) {
    var model = {};
    model.sesMakID = $("#sesMakID").val();
    model.sesMaker = $("#sesMaker").val();
    model.sesCarNM = $("#sesCarNM").val();
    model.CaseSet = vCaseSet;
    model.KbnSet = vKbnSet;
    model.pageNumber = 1;
    let number = !_conNumberSort ? getNumberSort(colNumber) : colNumber;
    model.colSort = number;
    var result = Framework.submitAjaxLoadData(model, "/SelGrd/LoadData");
    $('tr#pagination').remove();
    $('#trId').twbsPagination('destroy');
    UiPagination(result[0].totalPages)
    AddPagination(result[0].totalPages);
    ReloadListData(result);
    _conNumberSort = !_conNumberSort;
    _conNumber = number;
}
function getNumberSort(number) {
    if (number == 3) {
        return 5;
    } else if (number == 2) {
        return 7;
    } else if (number == 4) {
        return 6;
    } else {
        return 0;
    }
}
function ReloadListData(data) {
    $("#gvGrade").css("display", "inline-table");
    var row = '<tr id="tbremote">' +
        '<td align="left" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:11pt;font-weight:normal;width:60px;white-space:nowrap;">' +
        '<input type="submit" value="選択" onclick="SetFreeEst(`{{gradeName}}`,`{{regularCase}}`,`{{dispVol}}`);return false" style="font-family:ＭＳ Ｐゴシック;font-size:11pt;font-weight:bold;height:25px;">' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{gradeName}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{regularCase}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:11pt;font-weight:bold;white-space:nowrap;">{{dispVol}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{shift}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{driveTypeCode}}' + '</td>' +
        '</tr>';
    $('tr#tbremote').remove();
    var itemsArr = [];
    for (let i = 0; i < data.length; i++) {
        itemsArr.push(row.compose(data[i]));
    };
    SortPagination(itemsArr);
}
function InitPage() {
    let TotalPages = parseFloat($("#TotalPages").val());
    UiPagination(TotalPages)
    AddPagination(TotalPages);

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
        $("#gvGrade").css("display", "inline-table");
        $('#TablePage').remove();
        var tbody = $('#gvGrade').children('tbody');
        var table = tbody.length ? tbody : $('#gvGrade');
        var pageTable = '<tr id="pagination" align="center" style="color:White;background-color:#3C82ED;font-family:ＭＳ Ｐゴシック;font-size:14pt;font-weight:bold;white-space:nowrap;">' +
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
function SortPagination(itemsArr) {
    let p = 0;
    var tbody = $('#gvGrade').children('tbody');
    var items = $('#gvGrade').children('tbody')[0].childNodes;
    for (i = 0; i < items.length; ++i) {
        if (i > 0 && (items[i].id) == "pagination") {
            p = i;
        }
    }
    if (p != 0) {
        itemsArr.push(items[p]);
    }
    for (i = 0; i < itemsArr.length; ++i) {
        tbody.append(itemsArr[i]);
    }
}

function SetFreeEst(gradeName, carCase, dispVol) {
    var model = {};
    model.MakerName = $("#sesMaker").val();
    model.ModelName = $("#sesCarNM").val();
    model.GradeName = gradeName;
    model.CarCase = carCase;
    model.DispVol = dispVol;
    var result = Framework.submitAjaxFormUpdateAsync(model, "/SelGrd/SetFreeEst");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPage();
    }
}
