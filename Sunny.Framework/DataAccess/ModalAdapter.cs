using System;
using System.Collections.Generic;
using System.Text;
using NBearLite;
using System.Data;
using System.Transactions;

namespace DataAccess
{
    public enum EJoinType
    {
        Join,
        LeftJoin,
        RightJoin,
    }

    /// <summary>
    /// 作者：张虎
    /// 时间：2010-04-13
    /// 功能：基础的数据实体类
    /// 修改记录：1.2010-04-13  加入与NBearLite的表类的关联；加入EDatabase；
    ///           2.2010-04-14  加入查询列、查询条件、排序；
    /// </summary>
    public partial class ModalAdapter<T> where T:BaseModal
    {
        #region 构造器
        public ModalAdapter(IQueryTable table)
        {
            this._DB = DBHelper.GetDB(table);
            this._tableName = this._DB.QueryTable.GetTableName();
        }

        public ModalAdapter(IQueryTable table, EDatabase db)
        {
            this._DB = db;
            this._DB.QueryTable = table;
            this._tableName = this._DB.QueryTable.GetTableName();
        }
        #endregion

        #region 属性
        protected EDatabase _DB;
        public virtual EDatabase DB
        {
            get{return _DB;}
        }

        private T _Modal;
        public virtual T Modal { get {return _Modal; } }

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
        public ModalAdapter<T> Where(WhereClip where)
        {
            if (WhereClip.IsNullOrEmpty(this._Where))
                this._Where = where;
            else
                this._Where.And(where);
            return this;
        }

        public OrderByClip[] _OrderBys;
        /// <summary>
        /// 加入查询条件
        /// </summary>
        /// <param name="orderBys"></param>
        /// <returns></returns>
        public ModalAdapter<T> Order(params OrderByClip[] orderBys)
        {
            this._OrderBys = orderBys;
            return this;
        }

        private string _tableName;

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
                        sbOrder.Append(",").Append(this._tableName).Append('.').Append(orderclip);
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
        public ModalAdapter<T> Join(JoinTable joinTable)
        {
            if (this._SelectSection == null)
                this.Select(joinTable.QueryColumns);
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
        public ModalAdapter<T> Join(IQueryTable queryTable, string joinTableAliasName, WhereClip joinWhere, EJoinType joinType)
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
        public ModalAdapter<T> Join(IQueryTable queryTable, WhereClip joinWhere, EJoinType joinType)
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
        public ModalAdapter<T> Select(params ExpressionClip[] queryColumns)
        {
            this._SelectSection = this._DB.Select(this._ExtendColumns,queryColumns);
            return this;
        }

        /// <summary>
        /// 设置扩展的查询字段，必须在select方法前面执行。主要用于预定义静态查询字段外的补充
        /// </summary>
        /// <param name="queryColumns">查询的列</param>
        /// <returns>ModalAdapter</returns>
        public ModalAdapter<T> SetExtendColumn(params ExpressionClip[] queryColumns)
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

            return this._SelectSection.SetSelectRange(itemCount, startNum, this._DB.QueryTable.IDColumn);
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
            totalRecord = DBHelper.GetTotalRecord(this._DB,this._DB.QueryTable.IDColumn,this._Where,this._JoinTables);
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
            totalRecord = DBHelper.GetTotalRecord(this._DB, this._DB.QueryTable.IDColumn, this._Where, this._JoinTables);
            return GetSection(DBHelper.GetSkipCout(pageSize, pageIndex), pageSize).ToDataTable();
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

        #region 填充实体
        /// <summary>
        /// Fill Modal With Filter
        /// </summary>
        public void FillModalWithFilter()
        {
            this._Modal = this._SelectSection.Where(this._Where).ToSingleObject<T>();
        }

        /// <summary>
        /// Fill Modal With Filter
        /// </summary>
        public void FillModal(WhereClip where, params ExpressionClip[] culumns)
        {
            this._Modal = this._DB.Select(culumns).Where(this._Where).ToSingleObject<T>();
        }

        /// <summary>
        /// Fill Modal
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="culumns">需要填充的列</param>
        public void FillModal(object ID, params ExpressionClip[] culumns)
        {
            this._Modal = this._DB.Select(culumns).Where(this._DB.QueryTable.IDColumn == ID).ToSingleObject<T>();
        }

        /// <summary>
        /// Fill Modal
        /// </summary>
        /// <param name="ID">主键</param>
        /// <param name="joinTable">关联表</param>
        /// <param name="culumns">需要填充的列</param>
        public void FillModal(object ID, JoinTable joinTable, params ExpressionClip[] culumns)
        {
            this._Modal = joinTable.Join(this._DB.Select(culumns.Length > 0 ? culumns : joinTable.QueryColumns)).Where(this._DB.QueryTable.IDColumn == ID).ToSingleObject<T>();
        }
        #endregion

        #region 删除
        /// <summary>
        /// 根据主键ID删除
        /// </summary>
        /// <param name="ID"></param>
        /// <returns></returns>
        public bool Delete(object ID)
        {
            return this._DB.Delete(this._DB.QueryTable).Where(this._DB.QueryTable.IDColumn == ID).Execute() == 1;
        }

        /// <summary>
        /// 根据条件删除
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public int Delete(WhereClip where)
        {
            return this._DB.Delete(this._DB.QueryTable).Where(where).Execute();
        }

        /// <summary>
        /// 根据创建的条件删除
        /// </summary>
        /// <returns></returns>
        public int DeleteWithFilter()
        {
            return this._DB.Delete(this._DB.QueryTable).Where(this._Where).Execute();
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
            return this._DB.Save(this._SelectSection.ToDbCommand(),batchSize, dt) > 0;
        }
        #endregion

        #region Dic实体
        //private Dictionary<string, object> _Fileds;
        //public Dictionary<string, object> Fileds
        //{
        //    get { return this._Fileds; }
        //    set { this._Fileds = value; }
        //}

        //public T 
        #endregion
    }
}
