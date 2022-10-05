/*#
#Framework 
#Create [2022/08/01] By Hoài Phong 
 */
"use strict";

function _instanceof(left, right) { if (right !== null && typeof Symbol !== "undefined" && right[Symbol.hasInstance]) { return right[Symbol.hasInstance](left); } else { return left instanceof right; } }

function _classCallCheck(instance, Constructor) { if (!_instanceof(instance, Constructor)) { throw new TypeError("Cannot call a class as a function"); } }

function _defineProperties(target, props) { for (var i = 0; i < props.length; i++) { var descriptor = props[i]; descriptor.enumerable = descriptor.enumerable || false; descriptor.configurable = true; if ("value" in descriptor) descriptor.writable = true; Object.defineProperty(target, descriptor.key, descriptor); } }

function _createClass(Constructor, protoProps, staticProps) { if (protoProps) _defineProperties(Constructor.prototype, protoProps); if (staticProps) _defineProperties(Constructor, staticProps); return Constructor; }

var Framework =
    /*#__PURE__*/
    function () {
        function Framework() {
            _classCallCheck(this, Framework);
        }
        _createClass(Framework, null, [{
            key: "getFormData",
            value: function getFormData($form) {
                var unindexed_array = $form.serializeArray();

                var checkFalse = $($form).find('input[type=checkbox]:not(:checked)').map(function () {
                    return {
                        "name": this.name,
                        "value": 0
                    };
                }).get();
                var checkTrue = $($form).find('input[type=checkbox]:checked').map(function () {
                    return {
                        "name": this.name,
                        "value": 1
                    };
                }).get();
                //var checkRadioFalse = $($form).find('input[type=radio]:not(:checked)').map(function () {
                //    return {
                //        "name": this.name,
                //        "value": this.value
                //    };
                //}).get();
                var checkRadioTrue = $($form).find('input[type=radio]:checked').map(function () {
                    return {
                        "name": this.name,
                        "value": this.value
                    };
                }).get();
                var number = $form.find('input[type=number]').map(function () {
                    if (!this.value) {
                        return {
                            "name": this.name,
                            "value": null
                        };
                    }
                    else {
                        return {
                            "name": this.name,
                            "value": parseFloat(this.value)
                        };
                    }

                }).get();
                var currency = $form.find('input[data-type="currency"]').map(function () {
                    if (!this.value) {
                        return {
                            "name": this.name,
                            "value": null
                        };
                    }
                    else {
                        return {
                            "name": this.name,
                            "value": parseFloat(this.value.replace(/,/g, ''))
                        };
                    }

                }).get();
                var textarea = $form.find('textarea').map(function () {
                    return {
                        "name": this.name,
                        "value": this.value
                    };
                }).get();
                var hidden = $form.find('input[type=hidden]').map(function () {
                    return {
                        "name": this.name,
                        "value": this.value
                    };
                }).get();
                var text = $form.find('input[type=text]').not('input[data-type="currency"]').map(function () {
                    return {
                        "name": this.name,
                        "value": this.value
                    };
                }).get();
                unindexed_array = unindexed_array.concat(checkFalse, checkTrue, checkRadioTrue, number, currency, textarea, hidden, text);
                var indexed_array = {};
                $.map(unindexed_array, function (n, i) {
                    indexed_array[n['name']] = n['value'];
                });
                unindexed_array = null;
                checkFalse = null;
                checkTrue = null;
                //checkRadioFalse = null;
                checkRadioTrue = null;
                textarea = null
                hidden = null;
                text = null;
                return indexed_array;
            }
        },
        {
            key: "convertToDate",
            value: function convertToDate(value) {
                if (!value) {
                    return null;
                }
                else if (value.length > 8) {
                    return moment(value, DATE_FORMAT_MOMENT_WITH_TIME).toDate().toUTCString();
                }
                else {
                    return moment(value, DATE_FORMAT_MOMENT).toDate().toUTCString();
                }
            }
        },
        {
            key: "convertToCurrency",
            value: function convertToCurrency(value) {
                if (!value) null;
                else {
                    return parseFloat(value.replace(/,/g, ''));
                }
            }
        },
        {
            key: "resetForm",
            value: function resetForm($form) {
                $form.find("input, textarea").not(".defaultValue").val("");
                var validator = $form.not(".defaultValue").validate();
                validator.resetForm();
            }

        },
        {
            key: "getRandomNumber",
            value: function getRandomNumber() {
                return Math.random() * 10;
            }

        },
        {
            key: "GetDataFromUrl",
            value: function GetDataFromUrl(url) {
                var result = [];
                $.ajax({
                    type: "GET",
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function success(res) {
                        result = res.data;
                    },
                    error: function error(xhr, status, thrownError, _error2) {
                        result = [];
                    }
                });
                return result;
            }

        },
        {
            key: "GetObjectDataFromUrl",
            value: function GetObjectDataFromUrl(url) {
                var result = {};
                $.ajax({
                    type: "GET",
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function success(res) {
                        result = res;
                    },
                    error: function error(xhr, status, thrownError, _error2) {
                        result.success = false;
                        result.responseText = xhr.status + ' ' + thrownError;
                    }
                });
                return result;
            }

        },
        {
            key: "GetObjectDataFromUrlById",
            value: function GetObjectDataFromUrlById(url, id) {
                var result = {};
                $.ajax({
                    type: "GET",
                    data: id,
                    url: url,
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    async: false,
                    success: function success(data) {
                        result = data;
                    },
                    error: function error(xhr, status, thrownError, _error2) {
                        result = {};
                    }
                });
                return result;
            }
        },
        {
            key: "alertSuccess",
            value: function alertSuccess(responseText) {
                $.alert({
                    type: 'green',
                    icon: 'fa fa-check-circle',
                    title: 'Successfully!',
                    content: responseText
                });
            }
        }, {
            key: "alertError",
            value: function alertError(responseText) {
                $.alert({
                    type: 'red',
                    icon: 'fa fa fa-warning',
                    title: 'Error!',
                    content: responseText
                });
            }
        }, {
            key: "alertWarning",
            value: function alertWarning(responseText) {
                $.alert({
                    type: 'orange',
                    icon: 'fa fa fa-warning',
                    title: 'Warning!',
                    content: responseText
                });
            }
        },
        {
            key: "toastSuccess",
            value: function toastSuccess(responseText) {
                $.toast({
                    heading: 'Success',
                    text: responseText,
                    showHideTransition: 'slide',
                    icon: 'success',
                    position: 'top-right'
                });
            }
        },
        {
            key: "toastWarning",
            value: function toastWarning(responseText) {
                $.toast({
                    heading: 'Warning',
                    text: responseText,
                    showHideTransition: 'plain',
                    icon: 'warning',
                    position: 'top-right'
                });
            }
        },
        {
            key: "submitAjaxFormCreateAwait",
            value: function submitAjaxFormCreateAwait(data, url) {
                var result = {};
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        console.log(_error)
                        location.reload();
                    }
                });
                return result;
            }
        },
        {
            key: "submitAjaxLoadData",
            value: function submitAjaxLoadData(data, url) {
                var result = {};
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        console.log(_error)
                        location.reload();
                    }
                });
                return result;
            }
        },
        {
            key: "submitAjaxRedirect",
            value: function submitAjaxRedirect(data, url) {
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        requestData: data
                    },
                    complete: function (jqxhr) {
                        window.location.href(url)
                    }
                });
            }
        },
        {
            key: "submitAjaxFormDeleteEstNoAndEstSubNoAwait",
            value: function submitAjaxFormDeleteEstNoAndEstSubNoAwait(url) {
                var result = {};
                $.ajax({
                    type: "POST",
                    url: url,
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        console.log(_error)
                        location.reload();
                    }
                });
                return result;
            }
        },
        {
            key: "submitAjaxFormDeleteAwait",
            value: function submitAjaxFormDeleteAwait(data, url) {
                var result = {};
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        console.log(_error)
                        location.reload();

                    }
                });
                return result;
            }
        },
        {
            key: "submitAjaxFormUpdateAsync",
            value: function submitAjaxFormUpdateAsync(data, url) {

                var result = {};
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        console.log(_error)
                        location.reload();

                    }
                });
                return result;
            }
        },
        {
            key: "SummitForm",
            value: function SummitForm(path, params, method = 'post') {
                const form = document.createElement('form');
                form.method = method;
                form.action = path;

                for (const key in params) {
                    if (params.hasOwnProperty(key)) {
                        const hiddenField = document.createElement('input');
                        hiddenField.type = 'hidden';
                        hiddenField.name = key;
                        hiddenField.value = params[key];
                        form.appendChild(hiddenField);
                    }
                }
                document.body.appendChild(form);
                form.submit();
            }
        },
        {
            key: "formatNumber",
            value: function formatNumber(n) {
                return n.replace(/\D/g, "").replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            }
        },
        {
            key: "toCurrency",
            value: function toCurrency(input_val) {

                if (input_val === "") { return ""; }
                var original_len = input_val.length;
                var mark = "";
                if (input_val.charAt(0) === "-") {
                    mark = "-";
                }
                if (input_val.indexOf(".") >= 0) {
                    var decimal_pos = input_val.indexOf(".");
                    var left_side = input_val.substring(0, decimal_pos);
                    var right_side = input_val.substring(decimal_pos);

                    left_side = Framework.formatNumber(left_side);
                    right_side = Framework.formatNumber(right_side);
                    right_side = right_side.substring(0, 2);
                    return mark + left_side + "." + right_side;

                } else {
                    return mark + Framework.formatNumber(input_val);
                }
            }
        },
        {
            key: "SetSelected",
            value: function SetSelected(nameId, defaultValue) {
                let idOption = $("#" + nameId + " option");
                console.log(nameId);
                console.log(defaultValue);
                if (defaultValue === null || defaultValue === "" || defaultValue === " ") {
                    idOption[0].selected == true;
                }
                else {
                    $("#" + nameId + " option[value='" + defaultValue + "']").attr("selected", "selected");
                }
                return;
            }
        },
        {
            key: "SetSelectedString",
            value: function SetSelectedString(nameId, defaultValue) {
                let idOption = $("#" + nameId + " option");
                if (defaultValue === null || defaultValue === "" || defaultValue === " ") {
                    idOption[0].selected == true;
                }
                else {
                    let length = idOption.length;
                    for (let i = 1; i < length; i++) {
                        let value = idOption[i].value;
                        if (value.includes(defaultValue)) {
                            $("#" + nameId + " option[value='" + value + "']").attr("selected", "selected");
                            ;
                        }
                    }
                }
                return;
            }
        },
        {
            key: "SetSelectedNumber",
            value: function SetSelectedNumber(nameId, defaultValue) {
                let idOption = $("#" + nameId + " option");
                if (isNaN(defaultValue) || defaultValue === 0) {
                    idOption[0].selected == true;
                }
                else {
                    let length = idOption.length;
                    for (let i = 0; i < length; i++) {
                        let value = parseInt(idOption[i].value);
                        if (value === defaultValue) {
                            $("#" + nameId + " option[value='" + value + "']").attr("selected", "selected");
                            return;
                        }
                    }
                }
                return;
            }
        },
        {
            key: "SetCheckValueById",
            value: function SetCheckValueById(nameId, defaultValue) {
                console.log(nameId);
                $("#" + nameId + "").attr('checked', true);
                return;
            }
        },
        {
            key: "distinct",
            value: function distinct(value, index, self) {
                return self.indexOf(value) === index;

            }
        },
        {
            key: "GoBackReloadPage",
            value: function GoBackReloadPage() {
                window.location.href = "/Estmain?IsInpBack=1";

            }
        },

        {
            key: "GoBackErrorPage",
            value: function GoBackErrorPage(messageCode, messContent) {
                var param = {};
                param.messageCode = messageCode;
                param.messContent = messContent;
                Framework.SummitForm("/Error/ErrorPage", param)
                //var url = "/Error/ErrorPage?messageCode=" + messageCode + " &messContent=" + messContent;
                //window.location.href = url;

            }
        },
        {
            key: "GoBackPage",
            value: function GoBackPage() {
                let url = window.location.href;
                if (url.includes("#")) {
                    window.history.go(-2);
                } else {
                    window.history.back();
                }
            }
        },
        {
            key: "GoBackReloadPageUrl",
            value: function GoBackReloadPageUrl(PageUrl) {
                var ListUrl = ["/InpSitaCar", "/"];
                let LeaseFlag = parseInt($("#hidLeaseFlag").val());
                if (LeaseFlag == 1 && ListUrl.includes(PageUrl)) {
                    alert("リース画面でのみ、下取りの設定が可能。");
                } else {
                    window.location.href = PageUrl;
                }
            }
        },
        {
            key: "SummitForm",
            value: function SummitForm(path, params, method = 'post') {
                const form = document.createElement('form');
                form.method = method;
                form.action = path;

                for (const key in params) {
                    if (params.hasOwnProperty(key)) {
                        const hiddenField = document.createElement('input');
                        hiddenField.type = 'hidden';
                        hiddenField.name = key;
                        hiddenField.value = params[key];
                        form.appendChild(hiddenField);
                    }
                }

                document.body.appendChild(form);
                form.submit();
            }
        },
        {
            key: "OnSubmitForm",
            value: function OnSubmitForm(Url, idForm) {
                var model = Framework.getFormData($("#" + idForm));
                var result = Framework.submitAjaxFormUpdateAsync(model, Url);
                if (result.resultStatus == 0) {
                    Framework.GoBackReloadPage();
                } else {
                    Framework.GoBackErrorPage(result.messageCode, result.messageContent);
                }
            }
            },
            {
                key: "OnSubmitFormValueModel",
                value: function OnSubmitFormValueModel(Url, model) {                  
                    var result = Framework.submitAjaxFormUpdateAsync(model, Url);
                    if (result.resultStatus == 0) {
                        Framework.GoBackReloadPage();
                    } else {
                        Framework.GoBackErrorPage(result.messageCode, result.messageContent);
                    }
                }
            },
        {
            key: "SortDataTable",
            value: function SortDataTable(tableId, sortName) {
                var table = $("#" + tableId);
                $('#' + sortName)
                    .each(function () {
                        var th = $(this),
                            thIndex = th.index(),
                            inverse = false;
                        th.click(function () {

                            table.find('td').filter(function () {
                                return $(this).index() === thIndex;

                            }).sortElements(function (a, b) {
                                return $.text([a]) > $.text([b]) ?
                                    inverse ? -1 : 1
                                    : inverse ? 1 : -1;

                            }, function () {
                                return this.parentNode;
                            });
                            inverse = !inverse;

                        });

                    });
            }
        },

        {
            key: "SorDataTable1",
            value: function SorDataTable1(tableId, sortName) {
                var $table = $("#" + tableId);
                let sortOrder = 0;
                $table.find('th').each(function (col) {
                    $(this).hover(
                        function () {
                            $(this).addClass('focus');
                        },
                        function () {
                            $(this).removeClass('focus');
                        }
                    );
                    $(this).click(function () {
                        if ($(this).is('.asc')) {
                            $(this).removeClass('asc');
                            $(this).addClass('desc selected');
                            sortOrder = -1;
                        } else {
                            $(this).addClass('asc selected');
                            $(this).removeClass('desc');
                            sortOrder = 1;
                        }
                        $(this).siblings().removeClass('asc selected');
                        $(this).siblings().removeClass('desc selected');
                        var arrData = $table.find('tbody >tr:has(td)').get();
                        arrData.sort(function (a, b) {
                            var val1 = $(a).children('td').not("#tbremotepage").eq(col).text().toUpperCase();
                            var val2 = $(b).children('td').not("#tbremotepage").eq(col).text().toUpperCase();
                            if ($.isNumeric(val1) && $.isNumeric(val2))
                                return sortOrder == 1 ? val1 - val2 : val2 - val1;
                            else
                                return (val1 < val2) ? -sortOrder : (val1 > val2) ? sortOrder : 0;
                        });
                        $.each(arrData, function (index, row) {
                            $table.find('tbody').not("#tbremotepage").append(row);
                        });
                    });
                });
            }
        },
        ]);

        return Framework;
    }();


