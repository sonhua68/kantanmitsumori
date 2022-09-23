// JScript File
// Create Date 2022/08/28 by HoaiPhong
const FALSE_COLOR = '#ffccff';
const FALSE_COLOR_RES = '#D2E0FB';
const TRUE_COLOR = 'White';
const minrate = 0;
const maxrate = 20;
const defrate = 3.9;
const mintimes = 6;
const maxtimes = 84;
setInitialValue();
function setInitialValue() {
    chgBonus();
    chgBonus_Result();

    if ($('#hidLoanModifyFlag').val() == "1") {
        $('#chkProhibitAutoCalc').attr('checked', true);
        $('#chkProhibitAutoCalc').attr("disabled", true);
    }

}
function clickOK() {
    $('#chkProhibitAutoCalc').disabled = "";
}
function confInputValue(element) {
    var target = document.getElementById(element).value;
    if (target == 0) { target = "" }
    var targetOld = document.getElementById(element + '_old').value;
    if (targetOld == 0) { targetOld = "" }

    if (target != targetOld) {
        $('#hidLoanModifyFlag').val("1");
        $('#chkProhibitAutoCalc').attr('checked', true);
        $('#chkProhibitAutoCalc').attr("disabled", true);
    }
}
function chgBonus() {
    var form = $("#formInpLoan");
    var checkedValue = $('input[name=rdBonus]:checked', form).val();
    var valueRbBonusU = $("#rbBonusU").val();
    if (valueRbBonusU == (checkedValue)) {
        $('#Bonus').attr("disabled", false);
        $('#cbBonusFirst').attr("disabled", false);
        $('#cbBonusSecond').attr("disabled", false);
        $('#Bonus').css('background-color', TRUE_COLOR);
        $('#cbBonusFirst').css('background-color', TRUE_COLOR);
        $('#cbBonusSecond').css('background-color', TRUE_COLOR);
        if ($('#hidBonusSecond').value != "") {
            chgBonusMonth('PostBack');
            $('#cbBonusSecond').value = $('#hidBonusSecond').value;
        }

    } else {
        $('#Bonus').val("");
        $('#cbBonusFirst').val("");
        $('#cbBonusSecond').val("");
        $('#hidBonusSecond').val("");
        $('#Bonus').attr("disabled", true);
        $('#cbBonusSecond').attr("disabled", true);
        $('#Bonus').css('background-color', FALSE_COLOR);
        $('#cbBonusFirst').css('background-color', FALSE_COLOR);
        $('#cbBonusSecond').css('background-color', FALSE_COLOR);

    }
}
function resetBonus_Result() {
    chgBonus_Result();
    confInputValue('BonusCl');
    confInputValue('BonusFirstMonth');
    confInputValue('BonusSecondMonth');
    confInputValue('BonusTimes');
}
function chgBonus_Result() {
    var form = $("#formInpLoan");
    var checkedValue = $('input[name=rdBonus_Result]:checked', form).val();
    var valueRbBonusU = $("#rbBonusU_Result").val();

    if (valueRbBonusU == (checkedValue)) {
        $('#BonusCl').attr("disabled", false);
        $('#BonusFirstMonth').attr("disabled", false);
        $('#BonusSecondMonth').attr("disabled", false);
        $('#BonusTimes').attr("disabled", false);

        $('#BonusCl').css('background-color', TRUE_COLOR);
        $('#BonusFirstMonth').css('background-color', TRUE_COLOR);
        $('#BonusSecondMonth').css('background-color', TRUE_COLOR);
        $('#BonusTimes').css('background-color', TRUE_COLOR);

    } else {
        $('#BonusCl').val("");
        $('#BonusFirstMonth').val("");
        $('#BonusSecondMonth').val("");
        $('#BonusTimes').val("");
        $('#BonusCl').attr("disabled", true);
        $('#BonusFirstMonth').attr("disabled", true);
        $('#BonusSecondMonth').attr("disabled", true);
        $('#BonusTimes').attr("disabled", true);
        $('#BonusCl').css('background-color', FALSE_COLOR_RES);
        $('#BonusFirstMonth').css('background-color', FALSE_COLOR_RES);
        $('#BonusSecondMonth').css('background-color', FALSE_COLOR_RES);
        $('#BonusTimes').css('background-color', FALSE_COLOR_RES);
    }
}
function chgBonusMonth(mode) {
    let value = $("#cbBonusSecond").val();
    $("#cbBonusSecond").empty();
    let bf = $('#cbBonusFirst').val();
    if (bf == "6" || bf == "7" || bf == "8" || bf == "9") {
        $("#cbBonusSecond").append(new Option("", "0"));
        $("#cbBonusSecond").append(new Option("12", "12"));
        $("#cbBonusSecond").append(new Option("1", "1"));
        $("#cbBonusSecond").append(new Option("2", "2"));

    } else if (bf == "12" || bf == "1" || bf == "2") {
        $("#cbBonusSecond").append(new Option("", "0"));
        $("#cbBonusSecond").append(new Option("6", "6"));
        $("#cbBonusSecond").append(new Option("7", "7"));
        $("#cbBonusSecond").append(new Option("8", "8"));
        $("#cbBonusSecond").append(new Option("9", "9"));

    }
    $("#cbBonusSecond option[value='" + value + "']").attr("selected", "selected");
    if (mode != 'PostBack') {
        $('#hidBonusSecond').val("0");
    }
}

