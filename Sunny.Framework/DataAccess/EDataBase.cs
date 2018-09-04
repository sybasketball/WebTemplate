using System;
using NBearLite;
using System.Data;
using System.Transactions;
using CommonUtility.Utility;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Data.Common;

namespace DataAccess
{
    /// <summary>
    /// 时间：2010-04-12
    /// 功能：数据操作帮助类（基于NBearListe）
    /// 修改记录：1.2010-04-12  重写NBearListe数据操作对象Database，重载基本操作功能
    ///           2.2010-05-13  加入Sql运行记录及异常日志功能
    /// </summary>
    public class EDatabase : Database
    {
        #region 属性
        public EDatabase() : this("DefaultConnString") { }

        public EDatabase(string connectStrName) : base(connectStrName) { this.InitLog(); }
        #endregion

        #region 实现自定义Sql处理
        /// <summary>
        /// 使用事务执行自定义Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            return this.ExecuteNonQuery(CommandType.Text, sql);
        }

        /// <summary>
        /// 使用事务执行自定义Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public int ExecuteNonQueryWithTran(string sql)
        {
            return this.ExecuteNonQueryWithTran(CommandType.Text,sql);
        }

        /// <summary>
        /// 使用事务执行自定义Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="db">数据库对象</param>
        /// <returns></returns>
        public int ExecuteNonQueryWithTran(CommandType type,string sql)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                int nReturn = this.ExecuteNonQuery(type,sql);
                scope.Complete();
                return nReturn;
            }
        }

        /// <summary>
        /// 使用事务执行自定义Sql
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="db">数据库对象</param>
        /// <returns></returns>
        public int ExecuteNonQueryWithTran(DbCommand cmd)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                int nReturn = cmd.ExecuteNonQuery();
                scope.Complete();
                return nReturn;
            }
        }

        /// <summary>
        /// 使用事务执行自定义Sql,返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalar(string sql)
        {
            return this.ExecuteScalar(CommandType.Text, sql);
        }

        /// <summary>
        /// 使用事务执行自定义Sql,返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public object ExecuteScalarWithTran(string sql)
        {
            return this.ExecuteScalarWithTran(CommandType.Text,sql);
        }

        /// <summary>
        /// 使用事务执行自定义Sql,返回首行首列
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="db">数据库对象</param>
        /// <returns></returns>
        public object ExecuteScalarWithTran(CommandType type,string sql)
        {
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                object objReturn = this.ExecuteScalar(type,sql);
                scope.Complete();
                return objReturn;
            }
        }

        /// <summary>
        /// 执行自定义Sql,返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string sql)
        {
            return this.ExecuteDataTable(CommandType.Text,sql);
        }

        /// <summary>
        /// 执行自定义Sql,返回DataTable
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="db">数据库对象</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(CommandType type,string sql)
        {
            DataSet ds = this.ExecuteDataSet(type,sql);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }

        /// <summary>
        /// 执行自定义Sql,返回DataTable
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(DbCommand cmd)
        {
            DataSet ds = this.ExecuteDataSet(cmd);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            return null;
        }
        #endregion

        #region 日志
        private static bool isSaveSqlLog = ConfigurationManager.AppSettings["SqlLog"] == "true";
        private static bool isSaveErrorLog = ConfigurationManager.AppSettings["ErrorLog"] == "true";

        private void InitLog()
        {
            if (isSaveSqlLog)
                this.OnLog += SaveSqlLog; //Sql日常记录
            if (isSaveErrorLog)
                this.OnErrorLog += SaveErrorLog;//错误记录
        }

        private static void SaveSqlLog(string sqlLog)
        {
            SysLogging.SaveLog(ELogType.Data, ELogPriority.Inf, sqlLog);
        }

        private static void SaveErrorLog(string msgLog, Exception ex)
        {
            SysLogging.SaveDBError(ex,msgLog);
        }
        #endregion

        #region 操作
        ///// <summary>
        ///// 根据主键ID删除
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public bool Delete(IQueryTable table,string id)
        //{
        //    return DBHelper.DeleteByID(this, table, id);
        //}

        ///// <summary>
        ///// 根据主键ID删除
        ///// </summary>
        ///// <param name="ID"></param>
        ///// <returns></returns>
        //public int DeleteDatasByID(IQueryTable table, string ids)
        //{
        //    return DBHelper.DeleteByIDs(this, table, ids);
        //}

        ///// <summary>
        ///// 根据条件删除
        ///// </summary>
        ///// <param name="where"></param>
        ///// <returns></returns>
        //public int Delete(IQueryTable table, WhereClip where)
        //{
        //    return this.Delete(table).Where(where).Execute();
        //}

        public DataTable GetOneDataByID(IQueryTable table, string id)
        {
            if (!string.IsNullOrEmpty(id))
                return this.Select(table).Where(table.IDColumn == id).ToDataTable();
            return null;
        }
        #endregion
    }
}
