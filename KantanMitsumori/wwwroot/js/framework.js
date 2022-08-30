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
                        token: $("#Token").val(),
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        //result.success = false;
                        //result.responseText = xhr.status + ' ' + thrownError;
                    }
                });
                return result;
            }
        },
        {
            key: "submitAjaxFormDeleteEstNoAndEstSubNoAwait",
            value: function submitAjaxFormDeleteEstNoAndEstSubNoAwait(url) {
                var result = {};
                $.ajax({
                    type: "POST",
                    url: url,
                    data: {
                        token: $("#Token").val(),
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        //result.success = false;
                        //result.responseText = xhr.status + ' ' + thrownError;
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
                        token: $("#Token").val(),
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        //result.success = false;
                        //result.responseText = xhr.status + ' ' + thrownError;
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
                        //token: $("#Token").val(),
                        requestData: data
                    },
                    async: false,
                    success: function success(r) {
                        result = r;
                    },
                    error: function error(xhr, status, thrownError, _error) {
                        //result.success = false;
                        //result.responseText = xhr.status + ' ' + thrownError;
                    }
                });
                return result;
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
            key: "CreateDataOnRow",
            value: function CreateDataOnRow(r, $row, $rowClone, $td, $table, columns) {
                $.each(columns, function (i, item) {
                    switch (item.typeInput) {
                        case "status":
                            if (r.data[item.field] === null || r.data[item.field] === "") {
                                $row.find('.' + item.field).html('');
                            }
                            var find = item.options.filter(function (n) {
                                return n.value === r.data[item.field];
                            });
                            if (find[0].style) {
                                $row.find('.' + item.field).html('<span data-value="' + find[0].value + '" class="label ' + find[0].style + '">' + find[0].text + '</span>');
                            }
                            else {
                                $row.find('.' + item.field).html('<span data-value="' + find[0].value + '">' + find[0].text + '</span>');
                            }
                            break;
                        case "file":
                            $row.find('.' + item.field).html('<a class="link-css" href="' + r.data[item.field] + '" >Download</a>');
                            break;
                        case "DownloadfileAll":
                            $row.find('.' + item.field).html('<a class="link-css btnDownLoadAll"  href="javascript:void(0)" >DownloadFiles</a>');
                            break;
                        case "link":
                            $row.find('.' + item.field).html('<a class="link-css btnViewDetail" href="javascript:void(0)" >' + r.data[item.field] + '</a>');
                            break;
                        case "date":
                            var date = !r.data[item.field] ? "" : moment(r.data[item.field]).format(DATE_FORMAT_MOMENT);
                            $row.find('.' + item.field).text(date);
                            break;
                        case "checkbox":
                            var isCheck = r.data[item.field] ? "checked" : "";
                            $row.find('.' + item.field).html('<input ' + isCheck + ' class="' + item.field + ' hidden"  type="checkbox"><i class="fa "></i>');
                            break;
                        default:
                            $row.find('.' + item.field).text(r.data[item.field]);
                            break;
                    }
                });
                $row.removeClass("hidden");
                $table.columns.adjust();
            }
        },
        {
            key: "getStringInput",
            value: function getStringInput(name, formatCol, defaultValue) {
                var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                var Str = '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                    '<input type="text" ' + value + ' class="form-control" id="' + name + '" name="' + name + '" placeholder="' + name + '..." /></div>';
                return Str;
            }
        }, {
            key: "getStringNumber",
            value: function getStringNumber(name, formatCol, defaultValue) {
                var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                return '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                    '<input data-type="currency" type="text" ' + value + ' class="form-control" id="' + name + '" name="' + name + '" placeholder="' + name + '..." /></div>';
            }
        }, {
            key: "getStringTextarea",
            value: function getStringTextarea(name, formatCol, defaultValue) {
                var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                return '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                    '<textarea class="form-control" ' + value + ' rows="4" id="' + name + '" name="' + name + '" placeholder="' + name + '..."></textarea></div>';
            }
        },
        {
            key: "getStringTextarea",
            value: function getStringTextarea(name, formatCol, text, defaultValue) {
                var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                var txt = text ? text : name;
                return '<div class="form-group ' + formatCol + '"><label class="control-label">' + txt + ':</label>' +
                    '<textarea class="form-control" ' + value + ' rows="4" id="' + name + '" name="' + name + '" placeholder="' + txt + '..."></textarea></div>';
            }
        },
        {
            key: "getStringDatetime",
            value: function getStringDatetime(name, formatCol, defaultValue) {
                var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                return '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                    '<input type="text" ' + value + ' class="form-control flatpickr-input" id="' + name + '" name="' + name + '" placeholder="dd-MMM-yy" /></div>';
            }
        }, {
            key: "getStringLookup",
            value: function getStringLookup(name, formatCol, defaultValue, idLookup) {
                var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                return '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                    '<div class="input-group"><input readonly="readonly" ' + value + ' type="text" class="form-control" id="' + name + '" name="' + name + '" placeholder="' + name + '..." />' +
                    '<span class="input-group-btn"><button id="btn' + name + '" type="button" class="btn btn-default" data-toggle="modal" data-target="#' + idLookup + '">...</button>' +
                    '</span></div></div>';
            }
        },
        {
            key: "getStringLookup",
            value: function getStringLookup(name, formatCol, defaultValue, idLookup, urlLookup) {
                if (urlLookup) {
                    return '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                        '<div id="' + idLookup + '" class="input-group" data-id="' + name + '" data-value="' + defaultValue + '"></div>' +
                        '</div>';
                }
                else {
                    var value = defaultValue ? 'value="' + defaultValue + '"' : '';
                    return '<div class="form-group ' + formatCol + '"><label class="control-label">' + name + ':</label>' +
                        '<div class="input-group"><input readonly="readonly" ' + value + ' type="text" class="form-control" id="' + name + '" name="' + name + '" placeholder="' + name + '..." />' +
                        '<span class="input-group-btn"><button id="btn' + name + '" type="button" class="btn btn-default" data-toggle="modal" data-target="#' + idLookup + '">...</button>' +
                        '</span></div></div>';
                }
            }
        },
        {
            key: "getStringCheckbox",
            value: function getStringCheckbox(name, formatCol, defaultValue) {
                var value = defaultValue ? 'checked' : '';
                return '<div class="form-group ' + formatCol + '"><div class="checkbox"><label class="">' +
                    '<input ' + value + ' class="hidden" id="' + name + '" name="' + name + '" type="checkbox">' +
                    '<i class="fa fa-lg text-belize-hole"></i>' + name + '</label></div></div>';
            }
        },
        {
            key: "getStringCheckbox",
            value: function getStringCheckbox(name, formatCol, text, defaultValue) {
                var value = defaultValue ? 'checked' : '';
                var txt = text ? text : name;
                return '<div class="form-group ' + formatCol + '"><div class="checkbox"><label class="">' +
                    '<input ' + value + ' class="hidden" id="' + name + '" name="' + name + '" type="checkbox">' +
                    '<i class="fa fa-lg text-belize-hole"></i>' + txt + '</label></div></div>';
            }
        },
        {
            key: "getStringSelect",
            value: function getStringSelect(name, formatCol, options, defaultValue) {
                var op = "";
                console.log(options);
                for (var i = 0; i <= options.length - 1; i++) {
                    op += '<option value="' + options[i].value + '">' + options[i].text + '</option>';
                }
                return '<div class="form-group ' + formatCol + '">' + '<label>' + name + ':</label>' +
                    '<select class="form-control"  id="' + name + '" name="' + name + '">' + op + '</select></div>';
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
                if (defaultValue === null || defaultValue === "" || defaultValue === " ") {
                    idOption[0].selected == true;
                }
                else {
                    let length = idOption.length;
                    for (let i = 1; i < length; i++) {
                        let value = idOption[i].value;
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
                //var value = $("#" + nameId + "").val();
                //if (value.includes(defaultValue)) {
                //    $("#" + nameId + "").attr('checked', true);
                //} else {

                //}
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
                window.setTimeout(window.history.back(), 2000);
                location.reload();
                //console.log(idbtn);
                //$("#" + idbtn + "").click(function () {
                //    window.history.back();
                //    location.reload();
                //});

            }
        },

        ]);

        return Framework;
    }();


