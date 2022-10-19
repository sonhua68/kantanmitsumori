
// JScript File
// Create Date 2022/10/03 by HoaiPhong
// JScript File
var FALSE_COLOR = '#FF3333';
var DISABLE_COLOR = '#A9A9A9';
GetCarType();
GetContractPlan();
GetVolInsurance();
function setInitialValue() {
    // set init prpperties
    setInitProperties();
    // check isData
    if ($get('hidIsData').value == true) {
        // Set enable for button confirm
        buttonEnabling();
    }
}
function setInitProperties() {
    $get('hidCarType').value = chkNull($get('cboCarType').value);
    var valHidElecCar = chkNull($get('hidElectricCar').value)

    if ($get('hidCarType').value == 1) {
        if (valHidElecCar == 1) {
            $get('chkElectricCar').checked = true;
        }
        else {
            $get('chkElectricCar').checked = false;
        }
    }
    else {
        $get('chkElectricCar').checked = false;
        $get('chkElectricCar').disabled = true;
    }

    $get('hidContractPlan').value = chkNull($get('cbo_ContractPlan').value);
    $get('hidInsurExpanded').value = chkNull($get('cbo_InsurExpanded').value);

    var valOptionIns = chkNull($get('cbo_OptionInsurance').value)
    if (valOptionIns == 0) {
        $get('cbo_InsuranceCompany').disabled = true;
        $get('cbo_InsuranceCompany').value = '';
        $get('cbo_InsuranceCompany').style.backgroundColor = DISABLE_COLOR;

        document.getElementById('InsuranceFee').value = 0;
        document.getElementById('InsuranceFee').setAttribute('disabled', true);
        $get('InsuranceFee').style.backgroundColor = DISABLE_COLOR;
    }

    // Set Change Day For ExpiresDate
    onchangeDay();

    // Set ContractTimes & Calculator Lease End
    setContractTimesCalLeaseEnd();
}

// Set ContractTimes & Calculator Lease End
function setContractTimesCalLeaseEnd() {
    var valCarType = chkNull($get('cboCarType').value);

    var valFirstYear = chkNull($get('cboFirstYear').value);
    var valFirstMonth = chkNull($get('cboFirstMonth').value);

    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
    var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
    var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

    var valLeaseSttY = chkNull($get('cbo_LeaseSttY').value);
    var valLeaseSttM = chkNull($get('cbo_LeaseSttM').value);

    if ((valCarType != 0) && (valFirstYear != 0 && valFirstMonth != 0)) {
        $get('hidFirstReg').value = valFirstYear + ((parseInt(valFirstMonth) > 9) ? "" : "0") + valFirstMonth;
        if (valExpriesYear != 0 && valExpriesMonth != 0 && valExpriesDay != 0) {
            $get('hidExpiresDate').value = valExpriesYear + ((parseInt(valExpriesMonth) > 9) ? "" : "0") + valExpriesMonth + ((parseInt(valExpriesDay) > 9) ? "" : "0") + valExpriesDay;
        }
        else {
            $get('hidExpiresDate').value = '';
        }
        $get('hidLeaseSttMonth').value = valLeaseSttY + ((parseInt(valLeaseSttM) > 9) ? "" : "0") + valLeaseSttM;

        // Set value hidden carType
        $get('hidCarType').value = valCarType;

        // Set ContractTimes
        setContractTimes();

        // Change LeaseEnd
        changeLeaseEnd();
    }
}

// function get FirstTerm & AfterSecondTerm
function getValTerm(value) {
    var result = Framework.GetObjectDataFromUrl("/InpLeaseCalc/GetFirstAfterSecondTerm?carType=" + value);
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        callBackSetTerm(result.data);
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}

// function call back for cal cboContractTimes
function callBackSetTerm(rtnValue) {
    if (rtnValue) {
        $("#hidFirstTerm").val(rtnValue[0].firstTerm);
        $("#hidAfterSecondTerm").val(rtnValue[0].afterSecondTerm);
        // Set ContractTimes & Calculator Lease End
        setContractTimesCalLeaseEnd();
    }
}

function setContractTimes() {
    // clean select options ContractTimes
    removeOptions(document.getElementById('cbo_ContractTimes'));

    var hiddenFirstTerm = chkNull($get('hidFirstTerm').value);
    var hiddenAfSecondTerm = chkNull($get('hidAfterSecondTerm').value);
    var hiddenLeasePeriod = chkNull($get('hidLeasePeriod').value);
    var hiddenLeaseExpirationDate = $get('hidLeaseExpirationDate').value;

    $get('hidContractTimes').value = hiddenLeasePeriod;

    var hiddenFirstReg = $get('hidFirstReg').value.trim();
    var hiddenExpiresDate = $get('hidExpiresDate').value.trim();
    var hiddenLeaseSttMonth = $get('hidLeaseSttMonth').value.trim();

    if (hiddenExpiresDate == "" || (hiddenExpiresDate.substr(0, 6) < hiddenLeaseSttMonth)) {
        // clean select options ContractTimes
        removeOptions(document.getElementById('cbo_ContractTimes'));
    }
    else {
        var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
        var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
        var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

        var valLeaseSttY = chkNull($get('cbo_LeaseSttY').value);
        var valLeaseSttM = chkNull($get('cbo_LeaseSttM').value);

        // get lease start day
        var valLeaseSttDay = getLeaseStartDay(new Date(valExpriesYear, valExpriesMonth - 1, valExpriesDay), valLeaseSttY, valLeaseSttM - 1)

        var startDate = valLeaseSttY + '-' + valLeaseSttM.toString().padStart(2, '0') + '-' + valLeaseSttDay.toString().padStart(2, '0');
        var endDate = valExpriesYear + '-' + valExpriesMonth.toString().padStart(2, '0') + '-' + valExpriesDay.toString().padStart(2, '0');
        var curMonth = moment(endDate, "YYYY-MM-DD").diff(moment(startDate, "YYYY-MM-DD"), 'months', true);

        $get('hidDiffMonth').value = curMonth;

        curMonth = Math.floor((curMonth % 1.0) >= 0.8 ? curMonth + 1 : curMonth);

        // set option contract times
        setOptionContractTimes(curMonth, hiddenAfSecondTerm, hiddenLeasePeriod);
    }
}

