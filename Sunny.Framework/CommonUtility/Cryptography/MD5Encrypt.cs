using System;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;

namespace CommonUtility.Cryptography
{
    /// <summary>
    /// MD5������,ע�⾭MD5���ܹ�����Ϣ�ǲ���ת����ԭʼ���ݵ�
    /// ,�벻Ҫ���û����е���Ϣ��ʹ�ô˼��ܼ���,�����û�������,
    /// �뾡��ʹ�öԳƼ���
    /// </summary>
    public class MD5Encrypt
    {
        private MD5 md5;
        /// <summary>
        /// ���캯��
        /// </summary>
        public MD5Encrypt()
        {
            md5 = new MD5CryptoServiceProvider();
        }
        /// <summary>
        /// ���ַ����л�ȡɢ��ֵ
        /// </summary>
        /// <param name="str">Ҫ����ɢ��ֵ���ַ���</param>
        /// <returns></returns>
        public string GetMD5FromString(string str)
        {
            byte[] toCompute = Encoding.Unicode.GetBytes(str);
            byte[] hashed = md5.ComputeHash(toCompute, 0, toCompute.Length);
            return Encoding.ASCII.GetString(hashed);   
        }
        /// <summary>
        /// �����ļ�������ɢ��ֵ
        /// </summary>
        /// <param name="filePath">Ҫ����ɢ��ֵ���ļ�·��</param>
        /// <returns></returns>
        public string GetMD5FromFile(string filePath)
        {
            bool isExist = File.Exists(filePath);
            if (isExist)//����ļ�����
            {
                FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                StreamReader reader = new StreamReader(stream, Encoding.Unicode);
                string str = reader.ReadToEnd();
                byte[] toHash = Encoding.Unicode.GetBytes(str);
                byte[] hashed = md5.ComputeHash(toHash, 0, toHash.Length);
                stream.Close();
                return Encoding.ASCII.GetString(hashed);
            }
            else//�ļ�������
            {
                throw new FileNotFoundException("File not found!");
            }
        }
    }
}
