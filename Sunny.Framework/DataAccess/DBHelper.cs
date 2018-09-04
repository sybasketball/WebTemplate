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
    /// ʱ�䣺2010-04-12
    /// ���ܣ����ݲ��������ࣨ����NBearListe��
    /// �޸ļ�¼��1.2010-04-12  ��ʼ�����ݲ�������DB��
    ///           2.2010-04-13  �����ѯ���ܣ�
    /// </summary>
    public class DBHelper
    {
        public static EDatabase DB = new EDatabase();

        #region ��ѯ����
        /// <summary>
        /// ��ȡ��ҳ��ʼ��λ��
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public static int GetSkipCout(int pageSize, int pageIndex)
        {
            return pageSize * (pageIndex - 1);
        }
        /// <summary>
        /// ��ȡ�ܵ���������
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int GetTotalRecord(IQueryTable table, QueryColumn column, WhereClip where)
        {
            return Convert.ToInt32(DBHelper.DB.Select(table, column.Count()).Where(where).ToScalar());
        }

        /// <summary>
        /// ��ȡ�ܵ���������
        /// </summary>
        /// <param name="DB"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static int GetTotalRecord(EDatabase db, IQueryTable table, QueryColumn column, WhereClip where)
        {
            return Convert.ToInt32(db.Select(table,column.Count()).Where(where).ToScalar());
        }

        /// <summary>
        /// ��ȡ�ܼ�¼����
        /// </summary>
        /// <param name="column">����</param>
        /// <returns></returns>
        public static int GetTotalRecord(IQueryTable table, WhereClip where, List<JoinTable> joinTables)
        {
            return GetTotalRecord(DBHelper.DB,table, where, joinTables);
        }

        /// <summary>
        /// ��ȡ�ܼ�¼����
        /// </summary>
        /// <param name="column">����</param>
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

        #region ɾ������
        #region ��������idɾ��
        public static bool DeleteByID(IQueryTable table, string id)
        {
            return DeleteByID(DBHelper.DB,table,id);
        }

        public static bool DeleteByID(EDatabase db, IQueryTable table, string id)
        {
            return db.Delete(table).Where(table.IDColumn == id).Execute() == 1;
        }
        #endregion

        #region ��������id������ɾ��
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

        #region ��������ɾ��
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

        #region ��������ִ��
        /// <summary>
        /// ����IOperateSection��Excuteִ��
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
        /// ��IOperateSectionת��ΪSql���ִ��
        /// </summary>
        /// <param name="db">���ݿ����</param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static int BatchExecute(EDatabase db, params IOperateSection[] sections)
        {
            int nReturn = 0;
            int nMaxSqlLenth = 8000 - 1;//����ִ��Sql���ĳ���
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
        /// ��IOperateSectionת��ΪSql���ִ��
        /// </summary>
        /// <param name="db">���ݿ����</param>
        /// <param name="sections"></param>
        /// <returns></returns>
        public static int BatchExecute(EDatabase db, params string[] sqls)
        {
            int nReturn = 0;
            int nMaxSqlLenth = 8000 - 1;//����ִ��Sql���ĳ���
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
