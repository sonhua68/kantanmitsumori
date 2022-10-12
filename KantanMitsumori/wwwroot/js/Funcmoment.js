// JScript File
// Create Date 2022/10/05 by HoaiPhong
/*var Tday = moment("20221228", "YYYYMMDD")*/
/*console.log(Tday)*/
var Tday = moment();
const currentYear = getSystemYear();;
const currentMonth = getSystemMonth();
const currentDay = getSystemDay();
let month = getSystemMonth();
function getSystemMonth() {
    var month = Tday.format('M');
    return parseInt(month);
}

function getSystemDay() {
    var day = Tday.format('D');
    return parseInt(day)
}
function getSystemYear() {
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

