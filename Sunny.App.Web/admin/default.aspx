<%@ Page Language="C#" AutoEventWireup="true" Inherits="CommonUtility.UI.AdminPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>内容管理系统</title>
    <!--#include file="/Include/base_js.aspx"-->
    <script src="/Resource/easyui/plugins/jquery.validatebox.ext.js" type="text/javascript"></script>
    <script src="/ScriptBll/PublicBll.js" type="text/javascript"></script>

    <script type="text/javascript">
        //var menu_id = jQuery.url.param("menuid");
        $(function () {
            //menu.MainNavMenu();
            //menu.clickFirst("main_bts");
            //menu.SubMeun(menu_id);
            BasePage.GetUserState("#userName");
        });
        function DoSizePanel() {
            var h = $("#bodylay").height();
            $('#p').panel('resize', {
                width: 'auto',
                height: h
            });
        }
        $.extend($.fn.validatebox.defaults.rules, {
            confirmPass: {
                validator: function (value, param) {
                    var pass = $(param[0]).passwordbox('getValue');
                    return value == pass;
                },
                message: '重复密码不一致.'
            }
        })
    </script>
    <style type="text/css">
        #main_bts .l-btn-text {
            font-size:15px;
            color:#fff;
        }
    </style>
</head>
<body class="easyui-layout" style="margin:0px 0px;background: #fff;">    
   <div data-options="region:'north',split:false" id="top" style="height:45px;overflow: hidden;background:none; border:none; padding:0px;background: url('/Resource/images/top_bg.png') repeat-x bottom;">
        <div style="float:left; height:42px; width:150px; background-color:#666;color:white; line-height:42px;font-size:20px; padding-left:30px;">  
            <span>内容管理系统</span>
        </div>     
        <div id="main_bts" style="float:left; height:40px;padding-top:8px; padding-left:10px;">  
        </div>
            
        <div style="float:right; height:40px;padding-top:8px; padding-right:10px;">  
            <a href="javascript:void(0)" id="userName" class="easyui-linkbutton" data-options="iconCls:'icon-user_while',plain:true">用户名</a>
            <a href="javascript:void(0)" id="A2" class="easyui-linkbutton" data-options="iconCls:'icon-reset_while',plain:true" onclick="Command.EditCurrentUserPassword()">重置密码</a>            
		    <a href="javascript:void(0)" id="A3" class="easyui-linkbutton" data-options="iconCls:'icon-loginout_while',plain:true" onclick="BasePage.LoginOut()">退出系统</a>
        </div>
    </div>    
    <div id="bodylay" data-options="region:'center'" style=" background: #eee; padding:5px 10px;" border="false">
        <iframe id="content" src="project_manager.aspx" name="content" height="100%" width="100%" frameborder="0"></iframe>
    </div>
        
</body>
</html>
