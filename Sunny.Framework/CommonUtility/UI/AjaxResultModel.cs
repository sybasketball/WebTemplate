using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using CommonUtility.Utility;

namespace CommonUtility.UI
{
    public interface IAjaxResultModel {     };
    public class GridResultModel : IAjaxResultModel
    {
        public GridResultModel()
        {
        }
        public object rows { get; set; }
        public int total { get; set; }
        public override string ToString()
        {
            string result = JsonHelper.SerializeData(this);
            return result.Replace("\"rows\":null", "\"rows\":[]");
        }
    }
    public class CustomerGridResultModel : GridResultModel
    {
        public CustomerGridResultModel()
        {
        }
        public object columns { get; set; }
        public override string ToString()
        {
            string result = JsonHelper.SerializeData(this);
            return result.Replace("\"rows\":null", "\"rows\":[]").Replace("\"columns\":null", "\"columns\":[]");
        }
    }
    public class JsonEntityModel : IAjaxResultModel
    {
        public JsonEntityModel()
        {
        }
        private bool iserror = false;

        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { get { return iserror; } }
        public object dataEntity { get; set; }
        public override string ToString()
        {
            return dataEntity != null ? JsonHelper.SerializeData(this) : string.Empty;
        }
    }
    public class AjaxResult : IAjaxResultModel
    {
        private AjaxResult()
        {
        }

        private bool iserror = false;

        /// <summary>
        /// 是否产生错误
        /// </summary>
        public bool IsError { get { return iserror; } }

        /// <summary>
        /// 错误信息，或者成功信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 成功可能时返回的数据
        /// </summary>
        public object Data { get; set; }

        #region Error
        public static AjaxResult Error()
        {
            return new AjaxResult()
            {
                iserror = true
            };
        }
        public static AjaxResult Error(string message)
        {
            return new AjaxResult()
            {
                iserror = true,
                Message = message
            };
        }
        #endregion

        #region Success
        public static AjaxResult Success()
        {
            return new AjaxResult()
            {
                iserror = false
            };
        }
        public static AjaxResult Success(string message)
        {
            return new AjaxResult()
            {
                iserror = false,
                Message = message
            };
        }
        public static AjaxResult Success(object data)
        {
            return new AjaxResult()
            {
                iserror = false,
                Data = data
            };
        }
        public static AjaxResult Success(object data, string message)
        {
            return new AjaxResult()
            {
                iserror = false,
                Data = data,
                Message = message
            };
        }
        #endregion

        public override string ToString()
        {
            return JsonHelper.SerializeData(this);
        }
    }
}
