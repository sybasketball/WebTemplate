using System;
using System.Collections.Generic;
using System.Text;
using NBearLite;

namespace DataAccess
{
    public enum EJoinType
    {
        Join,
        LeftJoin,
        RightJoin,
    }

    /// <summary>
    /// 关联查询表
    /// </summary>
    public class JoinTable
    {
        public JoinTable(IQueryTable joinQueryTable, WhereClip joinWhere, EJoinType joinType, params ExpressionClip[] queryCulumns)
        {
            this.JoinQueryTable = joinQueryTable;
            this.JoinWhere = joinWhere;
            this.JoinType = joinType;
            this.QueryCulumns = queryCulumns;
        }

        public JoinTable(IQueryTable joinQueryTable, string aliasName, WhereClip joinWhere, EJoinType joinType, params ExpressionClip[] queryCulumns)
            : this(joinQueryTable, joinWhere, joinType, queryCulumns)
        {
            this.AliasName = aliasName;
        }

        public ExpressionClip[] QueryCulumns { get; set; }

        /// <summary>
        /// 关联表
        /// </summary>
        public IQueryTable JoinQueryTable { get; set; }

        /// <summary>
        /// 关联表别名
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        /// 关联条件
        /// </summary>
        public WhereClip JoinWhere { get; set; }

        /// <summary>
        /// 关联类型
        /// </summary>
        public EJoinType JoinType { get; set; }

        public SelectSqlSection Join(SelectSqlSection sqlSectin)
        {
            bool IsAlias = !string.IsNullOrEmpty(this.AliasName);
            switch (this.JoinType)
            {
                case EJoinType.Join:
                    if (IsAlias)
                        sqlSectin.Join(this.JoinQueryTable, this.AliasName, this.JoinWhere);
                    else
                        sqlSectin.Join(this.JoinQueryTable, this.JoinWhere);
                    break;
                case EJoinType.LeftJoin:
                    if (IsAlias)
                        sqlSectin.LeftJoin(this.JoinQueryTable, this.AliasName, this.JoinWhere);
                    else
                        sqlSectin.LeftJoin(this.JoinQueryTable, this.JoinWhere);
                    break;
                case EJoinType.RightJoin:
                    if (IsAlias)
                        sqlSectin.RightJoin(this.JoinQueryTable, this.AliasName, this.JoinWhere);
                    else
                        sqlSectin.RightJoin(this.JoinQueryTable, this.JoinWhere);
                    break;
            }
            return sqlSectin;
        }
    }
}