function chgBonusSecond() {
    let value = $('#cbBonusSecond').val();
    $('#hidBonusSecond').val(value);
}

function calcGankin() {
    var SalesSum = parseFloat($("#lbl_SalesSum").val());
    var Deposit = parseFloat($("#Deposit").val());
    if (!isNaN(Deposit)) {
        $("#lbl_Principal").val(SalesSum - Deposit);
    } else {
        $("#lbl_Principal").val(SalesSum);
    }

}

function calcResultGankin() {
    var SalesSum = parseFloat($("#SaleSumCl").val());
    var Deposit = parseFloat($("#DepositCl").val());
    if (!isNaN(Deposit)) {
        $("#Principal").val(SalesSum - Deposit);
    } else {
        $("#Principal").val(SalesSum);
    }

}

function btPayTimesUp_onclick() {
    numval = 0;
    $("#Msg").text('');
    var PayTimes = $("#PayTimes").val();
    if (isNaN(PayTimes)) {
        $("#Msg").text('お支払回数に数値以外は入力出来ません。');
        return;
    } else if (parseFloat(PayTimes) < 0) {
        $("#Msg").text('お支払回数にマイナス値は入力できません。');
        return;
    }
    if ($("#PayTimes").val() == '') {
        $("#PayTimes").val(12)
    } else {
        inval = PayTimes;
        numval = Number(inval);
        numval += 1;
        if (numval <= maxtimes) {
            $("#PayTimes").val(numval)
        } else {
            $("#Msg").text('最大回数です。');
            $("#PayTimes").val(maxtimes)
        }
    }
}
function btPayTimesDown_onclick() {
    numval = 0;
    $("#Msg").text('');
    var PayTimes = $("#PayTimes").val();
    if (isNaN(PayTimes)) {
        $("#Msg").text('お支払回数に数値以外は入力出来ません。');
        return;
    } else if (parseFloat(PayTimes) < 0) {
        $("#Msg").text('お支払回数にマイナス値は入力できません。');
        return;
    }

    if ($("#PayTimes").val() == '') {
        $("#PayTimes").val(12)
    } else {
        inval = PayTimes;
        numval = Number(inval);
        numval -= 1;
        if (numval >= mintimes) {
            $("#PayTimes").val(numval)
        } else {
            $("#Msg").text('最小回数です。');
            $("#PayTimes").val(mintimes)
        }
    }
}

