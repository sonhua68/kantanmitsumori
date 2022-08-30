// JScript File
// Create Date 2022/08/28 by HoaiPhong
const FALSE_COLOR = '#ffccff';
const FALSE_COLOR_RES = '#D2E0FB';
const TRUE_COLOR = 'White';
//最小・最大金利
const minrate = 0;
const maxrate = 20;
const defrate = 3.9;
//最小・最大回数
const mintimes = 6;
const maxtimes = 84;
function setInitialValue() {
    chgBonus();
    chgBonus_Result();

    if ($('#hidLoanModifyFlag').val() == "1") {
        // 手動修正したものに対して、自動計算は行えないようにガード
        $('#chkProhibitAutoCalc').attr('checked', true);
        $('#chkProhibitAutoCalc').attr("disabled", true);
    }

}

/**
 * [OK] ボタン押下時の処理
 * 
 */
function clickOK() {
    $('#chkProhibitAutoCalc').disabled = "";
}

/**
 * 手動修正の検知
 * 
 */
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

/**
 * ボーナス表示切り替え（ローン条件部）
 * 
 */
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

        // PostBack の場合、cbBonusSecond の値がクリアされてしまうのを回避
        if ($('#hidBonusSecond').value != "") {
            //chgBonusMonth('PostBack');
            $('#cbBonusSecond').value = $('#hidBonusSecond').value;
        }

    } else {
        $('#Bonus').val("");
        $('#cbBonusFirst').val("");
        $('#cbBonusSecond').val("");
        $('#hidBonusSecond').val("");
        $('#Bonus').attr("disabled", true);
        $('#cbBonusFirst').attr("disabled", true);
        $('#cbBonusSecond').attr("disabled", true);
        $('#Bonus').css('background-color', FALSE_COLOR);
        $('#cbBonusFirst').css('background-color', FALSE_COLOR);
        $('#cbBonusSecond').css('background-color', FALSE_COLOR);

    }
}

/**
 * ボーナス表示リセット（ローン計算情報部）
 * 
 */
function resetBonus_Result() {
    chgBonus_Result();
    confInputValue('BonusCl');
    confInputValue('BonusFirstMonth');
    confInputValue('BonusSecondMonth');
    confInputValue('BonusTimes');
}

/**
 * ボーナス表示切り替え（ローン計算情報部）
 * 
 */
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

/**
 * ボーナス月表示切り替え
 * 
 */
function chgBonusMonth(mode) {
    //frm.cbBonusSecond.options.length = 0;
    //var bf = $get('cbBonusFirst').value;

    //if (bf == "6" || bf == "7" || bf == "8" || bf == "9") {
    //    frm.cbBonusSecond.options[0] = new Option("", 0);
    //    frm.cbBonusSecond.options[1] = new Option("12", 12);
    //    frm.cbBonusSecond.options[2] = new Option("1", 1);
    //    frm.cbBonusSecond.options[3] = new Option("2", 2);
    //} else if (bf == "12" || bf == "1" || bf == "2") {
    //    frm.cbBonusSecond.options[0] = new Option("", 0);
    //    frm.cbBonusSecond.options[1] = new Option("6", 6);
    //    frm.cbBonusSecond.options[2] = new Option("7", 7);
    //    frm.cbBonusSecond.options[3] = new Option("8", 8);
    //    frm.cbBonusSecond.options[4] = new Option("9", 9);
    //}

    //if (mode != 'PostBack') {
    //    $get('hidBonusSecond').value = "0"
    //}
}

/**
 * ボーナス月表示切り替え
 * 
 */
function chgBonusSecond() {
    let value = $('#cbBonusSecond').val();
    $('#hidBonusSecond').val(value);
}

/**
 * 元金計算（ローン条件部）
 * 
 */
function calcGankin() {
    var SalesSum = parseFloat($("#lbl_SalesSum").val());
    var Deposit = parseFloat($("#Deposit").val());
    $("#lbl_Principal").val(SalesSum - Deposit);
}

/**
 * 元金計算（ローン計算情報部）
 * 
 */
function calcResultGankin() {
    var SalesSum = parseFloat($("#SaleSumCl").val());
    var Deposit = parseFloat($("#DepositCl").val());
    $("#Principal").val(SalesSum - Deposit);
}



/**
 * お支払回数 ▲(Up) ボタン押下時の処理
 * 
 */
function btPayTimesUp_onclick() {
    //初期化
    numval = 0;
    $("#Msg").val('');
    var PayTimes = parseInt($("#PayTimes").val());
    //数値チェック
    if (isNaN(PayTimes)) {
        $("#Msg").val('お支払回数に数値以外は入力出来ません。');
        return;
    } else if (isNaN(PayTimes) < 0) {
        $("#Msg").val('お支払回数にマイナス値は入力できません。');
        return;
    }

    if (PayTimes == '') {
        PayTimes = 12;
    } else {
        inval = PayTimes;
        numval = Number(inval);
        //10000プラス
        numval += 1;
        //最大値以内か
        if (numval <= maxtimes) {
            $("#PayTimes").val(numval)
        } else {
            $("#Msg").val('最大回数です。');
            $("#PayTimes").val(maxtimes)
        }
    }
}

/**
 * お支払回数 ▼(Down) ボタン押下時の処理
 * 
 */
