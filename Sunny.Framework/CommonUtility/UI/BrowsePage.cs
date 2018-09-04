using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility.Utility;
using System.Web;

namespace CommonUtility.UI
{
    public class BrowsePage : System.Web.UI.Page
    {
        /// <summary>
        /// 权限控制
        /// </summary>
        private void CheckPermission()
        {
            //权限判断
            if (SysContext.CurrentUserID == "0" || SysContext.CurrentUserID == "")
            {
                Response.Redirect("~/ietp/login.aspx");
                //Response.Redirect("~/ietp/login.aspx?FromUrl=" + HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
            }
            else if(SysContext.IsBrowser == "0" || SysContext.IsBrowser == "")
            {
                Response.Write("该用户没有手册浏览权限！请返回重新登录");
                Response.End();
            }
            else
            {
                SysContext.ReSet();
            }
        }
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            this.CheckPermission();
        }
    }
}
