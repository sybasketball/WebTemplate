using System;
using System.Collections.Generic;
using System.Text;
using NBearLite;
using System.Data;
using System.Transactions;

namespace DataAccess
{
    //public enum EJoinType
    //{
    //    Join,
    //    LeftJoin,
    //    RightJoin,
    //}

    /// <summary>
    /// 时间：2010-04-13
    /// 功能：基础的数据操作类
    /// 修改记录：1.2010-04-13  加入与NBearLite的表类的关联；加入EDatabase；
    ///           2.2010-04-14  加入查询列、查询条件、排序；
    /// </summary>
    public partial class SearchHelper
    {
        #region 构造器
        public SearchHelper(IQueryTable table)
        {
            this._Table = table;
            this._DB = DBHelper.DB;
        }

        public SearchHelper(IQueryTable table, EDatabase db)
        {
            this._Table = table;
            this._DB = db;
        }
        #endregion

        #region 属性
        protected IQueryTable _Table;

        protected EDatabase _DB;

        private ExpressionClip[] _ExtendColumns { get; set; }
        #endregion

        #region 条件
        private SelectSqlSection _SelectSection;

        public WhereClip _Where;
        /// <summary>
        /// Where语句,默认为Add
        /// </summary>
        /// <param name="where"></param>
        /// <returns>WhereClip</returns>
        public SearchHelper Where(WhereClip where)
        {
            if (WhereClip.IsNullOrEmpty(this._Where))
                this._Where = where;
            else
                this._Where.And(where);
            return this;
        }
        /// <summary>
        /// Where语句,使用Or连接
        /// </summary>
        /// <param name="where"></param>
        /// <returns>WhereClip</returns>
        public SearchHelper OrWhere(WhereClip where)
        {
            if (WhereClip.IsNullOrEmpty(this._Where))
                this._Where = where;
            else
                this._Where.Or(where);
            return this;
        }

        public OrderByClip[] _OrderBys;
        /// <summary>
        /// 加入查询条件
        /// </summary>
        /// <param name="orderBys"></param>
        /// <returns></returns>
        public SearchHelper Order(params OrderByClip[] orderBys)
        {
            this._OrderBys = orderBys;
            return this;
        }

        private StringBuilder sbOrder;
        /// <summary>
        /// 加入查询条件
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public void AddOrderByClip(string order)
        {
            if (!string.IsNullOrEmpty(order))
            {
                if (sbOrder == null)
                    sbOrder = new StringBuilder();

                string[] straOrders = order.Split(',');
                foreach (string orderclip in straOrders)
                {
                    if (orderclip.IndexOf(".") < 0)//加入表名
                        sbOrder.Append(",").Append(this._Table.GetTableName()).Append('.').Append(orderclip);
                    else
                        sbOrder.Append(",").Append(orderclip);
                }
            }
        }
        #endregion

        #region 关联查询
        /// <summary>
        /// 添加关联查询
        /// </summary>
        /// <param name="joinTable">关联表</param>
        /// <returns>ModalAdapter</returns>
        public SearchHelper Join(JoinTable joinTable)
        {
            if (this._SelectSection == null)
                this.Select(joinTable.QueryCulumns);
            joinTable.Join(this._SelectSection);
            this.JoinTables.Add(joinTable);
            return this;
        }

        /// <summary>
        /// 添加关联查询
        /// </summary>
        /// <param name="queryTable">关联表</param>
        /// <param name="joinTableAliasName">关联表别名</param>
        /// <param name="joinWhere">关联条件</param>
        /// <param name="joinType">关联类型</param>
        /// <returns>ModalAdapter</returns>
        public SearchHelper Join(IQueryTable queryTable, string joinTableAliasName, WhereClip joinWhere, EJoinType joinType)
        {
            switch (joinType)
            {
                case EJoinType.Join:
                    this._SelectSection.Join(queryTable, joinTableAliasName, joinWhere);
                    break;
                case EJoinType.LeftJoin:
                    this._SelectSection.LeftJoin(queryTable, joinTableAliasName, joinWhere);
                    break;
                case EJoinType.RightJoin:
                    this._SelectSection.RightJoin(queryTable, joinTableAliasName, joinWhere);
                    break;
            }
            this.JoinTables.Add(new JoinTable(queryTable, joinTableAliasName,joinWhere, joinType));
            return this;
        }

