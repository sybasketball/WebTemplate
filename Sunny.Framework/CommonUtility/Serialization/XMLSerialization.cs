using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CommonUtility.Serialization
{
    /// <summary>
    /// XML���л����߰�
    /// </summary>
    public class XMLSerialization<T>
    {
        /// <summary>
        /// ���������л���XML�ַ���
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <returns>XML�ַ���</returns>
        public static string ObjectToXmlString(T obj)
        {
            if (obj == null)
                return "";

            MemoryStream memStream = new MemoryStream();
            XmlSerializer packageSerializer = new XmlSerializer(typeof(T));
            packageSerializer.Serialize(memStream, obj);

            StreamReader sr = new StreamReader(memStream);
            sr.BaseStream.Position = 0;

            string s = sr.ReadToEnd();

            int i = s.Length;
            return s;
        }

        /// <summary>
        /// ��XML�ַ��������л�Ϊ����
        /// </summary>
        /// <param name="s">XML�ַ���</param>      
        /// <returns>����</returns>
        public static T XmlStringToObject(string s)
        {
            if (s == null || s.Length == 0)
                return default(T);

            MemoryStream memStream = new MemoryStream();

            StreamWriter sw = new StreamWriter(memStream);

            sw.Write(s);
            sw.Flush();
            memStream.Position = 0;

            XmlSerializer packageSerializer = new XmlSerializer(typeof(T));

            T obj = (T)packageSerializer.Deserialize(memStream);

            return obj;

        }
    }
}