// clean select options
function removeOptions(selectElement) {
    $(selectElement).empty();
    document.getElementById('lbl_LeaseEnd').innerHTML = ''
    $get('hidLeaseExpirationDate').value = ''
}

// function get lease start day
function getLeaseStartDay(expiresDate, leaseStartYear, leaseStartMonth) {
    if (expiresDate.getFullYear() == leaseStartYear && expiresDate.getMonth() == leaseStartMonth) {
        return expiresDate.getDate();
    }
    if (expiresDate.getDate() == 31) {
        return 1;
    }
    else {
        expiresDate.setDate(expiresDate.getDate() + 1);
        var nextDay = expiresDate.getDate();
        var lastDayOfMonth = getLastDayOfMonth(new Date(leaseStartYear, leaseStartMonth, 1));

        if (lastDayOfMonth >= nextDay) {
            return nextDay;
        }
        else {
            return 1;
        }
    }
}

// function get last day of month
function getLastDayOfMonth(currentDate) {
    var lastDateOfMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() + 1, 0);
    return lastDateOfMonth.getDate();
}

// set option contract times
function setOptionContractTimes(startVal, sumVal, selectedIndex) {
    var select = document.getElementById('cbo_ContractTimes');
    select.options[0] = new Option("", 0);
    if (sumVal != 0) {
        for (var i = startVal; i <= 96; i += sumVal) {
            if (i >= 24) {
                select.options[select.options.length] = new Option(i + "ヶ月", i);
                if (selectedIndex == i) {
                    select.options[select.options.length - 1].selected = true;
                }
            }
        }
    }

}


// function input only number
function onlyNumbers(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    // Set disabled for button confirm
    buttonDisabled();

    return true;
}

function onlyNumbersInsuranceFee(evt) {
    $get('InsuranceFee').style.backgroundColor = "White";

    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        return false;
    }

    // Set disabled for button confirm
    buttonDisabled();

    return true;
}

// function input only number & negative numbers
function onlyNegativeNumbers(evt, value) {
    $get('AdjustFee').style.backgroundColor = "White";
    if (evt.charCode >= 48 && evt.charCode <= 57 || evt.charCode == 45) {
        return true;
    }
    return false;
}

// function change car type
function changeCarType() {
    $get('cboCarType').style.backgroundColor = "White";
    var valCarType = chkNull($get('cboCarType').value);

    if (valCarType == 0) {
        // clean select options ContractTimes
        removeOptions(document.getElementById('cbo_ContractTimes'));
        $get('hidCarType').value = valCarType;
    }

    if (valCarType != 1) {
        $get('chkElectricCar').checked = false;
        $get('chkElectricCar').disabled = true;
        $get('hidElectricCar').value = 0;
    }
    else {
        $get('chkElectricCar').disabled = false;
    }

    // Set ContractTimes & Calculator Lease End
    $get('hidLeasePeriod').value = '';

    if ($get('hidCarType').value != valCarType) {
        getValTerm(valCarType);
    }

    // Set disabled for button confirm
    buttonDisabled();
}

// function change electric car
function changeElectricCar() {
    if (document.getElementById('chkElectricCar').checked) {
        $get('hidElectricCar').value = 1;
    }
    else {
        $get('hidElectricCar').value = 0;
    }

    // Set disabled for button confirm
    buttonDisabled();
}

// function check selected value first year month
function changeFirstYM() {
    $get('cboFirstYear').style.backgroundColor = "White";
    $get('cboFirstMonth').style.backgroundColor = "White";

    var valueFirstYear = chkNull($get('cboFirstYear').value);
    var valueFirstMonth = chkNull($get('cboFirstMonth').value);

    var todayDate = new Date();
    todayMonth = todayDate.getMonth() + 1;

    if (valueFirstYear != 0 && valueFirstMonth != 0) {
        if (valueFirstMonth != 0) {
            var firstElement = $get('cboFirstYear')[1].value;
            var lastElement = $get('cboFirstYear')[$get('cboFirstYear').length - 1].value;

            if (valueFirstYear == lastElement) {
                if (valueFirstMonth < todayMonth) {
                    $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
                    return false;
                }
            }

            if (valueFirstYear == firstElement) {
                if (valueFirstMonth > todayMonth) {
                    $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
                    return false;
                }
            }
        }

        // clean select options ContractTimes
        removeOptions(document.getElementById('cbo_ContractTimes'));
        $get('cbo_LeaseSttY').options[0].selected = true;    


        if (valueFirstYear == 0) {
            $get('cboFirstYear').style.backgroundColor = FALSE_COLOR;
        }
        if (valueFirstMonth == 0) {
            $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
        }

        // Set ContractTimes & Calculator Lease End
        setContractTimesCalLeaseEnd();
    }
    else {
        // clean select options ContractTimes
        removeOptions(document.getElementById('cbo_ContractTimes'));

        $get('cbo_LeaseSttY').options[0].selected = true;   

        if (valueFirstYear == 0) {
            $get('cboFirstYear').style.backgroundColor = FALSE_COLOR;
        }
        if (valueFirstMonth == 0) {
            $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
        }
        return false;
    }
    // Set disabled for button confirm
    buttonDisabled();
    return true;

}

