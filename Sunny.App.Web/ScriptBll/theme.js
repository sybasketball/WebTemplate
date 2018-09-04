var theme = {    
    Items: [
        {
            title: "蓝色",
            name: "metro-blue",
            css: [
                '<link tag="theme" href="/Resource/easyui/themes/metro-blue/easyui.css" rel="stylesheet" type="text/css" />',
                '<link tag="theme" href="/Resource/easyui/themes/metro-gray/icon.css" rel="stylesheet" type="text/css" />',
                '<link tag="theme" href="/Resource/easyui/themes/metro-blue/page.css" rel="stylesheet" type="text/css" />'
            ]
        },
        {
            title: "灰色",
            name: "metro-gray",
            css: [
                '<link tag="theme"  href="/Resource/easyui/themes/metro-gray/easyui.css" rel="stylesheet" type="text/css" />',
                '<link tag="theme"  href="/Resource/easyui/themes/metro-gray/icon.css" rel="stylesheet" type="text/css" />',
                '<link tag="theme"  href="/Resource/easyui/themes/metro-gray/page.css" rel="stylesheet" type="text/css" />'
            ]
        },
        {
            title: "bootstrap",
            name: "bootstrap",
            css: [
                '<link tag="theme"  href="/Resource/easyui/themes/bootstrap/easyui.css" rel="stylesheet" type="text/css" />',
                '<link tag="theme"  href="/Resource/easyui/themes/metro-gray/icon.css" rel="stylesheet" type="text/css" />',
                '<link tag="theme"  href="/Resource/easyui/themes/bootstrap/page.css" rel="stylesheet" type="text/css" />'
            ]
        }
    ],
    COOKIE_PREX: function () { return "IETM_THEME_" + book.CurrentUserInfo.UserId },
    InitPageTheme: function (callBack) {
        var index = this.GetCurrentTheme();
        this.SetPageTheme(index, callBack);
    },
    ChangePageTheme: function (index) {        
        $.cookies.set(this.COOKIE_PREX(), index, { hoursToLive: 100 });
        this.SetPageTheme(index);
    },
    SetPageTheme: function (index, callBack) {
        var item = this.Items[index];
        easyloader.locale = "zh_CN"; // 本地化设置
        easyloader.css = false;
        $("head link[tag='theme']").remove();
        $(item.css).each(function () {
            $("head").append(this.toString());
        })
        using(['window', 'messager', 'layout', 'tab', 'datagrid', 'form', 'tooltip', 'searchbox', "validatebox_ext"], callBack || function () {
            
        });
        var cw = book.GetContentWindow();
        if (cw) cw.theme.SetPageTheme(index);
    },
    SaveTheme:function()
    {
        $.cookies.set(this.COOKIE_PREX(), index, { hoursToLive: 100 });
    },
    GetCurrentTheme:function(){
        skinIndex = $.cookies.get(this.COOKIE_PREX());
        if (skinIndex == null) {
            skinIndex = 0;
        }
        return skinIndex;
    }
}