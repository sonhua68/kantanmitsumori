/*
 * realtime validation plugin
 *
 * Copyright (c) 2013 SystemIntegrator
 *
 * Dual licensed under the MIT and GPL licenses:
 *   http://www.opensource.org/licenses/mit-license.php
 *   http://www.gnu.org/licenses/gpl.html
 */
(function($) {

    $.fn.siValidation = function(options) {
        var validateRules = {
            required : true,
            digit : true,
            email : true,
            commodityCode : true
        }, validationSettings = $.extend({}, validateRules, options);

        var init = function() {
            var self = this, $self = $(this);

            $self.bind('focusout', clearMesssage);
            $self.bind('focus', removeMesssage);
            $self.bind('blur', validate);
        }, validate = function(e) {
            var attrClass = $(this).attr("class");
            if (!attrClass) {
                return;
            }
            var classes = attrClass.split(" ");
            for ( var i = 0; i < classes.length; i++) {
                var c = classes[i];
                if (c in validationSettings) {
                    if (validationSettings[c]) {
                        var item = $(this);
                        if (!validator.method[c](item)) {
                            addMessage(item, formatMessage(messageList[c], item.data("item-name")));
                        }
                    }
                }
            }
        }, messageList = {
            required : message.requiredMessage,
            digit : message.invalidDigit,
            email : message.invalidEmail,
            commodityCode : message.invalidCommodityCode

        }, formatMessage = function(fmt) {
            for (i = 1; i < arguments.length; i++) {
                var reg = new RegExp("\\{" + (i - 1) + "\\}", "g")
                fmt = fmt.replace(reg, arguments[i]);
            }
            return fmt;
        }, addMessage = function(element, message) {
            var html = "<div id='" + "m_" + element.attr("id") + "' class='valudatiomMessageArea'>";
            html += "<div class='triangle'></div><br/><div class='validationMessage'>" + message + "</div>"
            html += "</div>";
            var messageArea = $(html);
            element.after(messageArea);

            var left = element.offset().left;
            var messageLeft = messageArea.offset().left;
            messageArea.css("margin-left", left - messageLeft + 20);
            messageArea.click(function() {
                $(element).focus();
            });
        }, removeMesssage = function(element) {
            var id = "m_" + $(this).attr("id");
            $("#" + id).remove();
            $(element).unbind('focus', clearMesssage);
        }, clearMesssage = function(element) {
            var id = "m_" + $(this).attr("id");
            $(".valudatiomMessageArea").not("#" + id).remove();
            $(element).unbind('focusout', clearMesssage);
        };

        var validator = {
            method : {
                checkable : function(element) {
                    return (/radio|checkbox/i).test(element.type);
                },
                getLength : function(value, element) {
                    switch (element[0].nodeName.toUpperCase()) {
                    case "select":
                        return $("option:selected", element).length;
                    case "input":
                        if (this.checkable(element)) {
                            return this.findByName(element.name).filter(":checked").length;
                        }
                    }
                    return value.length;
                },
                required : function(element) {
                    var value = element.val();
                    return this.getLength(value, element) > 0;
                },
                digit : function(element) {
                    var value = element.val();
                    return !this.required(element) || /^-?(?:\d+|\d{1,3}(?:,\d{3})+)?(?:\.\d+)?$/.test(value);
                },
                email : function(element) {
                    var value = element.val();
                    return !this.required(element) || /^([a-zA-Z0-9_\.\-])+\@([a-zA-Z0-9\.\-])+$/.test(value);
                },
                commodityCode : function(element) {
                    var value = element.val();
                    var result = /^[-A-Za-z0-9_\\.\\+]*$/.test(value);
                    if (result){
                        result &= !/^[-_\\.\\+].*$/.test(value);
                        result &= !/^.*[-_\\.\\+]$/.test(value);
                        result &= !/[-_\\.\\+]{2,}/.test(value);
                    }
                    return !this.required(element) || result;
                }
            }
        }

        // construct
        this.each(function() {
            init.apply(this);
        });
        return this;
    };
})(jQuery);