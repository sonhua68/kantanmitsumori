// JScript File
// Create Date 2022/09/13 by HoaiPhong
/*var Tday = moment("20221228", "YYYYMMDD")*/
/*console.log(Tday)*/
var Tday = moment();
const currentYear = getYear();;
const currentMonth = getMonth();
const currentDay = getDay();
let month = getMonth();
var $thisFromY = "#ddlFromSelectY";
var $thisFromM = "#ddlFromSelectM";
var $thisFromD = "#ddlFromSelectD";
var $thisToY = "#ddlToSelectY";
var $thisToM = "#ddlToSelectM";
var $thisToD = "#ddlToSelectD";
let _conNumberSort = true;
let _conNumber = 0;
InitSelectList($thisFromY, $thisFromM, $thisFromD, currentYear, currentMonth, "this", "first", 1)
InitSelectList($thisToY, $thisToM, $thisToD, currentYear, currentMonth, "this", "first", 2)
GetListMaker();
SetInitToDay();
LoadData(1);
setCookie("btnHanei", "1", 1);
function GetListMaker() {
    var result = Framework.GetObjectDataFromUrl("/SerEst/GetMakerNameAndModelName?makerName=");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;
        $("#ddlMaker").append(new Option("", ''));
        for (let i = 0; i < length; i++) {
            let value = result.data[i].makerName;
            $("#ddlMaker").append(new Option(value, value));
        }

    } else if (result.resultStatus == -1) {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}
function SetInitToDay() {
    let lastDay = getDay();
    $("#ddlFromSelectD option[value='" + lastDay + "']").attr("selected", "selected");
    $("#ddlToSelectD option[value='" + lastDay + "']").attr("selected", "selected");
}
function setToDayChangeMonth(type) {
    let lastDay = getDay();
    let month = getMonth();
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
        } else if (result.resultStatus == -1) {
            Framework.GoBackErrorPage(result.messageCode, result.messageContent);
        }
    } else {
        $("#ddlModel").empty();
    }

}
function GetDayOfMonth(type) {
    let d = 1;
    let lastDay = getDay();
    let lastMonth = getMonth();
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
    if (result.resultStatus == -1) {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    } else {
        ReloadListData(result);
    }
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
    if (result.resultStatus == -1) {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    } else {
        $('tr#pagination').remove();
        $('#trId').twbsPagination('destroy');
        UiPagination(result[0].totalPages)
        AddPagination(result[0].totalPages);
        ReloadListData(result);
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
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
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
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
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
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
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
    InitSelectList($thisFromY, $thisFromM, $thisFromD, currentYear, currentMonth, "this", "first", 1);
    InitSelectList($thisToY, $thisToM, $thisToD, currentYear, currentMonth, "this", "first", 2);
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
        let fromY = parseInt($($thisFromY).val());
        let fromM = parseInt($($thisFromM).val());
        if (fromY == (currentYear - 1)) {
            InitSelectList($thisFromY, $thisFromM, $thisFromD, fromY, (fromM), "", "from", type)

        } else if (fromY = currentYear) {
            InitSelectList($thisFromY, $thisFromM, $thisFromD, fromY, (fromM), "this", "from", type)
        }
    } else {
        let toY = parseInt($($thisToY).val());
        let ToM = parseInt($($thisToM).val());
        if (toY == (currentYear - 1)) {
            InitSelectList($thisToY, $thisToM, $thisToD, toY, ToM, "", "from", type);
        } else if (toY = currentYear) {
            InitSelectList($thisToY, $thisToM, $thisToD, toY, ToM, "this", "from", type);
        }
    }
}
function InitSelectList(Y, M, D, year, month, ddflg, ddflg2, type) {
    let currentYear = getYear();
    let currentMonth = getMonth();
    let currentDay = getDay();
    $(Y).empty();
    $(M).empty();
    $(D).empty();
    var birthYear;
    var birthMonth;
    var birthDay;
    var dtBirth = SetFormatYear(currentYear, currentMonth, currentDay);
   
    dtBirth = moment(dtBirth).add(-3, 'month');
     dtBirth = moment(dtBirth).add(1, 'days');
    birthYear = parseInt(dtBirth.format('YYYY'));
    birthMonth = parseInt(dtBirth.format('M'));
    birthDay = parseInt(dtBirth.format('D'));
    if (birthYear == (getYear() - 1)) {
        $(Y).append(new Option(currentYear, currentYear));
        $(Y).append(new Option(birthYear, birthYear));
        if (year == currentYear - 1 && month < 3) {
            let dtBr = moment(dtBirth).add(1, 'days');
            month = parseInt(dtBr.format('M'));
        } else if (year == currentYear && month == currentMonth || year == currentYear && month > currentMonth) {
            month = currentMonth;
        }
        if (ddflg == "this") {
            for (let i = 1; i <= currentMonth; i++) {
                $(M).append(new Option(i, i));
            }
            if (currentMonth == month) {
                for (let i = 1; i <= currentDay; i++) {
                    $(D).append(new Option(i, i));
                }
                setSelectD(type, currentDay);
            } else {
                let daysInMonth = GetDaysInMonth(currentYear - 1, month);
                for (let i = 1; i <= daysInMonth; i++) {
                    $(D).append(new Option(i, i));
                }
            }
            setSelectM(type, month);
            setSelectY(type, currentYear);

        } else {
            let dtB = moment(dtBirth);
            let nMonth = parseInt(dtB.format('M'));
            let nDay = parseInt(dtB.format('D'));
            for (let i = nMonth; i <= 12; i++) {
                $(M).append(new Option(i, i));
            }
            if (currentMonth == month) {
                let daysInMonth = GetDaysInMonth(currentYear - 1, month)
                for (let i = 1; i <= daysInMonth; i++) {
                    $(D).append(new Option(i, i));
                }
            } else if (nMonth == month) {
                let daysInMonth = GetDaysInMonth(year, month);
                for (let i = nDay; i <= daysInMonth; i++) {
                    $(D).append(new Option(i, i));
                }
            } else {
                let daysInMonth = GetDaysInMonth(year, month)
                for (let i = 1; i <= daysInMonth; i++) {
                    $(D).append(new Option(i, i));
                }             
            }
            setSelectM(type, month);
            setSelectY(type, (currentYear - 1));
        }

    } else {
        let dtB = moment(dtBirth);
        let i = currentMonth;
        let nMonth = parseInt(dtB.format('M'));
        let nDay = parseInt(dtB.format('D'));
        do {
            $(M).append(new Option(i, i));
            i--;
        }
        while (i > (nMonth - 1));
        if (currentMonth == month) {
            for (let i = 1; i <= currentDay; i++) {
                $(D).append(new Option(i, i));
            }
            setSelectD(type, currentDay);
        } else if (nMonth == month) {
            let daysInMonth = GetDaysInMonth(year, month);
            for (let i = nDay; i <= daysInMonth; i++) {
                $(D).append(new Option(i, i));
            }

        } else {
            let daysInMonth = GetDaysInMonth(currentYear - 1, month)
            for (let i = 1; i <= daysInMonth; i++) {
                $(D).append(new Option(i, i));
            }
        }
        $(Y).append(new Option(currentYear, currentYear));
        setSelectM(type, month);
        setSelectY(type, (currentYear));
    }
    return;
}

