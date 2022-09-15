// JScript File
// Create Date 2022/09/15 by HoaiPhong
// JScript File



function inputChk() {
    
    document.getElementById("txMsg").innerHTML = "";
    var outMsg;

    outMsg = chkKingaku("金額欄", $('#Price').val());  
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
 

    outMsg = chkKingaku("金額欄", $('#Discount').val());
  
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }

    outMsg = chkKingaku("金額欄", $('#SalesSumNew').val());  
    if (outMsg != "") { document.getElementById("txMsg").innerHTML = outMsg; return false; }
    return true;

}

//合計計算
function EstSum() {

    //入力チェック
    if (inputChk() == false) { return; }

    var TaxAllT = document.getElementById("lbl_TaxAllT").innerHTML;

    var Price = cnvKin($('#Price').val());
    var Discount = cnvKin($('#Discount').val());
    var PriceNew = cnvKin($('#lbl_PriceNew').val());
    var RakuSatu = $('#RakuSatu').val();
    var Rikusou = $('#Rikusou').val();
    var Seibi = cnvKin($('#SyakenSeibi').val());
    var OpPrice = cnvKin($('#lbl_OpSpeCialPrice').val());
    var TaxIns = cnvKin($('#lbl_TaxInsAll').val());
    var TaxInsEquivalent = cnvKin($('#lbl_TaxInsEquivalentAll').val());
    var TaxFree = cnvKin($('#lbl_TaxFreeAll').val());
    var Daiko = cnvKin($('#lbl_DaikoAll').val());
    var TaxAll = cnvKin($('#lbl_TaxAll').val());
    var CarSaleKei = cnvKin($('#lbl_CarSaleKei').val());
    var ShitaPrice = cnvKin($('#lbl_ShitaPrice').val());
    var Zansai = cnvKin($('#lbl_Zansai').val());
    var SalesSum = cnvKin($('#lbl_SalesSum').val());//
    var ConTaxInputKb = $('#hidConTaxInputKb').val();
    var Tax = $('#hidTax').val();

    PriceNew = Number(Price) - Number(Discount);

    CarSaleKei = Number(Price) + Number(RakuSatu) + Number(Rikusou) + Number(Seibi) +
        Number(OpPrice) + Number(TaxIns) + Number(TaxInsEquivalent) + Number(TaxFree) + Number(Daiko) - Number(Discount);

    if (ConTaxInputKb.toLowerCase() == "false") {
        TaxAll = Math.floor((Number(CarSaleKei) - Number(TaxFree) - Number(TaxIns)) * Number(Tax));
        SalesSum = Number(TaxAll) + Number(CarSaleKei) - Number(ShitaPrice) + Number(Zansai);
    } else {
        TaxAll = Math.floor((Number(CarSaleKei) - Number(TaxFree) - Number(TaxIns)) / (Number(1) + Number(Tax)) * Number(Tax));
        SalesSum = Number(CarSaleKei) - Number(ShitaPrice) + Number(Zansai);
    }

   
    $("#lbl_PriceNew").val(PriceNew)
    $("#lbl_TaxAll").val(TaxAll)
    $("#lbl_CarSaleKei").val(CarSaleKei)
    $("#lbl_SalesSum").val(SalesSum)

}

//逆算
function bkEstSum() {
    
    $("#Discount").val("0");

    //入力チェック
    if (inputChk() == false) { return; }

    //値引き額 0 クリア後の合計再計算
    EstSum();

    var Price = cnvKin($('#Price').val());
    var RakuSatu = cnvKin($('#RakuSatu').val());
    var Rikusou = cnvKin($('#Rikusou').val());
    var Seibi = cnvKin($('#SyakenSeibi').val());
    var OpPrice = cnvKin($('#lbl_OpSpeCialPrice').val());
    var TaxIns = cnvKin($('#lbl_TaxInsAll').val());
    var TaxInsEquivalent = cnvKin($('#lbl_TaxInsEquivalentAll').val());
    var TaxFree = cnvKin($('#lbl_TaxFreeAll').val());
    var Daiko = cnvKin($('#lbl_DaikoAll').val());
    var TaxAll = cnvKin($('#lbl_TaxAll').val());
    var CarSaleKei = cnvKin($('#lbl_CarSaleKei').val());
    var ShitaPrice = cnvKin($('#lbl_ShitaPrice').val());
    var Zansai = cnvKin($('#lbl_Zansai').val());
    var SalesSum = cnvKin($('#lbl_SalesSum').val());
    var Discount = cnvKin($('#Discount').val());
    var PriceNew = cnvKin($('#lbl_PriceNew').val());
    var bkSalesSumNew = cnvKin($('#SalesSumNew').val());
    var ConTaxInputKb = $('#hidConTaxInputKb').val();
    var Tax = Number($('#hidTax').val());

    if (ConTaxInputKb.toLowerCase() == "false") {

        var TaxPrice = Number(bkSalesSumNew) - Number(Zansai) + Number(ShitaPrice) - Number(TaxIns) - Number(TaxFree);
        TaxAll = Math.floor(Number(TaxPrice) / (Number(1) + Number(Tax)) * Number(Tax));

        Discount = (Number(Price) + Number(RakuSatu) + Number(Rikusou) + Number(Seibi) + Number(OpPrice) + Number(TaxInsEquivalent) + Number(Daiko))
            - (Number(TaxPrice) - Number(TaxAll));

        PriceNew = Number(Price) - Number(Discount);

        CarSaleKei = Number(Price) - Number(Discount) + Number(RakuSatu) + Number(Rikusou) + Number(Seibi) + Number(OpPrice) + Number(TaxIns) + Number(TaxInsEquivalent) + Number(TaxFree) + Number(Daiko);

    } else {

        NowDiscount = Number(Discount);
        Discount = Number(SalesSum) - Number(bkSalesSumNew) + Number(NowDiscount);

        PriceNew = Number(Price) - Number(Discount);

        var TaxPrice = Number(Price) + Number(RakuSatu) + Number(Rikusou) + Number(Seibi) + Number(OpPrice) + Number(TaxInsEquivalent) + Number(Daiko)
            - Number(Discount);
        TaxAll = Math.floor((Number(TaxPrice)) / (Number(1) + Number(Tax)) * Number(Tax));

        CarSaleKei = Number(TaxPrice) + Number(TaxIns) + Number(TaxFree);

    }

    SalesSum = Number(bkSalesSumNew);
    $("#Discount").val(Discount)
    $("#lbl_PriceNew").val(PriceNew)
    $("#lbl_TaxAll").val(TaxAll)
    $("#lbl_CarSaleKei").val(CarSaleKei)
    $("#lbl_SalesSum").val(SalesSum)
    EstSum();

}
