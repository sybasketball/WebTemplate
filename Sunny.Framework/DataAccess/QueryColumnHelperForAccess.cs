using System;
using System.Collections.Generic;
using System.Text;
using NBearLite;
using System.Data;

namespace DataAccess
{
    public enum EFormartType
	{
	    Date,
        /// <summary>
        /// 带金钱符号的货币
        /// </summary>
        SignCurrency,
        Currency,
        /// <summary>
        /// 两位小数
        /// </summary>
        Fixed,
        Percent,
	}

    /// <summary>
    /// 作者：张虎
    /// 时间：2010-04-19
    /// 功能：QueryColumnHelper处理查询列
    /// 修改记录：1.2010-04-19 加入Access相关操作；
    /// </summary>
    public partial class QueryColumnHelper
    {
        const string AccFormat = "format({0}, '{1}') AS [{2}]";

        public static QueryColumn GetFormartColumns(QueryColumn column, EFormartType type)
        {
            string strFormat = null;
            switch (type)
            {
                case EFormartType.Date:
                    strFormat = "yyyy-mm-dd";
                    break;
                case EFormartType.SignCurrency:
                    strFormat = "Currency";
                    break;
                case EFormartType.Currency:
                    strFormat = "Standard";
                    break;
                case EFormartType.Fixed:
                    strFormat = "Fixed";
                    break;
                case EFormartType.Percent:
                    strFormat = "Percent";
                    break;
            }

            return GetFormartColumns(column, strFormat);
        }

        /// <summary>
        /// 获取格式化的QueryColumn
        /// </summary>
        /// <param name="column"></param>
        /// <param name="formatString">Access的Formart,可以使用,'000.00'控制数字格式,'00.00%'输出百分比等等</param>
        /// <returns></returns>
        public static QueryColumn GetFormartColumns(QueryColumn column, string formatString)
        {
            return GetFormartColumns(column.ColumnName, formatString, column.ColumnName);
        }

        /// <summary>
        /// 获取格式化的QueryColumn
        /// </summary>
        /// <param name="column"></param>
        /// <param name="formatString">Access的Formart</param>
        /// <param name="aliasName">别名</param>
        /// <returns></returns>
        public static QueryColumn GetFormartColumns(QueryColumn column, string formatString, string aliasName)
        {
            return GetFormartColumns(column.ColumnName, formatString, aliasName);
        }

        /// <summary>
        /// 获取格式化的QueryColumn
        /// </summary>
        /// <param name="columnName"></param>
        /// <param name="formatString"></param>
        /// <param name="aliasName"></param>
        /// <returns></returns>
        public static QueryColumn GetFormartColumns(string columnName, string formatString, string aliasName)
        {
            return new QueryColumn(string.Format(AccFormat, columnName,formatString, aliasName), DbType.String);
        }
    }
}
