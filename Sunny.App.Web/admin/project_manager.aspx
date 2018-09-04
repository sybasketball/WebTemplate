<%@ Page Language="C#" AutoEventWireup="true" Inherits="CommonUtility.UI.AdminPage" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>内容管理</title>
    <!--#include file="/Include/base_js.aspx"-->
    <script src="/ScriptBll/PublicBll.js" type="text/javascript"></script>
    <script src="/Resource/Jquery.plus/syValidator/syValidator.js" type="text/javascript"></script>
    <link href="/Resource/Jquery.plus/syValidator/Tooltip.css" rel="stylesheet" type="text/css" />
    <script src="/ScriptBll/SelectComboTree.js" type="text/javascript"></script>
    <script src="/Resource/kindeditor-4.1.10/kindeditor-all-min.js" type="text/javascript"></script>
    <script src="/ScriptBll/ProjectBll.js" type="text/javascript"></script>
    <script type="text/javascript">
        //var menu_id = jQuery.url.param("menuid");
        $(function () {
            ProjectBll.Select();
        });
        function Reload() {
            BasePage.ReloadGrid();
        }
    </script>
    <style type="text/css">
        .panel-header{ border-bottom:none;}
        .l-btn-plain-selected{ background:#ff6a00;color:#fff}
        .l-btn-plain-selected:hover{ background:#ff6a00;color:#fff;border-color:#ff6a00}
        .datagrid-view {min-height: 98px;}
    </style>
</head>
<body class="easyui-layout" data-options="fit:true" style="overflow: hidden;">    
    <div data-options="region:'center'"  style="border:0px;"  style="overflow:hidden">
        <header>
            <table border="0" cellpadding="0" cellspacing="0" style="border:0px;" width="100%;">
                <tr>
                    <td valign="middle"> 
                        <span class="my-panel-title icon-list">内容列表</span>
                    </td>
                    <td align="right"> 
                        <a href="#" class="easyui-linkbutton" iconcls="icon-add" plain="true" onclick="ProjectBll.Add()">添加内容</a>
                        <a href="#" class="easyui-linkbutton" iconcls="icon-edit" plain="true" onclick="ProjectBll.Edit()">编辑内容</a>
                        <a href="#" class="easyui-linkbutton" iconcls="icon-remove" plain="true" onclick="ProjectBll.Remove()">删除</a>
                        <a href="#" class="easyui-linkbutton" iconcls="icon-reload" plain="true" onclick="BasePage.ReloadGrid()">刷新</a>
                    </td>
                </tr>
            </table>
        </header>
        <table id="tt" data-options="fit:true,striped:true"></table>
    </div>
</body>
</html>
