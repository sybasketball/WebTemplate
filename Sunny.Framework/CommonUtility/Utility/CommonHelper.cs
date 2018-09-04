using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using CommonUtility.Cryptography;
using System.Globalization;

namespace CommonUtility.Utility
{
    /// <summary>
    /// 公共帮助类
    /// </summary>
    public partial class CommonHelper
    {
        #region 加密解密
        private static DESEncrypt Des = new DESEncrypt();

        /// <summary>
        /// 普通加密
        /// </summary>
        /// <param name="content">需要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        /// <example>
        /// 传入要加密的字符串，并获取返回值
        /// <code>
        /// ……
        /// string str = "Hello World!";
        /// 
        /// //加密Hello World!
        /// string returnValue = Des_Encrypt(str);
        /// ……
        /// </code>
        /// </example>
        public static string Des_Encrypt(string content)
        {
           return CommonHelper.Des.EncryptString_AsicII(content);
        }
        
        /// <summary>
        /// 普通解密
        /// </summary>
        /// <param name="content">需要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        /// <example>
        /// 传入要解密的字符串，并获取返回值
        /// <code>
        /// ……
        /// string str = "Hello World";
        /// 
        /// //加密Hello World
        /// string returnValue = CommonHelper.Des_Encrypt(str);
        /// 
        /// //解密Hello World
        /// returnValue = Des_Decrypt(returnValue);
        /// ……
        /// </code>
        /// </example>
        public static string Des_Decrypt(string content)
        {
            return CommonHelper.Des.DecryptString_AsicII(content);
        }

        /// <summary>
        /// 解密String,返回指定类型
        /// </summary>
        /// <param name="desString">要解密的字符串</param>
        /// <param name="defualtValue">传入的返回类型默认值（例如int类型可以传入-1）</param>
        /// <typeparam name="T">解密后的返回类型</typeparam>
        /// <returns>解密后的对象</returns>
        /// <example>
        /// 传入要解密的字符串和药转换的类型默认值，并获取返回值
        /// <code>
        /// ……
        /// int temp = -1;
        /// 
        /// string str = Des_Encrypt("12345");
        /// 
        /// //返回12345
        /// int returnValue = Des_DesString&lt;int&gt;(str,temp);
        /// ……
        /// </code>
        /// </example>
        public static T Des_DesString<T>(string desString, T defualtValue)
        {
            if (!string.IsNullOrEmpty(desString))
            {
                string strDes = Des_Decrypt(desString);
                if (!string.IsNullOrEmpty(strDes))
                    return Convert_String<T>(strDes, defualtValue);
            }
            return defualtValue;
        }

        /// <summary>
        /// 强加密(用于密码等)
        /// </summary>
        /// <param name="content">要加密的字符串</param>
        /// <returns>加密后的字符串</returns>
        /// <example>
        /// 传入要加密的字符串，并获取返回值
        /// <code>
        /// ……
        /// string str = "Hello World!";
        /// 
        /// //加密Hello World!
        /// string returnValue = Des_EncryptByStrong(str);
        /// ……
        /// </code>
        /// </example>
        public static string Des_EncryptByStrong(string content)
        {
            return CommonHelper.Des.EncryptString(content);
        }

        /// <summary>
        /// 强解密
        /// </summary>
        /// <param name="content">要解密的字符串</param>
        /// <returns>解密后的字符串</returns>
        /// <example>
        /// 传入要解密的字符串，并获取返回值
        /// <code>
        /// ……
        /// string str = "Hello World";
        /// 
        /// //加密Hello World
        /// string returnValue = CommonHelper.Des_EncryptByStrong(str);
        /// 
        /// //解密Hello World
        /// returnValue = Des_DecryptByStrong(returnValue);
        /// ……
        /// </code>
        /// </example>
        public static string Des_DecryptByStrong(string content)
        {
            return CommonHelper.Des.DecryptString(content);
        }
        #endregion

        #region 获取对象的Value

        /// <summary>
        /// 获取数据字典Value
        /// </summary>
        /// <param name="dic">要取值的键值对列表</param>
        /// <param name="key">要取的值的键</param>
        /// <returns>要取的值</returns>
        /// <example>
        /// <code>
        /// ……
        /// Dictionary@lt;int,string@gt; dic = new Dictionary@lt;int,string@gt;();
        /// dic.Add(1, "member");
        /// dic.Add(2, "manager");
        /// 
        /// //返回member
        /// string divValue = CommonHelper.GetValue_Dic@lt;int, string@gt;(dic, 1);
        /// ……
        /// </code>
        /// </example>
        public static TValue GetValue_Dic<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key)
        {
            if (dic.ContainsKey(key))
                return dic[key];

            return default(TValue);
        }
        #endregion

        #region 其他
        /// <summary>
        /// 把1,2,3,...,35,36转换成A,B,C,...,Y,Z
        /// 要转换成字母的数字（数字范围在闭区间[1,36]）
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        public static string NunberToChar(int num)
        {
            if (1 <= num && 36 >= num)
            {
                ASCIIEncoding asciiEncoding = new ASCIIEncoding();
                byte[] btNumber = new byte[] { (byte)num };
                return asciiEncoding.GetString(btNumber);
            }
            return null;
        }
        #endregion

        #region 公共方法
        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="isDateTime">是否是日期时间类型</param>
        /// <returns>返回格式化的字符,为空时返回空串</returns>
        public static string Format_DateTime(DateTime? date,bool isDateTime)
        {
            return date.HasValue ? Format_DateTime(date.Value, isDateTime) : string.Empty;
        }

        /// <summary>
        /// 格式化日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="isDateTime">是否是日期时间类型</param>
        /// <returns>返回格式化的字符</returns>
        public static string Format_DateTime(DateTime date, bool isDateTime)
        {
            return string.Format(isDateTime ? CommonConst.SysFormat.Format_DateTime : CommonConst.SysFormat.Format_Date, date);
        }

        /// <summary>
        /// 泛型类型转换
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <param name="defualtValue"></param>
        /// <returns></returns>
        public static T Convert_String<T>(string str, T defualtValue)
        {
            if (!string.IsNullOrEmpty(str))
            {
                object obj = Convert.ChangeType(str, typeof(T));
                if (obj != null)
                    return (T)obj;
            }
            return defualtValue;
        }

        public static bool ContainValue(string container, string value)
        {
            return ContainValue(container, value, ",");
        }

        public static bool ContainValue(string container, string value, string split)
        {
            //if (string.IsNullOrEmpty(container))
            //    return false;
           return string.Concat(split, container, split).IndexOf(string.Concat(split, value, split)) > -1;
        }
        #endregion
    }
}