function chkFirstReg() {
    var valueFirstYear = chkNull($get('cboFirstYear').value);
    var valueFirstMonth = chkNull($get('cboFirstMonth').value);

    var todayDate = new Date();
    todayMonth = todayDate.getMonth() + 1;

    if (valueFirstYear != 0 && valueFirstMonth != 0) {
        if (valueFirstMonth != 0) {
            var firstElement = $get('cboFirstYear')[1].value;
            var lastElement = $get('cboFirstYear')[$get('cboFirstYear').length - 1].value;

            if (valueFirstYear == lastElement) {
                if (valueFirstMonth < todayMonth) {
                    $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
                    return false;
                }
            }

            if (valueFirstYear == firstElement) {
                if (valueFirstMonth > todayMonth) {
                    $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
                    return false;
                }
            }
        }
    }
    else {
        // clean select options ContractTimes
        removeOptions(document.getElementById('cbo_ContractTimes'));

        $get('cbo_LeaseSttY').options[0].selected = true;
        $get('cbo_LeaseSttM').options[todayMonth - 1].selected = true;


        if (valueFirstYear == 0) {
            $get('cboFirstYear').style.backgroundColor = FALSE_COLOR;
        }
        if (valueFirstMonth == 0) {
            $get('cboFirstMonth').style.backgroundColor = FALSE_COLOR;
        }

        return false;
    }

    return true;
}


// function check selected value expires year
function changeExpiresYear() {
    //HoaiPhong Show a list of days of the month
    onchangeDay()
    $get('cbo_ExpiresYear').style.backgroundColor = "White";
    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
    var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
    var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

    if (valExpriesMonth != 0 && valExpriesDay != 0) {
        $get('cbo_ExpiresMon').style.backgroundColor = "White";
        $get('cbo_ExpiresDay').style.backgroundColor = "White";
    }

    if (valExpriesYear != 0) {
        if (valExpriesMonth == 0) {
            $get('cbo_ExpiresMon').style.backgroundColor = FALSE_COLOR;
        }
        if (valExpriesDay == 0) {
            $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
        }
        isCheckDay(valExpriesMonth, valExpriesDay);
    }
    else {
        $get('cbo_ExpiresYear').style.backgroundColor = FALSE_COLOR;
    }


    // Set ContractTimes & Calculator Lease End
    setContractTimesCalLeaseEnd();

    // Set disabled for button confirm
    buttonDisabled();

    return true;
}

