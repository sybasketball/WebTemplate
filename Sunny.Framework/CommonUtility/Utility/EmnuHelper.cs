using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.Data;

namespace CommonUtility.Utility
{
    public class EmnuHelper
    {
        /// <summary>
        /// Get Enum's EmnuAttribute
        /// Added by HuZhang 2006-1-12
        /// </summary>
        /// <param name="enumConst"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string GetEnumTextVal(int enumConst, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            string textVal = "";

            Type typeDescription = typeof(EmnuAttribute);
            FieldInfo fieldInfo = enumType.GetField(Enum.GetName(enumType, enumConst).ToString());

            if (fieldInfo != null)
            {
                object[] arr = fieldInfo.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    EmnuAttribute EmnuAttribute = (EmnuAttribute)arr[0];
                    textVal = EmnuAttribute.Text;
                }
            }

            return textVal;
        }

        public static string GetEnumTextVal(string enumName, Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            string textVal = "";

            Type typeDescription = typeof(EmnuAttribute);
            FieldInfo fieldInfo = enumType.GetField(enumName);

            if (fieldInfo != null)
            {
                object[] arr = fieldInfo.GetCustomAttributes(typeDescription, true);
                if (arr.Length > 0)
                {
                    EmnuAttribute EmnuAttribute = (EmnuAttribute)arr[0];
                    textVal = EmnuAttribute.Text;
                }
            }

            return textVal;
        }

        /// <summary>
        /// Get a table by Enum,the table has Text and Value columns
        /// Added by HuZhang 2006-1-12
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumTable(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            Type typeDescription = typeof(EmnuAttribute);

            FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    DataRow dr = dt.NewRow();

                    dr["Value"] = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();

                    object[] arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        EmnuAttribute aa = (EmnuAttribute)arr[0];
                        dr["Text"] = aa.Text;
                    }
                    else
                    {
                        dr["Text"] = field.Name;
                    }
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }

        /// <summary>
        /// Get a table by Enum,the table has Text and Value columns
        /// Added by HuZhang 2006-1-12
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static DataTable GetEnumTableForText(Type enumType)
        {
            if (!enumType.IsEnum)
            {
                throw new InvalidOperationException();
            }

            DataTable dt = new DataTable();
            dt.Columns.Add("Text", typeof(System.String));
            dt.Columns.Add("Value", typeof(System.String));

            FieldInfo[] fields = enumType.GetFields();

            foreach (FieldInfo field in fields)
            {
                if (field.FieldType.IsEnum == true)
                {
                    DataRow dr = dt.NewRow();

                    dr["Value"] = ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    dr["Text"] = enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null).ToString();
                    dt.Rows.Add(dr);
                }
            }

            return dt;
        }
    }
    #region EmnuAttribute
    /// <summary>
    /// EmnuAttribute
    /// Add Description to Enum
    /// Create by huzhang 2006-1-12
    /// </summary>
    [AttributeUsage(AttributeTargets.Enum,AllowMultiple = true)]
    public class EmnuAttribute : Attribute
    {
        string _Text;
        /// <summary>
        /// 显示的文本
        /// </summary>
        public string Text
        {
            get { return _Text; }
            set
            {
                _Text = value;
            }
        }

        //int _Value = -1;
        ///// <summary>
        ///// Value
        ///// </summary>
        //public int Value
        //{
        //    get { return _Value; }
        //    set { _Value = value; }
        //}

        string _Url;
        /// <summary>
        /// Url
        /// </summary>
        public string Url
        {
            get { return _Url; }
            set { _Url = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="text"></param>
        public EmnuAttribute(string text)
        {
            this._Text = text;
        }

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="value"></param>
        //public EmnuAttribute(string text, int value)
        //{
        //    this._Text = text;
        //    this._Value = value;
        //}

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="text"></param>      
        ///// <param name="url"></param>
        //public EmnuAttribute(string text, string url)
        //{
        //    this._Text = text;
        //    this._Url = url;
        //}

        ///// <summary>
        ///// 构造函数
        ///// </summary>
        ///// <param name="text"></param>
        ///// <param name="value"></param>
        ///// <param name="url"></param>
        //public EmnuAttribute(string text, int value, string url)
        //{
        //    this._Text = text;
        //    this._Url = url;
        //    this._Value = value;
        //}
    }
    #endregion

    //[AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
    //public class AttachDataAttribute : Attribute
    //{
    //    public AttachDataAttribute(object key, object value)
    //    {
    //        this.Key = key;
    //        this.Value = value;
    //    }

    //    public object Key { get; private set; }

    //    public object Value { get; private set; }
    //}

    //public static class AttachDataExtensions
    //{
    //    public static object GetAttachedData(this　ICustomAttributeProvider provider, object key)
    //    {
    //        var attributes = (AttachDataAttribute[])provider.GetCustomAttributes(typeof(AttachDataAttribute), false);
    //        attributes[0].Key.Equals(
    //        return attributes.First(a => a.Key.Equals(key)).Value;
    //    }

    //    public static T GetAttachedData<T>(this　ICustomAttributeProvider provider, object key)
    //    {
    //        return (T)provider.GetAttachedData(key);
    //    }

    //    public static object GetAttachedData(this　Enum value, object key)
    //    {
    //        return value.GetType().GetField(value.ToString()).GetAttachedData(key);
    //    }

    //    public static T GetAttachedData<T>(this　Enum value, object key)
    //    {
    //        return (T)value.GetAttachedData(key);
    //    }
    //}
}
