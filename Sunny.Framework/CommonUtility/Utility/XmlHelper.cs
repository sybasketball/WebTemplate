using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.IO;
using System.Web;
using System.Web.Caching;
using System.Configuration;

namespace CommonUtility.Utility
{
    public class XmlHelper
    {
        private static bool isCacheXml = ConfigurationManager.AppSettings["CacheXml"] == "true";

        public static XmlDocument loadXml(string cacheName, string path)
        {
            XmlDocument xml = HttpContext.Current.Cache[cacheName] as XmlDocument;
            if (xml != null)
                return xml;
            else
            {

                xml = new XmlDocument();
                string strPath = AppDomain.CurrentDomain.BaseDirectory + path;
                using (StreamReader stream = new StreamReader(AppDomain.CurrentDomain.BaseDirectory + path))
                    xml.Load(new XmlTextReader(stream));

                if (isCacheXml)//判断是否设置为缓存xml
                    HttpContext.Current.Cache.Insert(cacheName, xml, new CacheDependency(strPath));
                return xml;
            }
        }

        public static string GetNodeAttribute(XmlNode node, string name)
        {
            XmlAttribute attr = node.Attributes[name];
            if (attr != null)
                return attr.Value;
            return null;
        }
    }
}
