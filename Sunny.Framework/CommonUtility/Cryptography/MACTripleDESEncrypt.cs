using System;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.IO;
namespace CommonUtility.Cryptography
{
    /// <summary>
    /// ��������ǩ����hash��
    /// </summary>
    public class MACTripleDESEncrypt
    {
        private MACTripleDES mact;
        private string __key = "ksn168ch";
        private byte[] __data = null;
        /// <summary>
        /// ���캯��
        /// </summary>
        public MACTripleDESEncrypt()
        {
            mact = new MACTripleDES();
        }
        /// <summary>
        /// ��ȡ��������������ǩ������Կ
        /// </summary>
        public string Key
        {
            get { return this.__key; }
            set
            {
                int keyLength = value.Length;
                int[] keyAllowLengths = new int[] { 8, 16, 24 };
                bool isRight = false;
                foreach (int i in keyAllowLengths)
                {
                    if (keyLength == keyAllowLengths[i])
                    {
                        isRight = true;
                        break;
                    }
                }
                if (!isRight)
                    throw new ApplicationException("��������ǩ������Կ���ȱ�����8,16,24ֵ֮һ");
                else
                    this.__key = value;
            }
        }
        /// <summary>
        /// ��ȡ��������������ǩ�����û�����
        /// </summary>
        public byte[] Data
        {
            get { return this.__data; }
            set { this.__data = value; }
        }
        /// <summary>
        /// �õ�ǩ�����hashֵ
        /// </summary>
        /// <returns></returns>
        public string GetHashValue()
        {
            if (this.Data == null)
                throw new Exception("û������Ҫ��������ǩ�����û�" +
                    "����(property:Data)");
            byte[] key = Encoding.ASCII.GetBytes(this.Key);
            this.mact.Key = key;
            byte[] hash_b = this.mact.ComputeHash(this.mact.ComputeHash(this.Data));
            return Encoding.ASCII.GetString(hash_b);
        }
    }
}
