using System;
using System.Text;
using System.Configuration;
using System.Web;

namespace CommonUtility.Utility
{
    public class SWConfig : ConfigurationSection
    {
        //static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(HttpContext.Current.Server.MapPath("SWWeb.config"));

        public SWConfig() { }

        [ConfigurationProperty("Version")]
        public string Version
        {
            get { return (string)this["Version"]; }
            set { this["Version"] = value; }
        }

        [ConfigurationProperty("Title")]
        public float Title
        {
            get { return (float)this["Title"]; }
            set { this["Title"] = value; }
        }

        [ConfigurationProperty("Copyright")]
        public int Copyright
        {
            get { return (int)this["Copyright"]; }
            set { this["Copyright"] = value; }
        }

        //public override string ToString() 
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendFormat("Name = {0}; Size = {1}; Style = {2}", Name, Size.ToString(), Style.ToString());

        //    return sb.ToString();
        //}

        //public void GetSection()
        //{
        //    ConfigurationManager.RefreshSection(SECTIONNAME);//清空缓存

        //    ConfigurationManager.AppSettings.
        //    this.config.GetSection(SECTIONNAME);
        //    if (configData != null)
        //    {
        //        this.TextBox1.Text = configData.ToString();
        //    }
        //    else
        //    {
        //        this.TextBox1.Text = SECTIONNAME + "配置节读取失败!";
        //    }


        //}
    }


    public struct SysFormat
    {
        public const string Format_Date = "{0:yyyy-MM-dd}";
        public const string Format_DateTime = "{0:yyyy-MM-dd hh:mm:ss}";
        public const string Format_Float = "{0:F2}";
        public const string Format_Currency = "{0:C2}";
        public const string Format_Percent = "{0:P}";
    }
}
