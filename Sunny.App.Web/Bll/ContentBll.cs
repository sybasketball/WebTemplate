using CommonUtility.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Sunny.App.Web.Bll
{
    public class ContentModel {
        public int Id { get; set; } = 0;
        public string Title { get; set; }
        public string Summary { get; set; }
        public string Content { get; set; }
        
    }
    public class ContentBll
    {
        public static GridResultModel SelectAll()
        {
            GridResultModel result = new GridResultModel();
            AccessHelper acc = new AccessHelper();
            DataTable dt = acc.GetDataTableFromDB("select Id,Title,AddTime from news");
            if (dt != null)
            {
                result.rows = dt;
                result.total = dt.Rows.Count;
            }
            return result;
        }
        public static object Save(IDictionary<string, string> data)
        {
            try
            {
                string sql = AccessHelper.ConvertToUpdateSql(data);
                AccessHelper acc = new AccessHelper();
                if (acc.ExcuteSql(sql) > 0)
                {
                    return AjaxResult.Success("数据更新成功");
                }
                return null;
            }
            catch (Exception ex)
            {
                return AjaxResult.Error(ex.Message);
            }
        }
        public static object GetEntityByID(string id)
        {
            try
            {
                AccessHelper acc = new AccessHelper();
                DataTable dt = acc.GetDataTableFromDB("select Id,Title,AddTime,Content from news where Id = " + id);
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
            AccessHelper acc = new AccessHelper();           
            AjaxResult result = AjaxResult.Error("删除失败");
            try
            {
                string sql = "DELETE FROM news WHERE Id in ("+ postContent + ")";
                if (!string.IsNullOrEmpty(postContent))
                {
                    int count = acc.ExcuteSql(sql);
                    result = AjaxResult.Success("成功删除：“" + count + "”条数据");
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