function btPayTimesDown_onclick() {
    //初期化
    numval = 0;
    $("#Msg").val('');

    //数値チェック
    var PayTimes = parseFloat($("#PayTimes").val());
    if (isNaN(PayTimes)) {
        $("#Msg").val('お支払回数に数値以外は入力出来ません。');
        return;
    } else if (PayTimes < 0) {
        $("#Msg").val('お支払回数にマイナス値は入力できません。');
        return;
    }
    if (PayTimes == '') {
        PayTimes = 12;
    } else {
        inval = PayTimes;
        numval = Number(inval);
        //10000プラス
        numval -= 1;
        //最小値以内か
        if (numval >= mintimes) {
            $("#PayTimes").val(numval)
        } else {
            $("#Msg").val('最小回数です。');
            $("#PayTimes").val(mintimes)
        }
    }
}



/**
 * 金利 ▲(Up) ボタン押下時の処理
 * 
 */
function btMoneyRateUp_onclick() {
    //初期化
    numval = 0;
    $("#Msg").val('');
    let MoneyRate = parseFloat($("#MoneyRate").val());
    //数値チェック
    if (isNaN(MoneyRate)) {
        $("#Msg").val('金利に数値以外は入力出来ません。');
        return;
    } else if (MoneyRate < 0) {
        $("#Msg").val('金利にマイナス値は入力できません。');
        return;
    }

    if (MoneyRate == '') {
        MoneyRate.value = format(defrate, 1, 0);
    } else {
        inval = MoneyRate;
        numval = Number(inval);
        //小数点第2位以下四捨五入
        numval = format(numval + 0.1, 1, 0);
        //最大金利より大きい場合
        if (Number(numval) > Number(maxrate)) {
            let max = format(maxrate, 1, 0);
            $("#MoneyRate").val(max)
            $("#Msg").val('最大金利です。');
        } else {
            $("#MoneyRate").val(numval)

        }
    }
}

/**
 * 金利 ▼(Down) ボタン押下時の処理
 * 
 */
function btMoneyRateDown_onclick() {
    //初期化
    numval = 0;
    $("#Msg").val('');
    let MoneyRate = parseInt($("#MoneyRate").val());
    //数値チェック
    if (isNaN(MoneyRate)) {
        $("#Msg").val('金利に数値以外は入力出来ません。');
        return;
    } else if (MoneyRate < 0) {
        $("#Msg").val('金利にマイナス値は入力できません。');
        return;
    }

    if (MoneyRate == '') {
        MoneyRate = format(defrate, 1, 0);
    } else {
        inval = MoneyRate;
        numval = Number(inval);
        //小数点第2位以下四捨五入
        numval = format(numval - 0.1, 1, 0);
        //最低金利より小さい場合
        if (numval < Number(minrate)) {
            let min = format(minrate, 1, 0);
            $("#MoneyRate").val(min)
            $("#Msg").val('最低金利です。');
        } else {
            $("#MoneyRate").val(numval)
        }
    }
}
//val:数値;digit:桁;flg:(0:四捨五入;1:切り捨て;2:切り上げ)
function format(val, digit, flg) {
    val *= Math.pow(10.0, digit);
    if (flg == 0)
        val = Math.round(val)
    else if (flg == 1)
        val = Math.floor(val)
    else if (flg == 2)
        val = Math.ceil(val);

    val += "";
    //０埋め処理
    var tmp = digit - val.length;
    if (0 < tmp) for (i = 0; i < tmp; i++) val = "0" + val;

    //.挿入処理
    if (digit) {
        var pat = "";
        for (i = 0; i < digit; i++) pat += ".";
        val = val.replace(eval("/(" + pat + "$)/"), ".$1");
        val = val.replace(/^\./, "0\.");
    }
    return (val);
}

function CalInpLoan() {
    var model = {};
    model.SaleSumPrice = $("#lbl_SalesSum").val();
    model.Deposit = $("#Deposit").val();
    model.MoneyRate = $("#MoneyRate").val();
    model.PayTimes = $("#PayTimes").val();
    model.ConTax = $("#hidTaxRatio").val();
    model.FirstMonth = $("#cbFirstMonth").val();
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


    console.log(model);
    var result = Framework.submitAjaxFormUpdateAsync(model, "/InpLoan/CalInpLoan");
    if (result.resultStatus == 0) {
        console.log(result.data)
        let Items = result.data;
        $("#Fee").val(Items.fee);
        $("#PayTotal").val(Items.payTotal);
        $("#PayTimesCl").val(Items.payTimes);
        $("#FirstPayMonth").val(Items.firstPayMonth);
        $("#LastPayMonth").val(Items.lastPayMonth);
        $("#FirstPay").val(Items.lirstPay);
        $("#PayMonth").val(Items.payMonth);
        var form = $("#formInpLoan");
        var checkedValue = $('input[name=rdBonus]:checked', form).val();
        var valueRbBonusU = $("#rbBonusU").val();
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
        $("#Fee_old").val(Items.fee);
        $("#PayTotal_old").val(Items.payTotal);
        $("#PayTimesCl_old").val(Items.payTimes);
        $("#FirstPay_old").val(Items.firstPay);
        $("#PayMonth_old").val(Items.payMonth);
        $("#BonusCl_old").val(Items.bonus);
        $("#BonusTimes_old").val(Items.bonusTimes);
        $("#BonusFirstMonth_old").val(Items.bonusFirst);
        $("#BonusSecondMonth_old").val(Items.bonusSecond);
           
    }
};