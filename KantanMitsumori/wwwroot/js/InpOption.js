// JScript File
// Create Date 2022/09/06 by HoaiPhong
// JScript File

//入力チェック
CallData();
function inputChk() {
    $("#Msg").text("");
    var outMsg;

    outMsg = chkKingaku("金額（行１）", $('#OptionPrice1').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行２）", $('#OptionPrice2').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行３）", $('#OptionPrice3').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行４）", $('#OptionPrice4').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行５）", $('#OptionPrice5').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行６）", $('#OptionPrice6').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行７）", $('#OptionPrice7').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行８）", $('#OptionPrice8').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行９）", $('#OptionPrice9').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行１０）", $('#OptionPrice10').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行１１）", $('#OptionPrice11').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkKingaku("金額（行１２）", $('#OptionPrice12').val());
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }


    outMsg = chkBytes("品名（行１）", $('#OptionName1').val(), 40)

    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkBytes("品名（行２）", $('#OptionName2').val(), 40)

    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkBytes("品名（行３）", $('#OptionName3').val(), 40)

    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkBytes("品名（行４）", $('#OptionName4').val(), 40)

    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkBytes("品名（行５）", $('#OptionName5').val(), 40)

    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkBytes("品名（行６）", $('#OptionName6').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    outMsg = chkBytes("品名（行７）", $('#OptionName7').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }

    outMsg = chkBytes("品名（行８）", $('#OptionName8').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }

    outMsg = chkBytes("品名（行９）", $('#OptionName9').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }

    outMsg = chkBytes("品名（行１０）", $('#OptionName10').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }

    outMsg = chkBytes("品名（行１１）", $('#OptionName11').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }

    outMsg = chkBytes("品名（行１２）", $('#OptionName12').val(), 40)
    if (outMsg != "") { $("#Msg").html(outMsg); return false; }
    return true;

}

function EstSum() {
    if (inputChk() == false) { return; }
    var OptionPr1 = chkNull($('#OptionPrice1').val());
    var OptionPr2 = chkNull($('#OptionPrice2').val());
    var OptionPr3 = chkNull($('#OptionPrice3').val());
    var OptionPr4 = chkNull($('#OptionPrice4').val());
    var OptionPr5 = chkNull($('#OptionPrice5').val());
    var OptionPr6 = chkNull($('#OptionPrice6').val());
    var OptionPr7 = chkNull($('#OptionPrice7').val());
    var OptionPr8 = chkNull($('#OptionPrice8').val());
    var OptionPr9 = chkNull($('#OptionPrice9').val());
    var OptionPr10 = chkNull($('#OptionPrice10').val());
    var OptionPr11 = chkNull($('#OptionPrice11').val());
    var OptionPr12 = chkNull($('#OptionPrice12').val());
    var value = OptionPr1
        + OptionPr2
        + OptionPr3
        + OptionPr4
        + OptionPr5
        + OptionPr6
        + OptionPr7
        + OptionPr8
        + OptionPr9
        + OptionPr10
        + OptionPr11
        + OptionPr12;
    $("#OptionPriceAll").val(value);
}

function OptionOpen(CNo) {
    var $lbxOption = $("#lbxOption");
    var visibility = $($lbxOption).css("visibility");
    if (visibility == 'visible') {
        $lbxOption.hide().css("visibility", "hidden");
        return;
    }
    var wkBox = document.getElementById("OptionName" + CNo);
    var wkBoxRect = wkBox.getBoundingClientRect();  
    $lbxOption.css({
        left: wkBoxRect.left + 'px',
        top: wkBoxRect.top + 'px',
        height: '100px',
        width: '290px',
        visibility: 'visible',

    });
    $lbxOption.focus();
    $("#hdPos").val(CNo)
   
}

function indata() {
    var $lbxOption = $("#lbxOption");
    let index = $("#lbxOption option:selected").index()
    var posValue = parseInt($("#hdPos").val());
    let OptionName = $("#OptionName" + posValue);
    let valueIndex = $lbxOption[0].options[index].text
    OptionName.val(valueIndex) 
    $lbxOption.css("visibility", "hidden");  
    let idOption = $("#lbxOption option");
    idOption[0].selected == true;
}

function SelectOnBlur() {
    $("#lbxOption").css("visibility", "hidden");

}
function CallData() {
    var result = Framework.GetObjectDataFromUrl("/InpOption/GetData");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        console.log(result.data)
        let Items = result.data;
        $("#OptionPriceAll").val(Items.optionPriceAll)
        $("#EstNo").val(Items.estNo)
        $("#EstSubNo").val(Items.estSubNo)
        for (var i = 1; i <= 12; i++) {
            let OptionName = $("#OptionName" + i);
            let OptionPrice = $("#OptionPrice" + i);
            switch (i) {
                case 1:
                    OptionName.val(Items.optionName1)
                    OptionPrice.val(StringNull(Items.optionPrice1))
                    break;
                case 2:
                    OptionName.val(Items.optionName2)
                    OptionPrice.val(StringNull(Items.optionPrice2))
                    break;
                case 3:
                    OptionName.val(Items.optionName3)
                    OptionPrice.val(StringNull(Items.optionPrice3))
                    break;
                case 4:
                    OptionName.val(Items.optionName4)
                    OptionPrice.val(StringNull(Items.optionPrice4))
                    break;
                case 5:
                    OptionName.val(Items.optionName5)
                    OptionPrice.val(StringNull(Items.optionPrice5))
                    break;
                case 6:
                    OptionName.val(Items.optionName6)
                    OptionPrice.val(StringNull(Items.optionPrice6))
                    break;
                case 7:
                    OptionName.val(Items.optionName7)
                    OptionPrice.val(StringNull(Items.optionPrice7))
                    break;
                case 8:
                    OptionName.val(Items.optionName8)
                    OptionPrice.val(StringNull(Items.optionPrice8))
                    break;
                case 9:
                    OptionName.val(Items.optionName9)
                    OptionPrice.val(StringNull(Items.optionPrice9))
                    break;
                case 10:
                    OptionName.val(Items.optionName10)
                    OptionPrice.val(StringNull(Items.optionPrice10))
                    break;
                case 11:
                    OptionName.val(Items.optionName11)
                    OptionPrice.val(StringNull(Items.optionPrice11))
                    break;
                case 12:
                    OptionName.val(Items.optionName12)
                    OptionPrice.val(StringNull(Items.optionPrice12))
                    break;

                default:
                // code block
            }

        }
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}

