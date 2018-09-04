using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility.Utility;
using System.Web;

namespace CommonUtility.UI
{
    public class SystemManagePage : AdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (SysContext.IsSystem == "0")
            {
                Response.Write("该用户没有系统管理权限！请返回重新登录");
                Response.End();
            }
        }
    }
}
