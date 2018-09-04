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
    /// ����������
    /// </summary>
    public partial class CommonHelper
    {
        #region ���ܽ���
        private static DESEncrypt Des = new DESEncrypt();

        /// <summary>
        /// ��ͨ����
        /// </summary>
        /// <param name="content">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <example>
        /// ����Ҫ���ܵ��ַ���������ȡ����ֵ
        /// <code>
        /// ����
        /// string str = "Hello World!";
        /// 
        /// //����Hello World!
        /// string returnValue = Des_Encrypt(str);
        /// ����
        /// </code>
        /// </example>
        public static string Des_Encrypt(string content)
        {
           return CommonHelper.Des.EncryptString_AsicII(content);
        }
        
        /// <summary>
        /// ��ͨ����
        /// </summary>
        /// <param name="content">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <example>
        /// ����Ҫ���ܵ��ַ���������ȡ����ֵ
        /// <code>
        /// ����
        /// string str = "Hello World";
        /// 
        /// //����Hello World
        /// string returnValue = CommonHelper.Des_Encrypt(str);
        /// 
        /// //����Hello World
        /// returnValue = Des_Decrypt(returnValue);
        /// ����
        /// </code>
        /// </example>
        public static string Des_Decrypt(string content)
        {
            return CommonHelper.Des.DecryptString_AsicII(content);
        }

        /// <summary>
        /// ����String,����ָ������
        /// </summary>
        /// <param name="desString">Ҫ���ܵ��ַ���</param>
        /// <param name="defualtValue">����ķ�������Ĭ��ֵ������int���Ϳ��Դ���-1��</param>
        /// <typeparam name="T">���ܺ�ķ�������</typeparam>
        /// <returns>���ܺ�Ķ���</returns>
        /// <example>
        /// ����Ҫ���ܵ��ַ�����ҩת��������Ĭ��ֵ������ȡ����ֵ
        /// <code>
        /// ����
        /// int temp = -1;
        /// 
        /// string str = Des_Encrypt("12345");
        /// 
        /// //����12345
        /// int returnValue = Des_DesString&lt;int&gt;(str,temp);
        /// ����
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
        /// ǿ����(���������)
        /// </summary>
        /// <param name="content">Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <example>
        /// ����Ҫ���ܵ��ַ���������ȡ����ֵ
        /// <code>
        /// ����
        /// string str = "Hello World!";
        /// 
        /// //����Hello World!
        /// string returnValue = Des_EncryptByStrong(str);
        /// ����
        /// </code>
        /// </example>
        public static string Des_EncryptByStrong(string content)
        {
            return CommonHelper.Des.EncryptString(content);
        }

        /// <summary>
        /// ǿ����
        /// </summary>
        /// <param name="content">Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ���ַ���</returns>
        /// <example>
        /// ����Ҫ���ܵ��ַ���������ȡ����ֵ
        /// <code>
        /// ����
        /// string str = "Hello World";
        /// 
        /// //����Hello World
        /// string returnValue = CommonHelper.Des_EncryptByStrong(str);
        /// 
        /// //����Hello World
        /// returnValue = Des_DecryptByStrong(returnValue);
        /// ����
        /// </code>
        /// </example>
        public static string Des_DecryptByStrong(string content)
        {
            return CommonHelper.Des.DecryptString(content);
        }
        #endregion

        #region ��ȡ�����Value

        /// <summary>
        /// ��ȡ�����ֵ�Value
        /// </summary>
        /// <param name="dic">Ҫȡֵ�ļ�ֵ���б�</param>
        /// <param name="key">Ҫȡ��ֵ�ļ�</param>
        /// <returns>Ҫȡ��ֵ</returns>
        /// <example>
        /// <code>
        /// ����
        /// Dictionary@lt;int,string@gt; dic = new Dictionary@lt;int,string@gt;();
        /// dic.Add(1, "member");
        /// dic.Add(2, "manager");
        /// 
        /// //����member
        /// string divValue = CommonHelper.GetValue_Dic@lt;int, string@gt;(dic, 1);
        /// ����
        /// </code>
        /// </example>
        public static TValue GetValue_Dic<TKey, TValue>(Dictionary<TKey, TValue> dic, TKey key)
        {
            if (dic.ContainsKey(key))
                return dic[key];

            return default(TValue);
        }
        #endregion

        #region ����
        /// <summary>
        /// ��1,2,3,...,35,36ת����A,B,C,...,Y,Z
        /// Ҫת������ĸ�����֣����ַ�Χ�ڱ�����[1,36]��
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

        #region ��������
        /// <summary>
        /// ��ʽ������
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="isDateTime">�Ƿ�������ʱ������</param>
        /// <returns>���ظ�ʽ�����ַ�,Ϊ��ʱ���ؿմ�</returns>
        public static string Format_DateTime(DateTime? date,bool isDateTime)
        {
            return date.HasValue ? Format_DateTime(date.Value, isDateTime) : string.Empty;
        }

        /// <summary>
        /// ��ʽ������
        /// </summary>
        /// <param name="date">����</param>
        /// <param name="isDateTime">�Ƿ�������ʱ������</param>
        /// <returns>���ظ�ʽ�����ַ�</returns>
        public static string Format_DateTime(DateTime date, bool isDateTime)
        {
            return string.Format(isDateTime ? CommonConst.SysFormat.Format_DateTime : CommonConst.SysFormat.Format_Date, date);
        }

        /// <summary>
        /// ��������ת��
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
