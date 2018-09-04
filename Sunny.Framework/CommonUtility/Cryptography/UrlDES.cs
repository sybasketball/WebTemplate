using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace CommonUtility.Cryptography
{
    /// <summary>
    /// Security 的摘要说明。
    /// Security类实现.NET框架下的加密和解密。
    /// </summary>
    public class Security
    {
        public const string const_QueryStringKey = "abcdefgh"; //URL传输参数加密Key
        public const string const_PassWordKey = "hgfedcba";  //PassWord加密Key

        #region 加密
        /// <summary>
        /// 加密URL传输的字符串
        /// </summary>
        /// <param name="QueryString"></param>
        /// <returns></returns>
        public static string EncryptQueryString(string QueryString)
        {
            return Encrypt(QueryString, const_QueryStringKey, const_QueryStringKey);
        }

        /// <summary>
        /// 解密URL传输的字符串
        /// </summary>
        /// <param name="QueryString"></param>
        /// <returns></returns>
        public static string DecryptQueryString(string QueryString)
        {
            return Decrypt(QueryString, const_QueryStringKey, const_QueryStringKey);
        }

        /// <summary>
        /// 加密帐号口令
        /// </summary>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static string EncryptPassWord(string PassWord)
        {
            return Encrypt(PassWord, const_PassWordKey, const_QueryStringKey);
        }

        /// <summary>
        /// 解密帐号口令
        /// </summary>
        /// <param name="PassWord"></param>
        /// <returns></returns>
        public static string DecryptPassWord(string PassWord)
        {
            return Decrypt(PassWord, const_PassWordKey, const_QueryStringKey);
        }
        #endregion

        #region DEC
        /// <summary>
        /// DEC 加密过程
        /// </summary>
        /// <param name="pToEncrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Encrypt(string content, string key,string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();  //把字符串放到byte数组中  

            byte[] inputByteArray = Encoding.Default.GetBytes(content);

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);  //建立加密对象的密钥和偏移量
            des.IV = ASCIIEncoding.ASCII.GetBytes(iv);   //原文使用ASCIIEncoding.ASCII方法的GetBytes方法 
            MemoryStream ms = new MemoryStream();     //使得输入密码必须输入英文文本
            CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write);

            cs.Write(inputByteArray, 0, inputByteArray.Length);
            cs.FlushFinalBlock();

            StringBuilder ret = new StringBuilder();
            foreach (byte b in ms.ToArray())
                ret.AppendFormat("{0:X2}", b);

            ret.ToString();
            return ret.ToString();
        }

        /// <summary>
        /// DEC 解密过程
        /// </summary>
        /// <param name="pToDecrypt"></param>
        /// <param name="sKey"></param>
        /// <returns></returns>
        public static string Decrypt(string content, string key, string iv)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();

            byte[] inputByteArray = new byte[content.Length / 2];
            for (int x = 0; x < content.Length / 2; x++)
            {
                int i = (Convert.ToInt32(content.Substring(x * 2, 2), 16));
                inputByteArray[x] = (byte)i;
            }

            des.Key = ASCIIEncoding.ASCII.GetBytes(key);  //建立加密对象的密钥和偏移量，此值重要，不能修改  
            des.IV = ASCIIEncoding.ASCII.GetBytes(iv);
            string result = null;
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    try
                    {
                        cs.Write(inputByteArray, 0, inputByteArray.Length);
                        cs.FlushFinalBlock();
                        result = Encoding.Default.GetString(ms.ToArray());
                    }
                    catch {}
                }
            }
            return result;
        }
        #endregion

        /// <summary>
        /// 检查己加密的字符串是否与原文相同
        /// </summary>
        /// <param name="EnString"></param>
        /// <param name="FoString"></param>
        /// <param name="Mode"></param>
        /// <returns></returns>
        public static bool ValidateString(string EnString, string FoString, int Mode)
        {
            switch (Mode)
            {
                default:
                case 1:
                    return Decrypt(EnString, const_QueryStringKey, const_QueryStringKey) == FoString.ToString();
                case 2:
                    return Decrypt(EnString, const_PassWordKey, const_QueryStringKey) == FoString.ToString();
            }
        }
    }
}


