
var PrintBll = {
    DataGrid: null,
    InitPage: function () {
    },
    Select: function (url, containerId, tempId, callback, isReturnFullData) {
        BasePage.Ajax({
            type: 'GET',
            url: url,
            isReturnFullData:isReturnFullData,
            success: function (data, message) {
                $("#" + containerId).setTemplateElement(tempId, null, { filter_data: true });
                $("#" + containerId).processTemplate(data.Data||data.rows||data);
                if (callback) callback(data);
            }
        });
    },
    //分页显示
    PageSelect: function (url, containerId, tempId, pageSize, title) {
        BasePage.Ajax({
            type: 'GET',
            url: url,
            success: function (data, message) {
                var length = data.total;
                var page = Math.ceil(length / pageSize);
                for (var i = 0; i < page; i++) {
                    var start = i * pageSize;
                    var end = (i + 1) * pageSize;
                    if (end > length) end = length;
                    var newdata = { rows: data.rows.slice(start, end), title: title, pagesize: pageSize, pageindex: i + 1, totalpage: page, totalrecord: length };
                    PrintBll.BindPageTemplate(newdata, containerId, tempId, i + 1);
                }
            }
        });
    },
    BindPageTemplate: function (data, containerId, tempId, pageIndex) {
        var container = '<div id="page_' + pageIndex + '"></div>';
        $("#" + containerId).append($(container));
        $("#page_" + pageIndex).setTemplateElement(tempId, null, { filter_data: true });
        $("#page_" + pageIndex).processTemplate(data);
    }
}

