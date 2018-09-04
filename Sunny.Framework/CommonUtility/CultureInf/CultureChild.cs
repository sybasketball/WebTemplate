using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace CommonUtility.CultureInf
{
    /// <summary>
    /// 通常信息类（如:message,field）
    /// </summary>
    public class CMessageInfo : SWCultureInfo
    {
        private const string const_CacheKey = "{2692E9BE-6D85-4b98-98F6-76ACD1D12F44}";
        private const string const_XmlPath = "\\Common\\XML\\Message\\";
        private const string const_XPath = "/Msgs/Msg";

        public CMessageInfo() : base(const_CacheKey, const_XmlPath, const_XPath) { }
        public CMessageInfo(string cacheKey, string xmlInfPath, string xPath) : base(cacheKey, xmlInfPath, xPath) { }
    }

    /// <summary>
    /// 通常字段信息类
    /// </summary>
    public class CFieldInfo : SWCultureInfo
    {
        private const string const_CacheKey = "{8848622B-1516-4607-84A8-1877529CF5AC}";
        private const string const_XmlPath = "\\Common\\XML\\Field\\";
        private const string const_XPath = "/Msgs/Msg";
        public CFieldInfo(): base(const_CacheKey, const_XmlPath, const_XPath) { }
    }

    /// <summary>
    /// 枚举信息类
    /// </summary>
    public class CEnumInfo : SWCultureInfo
    {
        private const string const_CacheKey = "{4432BD71-8EF0-414c-A0F8-E967D4D2BB0A}";
        private const string const_XmlPath = "\\Common\\XML\\Enum\\";
        private const string const_XPath = "/Emnus/Emnu/EmnuValue";
        public CEnumInfo(): base(const_CacheKey, const_XmlPath, const_XPath) { }

        /// <summary>
        /// 重写获取xml的多语言信息
        /// </summary>
        /// <param name="xml"></param>
        /// <param name="xPath"></param>
        /// <returns></returns>
        protected override Dictionary<string, string> GetXmlMsg(System.Xml.XmlDocument xml, string xPath)
        {
            Dictionary<string, string> dicXmlMsg = new Dictionary<string, string>();
            XmlNodeList nodes = xml.SelectNodes(xPath);
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    XmlAttribute attr = node.Attributes["name"];
                    if (attr != null && !string.IsNullOrEmpty(attr.Value))
                        dicXmlMsg.Add(node.ParentNode.Attributes["name"].Value + "_" + attr.Value, node.InnerText);
                }
            }
            return dicXmlMsg;
        }

        /// <summary>
        /// 获取Enum的信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumName">枚举值的名称</param>
        /// <returns></returns>
        public string GetEnumText(Type enumType,string enumName)
        {
            if (!enumType.IsEnum)
                throw new InvalidOperationException();

            if (!string.IsNullOrEmpty(enumName))
            {
                string strText = this.GetInf(enumType.Name + "_" + enumName);
                return strText ?? enumName;
            }

            return string.Empty;
        }

        /// <summary>
        /// 获取Enum的信息
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="enumValue">枚举值</param>
        /// <returns></returns>
        public string GetEnumText(Type enumType, int enumValue)
        {
            string strName = Enum.GetName(enumType, enumValue);
            return GetEnumText(enumType, strName);
        }
    }
}