        /// <summary>
        /// 添加关联查询
        /// </summary>
        /// <param name="queryTable">关联表</param>
        /// <param name="joinWhere">关联条件</param>
        /// <param name="joinType">关联类型</param>
        /// <returns>ModalAdapter</returns>
        public SearchHelper Join(IQueryTable queryTable, WhereClip joinWhere, EJoinType joinType)
        {
            switch (joinType)
            {
                case EJoinType.Join:
                    this._SelectSection.Join(queryTable, joinWhere);
                    break;
                case EJoinType.LeftJoin:
                    this._SelectSection.LeftJoin(queryTable, joinWhere);
                    break;
                case EJoinType.RightJoin:
                    this._SelectSection.RightJoin(queryTable, joinWhere);
                    break;
            }
            this.JoinTables.Add(new JoinTable(queryTable, joinWhere, joinType));
            return this;
        }

        private List<JoinTable> _JoinTables;
        /// <summary>
        /// 关联查询表集合
        /// </summary>
        private List<JoinTable> JoinTables
        {
            get { if (this._JoinTables == null)this._JoinTables = new List<JoinTable>(); return this._JoinTables; }
        }
        #endregion

        #region 查询
        /// <summary>
        /// 初始化查询
        /// </summary>
        /// <param name="queryColumns">查询的列</param>
        /// <returns>ModalAdapter</returns>
        public SearchHelper Select(params ExpressionClip[] queryColumns)
        {
            this._SelectSection = this._DB.Select(this._Table,this._ExtendColumns,queryColumns);
            return this;
        }

        /// <summary>
        /// 设置扩展的查询字段，必须在select方法前面执行。主要用于预定义静态查询字段外的补充
        /// </summary>
        /// <param name="queryColumns">查询的列</param>
        /// <returns>ModalAdapter</returns>
        public SearchHelper SetExtendColumn(params ExpressionClip[] queryColumns)
        {
            this._ExtendColumns = queryColumns;
            return this;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public SelectSqlSection GetSection()
        {
            this._SelectSection.Where(this._Where);

            if (this._OrderBys != null)
                this._SelectSection.OrderBy(this._OrderBys);
            else if(this.sbOrder != null)
                this._SelectSection.OrderBy(this.sbOrder.ToString());

            return this._SelectSection;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public SelectSqlSection GetSection(int topNum)
        {
            return GetSection(0, topNum);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public SelectSqlSection GetSection(int startNum,int itemCount)
        {
            this._SelectSection.Where(this._Where);

            if (this._OrderBys != null)
                this._SelectSection.OrderBy(this._OrderBys);
            else if (this.sbOrder != null)
                this._SelectSection.OrderBy(this.sbOrder.Remove(0,1).ToString());

            return this._SelectSection.SetSelectRange(itemCount, startNum, this._Table.IDColumn);
        }

        /// <summary>
        /// 分页查询实体
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public SelectSqlSection GetSection(int pageSize, int pageIndex, ref int totalRecord)
        {
            totalRecord = DBHelper.GetTotalRecord(this._DB, this._Table,this._Where, this._JoinTables);
            return GetSection(DBHelper.GetSkipCout(pageSize, pageIndex), pageSize);
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public DataTable GetData()
        {
            return this.GetSection().ToDataTable();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public DataTable GetData(int topNum)
        {
            return this.GetSection(topNum).ToDataTable();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public DataTable GetData(int startNum, int itemCount)
        {
            return this.GetSection(startNum, itemCount).ToDataTable();
        }

        /// <summary>
        /// 分页查询实体
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public DataTable GetData(int pageSize, int pageIndex, ref int totalRecord)
        {
            totalRecord = DBHelper.GetTotalRecord(this._DB,this._Table, this._Where, this._JoinTables);
            return totalRecord > 0 ? GetSection(DBHelper.GetSkipCout(pageSize, pageIndex), pageSize).ToDataTable() : null;
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public T GetScalar<T>()
        {
            return this.GetSection().ToScalar<T>();
        }

        /// <summary>
        /// 查询实体
        /// </summary>
        /// <returns></returns>
        public object GetScalar()
        {
            return this.GetSection().ToScalar();
        }
        #endregion

        #region UpdateDatas
        /// <summary>
        /// 批量更新DataTable,(默认一次处理10条数据)
        /// </summary>
        /// <param name="dt">更新的DataTable</param>
        /// <returns></returns>
        public bool UpdateDatas(DataTable dt)
        {
            return UpdateDatas(dt, 10);
        }

        /// <summary>
        /// 批量更新DataTable
        /// </summary>
        /// <param name="dt">更新的DataTable</param>
        /// <param name="batchSize">一次处理的数据条数</param>
        /// <returns></returns>
        public bool UpdateDatas(DataTable dt, int batchSize)
        {
            if (this._SelectSection == null)
                this.GetSection();
            return this._DB.Save(this._SelectSection.ToDbCommand(), batchSize, dt) > 0;
        }
        #endregion
    }
}
