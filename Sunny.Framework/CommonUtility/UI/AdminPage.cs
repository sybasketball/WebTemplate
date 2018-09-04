using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility.Utility;
using System.Web;

namespace CommonUtility.UI
{
    public class AdminPage : System.Web.UI.Page
    {
        /// <summary>
        /// 权限控制
        /// </summary>
        private void CheckPermission()
        {
            //权限判断
            if (SysContext.CurrentUserID == "0" || SysContext.CurrentUserID == "")
            {
                //Response.Redirect("~/admin/login.aspx?FromUrl=" + HttpUtility.UrlEncode(Request.Url.AbsoluteUri));
                Response.Redirect("~/admin/login.aspx");
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
