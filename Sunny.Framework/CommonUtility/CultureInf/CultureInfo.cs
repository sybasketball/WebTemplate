using System;
using System.Collections.Generic;
using System.Xml;
using System.Web;
using System.IO;
using CommonUtility.IO;
using System.Web.Caching;
using System.Web.Configuration;
using System.Configuration;
using System.Text;

namespace CommonUtility.CultureInf
{
    public abstract class SWCultureInfo
    {
        #region 变量
        private string cacheKey;
        private string xmlInfPath;
        private string xPath;
        protected const string const_Key = "id";
        #endregion

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="ContextPath">配置文件目录路径</param>
        /// <param name="CacheKey">缓存名称</param>
        public SWCultureInfo(string cacheKey,string xmlInfPath, string xPath)
        {
            this.cacheKey = cacheKey;
            this.xmlInfPath = xmlInfPath;
            this.xPath = xPath;
            //this.InitCultureInfo();
        }
        #endregion

        #region 属性
        private Dictionary<string, Dictionary<string, string>> _allCultureInfos;
        private Dictionary<string, Dictionary<string, string>> allCultureInfos
        {
            get
            {
                if (this._allCultureInfos == null)
                    InitCultureInfo();
                return this._allCultureInfos;
            }
        }
        #endregion

        #region Init CultureInf
        /// <summary>
        /// 初始化多语言信息
        /// </summary>
        protected void InitCultureInfo()
        {
            this._allCultureInfos = HttpContext.Current.Cache[cacheKey] as Dictionary<string, Dictionary<string, string>>;
            if (this._allCultureInfos == null)
            {
                string strPath = AppDomain.CurrentDomain.BaseDirectory + xmlInfPath;
                this._allCultureInfos = new Dictionary<string, Dictionary<string, string>>();
                List<FileInfo> allMessageFiles = FileTools.GetFileInfoSet(strPath, "*.xml");
                foreach (FileInfo fi in allMessageFiles)
                {
                    XmlDocument xml = new XmlDocument();
                    using (StreamReader stream = new StreamReader(strPath + fi.Name))
                    {
                        xml.Load(new XmlTextReader(stream));
                    }
                    this._allCultureInfos.Add(fi.Name.Replace(".xml", string.Empty).ToLower(), GetXmlMsg(xml, xPath));
                }
                HttpContext.Current.Cache.Insert(cacheKey, allCultureInfos, new CacheDependency(strPath));
            }
        }

        /// <summary>
        /// 获取xml的多语言信息
        /// </summary>
        /// <param name="xml"></param>
        /// <returns></returns>
        protected virtual Dictionary<string, string> GetXmlMsg(XmlDocument xml, string xPath)
        {
            Dictionary<string, string> dicXmlMsg = new Dictionary<string, string>();
            XmlNodeList nodes = xml.SelectNodes(xPath);
            if (nodes != null)
            {
                foreach (XmlNode node in nodes)
                {
                    XmlAttribute attr = node.Attributes[const_Key];
                    if (attr != null && !string.IsNullOrEmpty(attr.Value))
                        dicXmlMsg.Add(attr.Value, node.InnerText);
                }
            }
            return dicXmlMsg;
        }
        #endregion

        #region Get Culture
        private const string CultureSeesionKey = "{59B85357-678C-4ed4-B87F-10B9326677D4}";
        /// <summary>
        /// 设置语言区域
        /// </summary>
        /// <param name="CultureName"></param>
        public static void SetCulture(string CultureName)
        {
            HttpContext.Current.Session[CultureSeesionKey] = CultureName;
        }
        /// <summary>
        /// 过去当前语言区域
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentCulture()
        {
            if (HttpContext.Current.Session[CultureSeesionKey] != null)
                return HttpContext.Current.Session[CultureSeesionKey].ToString();
            else
                return DefaultCulture;
        }

        private static string _DefaultCulture;
        /// <summary>
        /// 缺省的语言区域
        /// 在Webconfig的[system.web.globalization]中配置
        /// </summary>
        public static string DefaultCulture
        {
            get
            {
                if (string.IsNullOrEmpty(_DefaultCulture))
                {
                    GlobalizationSection gs = ConfigurationManager.GetSection("system.web/globalization") as GlobalizationSection;
                    _DefaultCulture = gs.Culture.ToLower();
                }
                return _DefaultCulture;
            }
        }
        /// <summary>
        /// 客户端的语言区域
        /// </summary>
        public string ClientCulture
        {
            get
            {
                return HttpContext.Current.Request.UserLanguages[0];
            }
        }
        #endregion

        #region Get Inf
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetInf(string key)
        {
            Dictionary<string, string> infs = GetInfDicByCulture(key);
            if (infs != null && infs.Count > 0 && infs.ContainsKey(key))
                return infs[key];
            return null;
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        /// <param name="key"></param>
        /// <param name="paras">格式化的参数组</param>
        /// <returns></returns>
        public string GetInf(string key, params string[] paras)
        {
            return string.Format(GetInf(key), paras); ;
        }

        /// <summary>
        /// 获取多个信息(以参数拼接)
        /// </summary>
        /// <param name="msgSplit">信息之间的拼接字符</param>
        /// <param name="key"></param> 
        /// <returns></returns>
        public string GetInfs(string msgSplit,params string[] keys)
        {
            StringBuilder sbInfs = new StringBuilder();
            string strInf = null;
            foreach (string key in keys)
            {
                strInf = GetInf(key);
                if (strInf.Length > 0)
                    sbInfs.Append(strInf).Append(msgSplit);
            }
            //剔除多余的拼接字符
            if (sbInfs.Length > 0)
                sbInfs.Remove(sbInfs.Length - msgSplit.Length - 1, msgSplit.Length);
            return sbInfs.ToString();
        }

        private Dictionary<string, string> GetInfDicByCulture(string key)
        {
            //多语言设置(可根据客户环境、也可进行版本切换)
             //if (this.AllCultureInfos != null && this.AllCultureInfos.Count > 0 && this.AllCultureInfos.ContainsKey(GetCurrentCulture()))

            //获取默认语言
            if (this.allCultureInfos != null && this.allCultureInfos.Count > 0 && this.allCultureInfos.ContainsKey(DefaultCulture))
                return this.allCultureInfos[DefaultCulture];

            return null;
        }
        #endregion
    }
}
