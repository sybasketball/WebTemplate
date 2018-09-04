<%@ Page Language="C#" AutoEventWireup="true" Inherits="CommonUtility.UI.NormalPage" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>新闻内容页面</title>
    <!--#include file="/Include/base_js.aspx"-->    
    <script src="/ScriptBll/PublicBll.js" type="text/javascript"></script> 
    <script src="/ScriptBll/ProjectBll.js" type="text/javascript"></script> 
    <script type="text/javascript">
        var id = jQuery.url.param("id");
        $(function () {
            ProjectBll.Load(id);
            $("#dateAndTime").html(getCurrentDateAndWeek());
        });
        function getCurrentDateAndWeek() {
            var date = new Date();
            var dateStr = date.getFullYear() + "年" + (date.getMonth() + 1) + "月" + date.getDate() + "日";
            var weekStr = "　星期" + "日一二三四五六".charAt(date.getDay());
            return dateStr + weekStr;
        };
    </script>
</head>
<style type="text/css">
a{ text-decoration:none;}
.banner{ width:1030px; margin:0 auto;}
.nav{ width:1030px; height:40px; line-height:40px; background:#066fb3; margin:0 auto;}
.nav ul li{ line-height:40px; padding:0 15px; float:left; list-style:none;}
.nav ul li a{ font-size:14px; font-weight:bold; color:#fff!important;;}
.nav_bottom{ width:1000px; height:80px; background:#eff5f8;   margin-top:10px;margin:0 auto;}
.nav_bottom ul li{ line-height:40px;list-style:none;}
.nav_bottom ul li a{ font-size:14px; display:block; float:left; padding:0 15px; font-weight:bold; color:#666!important;;}
#dateAndTime{ color:#fff!important; font-size:14px; font-weight:bold;}
#sddm7{ position:relative;}
#sddm7 div{position: absolute; top:28px; left:-50px; visibility: hidden; margin: 0;padding: 0px 10px 0px 10px;background: #f1f2f3;border:1px solid #ccc; width:130px; line-height: 30px;height:auto; z-index:3; }
#sddm7 div a{padding-left:10px; display:block; color:#333333!important;}
*{margin:0; padding:0;}
.gs{ width:1000px;  margin:0 auto;}
.title{ width:800px; margin:0 auto; height:50px; display:block; clear:both; text-align:center; line-height:50px; font-size:24px; font-weight:bold;}
.gs p{ text-indent:2em; font-size:18px; line-height:2em;}
.gs .left{ float:right; line-height:2em; font-size:18px; text-align:center; width:300px;}
.contentBody{ width:970px; margin:0 auto; padding:20px 30px; font-family:"宋体" }
.contentBody #title{font-size:25px; line-height:200%; text-align:center;font-weight:bold;}
.contentBody .publicTime{font-size:14px; text-align:center;}
.contentBody #contentDiv{font-size:15px;line-height:250%; margin-top:30px;min-height:400px; text-align:left; }
    .foot {width:1030px; margin:20px auto; font-size:14px;text-align:center; padding:15px 0; border-top:1px solid #ddd; font-family:"宋体"   }
</style>
<body>
    <center>
<div class="head">
	<div class="banner">
		<embed src="Resource/style/banner.swf" style=" width:1030px; height:183px"></embed>	
	</div>
	<div class="nav">
		<ul>
			<li>
			<a href="http://www.cqyteip.petrochina/sites/ejdw1/cy4c/Pages/default.aspx" target="_blank">首页</a></li>
			<li><a href="http://www.cqyteip.petrochina/Pages/default.aspx" target="_blank">长庆油田公司</a></li>
			<li><a href="http://www.cnpc/Pages/default.html" target="_blank">中国石油内网</a></li>
			<li><a href="http://www.cnpc.com.cn/" target="_blank">中国石油外网</a></li>
			<li><a href="http://info.cnpc/" target="_blank">信息资源网</a></li>
			<li><a href="http://www.cqyteip.petrochina/sites/ejdw1/cy4c/Pages/default2017.aspx" target="_blank">旧版回顾</a></li>
			<li style=" float:right; margin-right:15px">	        
                <div id="dateAndTime">                    

                </div>
            </li>
		</ul>
	</div>
	

	<div style="clear:both;"></div>
    </div>
    <div class="contentBody">
        <div id="title">
            
        </div>
        <div class="publicTime">
            <span>发布时间：</span><span id="publicTime"></span>
        </div>
        <div id="contentDiv">

        </div>
    </div>
    <div class="foot">
        版权所有：长庆油田第四采油厂
    </div>
  </center>
</body>
</html>
