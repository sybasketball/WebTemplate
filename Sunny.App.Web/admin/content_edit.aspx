<%@ Page Language="C#" AutoEventWireup="true" ValidateRequest="false" Inherits="CommonUtility.UI.AdminPage" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>    
    <title></title>
    <!--#include file="/Include/base_js.aspx"-->    
    <script src="/ScriptBll/PublicBll.js" type="text/javascript"></script>    
    <script src="/Resource/Jquery.plus/syValidator/syValidator.js" type="text/javascript"></script>
    <link href="/Resource/Jquery.plus/syValidator/Tooltip.css" rel="stylesheet" type="text/css" />
    <link href="/Resource/kindeditor-4.1.10/themes/simple/simple.css" rel="stylesheet" />
    <script src="/Resource/kindeditor-4.1.10/kindeditor-all.js" type="text/javascript"></script>
    <script src="/Resource/kindeditor-4.1.10/lang/zh_CN.js" type="text/javascript"></script>
    <style type="text/css">
        .ke-icon-innerlink {
				background-image: url(/Resource/kindeditor-4.1.10/themes/default/default.png);
				background-position: 0px -1248px;
				width: 16px;
				height: 16px;
			}
        .ke-icon-example1 {
				background-image: url(/Resource/kindeditor-4.1.10/themes/default/default.png);
				background-position: 0px -672px;
				width: 16px;
				height: 16px;
			}
        .ke-icon-insert3dFile {
				background-image: url(/Resource/kindeditor-4.1.10/themes/default/default.png);
				background-position: 0px -1266px;
				width: 16px;
				height: 16px;
			}
    </style>
    <script type="text/javascript">
        var editor;
        var id = jQuery.url.param("id");
        var classId = 1;
        var contentDlg;
        var editItem = [
		'source', '|', 'undo', 'redo', '|', 'preview', 'print', 'template', 'code', 'cut', 'copy', 'paste',
		'plainpaste', 'wordpaste', '|', 'justifyleft', 'justifycenter', 'justifyright',
		'justifyfull', 'insertorderedlist', 'insertunorderedlist', 'indent', 'outdent', 'subscript',
		'superscript', 'clearhtml', 'quickformat', 'selectall',  '/',
		'formatblock', 'fontname', 'fontsize', '|', 'forecolor', 'hilitecolor', 'bold',
		'italic', 'underline', 'strikethrough', 'lineheight', 'removeformat', '|', 'image', 'multiimage',
		'flash', 'media', 'insertfile', 'table', 'hr', 'emoticons', 'baidumap', 'pagebreak',
		'anchor', 'link', 'unlink', 'innerlink', 'insert3dFile'
        ];
        $(function () {
            editor = KindEditor.create('textarea[name="Content"]', {
                uploadJson: '/Handlers/upload/upload_json.ashx?projectid=' + classId,
                fileManagerJson: '/Handlers/upload/file_manager_json.ashx?projectid=' + classId,
                allowFileManager: true,
                resizeMode: 1,
                items:editItem,
                afterBlur: function () { this.sync(); }
            });
            if (id) {
                initForm(id);
            }
            else {
                $("#ClassId").val(classId);
            }
            $.syValidator.initValidatorForm({ formId: "ff" });
        });
        function cancel() {
            //if (parent) {
            //    if (id) {
            //        parent.closeNewContentTab("修改内容");
            //    }
            //    else {
            //        parent.closeNewContentTab("新增内容");
            //    }
            //}
            //window.close();
        }
        function RemoveHtmlLabel(str) {
            str = str.replace(/(\n)/g, "");
            str = str.replace(/(\t)/g, "");
            str = str.replace(/(\r)/g, "");
            str = str.replace(/<\/?[^>]*>/g, "");
            str = str.replace(/\s*/g, "");
            return str;
        }
        function save() {
            if (parent) {
                if ($.syValidator.formIsValidated("ff")) {                    
                    $('#ff').form("submit", {
                        url: "/Handlers/ProjectHandler.ashx?op=add",
                        
                        success: function (data) {
                            data = JSON2.parse(data);
                            if (data.IsError) {
                                BasePage.ErrorMsg(data.Message);
                            }
                            else {
                                var pw = parent;
                                if (pw) {
                                    pw.Reload();
                                    pw.BasePage.SuccessMsg("保存成功！");
                                    window.close();
                                }
                            }
                        }
                    });
                }
            }
        }
        function initForm(id)
        {
            BasePage.Ajax({
                type: "GET",
                url: "/Handlers/projectHandler.ashx?op=get_one_data&id=" + id,
                isLoading: false,
                isReturnFullData: true,
                success: function (data, message) {
                    if (data) {
                        var cdata = data.Data;
                        $('#Id').val(cdata[0]);
                        $('#Title').val(cdata[1]);
                        $('#Content').val(cdata[3]);
                        editor.html(cdata[3]);
                        //$('#KeyWord').tagsInput({height:25,width:'97%'});
                    }
                }
            });
        }
        function SelectContent() {
            //ContentBll.LoadContent();
        }
    </script>
</head>
<body class="easyui-layout">
    <div data-options="region:'center',border:false" style="overflow:auto;">
        <form id="ff" method="post" style="padding: 10px;">
                                <table class="form_table" style="width:100%;">
                                    <tbody>
                                        <tr>
                                            <th>标题:</th>
                                            <td>
                                                <input id="Title" name="Title" type="text" style="width: 98%;" enum="notempty" isvalidator="true"></input>
                                                <span id="TitleTip"></span>
                                            </td>
                                        </tr>
                                        <tr>
                                            <th>摘要:</th>
                                            <td>
                                                <textarea id="Summary" name="Summary" rows="3" style="width: 98%;" ></textarea>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                <textarea id="Content" name="Content"  style="width:98%;height:400px;visibility:hidden;" ></textarea>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                                <span style="display: none">
                                    <input type="text" id="Id" name="Id" />
                                    <input type="text" id="ClassId" name="ClassId" value="1" />
                                    <input id="reset" type="reset" />
                                </span>
                            </form>
    </div>
    <div data-options="region:'south',split:true,height:45"style="overflow: hidden;background:none; border:none; padding:0px;background:#F5F5F5; border-top:1px solid #DEDEDE; padding:6px;">
            <div style="float:right; margin-right:20px;">  
                <a id="save" href="javascript:void(0)" onclick="save()"   id="A2" class="easyui-linkbutton" data-options="width:80">保存</a>    
            </div>
    </div>
</body>
</html>
