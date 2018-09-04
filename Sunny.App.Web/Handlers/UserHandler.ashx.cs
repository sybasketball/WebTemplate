using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtility.Utility;

namespace Sunny.App.Web.Handlers
{
    /// <summary>
    /// UserHanderHandler 的摘要说明
    /// </summary>
    public class UserHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            string op = context.Request.QueryString["op"];
            object resultObj = null;
            string dataPara = "";
            switch (op)
            {
                case "select":
                    dataPara = GetSelectPara(context);
                    //resultObj = UserBll.Select(dataPara);
                    break;
                case "not_exit_user":
                    dataPara = context.Request.Params["UserName"];
                    //resultObj = !(UserBll.ExitUserName(dataPara));
                    //resultObj = resultObj.ToString().ToLower();
                    break;
                case "edit":
                case "add":
                    dataPara = GetUpdatePara(context);
                    //resultObj = UserBll.SaveEntry(dataPara);
                    break;
                case "update_pwd":
                    dataPara = GetUpdatePara(context);
                    //resultObj = UserBll.UpdatePassword(dataPara);
                    break;
                case "update_login_info":
                    dataPara = GetLoginInfoPara(context);
                    //resultObj = UserBll.UpdateLoginInfo(dataPara);
                    break;
                case "remove":
                    dataPara = context.Request.Params["ids"];
                    //resultObj = UserBll.DelByIDs(dataPara);
                    break;
                case "login":
                    dataPara = GetLoginPara(context);
                    //resultObj = UserBll.Login(dataPara);
                    break;
                case "loginout":
                    //resultObj = UserBll.Logout();
                    break;
                case "getNamesByIds":
                    dataPara = context.Request.Params["ids"];
                    //resultObj = UserBll.GetUserNameByIDs(dataPara);
                    break;
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
        private string GetLoginPara(HttpContext context)
        {
            string UserName = context.Request.Params["UserName"];
            string Password = context.Request.Params["Password"];
            var Body = new { UserName = UserName, Password = Password };
            return JsonHelper.SerializeData(Body);
        }
        private string GetLoginInfoPara(HttpContext context)
        {
            string Id = context.Request.Params["Id"];
            string UserName = context.Request.Params["UserName"];
            string Password = context.Request.Params["Password"];
            string RoleId = context.Request.Params["RoleId"];
            //if (UserName == null)
            //{
            //    var Body1 = new { Id = Id, Password = Password, RoleId = RoleId };
            //    return JsonHelper.SerializeData(Body1);
            //}
            var Body = new { Id = Id, UserName = UserName, Password = Password, RoleId = RoleId };
            return JsonHelper.SerializeData(Body);
        }
        private string GetUpdatePara(HttpContext context)
        {
            IDictionary<string, string> data = context.Request.Form.ToDictionary();
            int Id = ValidatorHelper.StrToInt(data["Id"], 0);
            if (Id < 1)
            {
                //if (UserBll.ExitUserName(data["UserName"]))
                //{
                //    context.Response.Write(AjaxResult.Error("该用户名已存在"));
                //    context.Response.End();
                //}
                //data.Add("AddUserId", SysContext.CurrentUserID);
            }
            return JsonHelper.SerializeData(data);
        }
        private string GetSelectPara(HttpContext context)
        {
            string where = context.Request.Params["Where"];
            string pageIndex = ValidatorHelper.StrToInt(context.Request.Params["page"], 1).ToString();
            string pageSize = ValidatorHelper.StrToInt(context.Request.Params["rows"], 5000).ToString();
            string sort = context.Request.Params["sort"];
            string order = context.Request.Params["order"];
            var Body = new { PageIndex = pageIndex, PageSize = pageSize, Sort = sort, Order = order, Where = where };
            return JsonHelper.SerializeData(Body);
        }
    }
}