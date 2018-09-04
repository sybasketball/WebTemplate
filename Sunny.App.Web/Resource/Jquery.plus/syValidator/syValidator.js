//***********************************************************
//根据原有验证框架进行改进
//有问题希望跟我交流下,谢谢支持 QQ287377132
//***********************************************************
//获得所有需要验证的标签

(function ($) {
    $.syValidator = {
        initValidatorForm: function (config) {
            //创建提示框
            $('body').append('<table id="tipTable" class="tableTip"><tr><td  class="leftImage"></td> <td class="contenImage" align="left"></td> <td class="rightImage"></td></tr></table>');
            //移动鼠标隐藏刚创建的提示框
            var isTip = config.isTip != null ? config.isTip : true;
            $(document).mouseout(function () { $('#tipTable').hide() });
            if (config.formId != null) {
                //alert(config.formId);
                var form = $("#" + config.formId);

                var items = form.find("[isValidator='true']");
                if (isTip) {
                    items.hover(function () {
                        if ($.syValidator.getValidatorState(this) != "ok") {
                            $.syValidator.showToolTip(this);
                        }
                    },
                    function () {
                        $.syValidator.hideToolTip();
                    })
                };
                items.bind("blur", function (event) {
                    $.syValidator.showFormItmeState(this);
                });
            }
        },
        //form是否通过验证
        formIsValidated: function (formId) {
            var form = $("#" + formId);
            var items = form.find("[isValidator='true']");
            var result = true;
            items.each(function () {
                result = $.syValidator.showFormItmeState(this) && result;
            });
            if (result) this.resetFormValidator(formId);
            return result;
        },
        showFormItmeState: function (item) {
            if ($.syValidator.isValidated(item)) {
                $(item).removeClass('tooltipinputerr').addClass('tooltipinputok');
                return true;
            }
            else {
                $(item).removeClass('tooltipinputok').addClass('tooltipinputerr');
                return false;
            }
        },
        TrimStr: function (str) {
            return str.replace(/(^\s*)|(\s*$)/g, "");
        },
        //重置验证状态
        resetFormValidator: function (formId) {
            $("#" + formId).find("[isValidator='true']").removeClass("tooltipinputerr tooltipinputok").removeAttr("ctip");
        },
        //获取字符串长度（汉字算两个字符，字母数字算一个）
        getByteLen:function (val) {
              var len = 0;
            for (var i = 0; i < val.length; i++) {
                var a = val.charAt(i);
                if (a.match(/[^\x00-\xff]/ig) != null) {
                    len += 2;
                }
                else {
                    len += 1;
                }
            }
            return len;
        },
        //是否通过验证，item为验证项
        isValidated: function (item) {
            var reg_enum = $(item).attr("enum") || "";
            var max_length = parseInt($(item).attr("max_length")) || 9999;
            var min_length = parseInt($(item).attr("min_length")) || -1;
            var reg_enums = reg_enum.split("|");
            var regs = this.regexEnum;
            var val = this.TrimStr($(item).val());
            var val_length = this.getByteLen(val);
            if (val_length > max_length) {
                $(item).attr("ctip", "你输入的字符过长");
                return false;
            }
            if (val_length < min_length) {
                $(item).attr("ctip", "你输入的字符过短");
                return false;
            }
            var result = false;
            for (var i = 0; i < reg_enums.length; i++) {
                var reg = regs[reg_enums[i]];
                if (reg) {
                    var thisReg = new RegExp(reg.reg);
                    if (thisReg.test(val)) {
                        result = true;
                    }
                    else {
                        $(item).attr("ctip", reg.message);
                        return false;
                    }
                }
                return true;
            }
            return result;
        },
        getValidatorState: function (item) {
            if ($(item).hasClass("tooltipinputok")) return "ok";
            else if ($(item).hasClass("tooltipinputerr")) return "error";
            else return "none";
        },
        showToolTip: function (obj) {
            var tip_txt = $(obj).attr('ctip') || $(obj).attr('tip');
            if (tip_txt == null || tip_txt == "") return;
            var offset = $(obj).offset();
            $('#tipTable').css({ left: offset.left + 'px', top: offset.top + $(obj).outerHeight(true) + 'px' });
            $('.contenImage').html(tip_txt);
            $('#tipTable').fadeIn("fast");
            
//            $(obj).tooltip({
//                position: 'bottom',
//                content: '<span style="color:#fff">' + tip_txt + '</span>',
//                onShow: function () {
//                    $(this).tooltip('tip').css({ backgroundColor: '#666', borderColor: '#666' });
//                }
//            });
        },
        hideToolTip: function () {
            $('#tipTable').hide();
        },
        getWidth: function (object) {
            return object.offsetWidth;
        },
        getToolTipLeft: function (object) {
            var offset = $(object).offset();
            return offset.left;
        },
        getToolTipTop: function (object) {
            var offset = $(object).offset();
            return  offset.top + $(object).outerHeight(true);
        },
        regexEnum: {
            intege: {
                reg: "^-?[1-9]\\d*$",
                message: "请输入整数"
            },
            intege1: {
                reg: "^[1-9]\\d*$",
                message: "请输入正整数"
            },
            intege2: {
                reg: "^-[1-9]\\d*$",
                message: "请输入负整数"
            },
            num: {
                reg: "^([+-]?)\\d*\\.?\\d+$",
                message: "请输入数字"
            },
            num1: {
                reg: "^[1-9]\\d*|0$",
                message: "请输入正数（正整数 + 0）"
            },
            num2: {
                reg: "^-[1-9]\\d*|0$",
                message: "请输入负数（负整数 + 0）"
            },
            decmal: {
                reg: "^([+-]?)\\d*\\.\\d+$",
                message: "请输入浮点数"
            },
            decmal1: {
                reg: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*$",
                message: "请输入正浮点数"
            },
            decmal2: {
                reg: "^-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*)$",
                message: "请输入负浮点数"
            },
            decmal3: {
                reg: "^-?([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0)$",
                message: "请输入浮点数"
            },
            decmal4: {
                reg: "^[1-9]\\d*.\\d*|0.\\d*[1-9]\\d*|0?.0+|0$",
                message: "请输入非负浮点数（正浮点数 + 0）"
            },
            decmal5: {
                reg: "^(-([1-9]\\d*.\\d*|0.\\d*[1-9]\\d*))|0?.0+|0$",
                message: "请输入非正浮点数（负浮点数 + 0）"
            },
            email: {
                reg: "^\\w+((-\\w+)|(\\.\\w+))*\\@[A-Za-z0-9]+((\\.|-)[A-Za-z0-9]+)*\\.[A-Za-z0-9]+$",
                message: "请输入正确的邮件地址"
            },
            color: {
                reg: "^[a-fA-F0-9]{6}$",
                message: "请输入正确的颜色代码"
            },
            url: {
                reg: "^http[s]?:\\/\\/([\\w-]+\\.)+[\\w-]+([\\w-./?%&=]*)?$",
                message: "请输入正确的url地址"
            },
            chinese: {
                reg: "^[\\u4E00-\\u9FA5\\uF900-\\uFA2D]+$",
                message: "请输入中文字符"
            },
            ascii: {
                reg: "^[\\x00-\\xFF]+$",
                message: "请输入ACSII字符"
            },
            zipcode: {
                reg: "^\\d{6}$",
                message: "请输入正确的邮编"
            },
            mobile: {
                reg: "^13[0-9]{9}|15[012356789][0-9]{8}|18[0256789][0-9]{8}|147[0-9]{8}$",
                message: "请输入正确的手机号码"
            },
            ip4: {
                reg: "^(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)\\.(25[0-5]|2[0-4]\\d|[0-1]\\d{2}|[1-9]?\\d)$",
                message: "请输入正确的Ipv4地址"
            },
            notempty: {
                reg: "^.+$",
                message: "不能为空"
            },
            picture: {
                reg: "(.*)\\.(jpg|bmp|gif|ico|pcx|jpeg|tif|png|raw|tga)$",
                message: "请输入正确的图片路径"
            },
            rar: {
                reg: "(.*)\\.(rar|zip|7zip|tgz)$",
                message: "请输入非正浮点数（负浮点数 + 0）"
            },
            date: {
                reg: "^\\d{4}(\\-|\\/|\.)\\d{1,2}\\1\\d{1,2}$",
                message: "请输入非正浮点数（负浮点数 + 0）"
            },
            qq: {
                reg: "^[1-9]*[1-9][0-9]*$",
                message: "请输入非正浮点数（负浮点数 + 0）"
            },
            tel: {
                reg: "^(([0\\+]\\d{2,3}-)?(0\\d{2,3})-)?(\\d{7,8})(-(\\d{3,}))?$",
                message: "请输入正确的电话号码(包括验证国内区号,国际区号,分机号)"
            },
            username: {
                reg: "^\\w+$",
                message: "请输入正确的用户名称。由数字、26个英文字母或者下划线组成的字符串"
            },
            letter: {
                reg: "^[A-Za-z]+$",
                message: "请输入字母"
            },
            letter_u: {
                reg: "^[A-Z]+$",
                message: "请输入大写字母"
            },
            letter_l: {
                reg: "^[a-z]+$",
                message: "请输入小写字母"
            },
            idcard: {
                reg: "^[1-9]([0-9]{14}|[0-9]{17})$",
                message: "请输入身份证"
            }
        }
    };
})(jQuery);
//***************************************************************************************************************************************************