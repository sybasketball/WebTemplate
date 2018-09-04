using System;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Data;


namespace CommonUtility.Utility
{
    /// <summary>
    /// 日志记录的源
    /// </summary>
    public enum ELogType
    {
        /// <summary>
        /// 数据交互层
        /// </summary>
        Data,
        /// <summary>
        /// 业务处理层
        /// </summary>
        BR,
        /// <summary>
        /// UI层
        /// </summary>
        UI,
        /// <summary>
        /// 底层框架层
        /// </summary>
        DevFramework
    }

    /// <summary>
    /// 优先级
    /// </summary>
    public enum ELogPriority
    {
        /// <summary>
        /// 跟踪
        /// </summary>
        Trace,
        /// <summary>
        /// 调试
        /// </summary>
        Debug,
        /// <summary>
        /// 信息
        /// </summary>
        Inf,
        /// <summary>
        /// 警告
        /// </summary>
        Warn,
        /// <summary>
        /// 错误
        /// </summary>
        Error,
        /// <summary>
        /// 灾难性错误
        /// </summary>
        Fatal,
    }

    /// <summary>
    /// 公共帮助类 - 日志处理
    /// </summary>
    /// <remarks>
    /// 日志格式：日志时间,日志类别,【日志所在层__Namespace】："错误描述",Error Message
    ///           2010-05-14,Fatal,【Data__ORM.SaveEntity】："实体保存错误",格式转化出错;
    /// </remarks>
    public class SysLogging
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private const string const_Error = "【{0}__{1}】：\"{2}\",{3}";

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logType">日志类型</param> 
        /// <param name="priority">日志级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">错误消息</param>
        public static void SaveLog(ELogType logType, ELogPriority priority, string message)
        {
            SaveLog(logType, priority, null, message, null);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logType">日志类型</param> 
        /// <param name="priority">日志级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">错误消息</param>
        public static void SaveLog(ELogType logType, ELogPriority priority, string nameSpace, string message)
        {
            SaveLog(logType, priority, nameSpace, message, null);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logType">日志类型</param> 
        /// <param name="priority">日志级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">错误消息</param>
        public static void SaveLog(ELogType logType, ELogPriority priority,Exception ex)
        {
            SaveLog(logType, priority,null,ex.Message, ex.StackTrace);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logType">日志类型</param> 
        /// <param name="priority">日志级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">错误消息</param>
        public static void SaveLog(ELogType logType, ELogPriority priority, string nameSpace, Exception ex)
        {
            SaveLog(logType, priority, nameSpace, ex.Message, ex.StackTrace);
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logType">日志类型</param> 
        /// <param name="priority">日志级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">错误消息</param>
        public static void SaveLog(ELogType logType, ELogPriority priority, string nameSpace, string message, string desc)
        {
            switch (priority)
            {
                case ELogPriority.Trace:
                    logger.Trace(const_Error, logType.ToString(), nameSpace, message, desc);
                    break;
                case ELogPriority.Debug:
                    logger.Debug(const_Error, logType.ToString(), nameSpace, message, desc);
                    break;
                case ELogPriority.Inf:
                    logger.Info(const_Error, logType.ToString(), nameSpace, message, desc);
                    break;
                case ELogPriority.Warn:
                    logger.Warn(const_Error, logType.ToString(), nameSpace, message, desc);
                    break;
                case ELogPriority.Error:
                    logger.Error(const_Error, logType.ToString(), nameSpace, message, desc);
                    break;
                case ELogPriority.Fatal:
                    logger.Fatal(const_Error, logType.ToString(), nameSpace, message, desc);
                    break;
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="logType">日志类型</param> 
        /// <param name="priority">日志级别</param>
        /// <param name="ex">异常</param>
        /// <param name="message">错误消息</param>
        public static void SaveDBError(Exception ex, string desc)
        {
            logger.Fatal(const_Error, ELogType.Data.ToString(), "DataBase", ex.Message, ex.TargetSite + "{" + desc + "}");
        }

        //public void GetLogs()
        //{
        //    string[] allLines = File.ReadAllLines(@"E:\Temp\Emp.csv");



        //    var query = from line in allLines

        //                let data = line.Split(',')

        //                select new

        //                {

        //                    ID = data[0],

        //                    FirstName = data[1],

        //                    LastName = data[2]

        //                };

        //    foreach (var s in query)
        //    {

        //        Console.WriteLine("[{0}] {1} {2}", s.ID, s.FirstName, s.LastName);

        //    }
        //}

        //public DataTable GetLog()
        //{
        //    string strPath = "c:\\test.csv";
        //    DataSet ds = new DataSet();
        //    try
        //    {
        //        File f;
        //        if (File.Exists(strPath))
        //        {
        //            string ConStr = "Provider=Microsoft.Jet.OLEDB.4.0;Data   Source=" + "c:\\" + ";Extended   Properties=\"Text;HDR=No;FMT=Delimited\\\"";
        //            System.Data.OleDb.OleDbConnection conn = new System.Data.OleDb.OleDbConnection(ConStr);
        //            System.Data.OleDb.OleDbDataAdapter da = new System.Data.OleDb.OleDbDataAdapter("Select   *   from   " + "test.csv", conn);
        //            da.Fill(ds, "test.csv");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show(ex.ToString);
        //    }
        //    return ds;
        //}
    }
}
