using System;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
using CommonUtility.Utility;

namespace CommonUtility.Cryptography
{
    /// <summary>
    /// DES�ӽ�����
    /// </summary>
    public class DESEncrypt
    {
        #region ˽���ֶ�
        private string iv = "abcdefgh"; //˽Կ
        private string key = "hgfedcba";
        private Encoding encoding = new UnicodeEncoding();
        private DES des;
        #endregion

        /// <summary>
        /// ���캯��
        /// </summary>
        public DESEncrypt()
        {
            des = new DESCryptoServiceProvider();
        }

        #region ����
        /// <summary>
        /// ���ü�����Կ
        /// </summary>
        public string EncryptKey
        {
            get { return this.key; }
            set
            {
                this.key = value;
            }
        }
        /// <summary>
        /// Ҫ�����ַ��ı���ģʽ
        /// </summary>
        public Encoding EncodingMode
        {
            get { return this.encoding; }
            set { this.encoding = value; }
        }
        #endregion

        #region ����

        /// <summary>
        /// Encrypt string
        /// </summary>
        /// <param name="strContent">string to be encrypted</param>
        /// <returns>string encrypted</returns>
        public string EncryptString(string strContent)
        {
            string result = string.Empty;
            byte[] byKey = null;
            byte[] byIV = null;
            byte[] inputByteArray = null;
            MemoryStream ms = new MemoryStream();
            CryptoStream cs = null;
            try
            {
                byKey = System.Text.Encoding.UTF8.GetBytes(this.key.Substring(0, 8));
                byIV = System.Text.Encoding.UTF8.GetBytes(this.iv);
                inputByteArray = Encoding.UTF8.GetBytes(strContent);
                //MemoryStream ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(byKey, byIV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                cs.Close();
                ms.Close();
                result = Convert.ToBase64String(ms.ToArray());
            }
            catch
            {
                if (cs != null)
                    cs.Close();
                if (ms != null)
                    ms.Close();

                result = null;
                this.Logging("DES���ܳ���", "EncryptString",strContent);
            }
            return result;
        }

        /// <summary>
        /// �򵥼����ַ���
        /// </summary>
        /// <param name="strContent">��Ҫ���ܵ��ַ�������</param>
        /// <returns>���ܺ������</returns>
        public string EncryptString_AsicII(string strContent)
        {
            string result = string.Empty;
            try
            {
                byte[] bytes = System.Text.UTF8Encoding.ASCII.GetBytes(strContent);
                for (int i = 0; i < bytes.Length; i++)
                {
                    result += Convert.ToString(bytes[i], 16).PadLeft(2, '0');
                }
            }
            catch
            {
                result = null;
                this.Logging( "AsicII���ܳ���","EncryptString_AsicII",strContent);
            }
            return result;
        }

        /// <summary>
        /// �򵥽����ַ���
        /// </summary>
        /// <param name="strContent">��Ҫ���ܵ��ַ�������</param>
        /// <returns>���ܺ������</returns>
        public string DecryptString_AsicII(string strContent)
        {
            string result = string.Empty;

            try
            {
                string tmpStr = string.Empty;
                int cnt = 0;

                for (int i = 0; i < strContent.Length; i++)
                {
                    cnt++;
                    tmpStr += strContent[i];
                    if (cnt % 2 == 0)
                    {
                        byte by = Convert.ToByte(tmpStr, 16);
                        byte[] bb = new byte[1];
                        bb[0] = by;
                        result += System.Text.UTF8Encoding.ASCII.GetString(bb);
                        cnt = 0;
                        tmpStr = string.Empty;
                    }
                }
            }
            catch
            {
                this.Logging("AsicII���ܳ���", "DecryptString_AsicII", strContent);
                result = null;
            }
            return result;

        }

