// JScript File
// Create Date 2022/09/13 by HoaiPhong
let Tday = new Date();
const currentYear = Tday.getFullYear();
const currentMonth = parseInt(Tday.getMonth());
const currentDay = Tday.getDate();
let month = parseInt(Tday.getMonth()) + 1;
var $thisFromY = "#ddlFromSelectY";
var $thisFromM = "#ddlFromSelectM";
var $thisFromD = "#ddlFromSelectD";
var $thisToY = "#ddlToSelectY";
var $thisToM = "#ddlToSelectM";
var $thisToD = "#ddlToSelectD";
let _conNumberSort = true;
let _conNumber = 0;
InitSelectList($thisFromY, $thisFromM, $thisFromD, currentYear, currentMonth, "this", "first",1)
InitSelectList($thisToY, $thisToM, $thisToD, currentYear, currentMonth, "this", "first",2)
GetListMaker();
//GetDayOfMonth(1);
//GetDayOfMonth(2);
SetInitToDay();
LoadData(1);
setCookie("btnHanei", "1", 1);

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
    model.colSort = 11;
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

function SortData(colNumber) {
    var model = Framework.getFormData($("#FormSerEst"));
    let sort = parseInt($("#SortPage").val());
    if (sort == 0) {
        let val = colNumber + sort;
        $("#SortPage").val(val)
        _conNumber = val;
    } else if (sort == colNumber) {
        let val = colNumber + 1;
        $("#SortPage").val(val);
        _conNumber = val;
    } else {
        $("#SortPage").val(colNumber);
        _conNumber = colNumber;
    }
    model.pageNumber = 1;
    model.colSort = _conNumber;
    var result = Framework.submitAjaxLoadData(model, "/SerEst/LoadData");
    $('tr#pagination').remove();
    $('#trId').twbsPagination('destroy');
    UiPagination(result[0].totalPages)
    AddPagination(result[0].totalPages);
    ReloadListData(result);
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
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  href="javascript:void(0)" onclick="CalcSum(`{{estNo}}`);return false"  type = "submit"  value = "選択"/>' + '</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" href="javascript:void(0)" onclick="AddEstimate(`{{estNo}}`);return false" type = "submit"  value = "再作成"' + '</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:120px;">{{estNo}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:90px;white-space:nowrap;">{{tradeDate}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{custKName}} </td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{carName}}</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style ="font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" type="submit" href="javascript:void(0)" onclick="DeleteEstimate(`{{estNo}}`);return false"  value = "削除" ' + '</td>' +
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
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" href="javascript:void(0)" onclick="CalcSum(`{{estNo}}`);return false"   type = "submit"  value = "選択"/>' + '</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style = "font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;"  type = "submit"  href="javascript:void(0)" onclick="AddEstimate(`{{estNo}}`);return false"  value = "再作成"' + '</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:120px;">{{estNo}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;width:90px;white-space:nowrap;">{{tradeDate}}</td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;white-space:nowrap;">{{custKName}} </td>' +
        '<td  align="left" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10pt;font-weight:bold;">{{carName}}</td>' +
        '<td  align="center" valign="middle" style="border-color:White;border-width:1px;border-style:Solid;font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:normal;width:70px;white-space:nowrap;">' + '<input style ="font-family:ＭＳ Ｐゴシック;font-size:10.5pt;font-weight:bold;height:25px;width:65px;" type="submit" href="javascript:void(0)" onclick="DeleteEstimate(`{{estNo}}`);return false"  value = "削除" ' + '</td>' +
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
    if (p != 0) {
        itemsArr.push(items[p]);
    }
    for (i = 0; i < itemsArr.length; ++i) {
        tbody.append(itemsArr[i]);
    }

}

function onChangeSelect(type) {
    if (type == 1) {
        let fromY = parseInt($($thisToY).val());
        let fromM = parseInt($($thisFromM).val());
        let nMonth = (fromM - 1);
        if (fromY == (currentYear - 1)) {
            InitSelectList($thisFromY, $thisFromM, $thisFromD, fromY, nMonth, "", "from", type)

        } else {
            InitSelectList($thisFromY, $thisFromM, $thisFromD, fromY, nMonth, "this", "from", type)
        }
    } else {
        let toY = parseInt($($thisToY).val());
        let ToM = parseInt($($thisToM).val());
        let nMonth = (ToM - 1);
        if (ToM == (currentYear - 1)) {
            InitSelectList($thisToY, $thisToM, $thisToD, toY, nMonth, "", "from", type);
        } else {
            InitSelectList($thisToY, $thisToM, $thisToD, toY, nMonth, "this", "from", type);
        }
    }
}
function InitSelectList(Y, M, D, year, month, ddflg, ddflg2,type) {
    let currentYear = Tday.getFullYear();
    let currentMonth = parseInt(Tday.getMonth());
    let currentDay = Tday.getDate();
    month = month + 1;
    $(Y).empty();
    $(M).empty();
    $(D).empty();
    var birthYear;
    var birthMonth;
    var dtBirth = new Date(currentYear, currentMonth, currentDay);
    console.log(dtBirth);
    currentMonth = currentMonth + 1;
    dtBirth.setMonth(dtBirth.getMonth() - 3);
    birthYear = dtBirth.getFullYear();
    birthMonth = dtBirth.getMonth();
    if (birthYear == (Tday.getFullYear() - 1)) {
        $(Y).append(new Option(birthYear, birthYear));
        $(Y).append(new Option(currentYear, currentYear));
        if (year == currentYear - 1 && month < 3) {
            dtBirth.setDate(dtBirth.getDate() + 1)
            month = dtBirth.getMonth();
        } else if (year == currentYear && month == currentMonth || year == currentYear && month > currentMonth) {
            month = currentMonth;
        }
        if (ddflg == "this") {
            // add month
            for (let i = 1; i <= currentMonth; i++) {
                $(M).append(new Option(i, i));
            }
            // add day
            if (currentMonth == month) {
                for (let i = 1; i <= currentDay; i++) {
                    $(D).append(new Option(i, i));
                }
                setSelectD(type, currentDay)
           /*     $(D + " option[value='" + currentDay + "']").attr("selected", "selected");*/
            } else {
                let day = new Date(currentYear - 1, month, 0);
                currentDay = parseInt(day.getDate());
                for (let i = 1; i <= currentDay; i++) {
                    $(D).append(new Option(i, i));
                }
            }
            //$(Y + " option[value='" + currentYear + "']").attr("selected", "selected");
             //$(M + " option[value='" + month + "']").attr("selected", "selected");
            setSelectY(type, currentYear)
            setSelectM(type, month)
           

        } else {
            dtBirth.setDate(dtBirth.getDate() + 1)
            let nMonth = dtBirth.getMonth() + 1;
            for (let i = nMonth; i <= 12; i++) {
                $(M).append(new Option(i, i));
            }

            if (currentMonth == month) {
                let day = new Date(currentYear - 1, month, 0);
                currentDay = parseInt(day.getDate());
                for (let i = 1; i <= currentDay; i++) {
                    $(D).append(new Option(i, i));
                }
            } else if (nMonth == month) {
                let day = new Date(year, nMonth, 0);
                currentDay = parseInt(day.getDate());
                Tday.setDate(Tday.getDate() + 1)
                let j = Tday.getDay();
                for (let i = j; i <= CurrentDay; i++) {
                    $(D).append(new Option(i, i));
                }
            } else {
                let day = new Date(year, month, 0);
                currentDay = parseInt(day.getDate());
                for (let i = j; i <= currentDay; i++) {
                    $(D).append(new Option(i, i));
                }
            }
            //$(Y + " option[value='" + currentYear - 1 + "']").attr("selected", "selected");
            setSelectY(type, (currentYear - 1))
        }

    } else {
        dtBirth.setDate(dtBirth.getDate() + 1)
        let i = currentMonth;
        let nMonth = dtBirth.getMonth() + 1;
        do {
            $(M).append(new Option(i, i));
            i--;
        }
        while (i > (nMonth - 1));
        if (currentMonth == month) {
            for (let i = 1; i <= currentDay; i++) {
                $(D).append(new Option(i, i));
            }
        } else if (nMonth == month) {
            let day = new Date(year, month, 0);
            currentDay = parseInt(day.getDate());
            for (let i = nMonth; i <= currentDay; i++) {
                $(D).append(new Option(i, i));
            }
        } else {
            let day = new Date(currentYear - 1, month, 0);
            currentDay = parseInt(day.getDate());
            for (let i = 1; i <= currentDay; i++) {
                $(D).append(new Option(i, i));
            }
        }
        $(Y).append(new Option(currentYear, currentYear));
    }
    return;
}

function setSelectD(type,D) {
    if (type == 1) {
        Framework.SetSelectedNumber("ddlFromSelectD", D)
    } else if (type == 2) {
        Framework.SetSelectedNumber("ddlToSelectD", D)
    }
}
function setSelectM(type,M) {
    if (type == 1) {
        Framework.SetSelectedNumber("ddlFromSelectM", M); 
    } else if (type == 2) {
        Framework.SetSelectedNumber("ddlToSelectM", M);
    }
}
function setSelectY(type,Y) {
    if (type == 1) {
        Framework.SetSelectedNumber("ddlFromSelectM", Y);
    } else if (type == 2) {
        Framework.SetSelectedNumber("ddlToSelectM", Y);
    }
}


