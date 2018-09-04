using CommonUtility.UI;
using CommonUtility.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sunny.App.Web.Bll
{
    public class UserBll
    {
        public static GridResultModel SelectAll()
        {
            GridResultModel result = new GridResultModel();
            AccessHelper acc = new AccessHelper();
            DataTable dt = acc.GetDataTableFromDB("select * from users");
            result.rows = dt.Rows;
            result.total = dt.Rows.Count;
            return result;
        }
        public static object Login(string userName,string password) {
            AjaxResult result = AjaxResult.Error("登陆失败");
            try
            {
                AccessHelper acc = new AccessHelper();
                DataTable dt = acc.GetDataTableFromDB("select Id,UserName,UserTitle from users where UserName='" + userName + "' and Password='" + password + "'");
                if (dt != null && dt.Rows.Count > 0)
                {
                    SysContext.SetCurrent(dt.Rows[0][0].ToString());
                    SysContext.CurrentUserID = dt.Rows[0][0].ToString();
                    SysContext.CurrentUserName = dt.Rows[0][1].ToString();
                    SysContext.CurrentUserTitle = dt.Rows[0][2].ToString();
                    result = AjaxResult.Success("登陆成功");
                }
                else
                {
                    throw new Exception("登录失败，用户名或密码错误");
                }
            }
            catch (Exception ex)
            {
                result = AjaxResult.Error(ex.Message);
            }
            return result;
        }
        public static bool UpdatePassword(string userId,string password)
        {
            try
            {
                string sql = "update users set [Password]='" + password + "' where Id = " + userId;
                AccessHelper acc = new AccessHelper();
                if (acc.ExcuteSql(sql) > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static bool ValidatorCurrentUserPassword(string password) {
            try
            {
                string sql = "select * from users where Id=" + SysContext.CurrentUserID + " and Password='" + password + "'";
                AccessHelper acc = new AccessHelper();
                DataTable dt = acc.GetDataTableFromDB(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public static object Logout()
        {
            AjaxResult result = AjaxResult.Error("退出失败");
            try
            {
                SysContext.ClearUserStatus();
                result = AjaxResult.Success("退出成功");
            }
            catch (Exception ex)
            {
                result = AjaxResult.Error(ex.Message);
            }
            return result;
        }
        public static object GetEntityByID(string id)
        {
            try
            {
                AccessHelper acc = new AccessHelper();
                DataTable dt = acc.GetDataTableFromDB("select Id,UserName,UserTitle from users where Id = " + id);
                if (dt != null && dt.Rows.Count > 0)
                {
                    AjaxResult reult = AjaxResult.Success("数据获取成功");
                    reult.Data = dt.Rows[0].ItemArray;
                    return reult;
                }
                return null;
            }
            catch (Exception ex)
            {
                return AjaxResult.Error(ex.Message);
            }
        }
        public static object DelByIDs(string postContent)
        {
            AjaxResult result = AjaxResult.Error("删除失败");
            try
            {
                if (!string.IsNullOrEmpty(postContent))
                {
                    //result = AjaxResult.Success("成功删除：“" + count + "”条数据");
                }
            }
            catch (Exception ex)
            {
                result = AjaxResult.Error(ex.Message);
            }
            return result;
        }
    }
}