        /// <summary>
        /// �����ַ���
        /// </summary>
        /// <param name="str">��Ҫ���ܵ��ַ���</param>
        /// <returns>���ܺ�2��������</returns>
        public byte[] EncryptStringReturnBytes(string str)
        {
            byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
            byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);//�õ�������Կ
            byte[] toEncrypt = this.EncodingMode.GetBytes(str);//�õ�Ҫ���ܵ�����
            byte[] encrypted;
            ICryptoTransform encryptor = des.CreateEncryptor(keyb, ivb);
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write);
            csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
            csEncrypt.FlushFinalBlock();
            encrypted = msEncrypt.ToArray();
            csEncrypt.Close();
            msEncrypt.Close();
            return encrypted;
        }

        /// <summary>
        /// ����ָ�����ļ�,����ɹ�����True,����false
        /// </summary>
        /// <param name="filePath">Ҫ���ܵ��ļ�·��</param>
        /// <param name="outPath">���ܺ���ļ����·��</param>
        public void EncryptFile(string filePath, string outPath)
        {
            bool isExist = File.Exists(filePath);
            if (isExist)//�������
            {
                byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
                byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);
                //�õ�Ҫ�����ļ����ֽ���
                FileStream fin = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(fin, this.EncodingMode);
                string dataStr = reader.ReadToEnd();
                byte[] toEncrypt = this.EncodingMode.GetBytes(dataStr);
                fin.Close();

                FileStream fout = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                ICryptoTransform encryptor = des.CreateEncryptor(keyb, ivb);
                CryptoStream csEncrypt = new CryptoStream(fout, encryptor, CryptoStreamMode.Write);
                try
                {
                    //���ܵõ����ļ��ֽ���
                    csEncrypt.Write(toEncrypt, 0, toEncrypt.Length);
                    csEncrypt.FlushFinalBlock();
                }
                catch
                {
                    this.Logging("File���ܳ���", "EncryptFile", filePath + "\",\"" + outPath);
                    //throw new ApplicationException(err.Message);
                }
                finally
                {
                    try
                    {
                        fout.Close();
                        csEncrypt.Close();
                    }
                    catch
                    {
                        ;
                    }
                }
            }
            else
            {
                throw new FileNotFoundException("File not found!");
            }
        }
        /// <summary>
        /// �ļ����ܺ��������ذ汾,�����ָ�����·��,
        /// ��ôԭ�����ļ��������ܺ���ļ�����
        /// </summary>
        /// <param name="filePath"></param>
        public void EncryptFile(string filePath)
        {
            this.EncryptFile(filePath, filePath);
        }
        /// <summary>
        /// Decrypt string
        /// </summary>
        /// <param name="strContent">string to be decrypted</param>
        /// <returns>string decrypted</returns>
        public string DecryptString(string strContent)
        {
            string result = null;
            if (!string.IsNullOrEmpty(strContent))
            {
                byte[] byKey = null;
                byte[] byIV = null;
                byte[] inputByteArray = new Byte[strContent.Length];
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = null;
                try
                {
                    byKey = System.Text.Encoding.UTF8.GetBytes(this.key.Substring(0, 8));
                    byIV = System.Text.Encoding.UTF8.GetBytes(this.iv);
                    DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                    inputByteArray = Convert.FromBase64String(strContent);
                    cs = new CryptoStream(ms, des.CreateDecryptor(byKey, byIV), CryptoStreamMode.Write);
                    cs.Write(inputByteArray, 0, inputByteArray.Length);
                    cs.FlushFinalBlock();
                    cs.Close();
                    ms.Close();
                    System.Text.Encoding encoding = new System.Text.UTF8Encoding();
                    result = encoding.GetString(ms.ToArray());
                }
                catch
                {
                    if (cs != null)
                        cs.Close();
                    if (ms != null)
                        ms.Close();
                    //throw error;
                    result = null;
                    this.Logging("DES���ܳ���", "DecryptString", strContent);
                }
            }
            return result;
        }
        /// <summary>
        /// Decrypt Binary
        /// </summary>
        /// <param name="toDecrypt">Binary  to be decrypted</param>
        /// <returns>string decrypted</returns>
        public string DecryptString(byte[] toDecrypt)
        {
            byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
            byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);
            //byte[] toDecrypt=this.EncodingMode.GetBytes(str);
            byte[] deCrypted = new byte[toDecrypt.Length];
            ICryptoTransform deCryptor = des.CreateDecryptor(keyb, ivb);
            MemoryStream msDecrypt = new MemoryStream(toDecrypt);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, deCryptor, CryptoStreamMode.Read);
            try
            {
                csDecrypt.Read(deCrypted, 0, deCrypted.Length);
            }
            catch
            {
                this.Logging("DES���ܳ���", "DecryptString", toDecrypt.ToString());
                //throw new ApplicationException(err.Message);
            }
            finally
            {
                try
                {
                    msDecrypt.Close();
                    csDecrypt.Close();
                }
                catch { ;}
            }
            return this.EncodingMode.GetString(deCrypted);

        }

        /// <summary>
        /// ����ָ�����ļ�
        /// </summary>
        /// <param name="filePath">Ҫ���ܵ��ļ�·��</param>
        /// <param name="outPath">���ܺ���ļ����·��</param>
        public void DecryptFile(string filePath, string outPath)
        {
            bool isExist = File.Exists(filePath);
            if (isExist)//�������
            {
                byte[] ivb = Encoding.ASCII.GetBytes(this.iv);
                byte[] keyb = Encoding.ASCII.GetBytes(this.EncryptKey);
                FileInfo file = new FileInfo(filePath);
                byte[] deCrypted = new byte[file.Length];
                //�õ�Ҫ�����ļ����ֽ���
                FileStream fin = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                //�����ļ�
                try
                {
                    ICryptoTransform decryptor = des.CreateDecryptor(keyb, ivb);
                    CryptoStream csDecrypt = new CryptoStream(fin, decryptor, CryptoStreamMode.Read);
                    csDecrypt.Read(deCrypted, 0, deCrypted.Length);
                }
                catch
                {
                    this.Logging("File���ܳ���", "DecryptFile", filePath + "\",\"" + outPath);
                    //throw new ApplicationException(err.Message);
                }
                finally
                {
                    try
                    {
                        fin.Close();
                    }
                    catch { ;}
                }
                FileStream fout = new FileStream(outPath, FileMode.Create, FileAccess.Write);
                fout.Write(deCrypted, 0, deCrypted.Length);
                fout.Close();
            }
            else
            {
                throw new FileNotFoundException("File not found!");
            }
        }
        /// <summary>
        /// �����ļ������ذ汾,���û�и������ܺ��ļ������·��,
        /// ����ܺ���ļ���������ǰ���ļ�
        /// </summary>
        /// <param name="filePath"></param>
        public void DecryptFile(string filePath)
        {
            this.DecryptFile(filePath, filePath);
        }
        #endregion

        #region Logging
        public void Logging(string message,string method,string param)
        {
            SysLogging.SaveLog(ELogType.DevFramework, ELogPriority.Error, this.ToString(), message, method + "(\"" + param + "\")");
        }
        #endregion
    }

}
