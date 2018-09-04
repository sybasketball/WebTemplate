var UserBll = {
    SearchWhere: {
        UserType: "Normal"
    },
    Columns: [[
            { field: 'Id', checkbox: true},
			{ field: 'UserName', title: '用户名', width: 60 },
			{ field: 'Name', title: '昵称', width: 60 },
			{ field: 'RoleId', hidden: true },
            { field: 'RoleTitle', title: '角色', width: 60 },
			{ field: 'Remark', title: '备注', width: 200 }
    ]],
    DataGrid: null,
    ReloadGrid: function () {
        if (this.DataGrid != null)
            BasePage.ReloadGrid(this.DataGrid);
    },
    ShowEditDialog: function (commandId, commandCode, commandTitle,data) {
        var dlg = "";
        dlg = BasePage.ShowDialogByUrl({
            title: commandTitle,
            href: "/HtmlPart/UserEdit.html",
            width: 400,
            initData: data,
            onSuccess: function () {   
                BasePage.SubmitForm({
                    commandTitle: commandTitle,
                    form: $(dlg).find("form"),
                    dlg: dlg,
                    grid: UserBll.DataGrid,
                    url: "/Handlers/UserHandler.ashx?op=" + commandCode + "&menuid=" + commandId
                });
            }
        });
    },
    Edit: function (commandId) {
        var row = BasePage.GetFirstSelectGridRow(this.DataGrid);
        if (row == null) {
            BasePage.InfoMsg("至少选择一行进行编辑");
            return;
        }
        this.ShowEditDialog(commandId, "edit", "编辑人员信息", row);
    },
    Add: function (commandId) {
        this.ShowEditDialog(commandId,"add", "添加人员");
    },
    Select: function () {
        var whereStr = encodeURIComponent(JSON2.stringify(this.SearchWhere));
        this.DataGrid = BasePage.InitGrid({
            columns: UserBll.Columns,
            sortName: "OrganizationId desc",
            //checkOnSelect: true,
            //selectOnCheck: true,
            //singleSelect:false,
            url: "/Handlers/UserHandler.ashx?op=select&where=" + whereStr
            //onCheck: function (index,row) {
            //    $('#tt').datagrid("clearSelections");
            //    $('#tt').datagrid("highlightRow", index);
            //}
        });
    },
    Remove: function (commandId, commandCode, commandTitle) {
        var ids = BasePage.GetSelectGridRowsIds(this.DataGrid);
        if (ids == "") {
            BasePage.InfoMsg("至少需要选择一行数据");
            return;
        }
        $.messager.confirm("删除确认", "你确定要删除吗？", function (issumbmit) {
            if (issumbmit) {
                BasePage.Ajax({
                    loadingMsg: "正在删除，请稍后。。。",
                    url: "/Handlers/UserHandler.ashx?op=remove&ids=" + ids,
                    datagrid: UserBll.DataGrid,
                    isClearSelection: true
                });
            }
        });
    },
    ResetPassword: function (commandId, commandCode, commandTitle) {
        var row = BasePage.GetFirstSelectGridRow(this.DataGrid);
        if (row == null) {
            BasePage.InfoMsg("至少选择一行进行编辑");
            return;
        }
        Command.ResetPassword(row.Id);
    },
    ShowUserInfoEditDialog: function (commandTitle, commandCode) {
        var dlg = "";
        dlg = BasePage.ShowDialog(commandTitle, 460, 280, $("#userInfoForm"), function () {
            if ($.syValidator.formIsValidated("userInfoForm")) {
                $('#userInfoForm').form("submit", {
                    url: "/Handlers/UserHandler.ashx?op=" + commandCode,
                    success: function (data) {
                        data = JSON2.parse(data);
                        if (data.IsError) {
                            BasePage.ErrorMsg(data.Message);
                        }
                        else {
                            BasePage.SuccessMsg(commandTitle + "成功！");
                            BasePage.CloseDialog(dlg);
                            UserBll.ReloadGrid();
                        }
                    }
                });
            }
        });
    },
    InitUserInfo: function (userId, trueName) {
        $("#userInfo_reset").click();
        $("#UserName").removeAttr("disabled");
        $('#password_tip').show();
        $.syValidator.resetFormValidator("userInfoForm");
        $("#UserId").val(userId);
        $("#UserName").val(trueName);
        this.ShowUserInfoEditDialog("初始化用户登录信息", "update_login_info");
    },
    UpdateUserLoginInfo: function (commandId, commandCode, commandTitle) {
        var row = BasePage.GetFirstSelectGridRow(this.DataGrid);
        if (row == null) {
            BasePage.InfoMsg("至少选择一行进行编辑");
            return;
        }
        if (row.UserName == null || row.UserName == "")
            this.InitUserInfo(row.Id, row.Name);
        else {
            $('#password_tip').hide();
            $("#UserName").attr("disabled", "disabled");
            $('#userInfoForm').form("load", row);
            this.ShowUserInfoEditDialog("更新用户登录信息", "update_login_info");
        }
    },
    GotoDepartment: function (orgid) {
        UserBll.SearchWhere.OrganizationId = orgid;
        UserBll.Select();
    },
    Search: function () {
        SelectComboTree.LoadTreeData($("#s_orgNameTree"), $("#s_orgNamePanel"), $("#s_orgName"), $("#s_orgId"), "/Handlers/OrganizationHandler.ashx?op=select", "Id", "Title", "Pid");
        var dlg = BasePage.ShowDialog("检索", 460, 260, $("#searchWin"), function () {
            var s_orgId = $("#s_orgId").val();
            var s_orgName = $("#s_orgName").val();
            var s_userName = $("#s_userName").val();
            var s_Name = $("#s_Name").val();
            if (s_orgId && s_orgName) UserBll.SearchWhere.OrganizationId = s_orgId; else UserBll.SearchWhere.OrganizationId = null;
            if (s_userName) UserBll.SearchWhere.UserName = s_userName; else UserBll.SearchWhere.UserName = null;
            if (s_Name) UserBll.SearchWhere.Name = s_Name; else UserBll.SearchWhere.Name = null;
            UserBll.Select();
            BasePage.CloseDialog(dlg);
        });
    }
}