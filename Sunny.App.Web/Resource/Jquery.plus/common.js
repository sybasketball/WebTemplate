function addData(title,contentID,width,height,grid){
    var dlg;
    dlg = BasePage.ShowDialog(title,width,height,$(contentID),function () {
        $(contentID).form('submit',{
            onSubmit:function () {
                var formData = $(contentID).serializeArray();

                $(grid).datagrid("appendRow",strToObj(formData));
            }
        })
        BasePage.SuccessMsg("保存成功");
        BasePage.CloseDialog(dlg);
    });
    return dlg;
}
function strToObj(formData){
    var serializeObj={};
    $(formData).each(function(){
        serializeObj[this.name]=this.value;
    });
    return serializeObj;
}
function delData(grid) {
    var rows = BasePage.GetSelectGridRows(grid);
    if (rows.length < 1 ) {
        BasePage.InfoMsg("至少需要选择一行数据");
        return;
    }
    $.messager.confirm("删除确认", "你确定要删除吗？", function (issumbmit) {
        if (issumbmit) {
            len = rows.length;
            for(i=len-1;i>=0;i--)
            {
                indx = $(grid).datagrid("getRowIndex",rows[i])
                $(grid).datagrid("deleteRow",indx);
            }

        }
    });
}
function editData(grid) {

}

function addTab(subtitle, url, icon, isClose) {
    isClose = isClose != undefined ? isClose : true;
	if(!$('#tabs').tabs('exists',subtitle)){
		$('#tabs').tabs('add',{
			title:subtitle,
			content:createFrame(url),
			closable: isClose,
			icon:icon
		});
	}else{
        $('#tabs').tabs('select', subtitle);
        var currTab = $('#tabs').tabs('getSelected');
        updateTab(currTab, url);
	}
	tabClose();
}
function getTreeNodePath(tree, node) {
    var path = node.text;
    var pnode = pnode = tree.tree("getParent", node.target); ;
    while (pnode != null) {
        path =  pnode.text + ">>" + path;     
        pnode = tree.tree("getParent", pnode.target);
    }
    return path;
}
function createFrame(url)
{
	var s = '<iframe scrolling="auto" id="contentFrame" frameborder="0"  src="'+url+'" style="width:100%;height:100%;"></iframe>';
	return s;
}

function tabClose()
{
	/*双击关闭TAB选项卡*/
	$(".tabs-inner").dblclick(function(){
		var subtitle = $(this).children(".tabs-closable").text();
		$('#tabs').tabs('close',subtitle);
	})
	/*为选项卡绑定右键*/
//	$(".tabs-inner").bind('contextmenu',function(e){
//		$('#mm').menu('show', {
//			left: e.pageX,
//			top: e.pageY
//		});

//		var subtitle =$(this).children(".tabs-closable").text();

//		$('#mm').data("currtab",subtitle);
//		$('#tabs').tabs('select',subtitle);
//		return false;
//	});
}
function updateTab(currTab, url) {
    $('#tabs').tabs('update', {
        tab: currTab,
        options: {
            closable: false,
            content: createFrame(url)
        }
    })
}
//绑定右键菜单事件
function tabCloseEven()
{
	//刷新
    $('#mm-tabupdate').click(function () {
        var currTab = $('#tabs').tabs('getSelected');
        var url = $(currTab.panel('options').content).attr('src');
        updateTab(currTab, url);
    })
	//关闭当前
	$('#mm-tabclose').click(function(){
		var currtab_title = $('#mm').data("currtab");
		$('#tabs').tabs('close',currtab_title);
	})
	//全部关闭
	$('#mm-tabcloseall').click(function(){
		$('.tabs-inner span').each(function(i,n){
			var t = $(n).text();
			$('#tabs').tabs('close',t);
		});
	});
	//关闭除当前之外的TAB
	$('#mm-tabcloseother').click(function(){
		$('#mm-tabcloseright').click();
		$('#mm-tabcloseleft').click();
	});
	//关闭当前右侧的TAB
	$('#mm-tabcloseright').click(function(){
		var nextall = $('.tabs-selected').nextAll();
		if(nextall.length==0){
			//msgShow('系统提示','后边没有啦~~','error');
			alert('后边没有啦~~');
			return false;
		}
		nextall.each(function(i,n){
			var t=$('a:eq(0) span',$(n)).text();
			$('#tabs').tabs('close',t);
		});
		return false;
	});
	//关闭当前左侧的TAB
	$('#mm-tabcloseleft').click(function(){
		var prevall = $('.tabs-selected').prevAll();
		if(prevall.length==0){
			alert('到头了，前边没有啦~~');
			return false;
		}
		prevall.each(function(i,n){
			var t=$('a:eq(0) span',$(n)).text();
			$('#tabs').tabs('close',t);
		});
		return false;
	});

	//退出
	$("#mm-exit").click(function(){
		$('#mm').menu('hide');
	})
}