function setSelectD(type, D) {
    if (type == 1) {
        Framework.SetSelectedNumber("ddlFromSelectD", D)
    } else if (type == 2) {
        Framework.SetSelectedNumber("ddlToSelectD", D)
    }
}
function setSelectM(type, M) {
    if (type == 1) {
        Framework.SetSelectedNumber("ddlFromSelectM", M);
    } else if (type == 2) {
        Framework.SetSelectedNumber("ddlToSelectM", M);
    }
}
function setSelectY(type, Y) {
    if (type == 1) {
        Framework.SetSelectedNumber("ddlFromSelectY", Y);
    } else if (type == 2) {
        Framework.SetSelectedNumber("ddlToSelectY", Y);
    }
}


function getMonth() {
    var month = Tday.format('M');
    return parseInt(month);
}

function getDay() {
    var day = Tday.format('D');
    return parseInt(day)
}
function getYear() {
    var year = Tday.format('YYYY');
    return parseInt(year)
}

function addDay(y, m, d, numberday) {
    var day = moment([y, m, d]).add(numberday, 'days')
    return day
}
function addYear(y, m, d, numberYeaer) {
    var day = moment([y, m, d]).add(numberday, 'month')
    return day
}

function addYear(y, m, d, numberYeaer) {
    var day = moment([y, m, d]).add(numberday, 'daysInMonth')
    return day;
}

function GetDaysInMonth(y, m) {
    var daysInMonth = moment([y, (m - 1)]).daysInMonth();
    return parseInt(daysInMonth)
}

function SetFormatYear(y, m, d) {
    var date = moment([y, m, d], "YYYYMMDD");
    return date;
}