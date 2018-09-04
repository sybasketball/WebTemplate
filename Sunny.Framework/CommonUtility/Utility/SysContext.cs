using System.Collections.Generic;
using System;
using System.Text;
using System.Web;
using CommonUtility.Utility;

namespace CommonUtility.Utility
{
    public sealed class SysContext
    {
        //用户状态的有效时间（分钟）
        private static double UserStatusValidity = 30;
        private static string Cookie_Pre = "25954BC5-4E3A-41F2-890D-A6CEA40B2333";
        public static void SetCurrent(string userid)
        {
            CurrentUserID = System.Web.HttpUtility.UrlEncode(userid, Encoding.UTF8); 
        }
        public static void ClearUserStatus()
        {
            CookieHelper.SetCookie(Cookie_Pre + "CurrentUserID", "0", DateTime.Now);
        }
        public static bool IsLogin {
            get {
                return !CurrentUserID.IsNullOrEmpty() && CurrentUserID != "0";
            }
        }        
        /// <summary>
        /// 当前用户组别ID
        /// </summary>
        public static string CurrentGroupID
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "CurrentGroupID"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "CurrentGroupID");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "CurrentGroupID", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {

                    CookieHelper.AddCookie(Cookie_Pre + "CurrentGroupID", value, DateTime.Now.AddMinutes(UserStatusValidity));
                } 
            }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public static string CurrentUserID
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "CurrentUserID"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "CurrentUserID");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "CurrentUserID", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {

                    CookieHelper.AddCookie(Cookie_Pre + "CurrentUserID", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                
            }
        }
        /// <summary>
        /// 上次登录时间
        /// </summary>
        public static string CurrentUserLastLoginTime
        {
            get
            {
                return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "CurrentUserLastLoginTime"));
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "CurrentUserLastLoginTime");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "CurrentUserLastLoginTime", value.ToString(), DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {
                    CookieHelper.AddCookie(Cookie_Pre + "CurrentUserLastLoginTime", value.ToString(), DateTime.Now.AddMinutes(UserStatusValidity));
                }
            }
        }
        public static string CurrentUserTitle
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "CurrentUserTitle"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "CurrentUserTitle");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "CurrentUserTitle", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {
                    CookieHelper.AddCookie(Cookie_Pre + "CurrentUserTitle", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }

            }
        }
        public static string CurrentUserName
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "CurrentUserName"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "CurrentUserName");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "CurrentUserName", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {

                    CookieHelper.AddCookie(Cookie_Pre + "CurrentUserName", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
            }
        }
        /// <summary>
        /// 是否为系统管理员
        /// </summary>
        public static string IsSystem
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "IsSystem"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "IsSystem");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "IsSystem", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {

                    CookieHelper.AddCookie(Cookie_Pre + "IsSystem", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
            }
        }
        /// <summary>
        /// 是否为项目管理员
        /// </summary>
        public static string IsProject
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "IsProject"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "IsProject");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "IsProject", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {

                    CookieHelper.AddCookie(Cookie_Pre + "IsProject", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
            }
        }
        /// <summary>
        /// 是否为内容浏览人员
        /// </summary>
        public static string IsBrowser
        {
            get
            {
                try
                {
                    return HttpUtility.UrlDecode(CookieHelper.GetCookieValue(Cookie_Pre + "IsBrowser"));
                }
                catch
                {
                    return "0";
                }
            }
            set
            {
                var cookie = CookieHelper.GetCookie(Cookie_Pre + "IsBrowser");
                value = HttpUtility.UrlEncode(value);
                if (cookie != null)
                {
                    CookieHelper.SetCookie(Cookie_Pre + "IsBrowser", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
                else
                {

                    CookieHelper.AddCookie(Cookie_Pre + "IsBrowser", value, DateTime.Now.AddMinutes(UserStatusValidity));
                }
            }
        }
        public static void ReSet()
        {
            CurrentUserID = CurrentUserID;
            CurrentUserLastLoginTime = CurrentUserLastLoginTime;
            CurrentUserTitle = CurrentUserTitle;
            CurrentUserName = CurrentUserName;
        }
    }
}
