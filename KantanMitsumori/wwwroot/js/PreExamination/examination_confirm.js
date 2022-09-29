$(document).ready(function() {

    $(".headerEstimate").hide();
    $(".header.js-floating").addClass('js-floatingSimple');

    var action = new Action("order", "examination_confirm", "init", examinationConfirmCallback);
    actionList.push(action);
});

var examinationConfirmCallback = function(data, messages) {

    if (messages.errorMessages.length > 0) {
        return;
    }

    $("#examinationConfirmArea").removeClass("hidden");

    examinationConfirmDesign();

};

function register() {
    var action = new Action();
    var actionInfo = new ActionInfo();

    actionInfo.packageId = "order";
    actionInfo.beanId = "examination_confirm";
    actionInfo.actionId = "register";

    action.actionInfo = actionInfo;

    action.callback = function(data, messages) {
        if (messages.errorMessages.length <= 0) {
            createCompletePage(data);
        } else {
            createErrorPage();
        }
    }

    submit(action, "formArea");
}

/**
 * 逋ｻ骭ｲ螳御ｺ�ｾ悟�逅�(豁｣蟶ｸ邨ゆｺ�)
 */
function createCompletePage(data) {
    document.charset='shift_jis';
    var linkerParam = '';
    if (typeof ga !== "undefined") {
        linkerParam = '&' + ga.getAll()[0].get("linkerParam");
    }

    var arr = getCookieArray();
    var a8value = arr['_a8_s00000022948001'];
    if (a8value) {
    	linkerParam = linkerParam + '&a8=' + a8value;
    }
    var stcidvalue = getStcid();
    if (stcidvalue) {
    	linkerParam = linkerParam + '&_stcid=' + stcidvalue;
    }

    $('<form/>', {action: data.examination.externalExaminationUrl + linkerParam, method: 'post', 'accept-charset': 'shift_jis'})
    .append($('<input/>', {type: 'hidden', id: 'orderNo', name: 'orderNo', value: data.examination.orderNo}))
    .append($('<input/>', {type: 'hidden', id: 'orderDatetime', name: 'orderDatetime', value: data.examination.orderDatetime}))
    .append($('<input/>', {type: 'hidden', id: 'commodityCode', name: 'commodityCode', value: data.examination.commodityCode}))
    .append($('<input/>', {type: 'hidden', id: 'skuCode', name: 'skuCode', value: data.examination.skuCode}))
    .append($('<input/>', {type: 'hidden', id: 'mkNm', name: 'mkNm', value: data.examination.mkNm}))
    .append($('<input/>', {type: 'hidden', id: 'commodityName', name: 'commodityName', value: data.examination.commodityName}))
    .append($('<input/>', {type: 'hidden', id: 'gradeName', name: 'gradeName', value: data.examination.gradeName}))
    .append($('<input/>', {type: 'hidden', id: 'fuelName', name: 'fuelName', value: data.examination.fuelName}))
    .append($('<input/>', {type: 'hidden', id: 'fuelType', name: 'fuelType', value: data.examination.fuelType}))
    .append($('<input/>', {type: 'hidden', id: 'chargerFlgName', name: 'chargerFlgName', value: data.examination.chargerFlgName}))
    .append($('<input/>', {type: 'hidden', id: 'haiki', name: 'haiki', value: data.examination.haiki}))
    .append($('<input/>', {type: 'hidden', id: 'missionCd', name: 'missionCd', value: data.examination.missionCd}))
    .append($('<input/>', {type: 'hidden', id: 'wheelDriveName', name: 'wheelDriveName', value: data.examination.wheelDriveName}))
    .append($('<input/>', {type: 'hidden', id: 'doorCd', name: 'doorCd', value: data.examination.doorCd}))
    .append($('<input/>', {type: 'hidden', id: 'roofNm', name: 'roofNm', value: data.examination.roofNm}))
    .append($('<input/>', {type: 'hidden', id: 'body', name: 'body', value: data.examination.body}))
    .append($('<input/>', {type: 'hidden', id: 'yuka', name: 'yuka', value: data.examination.yuka}))
    .append($('<input/>', {type: 'hidden', id: 'teiinA', name: 'teiinA', value: data.examination.teiinA}))
    .append($('<input/>', {type: 'hidden', id: 'teiinB', name: 'teiinB', value: data.examination.teiinB}))
    .append($('<input/>', {type: 'hidden', id: 'teiinC', name: 'teiinC', value: data.examination.teiinC}))
    .append($('<input/>', {type: 'hidden', id: 'sekisaiA', name: 'sekisaiA', value: data.examination.sekisaiA}))
    .append($('<input/>', {type: 'hidden', id: 'sekisaiB', name: 'sekisaiB', value: data.examination.sekisaiB}))
    .append($('<input/>', {type: 'hidden', id: 'sekisaiC', name: 'sekisaiC', value: data.examination.sekisaiC}))
    .append($('<input/>', {type: 'hidden', id: 'gateNm', name: 'gateNm', value: data.examination.gateNm}))
    .append($('<input/>', {type: 'hidden', id: 'cabNm', name: 'cabNm', value: data.examination.cabNm}))
    .append($('<input/>', {type: 'hidden', id: 'nidaiSiyo', name: 'nidaiSiyo', value: data.examination.nidaiSiyo}))
    .append($('<input/>', {type: 'hidden', id: 'tireKbn', name: 'tireKbn', value: data.examination.tireKbn}))
    .append($('<input/>', {type: 'hidden', id: 'keijoNm', name: 'keijoNm', value: data.examination.keijoNm}))
    .append($('<input/>', {type: 'hidden', id: 'ninKata', name: 'ninKata', value: data.examination.ninKata}))
    .append($('<input/>', {type: 'hidden', id: 'carPrice', name: 'carPrice', value: data.examination.carPrice}))
    .append($('<input/>', {type: 'hidden', id: 'bcolorNm', name: 'bcolorNm', value: data.examination.bcolorNm}))
    .append($('<input/>', {type: 'hidden', id: 'colorCost', name: 'colorCost', value: data.examination.colorCost}))
    .append($('<input/>', {type: 'hidden', id: 'opJoinName', name: 'opJoinName', value: data.examination.opJoinName}))
    .append($('<input/>', {type: 'hidden', id: 'opTotalCost', name: 'opTotalCost', value: data.examination.opTotalCost}))
    .append($('<input/>', {type: 'hidden', id: 'leasePeriod', name: 'leasePeriod', value: data.examination.leasePeriod}))
    .append($('<input/>', {type: 'hidden', id: 'mileage', name: 'mileage', value: data.examination.mileage}))
    .append($('<input/>', {type: 'hidden', id: 'zanka', name: 'zanka', value: data.examination.zanka}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName1', name: 'reopGroupName1', value: data.examination.reopGroupName1}))
    .append($('<input/>', {type: 'hidden', id: 'reopName1', name: 'reopName1', value: data.examination.reopName1}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost1', name: 'reopCost1', value: data.examination.reopCost1}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName2', name: 'reopGroupName2', value: data.examination.reopGroupName2}))
    .append($('<input/>', {type: 'hidden', id: 'reopName2', name: 'reopName2', value: data.examination.reopName2}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost2', name: 'reopCost2', value: data.examination.reopCost2}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName3', name: 'reopGroupName3', value: data.examination.reopGroupName3}))
    .append($('<input/>', {type: 'hidden', id: 'reopName3', name: 'reopName3', value: data.examination.reopName3}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost3', name: 'reopCost3', value: data.examination.reopCost3}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName4', name: 'reopGroupName4', value: data.examination.reopGroupName4}))
    .append($('<input/>', {type: 'hidden', id: 'reopName4', name: 'reopName4', value: data.examination.reopName4}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost4', name: 'reopCost4', value: data.examination.reopCost4}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName5', name: 'reopGroupName5', value: data.examination.reopGroupName5}))
    .append($('<input/>', {type: 'hidden', id: 'reopName5', name: 'reopName5', value: data.examination.reopName5}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost5', name: 'reopCost5', value: data.examination.reopCost5}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName6', name: 'reopGroupName6', value: data.examination.reopGroupName6}))
    .append($('<input/>', {type: 'hidden', id: 'reopName6', name: 'reopName6', value: data.examination.reopName6}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost6', name: 'reopCost6', value: data.examination.reopCost6}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName7', name: 'reopGroupName7', value: data.examination.reopGroupName7}))
    .append($('<input/>', {type: 'hidden', id: 'reopName7', name: 'reopName7', value: data.examination.reopName7}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost7', name: 'reopCost7', value: data.examination.reopCost7}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName8', name: 'reopGroupName8', value: data.examination.reopGroupName8}))
    .append($('<input/>', {type: 'hidden', id: 'reopName8', name: 'reopName8', value: data.examination.reopName8}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost8', name: 'reopCost8', value: data.examination.reopCost8}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName9', name: 'reopGroupName9', value: data.examination.reopGroupName9}))
    .append($('<input/>', {type: 'hidden', id: 'reopName9', name: 'reopName9', value: data.examination.reopName9}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost9', name: 'reopCost9', value: data.examination.reopCost9}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName10', name: 'reopGroupName10', value: data.examination.reopGroupName10}))
    .append($('<input/>', {type: 'hidden', id: 'reopName10', name: 'reopName10', value: data.examination.reopName10}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost10', name: 'reopCost10', value: data.examination.reopCost10}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName11', name: 'reopGroupName11', value: data.examination.reopGroupName11}))
    .append($('<input/>', {type: 'hidden', id: 'reopName11', name: 'reopName11', value: data.examination.reopName11}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost11', name: 'reopCost11', value: data.examination.reopCost11}))
    .append($('<input/>', {type: 'hidden', id: 'reopGroupName12', name: 'reopGroupName12', value: data.examination.reopGroupName12}))
    .append($('<input/>', {type: 'hidden', id: 'reopName12', name: 'reopName12', value: data.examination.reopName12}))
    .append($('<input/>', {type: 'hidden', id: 'reopCost12', name: 'reopCost12', value: data.examination.reopCost12}))
    .append($('<input/>', {type: 'hidden', id: 'maintenanceName', name: 'maintenanceName', value: data.examination.maintenanceName}))
    .append($('<input/>', {type: 'hidden', id: 'downPaymentPrice', name: 'downPaymentPrice', value: data.examination.downPaymentPrice}))
    .append($('<input/>', {type: 'hidden', id: 'bonusPrice', name: 'bonusPrice', value: data.examination.bonusPrice}))
    .append($('<input/>', {type: 'hidden', id: 'bonusAddMonth1', name: 'bonusAddMonth1', value: data.examination.bonusAddMonth1}))
    .append($('<input/>', {type: 'hidden', id: 'bonusAddMonth2', name: 'bonusAddMonth2', value: data.examination.bonusAddMonth2}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName1', name: 'otherCostName1', value: data.examination.otherCostName1}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost1', name: 'otherCost1', value: data.examination.otherCost1}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName2', name: 'otherCostName2', value: data.examination.otherCostName2}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost2', name: 'otherCost2', value: data.examination.otherCost2}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName3', name: 'otherCostName3', value: data.examination.otherCostName3}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost3', name: 'otherCost3', value: data.examination.otherCost3}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName4', name: 'otherCostName4', value: data.examination.otherCostName4}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost4', name: 'otherCost4', value: data.examination.otherCost4}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName5', name: 'otherCostName5', value: data.examination.otherCostName5}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost5', name: 'otherCost5', value: data.examination.otherCost5}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName6', name: 'otherCostName6', value: data.examination.otherCostName6}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost6', name: 'otherCost6', value: data.examination.otherCost6}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName7', name: 'otherCostName7', value: data.examination.otherCostName7}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost7', name: 'otherCost7', value: data.examination.otherCost7}))
    .append($('<input/>', {type: 'hidden', id: 'otherCostName8', name: 'otherCostName8', value: data.examination.otherCostName8}))
    .append($('<input/>', {type: 'hidden', id: 'otherCost8', name: 'otherCost8', value: data.examination.otherCost8}))
    .append($('<input/>', {type: 'hidden', id: 'promotionExpense', name: 'promotionExpense', value: data.examination.promotionExpense}))
    .append($('<input/>', {type: 'hidden', id: 'partnerFee', name: 'partnerFee', value: data.examination.partnerFee}))
    .append($('<input/>', {type: 'hidden', id: 'examCompanyName', name: 'examCompanyName', value: data.examination.examCompanyName}))
    .append($('<input/>', {type: 'hidden', id: 'examGuarantyRate', name: 'examGuarantyRate', value: data.examination.examGuarantyRate}))
    .append($('<input/>', {type: 'hidden', id: 'interestRate', name: 'interestRate', value: data.examination.interestRate}))
    .append($('<input/>', {type: 'hidden', id: 'spCarFlgName', name: 'spCarFlgName', value: data.examination.spCarFlgName}))
    .append($('<input/>', {type: 'hidden', id: 'unitGrossProfit', name: 'unitGrossProfit', value: data.examination.unitGrossProfit}))
    .append($('<input/>', {type: 'hidden', id: 'leasePrice', name: 'leasePrice', value: data.examination.leasePrice}))
    .append($('<input/>', {type: 'hidden', id: 'reviewUrl', name: 'reviewUrl', value: data.examination.reviewUrl}))
    .append($('<input/>', {type: 'hidden', id: 'taxRate', name: 'taxRate', value: data.examination.taxRate}))
    .append($('<input/>', {type: 'hidden', id: 'orderToken', name: 'orderToken', value: data.examination.orderToken}))
    .append($('<input/>', {type: 'hidden', id: 'form068', name: 'form068', value: data.examination.form068}))
    .append($(data.examination.tdAlignmentScript))
    .appendTo(document.body)
    .submit();
}

