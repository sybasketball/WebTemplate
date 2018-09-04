(function ($) {
    //全局系统对象
    window['menu'] = {};
    menu.columns = [[
        { field: 'MenuTitle', title: '菜单名称', width: 100},
        { field: 'Code', title: '编码', width: 80 },
        { field: 'Type', title: '类型', width: 50, fixed: true },
        { field: 'Url', title: '页面地址', width: 200},
        { field: 'Remark', title: '菜单描述', width: 200 }
   ]];
    menu.selectAll = function () {
        $("#tt").treegrid({
            columns: menu.columns,
            lines: true,
            rownumbers: true,
            animate: true,
            collapsible: true,
            fitColumns: true,
            checkbox: false,
            idField: 'Id',
            treeField: 'MenuTitle',
            cascadeCheck: false,
            fit: true,
            onContextMenu: function (e, row) {
                if (row) {
                    e.preventDefault();
                    $(this).treegrid('select', row.id);
                    $('#mm').menu('show', {
                        left: e.pageX,
                        top: e.pageY
                    });
                }
            },
            pagination: false,
            singleSelect: true,
            sortName: "OrderNumber asc",
            url: "/Handlers/MenuHandler.ashx?op=select_tree_data"
        });
    };
    menu.add = function () {
        menu.ShowEditDialog();
    },
    menu.clickFirst = function (panelId) {
        var bts = $("#" + panelId).find(".easyui-linkbutton");
        if (bts && bts.length > 0)
        {
            bts[0].click();
        }
    },
    //获取
    menu.select = function (pid, callback) {
        //var Where = "&Where={'Pid':" + pid + "}";
        var url = '../Handlers/MenuHandler.ashx?op=get_user_menu&time=' + new Date().getTime();
        $.ajax({
            type: 'GET',
            data: { "sort": "OrderNumber asc", "pid": +pid},
            url: url,
            async: false,
            dataType: "json",
            error: function () {
                error("错误");
            }
        })
        .done(callback);
    };
    menu.MainNavMenu = function () {
        menu.select(0, function (data) {
            $.each(data.rows, function (i, item) {
                if (item.Type == 0) {
                    var meunItem = $('<a href="' + item.Url + '?menuid=' + item.Id + '" target="content" id="menu_' + i + '" class="easyui-linkbutton" data-options="plain:true" iconCls="' + item.LargeIcon + '">' + item.MenuTitle + '</a>');
                    $("#main_bts").append(meunItem);
                }
            });
            $.parser.parse("#main_bts");
            //menu.BindMainNavMenuEvent($("#mainMenuList a"));
        });
    };
    menu.SubMeun = function (parentMenuid) {
        menu.select(parentMenuid, function (data) {
            $.each(data.rows, function (i, item) {
                var pidString = "";
                try {
                    if (pid) pidString = " &pid=" + pid;
                } catch (e) {}
                if (item.Type == 0) {
                    var meunItem = $('<a href="' + item.Url + '?menuid=' + item.Id + pidString + '" target="sub_content" id="menu_' + i + '" class="easyui-linkbutton" data-options="plain:true" iconCls="' + item.iconCls + '">' + item.MenuTitle + '</a>');
                    $("#subMenuList").append(meunItem);
                }
            });
            $.parser.parse("#subMenuList");
            menu.clickFirst("subMenuList");
        });
    };
    menu.BindMainNavMenuEvent = function (bt) {
        $(bt).click(function () {
            var url = $(this).attr("ref");
            var icon = $(this).attr("iconCls").replace("_32","");
            var text = $(this).text();
            var isClose = $(this).attr("isClose");
            if (isClose == "false") isClose = false;
            else isClose = true;
            //isClose = isClose != undefined ? isClose : true;
            
            addTab(text, url, icon, isClose);
        });
    };
    menu.ShowEditDialog = function (commandId, commandCode, commandTitle) {
        var dlg = "";
        dlg = BasePage.ShowDialog(commandTitle, 460, 420, $("#ff"), function () {
            BasePage.SubmitForm({
                commandTitle: commandTitle,
                dlg: dlg,
                grid: Role.DataGrid,
                url: "/Handlers/MenuHandler.ashx?op=" + commandCode + "&menuid=" + commandId
            });
        });
    };
    menu.ToolBarCommand = function (pid) {
        menu.select(pid, function (data) {
            $.each(data.rows, function (i, item) {
                if (item.Type == 1) {
                    var mid = 'menu_' + i;
                    var command = item.Code;
                    var onclick = 'onclick="' + command + '(\'' + item.Id + '\')"';
                    var icon = 'iconCls="' + item.iconCls + '"';
                    if (command == null || command == "") onclick = "";
                    if (item.iconCls == null || item.iconCls == "") icon = "";
                    var meunItem = $('<a href="javascript:void(0)" id="' + mid + '" class="easyui-linkbutton" ' + icon + ' data-options="plain:true" ' + onclick + '>' + item.MenuTitle + '</a>');
                    $("#commandMenu").append(meunItem);
                }
                //menu.ToolBarSubCommand(item.Id, "#" + mid);
            });
            $.parser.parse("#commandMenu");
            //menu.BindMainNavMenuEvent();
        });
    }
    menu.ToolBarSubCommand = function (pid, containMenu) {
        menu.select(pid, function (data) {
            if (data != null && data.rows != null && data.rows.length > 0) {
                var subMeunId = 'sub_menu_' + pid;
                var subMeun = '<div id="sub_menu_' + pid + '" style="width:100px;"></div>';
                $("#subMenu").append(subMeun);
                $.each(data.rows, function (i, item) {
                    var meunItem = $('<div id="menu_' + i + '" onclick="Command.Run(\'' + item.Id + '\',\'' + item.FnCode + '\',\'' + item.MenuTitle + '\')" iconCls="' + item.MenuIcon + '">' + item.MenuTitle + '</div>');
                    $("#" + subMeunId).append(meunItem);
                });
                $(containMenu).attr("menu", "#" + subMeunId);
                $(containMenu).attr("class", "easyui-menubutton");
            }
        });
    }
    
})(jQuery);