using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommonUtility.Utility;
using System.Data;
using CommonUtility.UI;
using System.Collections.Specialized;
using Sunny.App.Web.Bll;

namespace Sunny.App.Web.Handlers
{
    public static class NVCExtender
    {
        public static IDictionary<string, string> ToDictionary(
                                            this NameValueCollection source)
        {
            return source.AllKeys.ToDictionary(k => k, k => source[k]);
        }
    }
    /// <summary>
    /// OrganizationHandler 的摘要说明
    /// </summary>
    public class ProjectHandler : IHttpHandler
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
                    case "select":
                        //dataPara = GetSelectPara(context); 
                        resultObj = ContentBll.SelectAll();
                        break;
                    case "edit":
                    case "add":
                        resultObj = ContentBll.Save(context.Request.Form.ToDictionary());
                        break;
                    case "get_one_data":
                        resultObj = ContentBll.GetEntityByID(context.Request.Params["id"]);
                        break;
                    case "remove":
                        dataPara = context.Request.Params["Ids"];
                        resultObj = ContentBll.DelByIDs(dataPara);
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
        
        private IDictionary<string, string> GetUpdatePara(HttpContext context)
        {
            IDictionary<string,string> data = context.Request.Form.ToDictionary();
            //data.Add("AddUserId", SysContext.CurrentUserID);
            return data;
        }
        private string GetSelectPara(HttpContext context)
        {
            string where = context.Request.Params["Where"];
            string pageIndex = ValidatorHelper.StrToInt(context.Request.Params["page"], 0).ToString();
            string pageSize = ValidatorHelper.StrToInt(context.Request.Params["rows"], 0).ToString();
            string sort = context.Request.Params["sort"];
            var Body = new { PageIndex = pageIndex, PageSize = pageSize, Sort = sort, Where = where };
            return JsonHelper.SerializeData(Body);
        }
    }
}