/**
 * 逋ｻ骭ｲ螳御ｺ�ｾ悟�逅�(逡ｰ蟶ｸ邨ゆｺ�)
 */
function createErrorPage() {
    $("#examinationConfirmArea").removeClass("hidden");
}

var examinationConfirmDesign = function() {
  // 繝√ぉ繝�け繝懊ャ繧ｯ繧ｹ繧偵け繝ｪ繝�け縺励◆繧峨�繧ｿ繝ｳ繧呈ｴｻ諤ｧ蛹�
  $(".js-agree input").click(function() {
      if ($(this).prop("checked") == true) {
          $(".js-entryButton").removeClass("buttonDisabled");
      } else {
          $(".js-entryButton").addClass("buttonDisabled");
      }
  });
};

/**
 * Cookie繧帝�蛻励↓譬ｼ邏�
 */
function getCookieArray(){
    var arr = new Array();
    if(document.cookie != ''){
        var tmp = document.cookie.split('; ');
        for(var i=0;i<tmp.length;i++){
            var data = tmp[i].split('=');
            arr[data[0]] = decodeURIComponent(data[1]);
        }
    }
    return arr;
}

function getStcid(){
    var href = document.getElementById('stcid').href;
    var ret = '';
    if (href != null) {
        var tmp =  href.indexOf('_stcid');
        if (tmp != -1) {
            var stcid = href.slice(tmp);
            stcid = stcid.split('&')[0];
            var data = stcid.split('=');
            if (data.length > 1) {
                ret = data[1];
            }
        }
    }
    return ret;
}