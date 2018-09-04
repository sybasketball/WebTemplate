//参数
var MenuId = jQuery.url.param("menuid");


//页面基类
var BasePage = {
    PageId: 0,
    PageTemplate: "基础类",
    Mask: function () {
        $("<div class='l-window-mask' style='display: block;'></div>").appendTo('body');
    },
    ToMyProject: function () {
        window.location = "project_manager.aspx"
    },
    HideLoading: function () {
        $(".jloading").remove();
        $(".l-window-mask").remove();
    },
    ShowLoading: function (message) {
        message = message || "正在加载中...";
        $('body').append("<div class='jloading'>" + message + "</div>");
        this.Mask();
    },
    //当前表格操作
    InitGrid: function (options) {
        return $(options.GridId || '#tt').datagrid({
            fitColumns: options.fitColumns != undefined ? options.fitColumns : true,
            singleSelect: options.singleSelect != undefined ? options.singleSelect : true,
            width: options.width || 660,
            height: options.height || 250,
            toolbar: options.toolbar,
            border: options.border != undefined ? options.border : true,
            fit: options.fit != undefined ? options.fit : true,
            idField: options.idField || 'Id',
            sortName: options.sortName || 'Id desc',
            rownumbers: options.rownumbers != undefined ? options.rownumbers : true,
            pagination: options.pagination != undefined ? options.pagination : true,
            pageSize: 30,
            url: options.url,
            columns: options.columns,
            frozenColumns: options.frozenColumns,
            checkOnSelect: options.checkOnSelect != undefined ? options.checkOnSelect : true,
            selectOnCheck: options.selectOnCheck != undefined ? options.selectOnCheck : false,
            emptyMsg: '<span class="emptyMsg">暂无数据</span>',
            onLoadError: function () {
                BasePage.SuccessMsg("数据加载失败");
            },
            onLoadSuccess: function (data) {
                $(options.GridId || '#tt').datagrid("unselectAll");
                $(options.GridId || '#tt').datagrid("uncheckAll");
                $(".datagrid-cell-check input[type='checkbox']").bind("click",
                    function () {
                        var rowId = $(this).val();
                        var isChecked = $(this).attr("checked") == "checked";
                        var rowIndex = $(options.GridId || '#tt').datagrid("getRowIndex", rowId)
                        if (rowId)
                            if (isChecked)
                                $(options.GridId || '#tt').datagrid("selectRow", rowIndex);
                            else
                                $(options.GridId || '#tt').datagrid("unselectRow", rowIndex);

                    }
                )
                $(".datagrid-header-check input[type='checkbox']").bind("click",
                    function () {
                        var isChecked = $(this).attr("checked") == "checked";
                        if (!isChecked)
                            $(options.GridId || '#tt').datagrid("unselectAll");

                    }
                )
                $.parser.parse('.datagrid-body');
                if (options.onLoadSuccess) options.onLoadSuccess(data);
            },
            onDblClickRow: function (rowIndex, rowData) {
                if (options.onDblClickRow) options.onDblClickRow(rowIndex, rowData);
            },
            onSelect: function (rowIndex, rowData) {
                //alert("onSelect");
                if (options.onSelect) options.onSelect(rowIndex, rowData);
            },
            onCheck: function (rowIndex, rowData) {
                //alert("onCheck");
                if (options.onCheck) options.onCheck(rowIndex, rowData);
                //else $(this).datagrid("selectRow", rowIndex);
            },
            onClickRow: function (rowIndex, rowData) {
                if(options.onClickRow) options.onClickRow(rowIndex, rowData);
            }
        });
    },
    SubmitForm: function (options)
    {
        var p = options || {};
        var form = $(p.form || '#ff');
        var grid = p.grid || undefined; 
        var dlg = p.dlg || undefined; 
        var commandTitle = p.commandTitle || "保存数据";
        form.form("submit", {
            url: p.url,            
            onSubmit: function () {
                var isValid = $(this).form('validate');
                if (!isValid) {
                    $.messager.progress('close');	// hide progress bar while the form is invalid
                }
                return isValid;	// return false will stop the form submission
            },
            success: function (data) {
                if (options.success) options.success(data);
                else {
                    data = JSON2.parse(data);
                    if (data.IsError) {
                        BasePage.ErrorMsg(data.Message);
                    }
                    else {
                        BasePage.SuccessMsg(commandTitle + "成功！");
                        if (dlg) BasePage.CloseDialog(dlg);
                        if (grid) BasePage.ReloadGrid(grid);
                    }
                }
            }
        });
    },
    GetSelectGridRows: function (grid) {
        return $(grid).datagrid("getChecked");
    },
    GetSelectGridRowsIds: function (grid, str) {
        if (!str) str = "";
        var rows = this.GetSelectGridRows(grid);
        var ids = "";
        $.each(rows, function (i, row) {
            ids += str + row.Id + str + ",";
        });
        return ids.length > 0 ? ids.substr(0, ids.length - 1) : ids;
    },
    GetCheckedTreeGridRowsIds: function (grid, str) {
        if (!str) str = "";
        var rows = $(grid).treegrid("getCheckedNodes");
        var ids = "";
        $.each(rows, function (i, row) {
            ids += str + row.Id + str + ",";
        });
        return ids.length > 0 ? ids.substr(0, ids.length - 1) : ids;
    },
    GetFirstSelectGridRow: function (grid) {
        if ($(grid).length > 0 && $(grid)[0].id == "treeGrid") {
            return $(grid).datagrid("getSelected");
        }
        return $(grid).datagrid("getSelected");
    },
    SelectGridRowsByIds: function (grid, ids) {
        var ids = ids.split(',');
        $.each(ids, function (i, id) {
            $(grid).datagrid("selectRecord", id);
        });
    },
    ReloadGrid: function (grid, isClearSelection) {
        grid = grid || "#tt";
        if ($(grid).length > 0 && $(grid)[0].id == "treeGrid") {
            $(grid).treegrid("reload");
            if (isClearSelection) {
                $(grid).treegrid("clearSelections");
                $(grid).treegrid("clearChecked");
            }
        }
        else {
            $(grid).datagrid("reload");
            if (isClearSelection) {
                $(grid).datagrid("clearSelections");
                $(grid).datagrid("clearChecked");
            }
        }
    },
    //提示
    SuccessMsg: function (message) {
        success(message);
    },
    ErrorMsg: function (message) {
        error(message);
    },
    InfoMsg: function (message) {
        info(message);
    },
    ShowDialogByUrl: function (options) {
        var p = options || {};
        var title = p.title || "对话框";
        var width = p.width || 400;
        var height = p.height || 'auto';
        var href = p.href || null;
        var dlgId = BasePage.NewGuid();
        var dlg = $("<div id='" + dlgId +"'></div>").dialog({
            title: title,
            width: width,
            height: height,
            href: href, 
            top: 20,
            modal: true,
            onClose: function () {
                $("#" + dlgId).remove();
            },
            onLoad: function () {
                if (p.initData) {
                    var form = $(dlg).find("form");
                    if (form && form.length > 0) {
                        setTimeout(function () {
                            form.form("load", p.initData);
                        }, "150");
                    }
                }
            },
            buttons: [{
                text: '保存',
                width: 55,
                iconCls: 'icon-submit_while',
                handler: p.onSuccess || function (dlg) {
                    BasePage.SuccessMsg("保存成功");
                    BasePage.CloseDialog(dlg);
                }
            }, {
                text: '取消',
                width: 55,
                iconCls: 'icon-cancel_while',
                handler: function () {
                    BasePage.CloseDialog(dlg);
                }
            }]
        });
        return dlg;
    },
    ShowDialog: function (title, width, height, content, successCallBack) {
        var dlg = content.dialog({
            title: title,
            width: width,
            height: height,
            top: 20,
            modal: true,
            buttons: [{
                text: '保存',
                width: 55,
                iconCls: 'icon-submit_while',
                handler: successCallBack || function (dlg) {
                    BasePage.SuccessMsg("保存成功");
                    BasePage.CloseDialog(dlg);
                }
            }, {
                text: '取消',
                width: 55,
                iconCls: 'icon-cancel_while',
                handler: function () {
                    BasePage.CloseDialog(dlg);
                }
            }]
        });
        return dlg;
    },
    CloseDialog: function (win) {
        $(win).window("close");
    },
    Ajax: function (options) {
        var p = options || {};
        var isLoading = p.isLoading != undefined ? p.isLoading : true;
        var isAsync = p.async != undefined ? p.async : true;
        var grid = p.datagrid || undefined;
        var isReturnFullData = p.isReturnFullData != undefined ? p.isReturnFullData : false;
        $.ajax({
            cache: false,
            async: isAsync,
            type: 'POST' || p.type,
            url: p.url,
            data: p.data || {},
            dataType: "json" || p.dataType,
            beforeSend: function () {
                if (p.beforeSend)
                    p.beforeSend();
                else
                    if (isLoading) BasePage.ShowLoading(p.loadingMsg || "正在处理你的请求，请稍后。。。");
            },
            complete: function () {
                if (p.complete)
                    p.complete();
                else
                    if (isLoading) BasePage.HideLoading();
            },
            success: function (result) {
                if (!result) return;
                if (!result.IsError) {
                    if (p.success)
                        if (isReturnFullData) {
                            p.success(result);
                        }
                        else {
                            p.success(result.Data || result.rows, result.Message);
                        }
                    else {
                        BasePage.SuccessMsg(result.Message);
                        if (grid != undefined) {
                            BasePage.ReloadGrid(grid, options.isClearSelection);
                        }
                    }
                }
                else {
                    if (p.error)
                        p.error(result.Message);
                    else
                        BasePage.ErrorMsg(result.Message);
                }
            },
            error: function (result, b) {
                BasePage.ErrorMsg('发现系统错误 <BR>错误码：' + result.status);
            }
        });
    },
    TrimStr: function (str) {
        return str.replace(/(^\s*)|(\s*$)/g, "");
    },
    GetFormatDate: function (date, dateformat) {
        if (isNaN(date)) return null;
        var format = dateformat;
        var o = {
            "M+": date.getMonth() + 1,
            "d+": date.getDate(),
            "h+": date.getHours(),
            "m+": date.getMinutes(),
            "s+": date.getSeconds(),
            "q+": Math.floor((date.getMonth() + 3) / 3),
            "S": date.getMilliseconds()
        }
        if (/(y+)/.test(format)) {
            format = format.replace(RegExp.$1, (date.getFullYear() + "")
            .substr(4 - RegExp.$1.length));
        }
        for (var k in o) {
            if (new RegExp("(" + k + ")").test(format)) {
                format = format.replace(RegExp.$1, RegExp.$1.length == 1 ? o[k]
                : ("00" + o[k]).substr(("" + o[k]).length));
            }
        }
        return format;
    },
    FormatJsonData: function (value, dateformat) {
        if (!value) return "";
        // /Date(1328423451489)/
        if (typeof (value) == "string" && /^\/Date/.test(value)) {
            value = value.replace(/^\//, "new ").replace(/\/$/, "");
            eval("value = " + value);
        }
        if (value instanceof Date) {
            var format = dateformat || "yyyy-MM-dd";
            return this.GetFormatDate(value, format);
        }
        else {
            return value.toString();
        }
    },
    ShowPrintWindow: function (url) {
        newwin = window.open("", "", "scrollbars")
        if (document.all) {
            newwin.moveTo(0, 0)
            newwin.resizeTo(screen.availWidth, screen.availHeight)
        }
        newwin.location = url;
    },
    LoginOut: function () {
        this.Ajax({
            url: "../Handlers/ValidatorHandler.ashx?op=loginout",
            success: function () {
                location.href = "login.aspx";
            }
        });
    },
    GetUserState: function (containId) {
        this.Ajax({
            url: "../Handlers/ValidatorHandler.ashx?op=get_current_user_info",
            success: function (data) {
                $(containId).html('<span class="l-btn-left"><span class="l-btn-text">' + data.UserTitle + "</span></span>");
            },
            error: function (msg) {
                $(containId).html('<span class="l-btn-left"><span class="l-btn-text">' + msg + "</span></span>");
            }
        });
    },
    GetCurrentLoginUser: function () {
        var user = null;
        this.Ajax({
            url: "../Handlers/ValidatorHandler.ashx?op=get_current_user_info",
            async: false,
            success: function (data) {
                user = data;
            },
            error: function (msg) {
                location.href = "login.aspx";
            }
        });
        return user;
    },
    ShowEditPanel: function (options) {
        var p = options || {};
        var width = p.width || 500;
        var title = p.title || "侧边栏标题";
        var panel = '<div id="rightPanel" class="rightPanel" style="width:' + width + 'px">'+
                        '<div id="rightPanel2"  class="easyui-panel"  fit="true" style="padding:0px 6px;">' +
                            '<header>' +
                                '<div style="text-align:center">' +
                                    '<span class="my-panel-title icon-edit"> ' + title + '</span>' +
                                '</div>' +                                
                            '</header>' +
                        '</div > ' +
                    '</div > ';
        $("<div id='panelMask' class='l-window-mask2' style='display: block;'></div>").appendTo('body');
        $("#panelMask").on("click",function () {
            $('#rightPanel').remove();
            $(this).remove();
            if (p.onClose) p.onClose;
        }); 
        $('body').append(panel);
        $.parser.parse('#rightPanel');
        if (p.content)
        {
            $('#rightPanel2').panel({
                content: p.content
            });
            if (p.onLoad) p.onLoad();
        }
        else if (p.url) {
            if (p.isInIframe) {
                $('#rightPanel2').panel({
                    content: '<iframe id="content" src="' + p.url + '" name="content" height="100%" width="100%" frameborder="0"></iframe>'
                });
            }
            else {
                $('#rightPanel2').panel({
                    href: p.url,
                    onLoad: p.onLoad
                });
            }
        }
        //$('#rightPanel2').panel("onBeforeLoad", p.onLoad)
    },    
    NewGuid: function () {
        var guid = "";
        for (var i = 1; i <= 32; i++) {
            var n = Math.floor(Math.random() * 16.0).toString(16);
            guid += n;
            if ((i == 8) || (i == 12) || (i == 16) || (i == 20))
                guid += "-";
        }
        return guid + "";
    },
    //yyyy-MM-dd HH:MM:SS
    GetNowFormatDate: function () {
        var date = new Date();
        var seperator1 = "-";
        var seperator2 = ":";
        var month = date.getMonth() + 1;
        var strDate = date.getDate();
        if (month >= 1 && month <= 9) {
            month = "0" + month;
        }
        if (strDate >= 0 && strDate <= 9) {
            strDate = "0" + strDate;
        }
        var currentdate = date.getFullYear() + seperator1 + month + seperator1 + strDate
                + " " + date.getHours() + seperator2 + date.getMinutes()
                + seperator2 + date.getSeconds();
        return currentdate;
    }

};
//命令基类
var Command = {
    CommandType: "",
    Run: function (commandId, commandCode, commandTitle) {
        switch (commandCode) {
            case "add":
                Add(commandId, commandCode, commandTitle);
                break;
            case "edit":
                Edit(commandId, commandCode, commandTitle);
                break;
            case "remove":
                Remove(commandId, commandCode, commandTitle);
                break;
            case "print_bmk":
                Print_bmk(commandId, commandCode, commandTitle);
                break;
            case "print_tjb":
                Print_tjb(commandId, commandCode, commandTitle);
                break;
            case "print_txl":
                Print_txl(commandId, commandCode, commandTitle);
                break;
            case "print_dmc":
                Print_dmc(commandId, commandCode, commandTitle);
                break;
            case "print_xjk":
                Print_xjk(commandId, commandCode, commandTitle);
                break;
            case "edit_power":
                EditPower(commandId, commandCode, commandTitle);
                break;
            case "reset_pwd":
                ResetPassword(commandId, commandCode, commandTitle);
                break;
            case "update_login_info":
                UserBll.UpdateUserLoginInfo(commandId, commandCode, commandTitle);
                break;
            default:
                BasePage.ErrorMsg("“" + commandTitle + "”，没有绑定事件");
                break;
        }
    },
    ResetPassword: function (id) {
        var passForm = '<form id="reset_pwd_form" method="post" style="padding:10px;">' +
                       ' <div class="form_lable">新密码</div>' +
                       ' <div>' +
                       '     <input id="RePassword"  name="Password" class="easyui-validatebox" required="true" type="password" style=" width:98%;"></input>' +
                       ' </div>' +
                       ' <div class="form_lable">重复新密码</div>' +
                       ' <div>' +
                       '     <input id="RePassword1"  name="Password2" class="easyui-validatebox" required="true" validType="equalTo[\'#RePassword\']" type="password" style=" width:98%;"></input>' +
                       ' </div>' +
                       ' <span style="display:none">' +
                       '     <input type="hidden" id="Id_2"  name="Id" value="' + id + '"/>' +
                       '     <input id= "resetBt" type="reset" />' +
                       ' </span>' +
                    ' </form>';
        $("#reset_pwd_form").remove()
        var dlg = BasePage.ShowDialog("密码重置", 360, 220, passForm, function () {
            if ($('#reset_pwd_form').form('validate')) {
                $('#reset_pwd_form').form("submit", {
                    url: "/Handlers/UserHandler.ashx?op=update_pwd",
                    success: function (data) {
                        BasePage.SuccessMsg("重置密码成功！");
                        BasePage.CloseDialog(dlg);
                    }
                });
            }
        });
    },
    EditCurrentUserPassword: function (id) {
        var dlg = "";
        dlg = BasePage.ShowDialogByUrl({
            title: "密码重置",
            href: "/HtmlPart/ResetPassword.html",
            width: 360,
            onSuccess: function () {
                BasePage.SubmitForm({
                    form: $(dlg).find("form"),
                    dlg: dlg,
                    url: "/Handlers/ValidatorHandler.ashx?op=edit_current_user_pwd",
                    success: function (data) {
                        data = JSON2.parse(data);
                        if (!data.IsError) {
                            $.messager.alert('成功提示', '重置密码成功！请使用新密码重新登录系统', 'info', function () {
                                BasePage.CloseDialog(dlg);
                                location.href = "login.aspx";
                            });
                        }
                        else {
                            BasePage.ErrorMsg(data.Message);
                        }
                    }
                });
            }
        });
    }
};
//Combox数据
var ComboxDataInit = {
    InitCombox: function (combox, url, callBack) {
        BasePage.Ajax({
            type: "GET",
            url: url,
            isAsync: false,
            isLoading: false,            
            success: callBack || function (data, message) {
                $(combox).combobox({
                    data: data
                });
            }
        });
    },
    InitRoleCombox: function (combox, where, emptyText, defaultValue) {
        emptyText = emptyText || "-------请选择角色------";
        defaultValue = defaultValue || "";
        $(combox).find("option").remove();
        this.InitCombox(combox, "/Handlers/ValidatorHandler.ashx?op=role_comboxlist&where=" + JSON2.stringify(where),
            function (data, message) {                
                $(combox).append('<option selected="selected" value="">' + emptyText + '</option>');
                $.each(data, function (i, item) {
                    $(combox).append('<option value="' + item.Id + '">' + item.Title + '</option>');
                });
                $(combox).val(defaultValue);
            }
        );
    },
    InitPostCombox: function (combox, where, emptyText, defaultValue) {
        emptyText = emptyText || "-------请选择职位------";
        defaultValue = defaultValue || "";
        $(combox).find("option").remove();
        this.InitCombox(combox, "/Handlers/ValidatorHandler.ashx?op=post_comboxlist&where=" + JSON2.stringify(where),
            function (data, message) {
                $(combox).append('<option selected="selected" value="">' + emptyText + '</option>');
                $.each(data, function (i, item) {
                    $(combox).append('<option value="' + item.Id + '">' + item.PostName + '</option>');
                });
                $(combox).val(defaultValue);
            }
        );
    },
    InitUserCombox: function (combox, where, emptyText, defaultValue) {
        emptyText = emptyText || "-------请选择------";
        defaultValue = defaultValue || "";
        $(combox).find("option").remove();
        this.InitCombox(combox, "/Handlers/ValidatorHandler.ashx?op=user_comboxlist&where=" + JSON2.stringify(where),
            function (data, message) {
                $(combox).append('<option selected="selected" value="">' + emptyText + '</option>');
                $.each(data, function (i, item) {
                    $(combox).append('<option value="' + item.Id + '">' + item.Name + '</option>');
                });
                $(combox).val(defaultValue);
            }
        );
    }
};
//打印窗口
var PrintWindow = {
    print_start: function () {
        print();
    },
    print_view: function () {
        document.all.WebBrowser.ExecWB(7, 1);
    },
    print_setting: function () {
        document.all.WebBrowser.ExecWB(8, 1);
    },
    CloseWindow: function () {
        window.close();
    }
}