function btMoneyRateUp_onclick() {
    numval = 0;
    $("#Msg").text('');
    let MoneyRate = ($("#MoneyRate").val());
    if (isNaN(MoneyRate)) {
        $("#Msg").text('金利に数値以外は入力出来ません。');
        return;
    } else if (parseFloat(MoneyRate) < 0) {
        $("#Msg").text('金利にマイナス値は入力できません。');
        return;
    }

    if ($("#MoneyRate").val() == '') {
        MoneyRate = format(defrate, 1, 0);
        $("#MoneyRate").val(MoneyRate)
    } else {
        inval = MoneyRate;
        numval = Number(inval);
        numval = format(numval + 0.1, 1, 0);
        if (Number(numval) > Number(maxrate)) {
            let max = format(maxrate, 1, 0);
            $("#MoneyRate").val(max)
            $("#Msg").text('最大金利です。');
        } else {
            $("#MoneyRate").val(numval)

        }
    }
}
function btMoneyRateDown_onclick() {
    numval = 0;
    $("#Msg").text('');
    let MoneyRate = ($("#MoneyRate").val());
    if (isNaN(MoneyRate)) {
        $("#Msg").text('金利に数値以外は入力出来ません。');
        return;
    } else if (parseFloat(MoneyRate) < 0) {
        $("#Msg").text('金利にマイナス値は入力できません。');
        return;
    }

    if ($("#MoneyRate").val() == '') {
        MoneyRate = format(defrate, 1, 0);
        $("#MoneyRate").val(MoneyRate)
    } else {
        inval = MoneyRate;
        numval = Number(inval);
        numval = format(numval - 0.1, 1, 0);
        if (numval < Number(minrate)) {
            let min = format(minrate, 1, 0);
            $("#MoneyRate").val(min)
            $("#Msg").text('最低金利です。');
        } else {
            $("#MoneyRate").val(numval)
        }
    }
}
function CalInpLoan() {
    $("#Msg").text("");
    $("#hidLoanModifyFlag").val("0");
    $("#chkProhibitAutoCalc").attr('checked', false);
    $('#chkProhibitAutoCalc').attr("disabled", false);
    let rdBonus = $("input[type='radio'][name='rdBonus']:checked").val() == "rbBonusU" ? true : false;
    var model = {};
    model.SaleSumPrice = $("#lbl_SalesSum").val();
    model.Deposit = $("#Deposit").val();
    model.MoneyRate = $("#MoneyRate").val();
    model.PayTimes = $("#PayTimes").val();
    model.ConTax = $("#hidTaxRatio").val();
    model.FirstMonth = $("#cbFirstMonth").val();
    model.rdBonus = rdBonus;
    var form = $("#formInpLoan");
    var checkedValue = $('input[name=rdBonus]:checked', form).val();
    var valueRbBonusU = $("#rbBonusU").val();
    if (valueRbBonusU == (checkedValue)) {
        model.Bonus = $("#Bonus").val();
        model.BonusFirst = $("#cbBonusFirst").val();
        model.BonusSecond = $("#cbBonusSecond").val();
    } else {
        model.Bonus = "0";
        model.BonusFirst = "0";
        model.BonusSecond = "0";
    }
    var result = Framework.submitAjaxFormUpdateAsync(model, "/InpLoan/CalInpLoan");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let Items = result.data;
        $("#Fee").val(Items.fee);
        $("#PayTotal").val(Items.payTotal);
        $("#PayTimesCl").val(Items.payTimes);
        $("#FirstPayMonth").val(Items.firstPayMonth);
        $("#LastPayMonth").val(Items.lastPayMonth);
        $("#FirstPay").val(Items.firstPay);
        $("#PayMonth").val(Items.payMonth);
        $("#MoneyRateCl").val(Items.moneyRate)
        $("#Deposit").val(Items.deposit)
        $("#DepositCl").val(Items.deposit)
        $("#Principal").val(Items.principal)
        var form = $("#formInpLoan");
        var checkedValue = $('input[name=rdBonus]:checked', form).val();
        var valueRbBonusU = $("#rbBonusU").val();
        if (valueRbBonusU == (checkedValue)) {
            $("#rbBonusU_Result").prop("checked", true);
            $('#BonusCl').attr("disabled", false);
            $('#BonusFirstMonth').attr("disabled", false);
            $('#BonusSecondMonth').attr("disabled", false);
            $('#BonusTimes').attr("disabled", false);
            $('#BonusTimes').val(Items.bonusTimes)
            $('#BonusTimes').css('background-color', TRUE_COLOR);
            $('#BonusCl').val(Items.bonus)
            $('#BonusCl').css('background-color', TRUE_COLOR);
            $('#BonusFirstMonth').css('background-color', TRUE_COLOR);
            $('#BonusFirstMonth').val(Items.bonusFirst)
            $('#BonusSecondMonth').css('background-color', TRUE_COLOR);
            $('#BonusSecondMonth').val(Items.bonusSecond)
        } else {
            $("#rbBonusM_Result").prop("checked", true);
            $('#BonusCl').val("");
            $('#BonusFirstMonth').val("");
            $('#BonusSecondMonth').val("");
            $('#BonusTimes').val("");
            $('#BonusCl').attr("disabled", true);
            $('#BonusFirstMonth').attr("disabled", true);
            $('#BonusSecondMonth').attr("disabled", true);
            $('#BonusTimes').attr("disabled", true);
            $('#BonusCl').css('background-color', FALSE_COLOR_RES);
            $('#BonusFirstMonth').css('background-color', FALSE_COLOR_RES);
            $('#BonusSecondMonth').css('background-color', FALSE_COLOR_RES);
            $('#BonusTimes').css('background-color', FALSE_COLOR_RES);
        }
        $("#Fee_old").val(Items.fee);
        $("#PayTotal_old").val(Items.payTotal);
        $("#PayTimesCl_old").val(Items.payTimes);
        $("#FirstPay_old").val(Items.firstPay);
        $("#PayMonth_old").val(Items.payMonth);
        $("#BonusCl_old").val(Items.bonus);
        $("#BonusTimes_old").val(Items.bonusTimes);
        $("#BonusFirstMonth_old").val(Items.bonusFirst);
        $("#BonusSecondMonth_old").val(Items.bonusSecond);
        $("#MoneyRateCl_old").val(Items.moneyRate)

    } else {
        let Items = result.data;
        if (typeof (Items) != "undefined") {
            $("#Msg").html(Items.calcInfo)
            CleanForm();
            resetBonus_Result();
            calcResultGankin();
        } else {
            location.reload();
        }
    }
};
function CleanForm() {
    $("#Fee").val("");
    $("#PayTotal").val("");
    $("#PayTimesCl").val("");
    $("#FirstPayMonth").val("");
    $("#LastPayMonth").val("");
    $("#FirstPay").val("");
    $("#PayMonth").val("");
    $("#MoneyRateCl").val("")
    $("#DepositCl").val("")
    $("#rbBonusM_Result").prop("checked", true);
    $("#Fee_old").val("");
    $("#PayTotal_old").val("");
    $("#PayTimesCl_old").val("");
    $("#FirstPay_old").val("");
    $("#PayMonth_old").val("");
    $("#BonusCl_old").val("");
    $("#BonusTimes_old").val("");
    $("#BonusFirstMonth_old").val("");
    $("#BonusSecondMonth_old").val("");
    $("#MoneyRateCl_old").val("");
}

function RestPage() {
    $("#Deposit").val("");
    $("#PayTimes").val("");
    $("#rbBonusM").prop("checked", true);
    $('#Bonus').attr("disabled", false);
    $('#cbBonusFirst').attr("disabled", false);
    $('#cbBonusSecond').attr("disabled", false);
    $('#Bonus').css('background-color', TRUE_COLOR);
    $('#cbBonusFirst').css('background-color', TRUE_COLOR);
    $('#cbBonusSecond').css('background-color', TRUE_COLOR);
    CleanForm();
    chgBonus();
    chgBonus_Result()
    $("#chkProhibitAutoCalc").attr('checked', false);
    $('#chkProhibitAutoCalc').attr("disabled", false);
}