using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace CommonUtility.Serialization
{
    /// <summary>
    /// XML序列化工具包
    /// </summary>
    public class XMLSerialization<T>
    {
        /// <summary>
        /// 将对象序列化成XML字符串
        /// </summary>
        /// <param name="obj">源对象</param>
        /// <returns>XML字符串</returns>
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
        /// 将XML字符串反序列化为对象
        /// </summary>
        /// <param name="s">XML字符串</param>      
        /// <returns>对象</returns>
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
