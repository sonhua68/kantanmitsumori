// JScript File
// Create Date 2022/09/09 by HoaiPhong


initPage();
function GoNextPage_bk(pageNumber) {
    var model = {};
    model.sesMakID = $("#sesMakID").val();
    model.sesMaker = $("#sesMaker").val();
    model.sesCarNM = $("#sesCarNM").val();
    model.pageNumber = pageNumber
    Framework.SummitForm("/SelGrd", model)
}

function GoNextPage(pageNumber) {
    var model = {};
    model.sesMakID = $("#sesMakID").val();
    model.sesMaker = $("#sesMaker").val();
    model.sesCarNM = $("#sesCarNM").val();
    model.pageNumber = pageNumber
    var result = Framework.submitAjaxLoadData(model, "/SelGrd/LoadData");
    console.log(result);
    ReloadListData(result);
}
function ReloadListData(data) {
    $("#gvGrade").css("display", "inline-table");
    var tbody = $('#gvGrade').children('tbody');
    var table = tbody.length ? tbody : $('#gvGrade');
    var row = '<tr  id="tbremote">' +
        '<td align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:11pt;font-weight:normal;width:60px;white-space:nowrap;">' +
        '<input type="button" value="選択" onclick="Framework.GoBackReloadPageUrl("/");return false" style="font-family:ＭＳ Ｐゴシック;font-size:11pt;font-weight:bold;height:25px;">' +
        '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{gradeName}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{regularCase}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:11pt;font-weight:bold;white-space:nowrap;">{{dispVol}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{shift}}' + '</td>' +
        '<td align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{driveTypeCode}}' + '</td>' +
        '</tr>';
    $('tr#tbremote').remove();
    for (let i = 0; i < data.length; i++) {
        table.append(row.compose(data[i]));
    };
    SortPagination();
}
function initPage() {
    let TotalPages = parseFloat($("#TotalPages").val());
    console.log(TotalPages)
    UiPagination(TotalPages)
    addPagination(TotalPages);

}
function addPagination(totalPages) {
    $('#trId').twbsPagination({
        totalPages: totalPages,
        visiblePages: 10,
        next: '次',
        prev: '前',
        onPageClick: function (event, page) {
            console.log(page);
            if (page > 1) {
                GoNextPage(page)
            }
        }
    });
}
function UiPagination(totalPages) {
    if (totalPages > 1) {
        $("#gvGrade").css("display", "inline-table");
        $('#TablePage').remove();
        var tbody = $('#gvGrade').children('tbody');
        var table = tbody.length ? tbody : $('#gvGrade');

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
function SortPagination() {
    let p = 0;
    var tbody = $('#gvGrade').children('tbody');
    var items = $('#gvGrade').children('tbody')[0].childNodes;
    var itemsArr = [];
    for (i = 0; i < items.length; ++i) {
        if (i > 0 && (items[i].id) == "pagination") {
            p = i;
        } else {
            itemsArr.push(items[i]);
        }
        if (i == (items.length - 1)) {
            itemsArr.push(items[p]);
        }
    }
    for (i = 0; i < itemsArr.length; ++i) {
        tbody.append(itemsArr[i]);
    }
}