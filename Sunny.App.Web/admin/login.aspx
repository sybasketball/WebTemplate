<%@ Page Language="C#" AutoEventWireup="true" Inherits="CommonUtility.UI.NormalPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>用户登录</title>
    <!--#include file="/Include/base_js.aspx"-->
    <script src="/ScriptBll/PublicBll.js" type="text/javascript"></script>  
    <script src="/Resource/Jquery.plus/syValidator/syValidator.js" type="text/javascript"></script>
    <link href="/Resource/Jquery.plus/syValidator/Tooltip.css" rel="stylesheet" type="text/css" />
    <link href="/Resource/easyui/themes/color.css" rel="stylesheet" />
    <script type="text/javascript">
        //var FromUrl = jQuery.url.param("FromUrl");
        var FromUrl = "";
        $(function () {
            if (top != self) top.location.href = location.href;
            $.syValidator.initValidatorForm({ formId: "form1" });
        });
        function submitBtn() {
            $('#form1').form("submit", {
                url: "/Handlers/ValidatorHandler.ashx?op=login",
                success: function (data) {
                    data = JSON2.parse(data);
                    if (!data.IsError) {
                        BasePage.SuccessMsg("登陆成功！");
                        FromUrl = encodeURIComponent("default.aspx");
                        //if (!FromUrl) {
                            //if ($("#role").val() == 1) {
                            //    FromUrl = encodeURIComponent("default.aspx");
                            //}
                            //else FromUrl = encodeURIComponent("project_manager.aspx");
                        //}
                        location.href = decodeURIComponent(FromUrl);
                    }
                    else BasePage.ErrorMsg(data.Message);
                }
            });
        }
        function KeyDown(event) {
            if (event.keyCode == 13) {
                submitBtn();
            }            
        }

    </script>
    <style type="text/css">
        .login_form{ margin:30px 50px;}
        .login_form .textbox-addon{ display:block; font-size:15px; text-indent:10px; margin-top:12px;}
        .footer{ line-height:50px;font-size:13px; text-align:center; }
    </style>
</head>
<body class="easyui-layout">
	<div data-options="region:'north',split:false" style="height:130px;overflow: hidden; background:url(/Resource/images/login_top_bg.png); border:none; padding:0px;">
        <div style=" text-align:center; width:1000px; margin:5px auto;">
            
        </div>
    </div>    
    <div data-options="region:'center'" style="overflow: hidden;background:#ededed;">
        <div style="width:1000px; margin:0 auto;height:620px;">
            <center>
            <div style="height:430px; width:430px; margin-top:40px; background:#fff;">
                <div style="height:50px; font-family:微软雅黑; font-size:25px; color:#666; line-height:50px; padding-top:20px;">
                    <strong>用户登录</strong>
                </div>
                <form class="login_form" id="form1" method="post" onkeypress="KeyDown(event)">

                    <div style="margin-bottom:10px">
                        <input id="UserName"  name="UserName" class="easyui-textbox" style="width:100%;height:40px;padding:12px" data-options="prompt:'用户名',iconCls:'icon-user',iconAlign:'left',iconWidth:20"/>
                    </div>
                    <div style="margin-bottom:20px">
                        <input id="Password"  name="Password" class="easyui-passwordbox" type="password" style="width:100%;height:40px;padding:12px" data-options="prompt:'密码    ',showEye:false,revealed:true,iconCls:'icon-reset',iconAlign:'left',iconWidth:20"/>
                    </div>
                    <p>
                        <a href="javascript:void(0)" class="easyui-linkbutton c1" onclick="submitBtn()" style="width:120px; height:30px; font-size:22px;">登陆</a>
                        <a href="javascript:void(0)" class="easyui-linkbutton c2" onclick="submitBtn()" style="width:120px; height:30px; font-size:22px;">重置</a>
                    </p>
                    <p>提示：如果忘记您的登录密码请与管理员联系！</p>
                    <input style="display:none" type="reset" id="reset1"/>
                </form>
            </div>  
            </center>
        </div>
    </div>
    <div data-options="region:'south',split:false,border:false,height:50" class="footer">
        版权所有：长庆油田第四采油厂
    </div>
</body>
</html>