//弹出信息窗口 title:标题 msgString:提示信息 msgType:信息类型 [error,info,question,warning]
function msgShow(title, msgString, msgType) {
	top.$.messager.alert(title, msgString, msgType);
}
function autoMessage(title, msgString) {
    top.$.messager.show({
        title: title,
        msg: msgString,
        timeout: 3000,
        icon:'error',
        showType: 'slide'
    });  
}
function error(title) {
    msgShow("错误提示", title, "error");
}
function info(title) {
    msgShow("提示", title, "info");
}
function success(title) {
    autoMessage("成功提示", title);
}
function warning(title) {
    msgShow("警告", title, "warning");
}
function question(title) {
    msgShow("提示", title, "question");
}
(function ($) {
//全局系统对象
window['LG'] = {};
LG.cookies = (function () {
    var fn = function () {
    };
    fn.prototype.get = function (name) {
        var cookieValue = "";
        var search = name + "=";
        if (document.cookie.length > 0) {
            offset = document.cookie.indexOf(search);
            if (offset != -1) {
                offset += search.length;
                end = document.cookie.indexOf(";", offset);
                if (end == -1) end = document.cookie.length;
                cookieValue = decodeURIComponent(document.cookie.substring(offset, end))
            }
        }
        return cookieValue;
    };
    fn.prototype.set = function (cookieName, cookieValue, DayValue) {
        var expire = "";
        var day_value = 1;
        if (DayValue != null) {
            day_value = DayValue;
        }
        expire = new Date((new Date()).getTime() + day_value * 86400000);
        expire = "; expires=" + expire.toGMTString();
        document.cookie = cookieName + "=" + encodeURIComponent(cookieValue) + ";path=/" + expire;
    }
    fn.prototype.remvoe = function (cookieName) {
        var expire = "";
        expire = new Date((new Date()).getTime() - 1);
        expire = "; expires=" + expire.toGMTString();
        document.cookie = cookieName + "=" + escape("") + ";path=/" + expire;
        /*path=/*/
    };

    return new fn();
})();

    //遮罩
    LG.mask=function ()
    {
        $("<div class='l-window-mask' style='display: block;'></div>").appendTo('body');
    };
    //取消遮罩
    LG.unmask=function ()
    {
        $(".l-window-mask").remove();
    };
    //预加载图片
    LG.prevLoadImage = function (rootpath, paths) {
        for (var i in paths) {
            $('<img />').attr('src', rootpath + paths[i]);
        }
    };
    //显示loading
    LG.showLoading = function (message) {
        message = message || "正在加载中...";
        $('body').append("<div class='jloading'>" + message + "</div>");
        //$.ligerui.win.mask();
        LG.mask();
    };
    //隐藏loading
    LG.hideLoading = function (message) {
        $('body > div.jloading').remove();
        LG.unmask();
        //$.ligerui.win.unmask({ id: new Date().getTime() });
    };
    //提交服务器请求
    //返回json格式
    //1,提交给类 options.type  方法 options.method 处理
    //2,并返回 AjaxResult(这也是一个类)类型的的序列化好的字符串
    LG.ajax = function (options) {
        var p = options || {};
        var ashxUrl = options.ashxUrl || "../handler/ajax.ashx?";
        var url = p.url || ashxUrl + $.param({ type: p.type, method: p.method });
        $.ajax({
            cache: false,
            async: true,
            url: url,
            data: p.data,
            dataType: 'json', 
            type: 'post',
            beforeSend: function () {
                LG.loading = true;
                if (p.beforeSend)
                    p.beforeSend();
                else
                    LG.showLoading(p.loading);
            },
            complete: function () {
                LG.loading = false;
                if (p.complete)
                    p.complete();
                else
                    LG.hideLoading();
            },
            success: function (result) {
                if (!result) return;
                if (!result.IsError) {
                    if (p.success)
                        p.success(result.Data, result.Message);
                }
                else {
                    if (p.error)
                        p.error(result.Message);
                }
            },
            error: function (result, b) {
                error('发现系统错误 <BR>错误码：' + result.status);
            }
        });
    };
})(jQuery);