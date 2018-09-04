using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtility.Utility;
using CommonUtility.UI;
using Sunny.App.Web.Bll;

namespace Sunny.App.Web.Handlers
{
    /// <summary>
    /// ValidatorHandler 的摘要说明
    /// </summary>
    public class ValidatorHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string op = context.Request.QueryString["op"];
            object resultObj = null;
            string dataPara = "";
            try
            {
                switch (op)
                {
                    
                    case "exit_user_name":
                        dataPara = context.Request.Params["UserName"];
                        //resultObj = UserBll.ExitUserName(dataPara);
                        break;
                    case "edit_current_user_pwd":
                        resultObj = EditCurrentUserPassword(context);
                        break;
                    case "get_current_user_info":
                        resultObj = GetUserInfo();
                        break;
                    case "login":
                        string userName = context.Request.Params["UserName"];
                        string pwd = context.Request.Params["Password"];
                        resultObj = UserBll.Login(userName, pwd);
                        break;
                    case "loginout":
                        resultObj = UserBll.Logout();
                        break;
                }
            }
            catch (Exception ex)
            {
                resultObj = AjaxResult.Error(ex.Message);
            }
            context.Response.Write(resultObj.ToString());
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        private object EditCurrentUserPassword(HttpContext context)
        {
            AjaxResult result = AjaxResult.Error("更新密码失败");
            string userId = SysContext.CurrentUserID;
            if (SysContext.IsLogin)
            {
                string oldPassword = context.Request.Params["OldPassword"];
                string newPassword = context.Request.Params["RePassword"];
                if (UserBll.ValidatorCurrentUserPassword(oldPassword))
                {
                    if (UserBll.UpdatePassword(userId, newPassword))
                    {
                        result = AjaxResult.Success("更新密码成功");
                        UserBll.Logout();
                    }
                }
                else result = AjaxResult.Error("旧密码验证失败！");
            }
            else result = AjaxResult.Error("此用户登陆凭证失效，请重新登陆！");
            return result;
        }
        private object GetUserInfo() {
            AjaxResult result = AjaxResult.Error("登陆状态失效");
            if (SysContext.IsLogin)
            {
                string userName = SysContext.CurrentUserName;
                string userTitle = SysContext.CurrentUserTitle;
                string userId = SysContext.CurrentUserID;
                string lastTime = SysContext.CurrentUserLastLoginTime;
                var userInfo = new
                {
                    UserName = userName,
                    UserTitle = userTitle,
                    UserId = userId,
                    LastLoginTime = lastTime
                };
                result = AjaxResult.Success(userInfo);
            }
            return result;
        }
    }
}