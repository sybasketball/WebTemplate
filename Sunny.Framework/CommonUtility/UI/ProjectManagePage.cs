using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CommonUtility.Utility;
using System.Web;

namespace CommonUtility.UI
{
    public class ProjectManagePage : AdminPage
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (SysContext.IsProject == "0")
            {
                Response.Write("该用户没有项目管理权限！请返回重新登录");
                Response.End();
            }
        }
    }
}
