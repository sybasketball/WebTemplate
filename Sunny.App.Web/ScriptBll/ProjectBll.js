var ProjectBll = {
    SearchWhere: {
        ProjectState: 0
    },
    Columns: [[
        { field: '0', checkbox: true },
        {
            field: '-1', title: '', width: 130, fixed: true, align:'center',
            formatter: function (value, row, index) {
                var browser = '<a href="javascript:void(0)" onclick="window.open(\'/news.aspx?id=' + row[0]+'\')" id="userName" class="easyui-linkbutton" data-options="iconCls:\'icon-browser\',plain:true">预览</a>';
                var edit = '<a href="javascript:void(0)" onclick="ProjectBll.OpentEdit(' + row[0] +')" id="userName" class="easyui-linkbutton" data-options="iconCls:\'icon-edit\',plain:true">编辑</a>';
                return browser + edit;
            }
        },
        { field: '1', title: '信息名称', width: 60 },
        {
            field: '2', title: '发布时间', width: 60, 
            formatter: function (value, row, index) {
                return BasePage.GetFormatDate(new Date(value), "yyyy-MM-dd hh:mm:ss");
            }
        }
    ]],
    DataGrid: null,
    ReloadGrid: function () {
        if (this.DataGrid != null)
            BasePage.ReloadGrid(this.DataGrid);
    },
    ShowEditDialog: function (commandId,commandCode, commandTitle,data) {
        var dlg = "";
        dlg = BasePage.ShowDialogByUrl({
            title: commandTitle,
            href: "/HtmlPart/ProjectEdit.html",
            width: 400,
            initData: data,
            onSuccess: function () {
                BasePage.SubmitForm({
                    commandTitle: commandTitle,
                    form: $(dlg).find("form"),
                    dlg: dlg,
                    grid: ProjectBll.DataGrid,
                    url: "/Handlers/ProjectHandler.ashx?op=" + commandCode
                });
            }
        });
    },
    OpentEdit:function (id) {
        BasePage.ShowEditPanel({
            isInIframe: true,
            title: "编辑新闻",
            width: 800,
            url: "content_edit.aspx?id=" + id
        });
        //BasePage.ShowPrintWindow("content_edit.aspx?id=" + row[0]);
    },
    Edit: function (commandId) {
        var row = BasePage.GetFirstSelectGridRow(this.DataGrid);
        if (row == null) {
            BasePage.InfoMsg("至少选择一行进行编辑");
            return;
        }
        this.OpentEdit(row[0]);
        //BasePage.ShowPrintWindow("content_edit.aspx?id=" + row[0]);
    },
    Add: function (commandId) {
        BasePage.ShowEditPanel({
            isInIframe: true,
            title: "增加新闻",
            width: 800,
            url: "content_edit.aspx"
        });
        //BasePage.ShowPrintWindow("content_edit.aspx");
    },
    Select: function () {
        var whereStr = encodeURIComponent(JSON2.stringify(this.SearchWhere));
        this.DataGrid = BasePage.InitGrid({
            columns: ProjectBll.Columns,
            sortName: "Id desc",
            url: "/Handlers/ProjectHandler.ashx?op=select",
            onDblClickRow: function (rowIndex, rowData) {
                window.open("/news.aspx?id=" + rowData[0]);
            }
        });
    },
    Load: function (id) {
        BasePage.Ajax({
            type: "GET",
            url: "/Handlers/ProjectHandler.ashx?op=get_one_data&id=" + id,
            isLoading: false,
            isReturnFullData: true,
            success: function (data, message) {
                if (data) {
                    $(document).attr("title", data.Data[1]);
                    $("#title").html(data.Data[1]);
                    $("#publicTime").html(data.Data[2]);
                    $("#contentDiv").html(data.Data[3]);
                }
            }
        });
    },
    GetRowsIds: function (grid, str) {
        if (!str) str = "";
        var rows = BasePage.GetSelectGridRows(grid);
        var ids = "";
        $.each(rows, function (i, row) {
            ids += str + row[0] + str + ",";
        });
        return ids.length > 0 ? ids.substr(0, ids.length - 1) : ids;
    },
    Remove: function (commandId, commandCode, commandTitle) {
        var ids = this.GetRowsIds(this.DataGrid);
        if (ids == "") {
            BasePage.InfoMsg("至少需要选择一行数据");
            return;
        }
        $.messager.confirm("删除确认", "你确定要删除吗？", function (issumbmit) {
            if (issumbmit) {
                BasePage.Ajax({
                    loadingMsg: "正在删除，请稍后。。。",
                    url: "/Handlers/ProjectHandler.ashx?op=remove&ids=" + ids,
                    datagrid: ProjectBll.DataGrid,
                    isClearSelection: true
                });
            }
        });
    }
}