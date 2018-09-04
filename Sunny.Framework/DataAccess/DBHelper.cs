using System;
using NBearLite;
using CommonUtility.Utility;
using System.Collections.Generic;
using System.Data;
using System.Transactions;
using System.Text;

namespace DataAccess
{
    /// <summary>
    /// 时间：2010-04-12
    /// 功能：数据操作帮助类（基于NBearListe）
    /// 修改记录：1.2010-04-12  初始化数据操作对象DB；
    ///           2.2010-04-13  加入查询功能；
    /// </summary>
    public class DBHelper
    {
        public static EDatabase DB = new EDatabase();

        #region 查询方法
        /// <summary>
        /// 获取分页开始的位置
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static int GetSkipCout(int pageSize, int pageIndex)
        {
            return pageSize * (pageIndex - 1);
        }
        /// <summary>
        /// 获取总的数据条数
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int GetTotalRecord(IQueryTable table, QueryColumn column, WhereClip where)
        {
            return Convert.ToInt32(DBHelper.DB.Select(table, column.Count()).Where(where).ToScalar());
        }

        /// <summary>
        /// 获取总的数据条数
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int GetTotalRecord(EDatabase db, IQueryTable table, QueryColumn column, WhereClip where)
        {
            return Convert.ToInt32(db.Select(table,column.Count()).Where(where).ToScalar());
        }

        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <param name="column">主键</param>
        /// <returns></returns>
        public static int GetTotalRecord(IQueryTable table, WhereClip where, List<JoinTable> joinTables)
        {
            return GetTotalRecord(DBHelper.DB,table, where, joinTables);
        }

        /// <summary>
        /// 获取总记录条数
        /// </summary>
        /// <param name="column">主键</param>
        /// <returns></returns>
        public static int GetTotalRecord(EDatabase db, IQueryTable table, WhereClip where, List<JoinTable> joinTables)
        {
            SelectSqlSection sql = db.Select(table, table.IDColumn.Count());
            if (joinTables != null && joinTables.Count > 0)
            {
                foreach (JoinTable joinTable in joinTables)
                    joinTable.Join(sql);
            }
            object obj = sql.Where(where).ToScalar();
            if (obj != null)
                return Convert.ToInt32(obj);
            else
                return 0;
        }

        public static DataTable GetOneDataByID(IQueryTable table, object id)
        {
            if (id != null)
                return DBHelper.DB.Select(table).Where(table.IDColumn == id).ToDataTable();
            return null;
        }

        public static DataTable GetOneDataByID(EDatabase db, IQueryTable table, object id)
        {
            if (id != null)
                return db.Select(table).Where(table.IDColumn == id).ToDataTable();
            return null;
        }
        #endregion

        #region 删除方法
        #region 根据主键id删除
        public static bool DeleteByID(IQueryTable table, string id)
        {
            return DeleteByID(DBHelper.DB,table,id);
        }

        public static bool DeleteByID(EDatabase db, IQueryTable table, string id)
        {
            return db.Delete(table).Where(table.IDColumn == id).Execute() == 1;
        }
        #endregion

        #region 根据主键id串批量删除
        public static int DeleteByIDs(IQueryTable table, string ids)
        {
            return DeleteByIDs(DBHelper.DB, table, ids, false);
        }

        public static int DeleteByIDs(IQueryTable table, string ids, bool withTran)
        {
            return DeleteByIDs(DBHelper.DB, table, ids, withTran);
        }

        public static int DeleteByIDs(EDatabase db, IQueryTable table, string ids)
        {
            return DeleteByIDs(db, table, ids, false);
        }

        public static int DeleteByIDs(EDatabase db, IQueryTable table, string ids, bool withTran)
        {
            return DeleteByWhere(db, table, table.IDColumn.In(ids.Split(',')), withTran);
        }
        #endregion

        #region 根据条件删除
        public static int DeleteByWhere(IQueryTable table, WhereClip where, bool withTran)
        {
            return DeleteByWhere(DBHelper.DB, table, where, withTran);
        }

        public static int DeleteByWhere(EDatabase db,IQueryTable table, WhereClip where, bool withTran)
        {
            DeleteSqlSection delSection = db.Delete(table).Where(where);
            return withTran ? delSection.ExecuteWithTran() : delSection.Execute();
        }
        #endregion
        #endregion

        #region 事务批量执行
        /// <summary>
        /// 根据IOperateSection的Excute执行
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static int BatchExecute(params IOperateSection[] sections)
        {
            int nReturn = 0;
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                for (int i = 0; i < sections.Length; i++)
                {
                    nReturn += sections[i].Execute();
                }
                scope.Complete();
            }
            return nReturn;
        }

        /// <summary>
        /// 将IOperateSection转换为Sql语句执行
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static int BatchExecute(EDatabase db, params IOperateSection[] sections)
        {
            int nReturn = 0;
            int nMaxSqlLenth = 8000 - 1;//最大的执行Sql语句的长度
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                StringBuilder sbSql = new StringBuilder();
                for (int i = 0; i < sections.Length; i++)
                {
                    string strSql = sections[i].ToDbCommandText();

                    if (sbSql.Length + strSql.Length < nMaxSqlLenth)
                        sbSql.Append(strSql).Append(";");
                    else
                    {
                        nReturn += db.ExecuteNonQuery(sbSql.ToString());
                        sbSql.Remove(0, sbSql.Length);
                    }
                }
            }
            return nReturn;
        }

        /// <summary>
        /// 将IOperateSection转换为Sql语句执行
        /// </summary>
        /// <param name="db">数据库对象</param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static int BatchExecute(EDatabase db, params string[] sqls)
        {
            int nReturn = 0;
            int nMaxSqlLenth = 8000 - 1;//最大的执行Sql语句的长度
            using (TransactionScope scope = new TransactionScope(TransactionScopeOption.Required))
            {
                StringBuilder sbSql = new StringBuilder();
                for (int i = 0; i < sqls.Length; i++)
                {
                    string strSql = sqls[i];

                    if (sbSql.Length + strSql.Length < nMaxSqlLenth)
                        sbSql.Append(strSql).Append(";");
                    else
                    {
                        nReturn += db.ExecuteNonQuery(sbSql.ToString());
                        sbSql.Remove(0, sbSql.Length);
                    }
                }
            }
            return nReturn;
        }
        #endregion
    }
}