function chkExpiresDate() {
    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
    var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
    var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

    if (valExpriesYear == 0 && valExpriesMonth == 0 && valExpriesDay == 0) {
        $get('cbo_ExpiresYear').style.backgroundColor = FALSE_COLOR;
        $get('cbo_ExpiresMon').style.backgroundColor = FALSE_COLOR;
        $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
        return false;
    }

    if (valExpriesYear == 0) {
        $get('cbo_ExpiresYear').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    if (valExpriesMonth == 0) {
        $get('cbo_ExpiresMon').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    if (valExpriesDay == 0) {
        $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
        return false;
    }

    return true;
}


// function check selected value expires month
function changeExpiresMonth() {
    $get('cbo_ExpiresMon').style.backgroundColor = "White";

    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
    var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
    var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

    //HoaiPhong Show a list of days of the month  
    onchangeDay()

    if (valExpriesYear != 0 && valExpriesDay != 0) {
        $get('cbo_ExpiresYear').style.backgroundColor = "White";
        $get('cbo_ExpiresDay').style.backgroundColor = "White";
    }

    if (valExpriesMonth != 0) {
        if (valExpriesYear == 0) {
            $get('cbo_ExpiresYear').style.backgroundColor = FALSE_COLOR;
        }
        if (valExpriesDay == 0) {
            $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
        }

        isCheckDay(valExpriesMonth, valExpriesDay);

    }
    else {
        $get('cbo_ExpiresMon').style.backgroundColor = FALSE_COLOR;
    }

    // Set ContractTimes & Calculator Lease End
    setContractTimesCalLeaseEnd();

    // Set disabled for button confirm
    buttonDisabled();

    return true;
}

function changeExpiresDay() {
    $get('cbo_ExpiresDay').style.backgroundColor = "White";

    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
    var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
    var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

    if (valExpriesYear != 0 && valExpriesMonth != 0) {
        $get('cbo_ExpiresYear').style.backgroundColor = "White";
        $get('cbo_ExpiresMon').style.backgroundColor = "White";
    }

    if (valExpriesDay != 0) {
        if (valExpriesYear == 0) {
            $get('cbo_ExpiresYear').style.backgroundColor = FALSE_COLOR;
        }
        if (valExpriesMonth == 0) {
            $get('cbo_ExpiresMon').style.backgroundColor = FALSE_COLOR;
        }
    }
    else {
        $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
    }
    // Set ContractTimes & Calculator Lease End
    setContractTimesCalLeaseEnd();
    // Set disabled for button confirm
    buttonDisabled();
    return true;
}

// function check selected value first year month
function changeLeaseSttYM() {
    $get('cbo_LeaseSttY').style.backgroundColor = "White";
    $get('cbo_LeaseSttM').style.backgroundColor = "White";

    var valLeaseYear = chkNull($get('cbo_LeaseSttY').value);
    var valLeaseMonth = chkNull($get('cbo_LeaseSttM').value);

    if (valLeaseYear != 0 && valLeaseMonth != 0) {
        var firstElement = $get('cboFirstYear')[1].value;
        if (valLeaseYear == firstElement) {
            var todayDate = new Date();
            todayDate = todayDate.getMonth() + 1;

            if (valLeaseMonth < todayDate) {
                $get('cbo_LeaseSttM').style.backgroundColor = FALSE_COLOR;
                return false;
            }
        }
        // Set ContractTimes & Calculator Lease End
        setContractTimesCalLeaseEnd();
    }
    // Set disabled for button confirm
    buttonDisabled();
    return true;
}

function chkLeaseSttMonth() {
    var valLeaseYear = chkNull($get('cbo_LeaseSttY').value);
    var valLeaseMonth = chkNull($get('cbo_LeaseSttM').value);

    if (valLeaseYear != 0 && valLeaseMonth != 0) {
        var firstElement = $get('cboFirstYear')[1].value;

        if (valLeaseYear == firstElement) {
            var todayDate = new Date();
            todayDate = todayDate.getMonth() + 1;

            if (valLeaseMonth < todayDate) {
                $get('cbo_LeaseSttM').style.backgroundColor = FALSE_COLOR;
                return false;
            }
        }
    }
    return true;
}

// function change ContractTimes
function changeContractTimes() {
    // Set disabled for button confirm
    buttonDisabled();

    // function change LeaseEnd
    changeLeaseEnd();
}

// function change LeaseEnd
function changeLeaseEnd() {
    $get('cbo_ContractTimes').style.backgroundColor = "White";
    var hiddenExpiresDate = $get('hidExpiresDate').value.trim();
    var valContractTimes = chkNull($get('cbo_ContractTimes').value);
    $get('hidContractTimes').value = valContractTimes;
    $get('hidLeasePeriod').value = valContractTimes;

    if (valContractTimes != 0) {
        var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
        var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
        var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);
        var dateExpries = new Date(valExpriesYear, valExpriesMonth - 1, valExpriesDay);

        var valLeaseSttY = chkNull($get('cbo_LeaseSttY').value);
        var valLeaseSttM = chkNull($get('cbo_LeaseSttM').value);
        var valLeaseSttD = getLeaseStartDay(dateExpries, valLeaseSttY, valLeaseSttM - 1);
        var dateLease = new Date(valLeaseSttY, valLeaseSttM - 1, valLeaseSttD);

        var paramDate = moment(dateLease);

        var valDiffMonth = Number($get('hidDiffMonth').value);

        paramDate.add(valContractTimes, 'M');

        var lastDayOfMonth = getLastDayOfMonth(new Date(valExpriesYear, valExpriesMonth - 1, 1));

        if (valLeaseSttD == 1 && lastDayOfMonth >= valExpriesDay) {
            paramDate.add(-1, 'M');
        }

        var chkLastDayOfMonth = getLastDayOfMonth(new Date(paramDate.year(), paramDate.month(), 1));
        var dateFormat = new Date();

        if (chkLastDayOfMonth >= valExpriesDay) {
            dateFormat = new Date(paramDate.year(), paramDate.month(), valExpriesDay);

        } else {
            dateFormat = new Date(paramDate.year(), paramDate.month(), chkLastDayOfMonth);
        }

        $get('hidLeaseExpirationDate').value = dateFormat.getFullYear() + (dateFormat.getMonth() + 1 > 9 ? '' : '0') + (dateFormat.getMonth() + 1) + (dateFormat.getDate() > 9 ? '' : '0') + dateFormat.getDate();
        var valueLeaseEnd = dateFormat.getFullYear() + '年' + (dateFormat.getMonth() + 1) + '月' + dateFormat.getDate() + '日';
        document.getElementById('lbl_LeaseEnd').innerHTML = valueLeaseEnd;
    }
    else {
        $get('hidLeaseExpirationDate').value = '';
        document.getElementById('lbl_LeaseEnd').innerHTML = $get('hidLeaseExpirationDate').value;
    }
}

// function change contract plan
function changeContractPlan() {
    $get('hidContractPlan').value = $get('cbo_ContractPlan').value;
    // Set disabled for button confirm
    buttonDisabled();
}

// function change contract plan
function changeExtendedGuarantee() {
    $get('hidInsurExpanded').value = $get('cbo_InsurExpanded').value;
    // Set disabled for button confirm
    buttonDisabled();
}
// function change option Insurance
function changeOptionIns(value) {
    var DISABLE_COLOR = '#A9A9A9';
    var valOpIns = chkNull($get('cbo_OptionInsurance').value);
    $get('hidOptionInsurance').value = valOpIns;

    if (valOpIns == 1) {
        $get('cbo_InsuranceCompany').disabled = false;
        $get('cbo_InsuranceCompany').style.backgroundColor = "White";
        $get('InsuranceFee').disabled = false;
        $get('InsuranceFee').style.backgroundColor = "White";
        var select = document.getElementById('cbo_InsuranceCompany');
        select.options[0].selected = true;
        $get('hidInsuranceCompany').value = chkNull($get('cbo_InsuranceCompany').value);
        document.getElementById('InsuranceFee').value = 0;
    }
    else {
        $get('cbo_InsuranceCompany').disabled = true;
        $get('cbo_InsuranceCompany').value = '';
        $get('cbo_InsuranceCompany').style.backgroundColor = DISABLE_COLOR;
        $get('hidInsuranceCompany').value = -1;
        document.getElementById('InsuranceFee').value = 0;
        document.getElementById('InsuranceFee').setAttribute('disabled', true);
        $get('InsuranceFee').style.backgroundColor = DISABLE_COLOR;
    }
    // Set disabled for button confirm
    if (value == true) {
        buttonDisabled();
    }

}

// function change InsuranceCompany
function changeInsuranCompany() {
    $get('cbo_InsuranceCompany').style.backgroundColor = "White";
    var valInsCompany = chkNull($get('cbo_InsuranceCompany').value);
    $get('hidInsuranceCompany').value = valInsCompany;
    // Set disabled for button confirm
    buttonDisabled();
}

// function check InsuranceCompany
function chkInsuranCompany() {
    var valInsCompany = chkNull($get('hidInsuranceCompany').value);

    if (valInsCompany == 99) {
        $get('cbo_InsuranceCompany').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    return true;
}

// Set disabled for button confirm when onblur text
function chgSetBtnConfim() {
    // Set disabled for button confirm
    buttonDisabled();
}

// check selected carType # empty
function chkCarType() {
    var carType = chkNull($get('cboCarType').value);

    if (carType == 0) {
        $get('cboCarType').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    return true;
}

/**
 * [Button] Lease Calculator
 * 
 */
function inputChk() {
    if (!chkCarType()) {
        return false;
    }
    if (!chkFirstReg()) {
        return false;
    }
    if (!chkExpiresDate()) {
        return false;
    }
    if (!chkLeaseSttMonth()) {
        return false;
    }
    if (!chkExpiresDateOverLeaseSttMonth()) {
        $get('cbo_ExpiresYear').style.backgroundColor = FALSE_COLOR;
        $get('cbo_ExpiresMon').style.backgroundColor = FALSE_COLOR;
        $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    if (!chkLeasePeriodLeaseExpirationDate()) {
        $get('cbo_ContractTimes').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    if (!chkInsuranCompany()) {
        return false;
    }
    if ($get('cbo_OptionInsurance').value == 1 && chkNull($get('InsuranceFee').value) == 0) {

        $get('InsuranceFee').style.backgroundColor = FALSE_COLOR;
        return false;
    }

    // check input number AdjustFee
    if (!chkInputNumbersAdjustFee()) {
        $get('AdjustFee').style.backgroundColor = FALSE_COLOR;
        return false;
    }
    if (!chkValueInput()) {
        return false;
    }
    return true;
}

// check ExpiresDate over LeaseSttMonth
function chkExpiresDateOverLeaseSttMonth() {
    $get('cbo_ExpiresYear').style.backgroundColor = "White";
    $get('cbo_ExpiresMon').style.backgroundColor = "White";
    $get('cbo_ExpiresDay').style.backgroundColor = "White";
    var valExpiresDate = chkNull($get('hidExpiresDate').value.substr(0, 6))
    var valLeaseSttMonth = chkNull($get('hidLeaseSttMonth').value)
    if (valExpiresDate < valLeaseSttMonth) {
        // clean select options ContractTimes
        removeOptions(document.getElementById('cbo_ContractTimes'));
        return false;
    }
    return true;
}

// check LeasePeriod & LeaseExpirationDate
function chkLeasePeriodLeaseExpirationDate() {
    if (chkNull($get('hidLeasePeriod').value) == 0 || chkNull($get('hidContractTimes').value) == 0) {
        return false;
    }
    return true;
}

function chkValueInput() {
    let iscallBack = true;
    var result = Framework.GetObjectDataFromUrl("/InpLeaseCalc/GetUnitPriceRatesLimit");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        iscallBack = callBackSetUnitPriceRatesLimit(result.data)
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
    return iscallBack;
}

function callBackSetUnitPriceRatesLimit(rtnValInput) {
    document.getElementById("lbl_ErrPrePay").style.display = "none";
    document.getElementById("lbl_ErrTradeIn").style.display = "none";
    document.getElementById("lbl_ErrAdjustFee").style.display = "none";

    // Get value input
    var valPrePay = chkNull($get('PrePay').value);
    var valTradeIn = chkNull($get('TradeIn').value);
    var valAdjustFee = chkNull($get('AdjustFee').value);

    // init value input for cal
    var valCalPrePay = 0;
    var valCalTradeIn = 0;
    var valCalAdjustFee = 0;

    if (valPrePay != 0) {
        valCalPrePay = valPrePay % rtnValInput.unitPrice;
    }
    if (valTradeIn != 0) {
        valCalTradeIn = valTradeIn % rtnValInput.unitPrice;
    }
    if (valAdjustFee != 0) {
        valCalAdjustFee = valAdjustFee % rtnValInput.unitPrice;
    }

    // check value input numeric % unitPrice
    if (valCalPrePay != 0 || valCalTradeIn != 0 || valCalAdjustFee != 0) {
        if (valCalPrePay != 0) {
            document.getElementById("lbl_ErrPrePay").style.display = "inline";
            document.getElementById("lbl_ErrPrePay").innerHTML = rtnValInput.unitPrice + '円単位で入力してください。';
        }
        if (valCalTradeIn != 0) {
            document.getElementById("lbl_ErrTradeIn").style.display = "inline";
            document.getElementById("lbl_ErrTradeIn").innerHTML = rtnValInput.unitPrice + '円単位で入力してください。';
        }
        if (valCalAdjustFee != 0) {
            document.getElementById("lbl_ErrAdjustFee").style.display = "inline";
            document.getElementById("lbl_ErrAdjustFee").innerHTML = rtnValInput.unitPrice + '円単位で入力してください。';

            if ((valAdjustFee < (rtnValInput.lowerLimit)) || (valAdjustFee > rtnValInput.upperLimit)) {
                document.getElementById("lbl_ErrAdjustFee").innerHTML = rtnValInput.unitPrice + '円単位で入力してください。' + ' ' + rtnValInput.lowerLimit + '～' + rtnValInput.upperLimit + '円に収まるように入力してください。';
            }
        }
        else if ((valAdjustFee < (rtnValInput.lowerLimit)) || (valAdjustFee > rtnValInput.upperLimit)) {
            document.getElementById("lbl_ErrAdjustFee").style.display = "inline";
            document.getElementById("lbl_ErrAdjustFee").innerHTML = rtnValInput.lowerLimit + '～' + rtnValInput.upperLimit + '円に収まるように入力してください。';
        }

        return false;
    }

    // check value input numeric AdjustFee over LowerLimit & UpperLimit
    if ((valAdjustFee < (rtnValInput.lowerLimit)) || (valAdjustFee > rtnValInput.upperLimit)) {
        document.getElementById("lbl_ErrAdjustFee").style.display = "inline";
        document.getElementById("lbl_ErrAdjustFee").innerHTML = rtnValInput.lowerLimit + '～' + rtnValInput.upperLimit + '円に収まるように入力してください。';

        return false;
    }
    calcDateDiff();
    return true;
    // Wait 0.5 second for above calculation complete
    //setTimeout(function () { return __doPostBack('btnLeaseCalc', ''); }, 500);

}

// check input number AdjustFee
function chkInputNumbersAdjustFee() {
    var value = $get('AdjustFee').value;

    if (!isNaN(Number(value))) {
        return true;
    }
    else {
        return false;
    }
}

/**
 * show ShowErrorMessage
 * Create by HoaiPhong
 */
function isShowErrorMessage(valCheck) {
    document.getElementById("lbl_ErrorMessage").style.display = 'contents';
    document.getElementById("lbl_ErrorMessage").innerHTML = '';

    if (valCheck == '' || typeof valCheck == "undefined") {
        document.getElementById("lbl_ErrorMessage").innerHTML = 'この車両はリース対象外です。';

    }
    else {
        document.getElementById("lbl_ErrorMessage").innerHTML = '月額リース料' + valCheck + '円以上になるように調整してください。';
    }

    // Set disabled for button confirm
    buttonDisabled();

}

/**
 * Button Disabled   
 * Create by HoaiPhong
 */
function buttonDisabled() {
    document.getElementById("btnExamination").disabled = true;
    document.getElementById("btnBookExam").disabled = true;
    document.getElementById("btnPrintShow").disabled = true;

    return;
}
/**
 *  Button Enabling
 * Create by HoaiPhong
 */
function buttonEnabling() {
    document.getElementById("btnBookExam").disabled = false;
    document.getElementById("btnExamination").disabled = false;
    document.getElementById("btnPrintShow").disabled = false;
    $get('hidIsData').value == true;
    return;
}
/**
/**
 *  Show a list of days of the month
 * Create [2022/07/26] by HoaiPhong 
 */
function onchangeDay() {
    var vExpiresYear = chkNull($get('cbo_ExpiresYear').value)
    var vcbo_ExpiresMon = chkNull($get('cbo_ExpiresMon').value)
    let lengthExpiresDay = document.getElementById("cbo_ExpiresDay").length;
    removeAttributeExpiresDay()
    if (vExpiresYear != 0 || vcbo_ExpiresMon != 0) {
        let year = parseInt(vExpiresYear);
        let month = parseInt(vcbo_ExpiresMon);
        let day = new Date(year, month, 0);
        let lastDayOfMonth = parseInt(day.getDate()) + 1;
        for (let i = lastDayOfMonth; i < lengthExpiresDay; i++) {
            document.getElementById("cbo_ExpiresDay")[i].setAttribute("style", "display:none");
        }
    }
}
/**
/**
 *  removeAttributeExpiresDay
 * Create [2022/07/26] by HoaiPhong 
 */
function removeAttributeExpiresDay() {
    for (let i = 28; i <= 31; i++) {
        document.getElementById("cbo_ExpiresDay").getElementsByTagName("option")[i].removeAttribute("style");
    }
}
/**
/**
 *  isCheckDay
 * Create [2022/07/28] by HoaiPhong 
 */
function isCheckDay(valExpriesMonth, valExpriesDay) {
    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value)
    let day = new Date(valExpriesYear, valExpriesMonth, 0);
    let lastDayOfMonth = parseInt(day.getDate());
    if (lastDayOfMonth < valExpriesDay && valExpriesDay != 0) {
        $get('cbo_ExpiresDay').options[0].selected = true;
        $get('cbo_ExpiresDay').style.backgroundColor = FALSE_COLOR;
        return false;
    }
}

/**
 * appendLogUI
 * Create [2022/08/10] by HoaiPhong 
 */
function appendLogUI(array) {
    for (var item in array) {
        var div = document.getElementById("listlogLease")
        var span = document.createElement("span");
        span.setAttribute("style", "font-size:12pt;font-weight:bold;");
        var br = document.createElement("br");
        span.classList += 'inplabel';
        span.id = 'Label1';
        span.innerHTML = array[item];
        div.appendChild(span);
        div.appendChild(br)

    }
}
/**
 * calcDateDiff
 * Create [2022/08/10] by HoaiPhong 
 */
function calcDateDiff() {
    var valExpriesYear = chkNull($get('cbo_ExpiresYear').value);
    var valExpriesMonth = chkNull($get('cbo_ExpiresMon').value);
    var valExpriesDay = chkNull($get('cbo_ExpiresDay').value);

    var valLeaseSttY = chkNull($get('cbo_LeaseSttY').value);
    var valLeaseSttM = chkNull($get('cbo_LeaseSttM').value);
    // get lease start day
    var valLeaseSttDay = getLeaseStartDay(new Date(valExpriesYear, valExpriesMonth - 1, valExpriesDay), valLeaseSttY, valLeaseSttM - 1);

    // set lease start day
    $get('hidLeaseSttDay').value = valLeaseSttDay;
    var valFirstRegY = chkNull($get('cboFirstYear').value);
    var valFirstRegM = chkNull($get('cboFirstMonth').value);
    var lastDayOfMonth = getLastDayOfMonth(new Date(valFirstRegY, valFirstRegM - 1, 1));
    var valFirstRegD = 1;
    if (lastDayOfMonth >= valExpriesDay) {
        // use expires day
        valFirstRegD = valExpriesDay;
    } else {
        // last day of registration month
        valFirstRegD = lastDayOfMonth;
    }
    var valHidLeaseEndDate = chkNull($get('hidLeaseExpirationDate').value);
    var valLeaseEndY = parseInt(valHidLeaseEndDate.toString().substring(0, 4));
    var valLeaseEndM = parseInt(valHidLeaseEndDate.toString().substring(4, 6));
    var valLeaseEndD = parseInt(valHidLeaseEndDate.toString().substring(6, 8));
    var leaseEndDate = new Date(valLeaseEndY, valLeaseEndM - 1, valLeaseEndD);

    var firstRegDate = new Date(valFirstRegY, valFirstRegM - 1, valFirstRegD);
    var leaseSttDate = new Date(valLeaseSttY, valLeaseSttM - 1, valLeaseSttDay);
    // Add 13 year from first registration
    var after13YearDate = moment(firstRegDate).add(156, 'M');

    if ((valFirstRegM.toString().padStart(2, '0') + '' + valFirstRegD.toString().padStart(2, '0')) > '0401') {
        // fiscal year in april
        after13YearDate.add(12, 'M');    // Next year
        after13YearDate.set('month', 2); // March
        after13YearDate.set('date', valFirstRegD);
    }
    // after 13 year not in lease period
    if (after13YearDate <= leaseSttDate || leaseEndDate < after13YearDate) {
        $get('hidCalcDiffMonth').value = 0;
    } else {
        var diffMonth = moment(after13YearDate, "YYYY-MM-DD").diff(moment(leaseSttDate, "YYYY-MM-DD"), 'months', true);
        diffMonth = Math.floor((diffMonth % 1.0) >= 0.8 ? diffMonth + 1 : diffMonth);
        $get('hidCalcDiffMonth').value = diffMonth;
    }
}

/**
 * GetCarType
 * Create [2022/09/02] by HoaiPhong 
 */
function GetCarType() {
    var result = Framework.GetObjectDataFromUrl("/InpLeaseCalc/GetCarType");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;
        $("#cboCarType").append(new Option("", ''));
        $("#cboCarType").append(new Option(result.data[1].carTypeName, result.data[1].carType));
        for (let i = 0; i < length; i++) {
            let key = result.data[i].carType;
            let value = result.data[i].carTypeName;
            if (key != 2) {
                $("#cboCarType").append(new Option(value, key));
            }
        }
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}
/**
 * GetContractPlan
 * Create [2022/09/02] by HoaiPhong 
 */
function GetContractPlan() {
    var result = Framework.GetObjectDataFromUrl("/InpLeaseCalc/GetContractPlan");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;
        for (let i = 0; i < length; i++) {
            let key = result.data[i].id;
            let value = result.data[i].planName;
            $("#cbo_ContractPlan").append(new Option(value, key));
        }
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}
/**
 * GetVolInsurance
 * Create [2022/09/02] by HoaiPhong 
 */
function GetVolInsurance() {
    var result = Framework.GetObjectDataFromUrl("/InpLeaseCalc/GetVolInsurance");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        let length = result.data.length;     
        $("#cbo_InsuranceCompany").append(new Option('選択してください', "99"));

        for (let i = 0; i < length; i++) {
            let key = result.data[i].id;
            let value = result.data[i].companyName;
            if (key != "99") {
                $("#cbo_InsuranceCompany").append(new Option(value, key));
            }
        }
        $get('cbo_InsuranceCompany').value = '';
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}
/**
 * InpLeaseCal
 * Create [2022/09/02] by HoaiPhong 
 */
function InpLeaseCal() {
    document.getElementById("lbl_ErrorMessage").style.display = 'contents';
    document.getElementById("lbl_ErrorMessage").innerHTML = '';
    if (inputChk()) {
        var model = Framework.getFormData($("#formInpLeaseCalc"));
        model.FirstReg = SetFirstReg(model.FirstReg);
        model.LeaseSttMonth = SetLeaseSttMonth(model.LeaseSttMonth);
        var result = Framework.submitAjaxFormUpdateAsync(model, "/InpLeaseCalc/InpLeaseCal");
        if (result.resultStatus == 0 && result.messageCode === 'I0002') {
            var item = result.data;
            $("#hidIsData").val(item.IsShowButton);
            $("#lbl_MonthlyLease").text(item.priceEnd);
            if (parseInt(item.priceEnd) > 0) {
                $("#Label15").text("円");
            }
            buttonEnabling();
            appendLogUI(item.listUILog);
        } else if (result.resultStatus == 0 && result.messageCode === 'I0003') {
            var item = result.data;
            if (item.priceLeaseFeeLowerLimit != 0) { isShowErrorMessage(item.priceLeaseFeeLowerLimit) } else {
                isShowErrorMessage("");
            }
        } else {
            Framework.GoBackErrorPage(result.messageCode, result.messageContent);
        }
    }

}
/**
 * btnExamination
 * Create [2022/09/02] by HoaiPhong 
 */
function Examination() {
    let leaseProgress = 2;
    var result = Framework.submitAjaxFormUpdateAsync(leaseProgress, "/InpLeaseCalc/UpdateLeaseProgressIde");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPageUrl("/PreExamination")
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}
/**
 * btnBookExam
 * Create [2022/09/02] by HoaiPhong 
 */
function BookExam() {
    let leaseProgress = 1;
    var result = Framework.submitAjaxFormUpdateAsync(leaseProgress, "/InpLeaseCalc/UpdateLeaseProgressIde");
    if (result.resultStatus == 0 && result.messageCode === 'I0002') {
        Framework.GoBackReloadPageUrl("/PreExamination")
    } else {
        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
    }
}
/**
 * btnPrintShow
 * Create [2022/09/02] by HoaiPhong 
 */
function PrintShow() {
    Framework.GoBackReloadPageUrl("/Report/DownloadEstimateReport")

}
/**
 * SetFirstReg
 * Create [2022/09/02] by HoaiPhong 
 */
function SetFirstReg(FirstReg) {
    var valFirstRegY = chkNull($get('cboFirstYear').value);
    var valFirstRegM = chkNull($get('cboFirstMonth').value);
    var valExpiresDay = chkNull($get('cbo_ExpiresDay').value);
    var lastDayOfMonth = GetDaysInMonth(valFirstRegY, valFirstRegM);
    if (lastDayOfMonth > parseInt(valExpiresDay)) {
        let day = (parseInt(valExpiresDay) < 10 ? +"0" + valExpiresDay.toString() : valExpiresDay.toString())
        return FirstReg + day;
    } else {
        let day = (lastDayOfMonth) < 10 ? +"0" + lastDayOfMonth.toString() : lastDayOfMonth.toString();
        return FirstReg + day;
    }
}
/**
 * SetLeaseSttMonth
 * Create [2022/09/02] by HoaiPhong 
 */
function SetLeaseSttMonth(LeaseSttMonth) {
    var valExpiresDay = chkNull($get('cbo_ExpiresDay').value);
    let day = (parseInt(valExpiresDay) < 10 ? +"0" + valExpiresDay.toString() : valExpiresDay.toString())
    return LeaseSttMonth + day;
}
/**
 * setValueCbbYear
 * Create [2022/09/02] by HoaiPhong 
 */
function setValueCbbYear(Id, value) {
    var Year = value.toString().substring(0, 4);
    Framework.SetSelectedString(Id, Year);
}
/**
 * setValueCbbMonth
 * Create [2022/09/02] by HoaiPhong 
 */
function setValueCbbMonth(Id, value) {
    var Month = value.toString().substring(4, 6);
    if (parseInt(Month) < 10) {
        Month = Month.substring(1, 2)
    }
    Framework.SetSelectedNumber(Id, Month);
    setContractTimesCalLeaseEnd();
}
/**
 * setValueCbbDay
 * Create [2022/09/02] by HoaiPhong 
 */
function setValueCbbDay(Id, value) {
    var Day = value.toString().substring(8, 6);
    if (parseInt(Day) < 10) {
        Day = Day.substring(1, 2)
    }
    Framework.SetSelectedNumber(Id, Day);

}
