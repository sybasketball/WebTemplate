using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace CommonUtility.Utility
{
    /// <summary>
    /// Html生成，按Head、Contents、Foot生成Html
    /// </summary>
    public class DataBuilder
    {
        public string format_Head;
        public string format_Foot;
        public string format_Content;

        public StringBuilder sbData;
        public StringBuilder sbItem;

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataBuilder()
        {
            sbData = new StringBuilder();
            sbItem = new StringBuilder();
        }

        /// <summary>
        /// 构造函数，设置要生成的Html的模板
        /// </summary>
        /// <param name="formatContent">要生成的Html的模板</param>
        public DataBuilder(string formatContent)
            : this()
        {
            this.format_Content = formatContent;
        }

        /// <summary>
        /// 构造函数，设置要生成的Html的模板
        /// </summary>
        /// <param name="formatHead">模板头部</param>
        /// <param name="formatContent">模板内容</param>
        /// <param name="formatFoot">模板底部</param>
        public DataBuilder(string formatHead, string formatContent, string formatFoot)
            : this(formatContent)
        {
            this.format_Head = formatHead;
            this.format_Content = formatContent;
            this.format_Foot = formatFoot;
        }
        #endregion

        #region Add Item
        /// <summary>
        /// 为模板内容填充数据
        /// </summary>
        /// <param name="items">要填充的数据</param>
        public void AddFormatContent(params object[] items)
        {
            sbData.AppendFormat(format_Content, items);
        }
        #endregion

        /// <summary>
        /// 获取生成的Html
        /// </summary>
        /// <returns></returns>
        public string GetFormatData()
        {
            if (!string.IsNullOrEmpty(format_Head))
                sbData.Insert(0, format_Head);

            if (!string.IsNullOrEmpty(format_Foot))
                sbData.Append(format_Foot);

            return sbData.ToString();
        }

        /// <summary>
        /// 获取生成的Html
        /// </summary>
        /// <param name="dt">要填充的数据</param>
        /// <returns></returns>
        public string GetFormatData(DataTable dt)
        {
            foreach (DataRow drw in dt.Rows)
            {
                object[] objaData = new object[dt.Columns.Count];
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    if (!drw.IsNull(i))
                        objaData[i] = drw[i];
                }
                this.AddFormatContent(objaData);
            }
            return GetFormatData();
        }
    }

    public class DataBuilderForCustItem : DataBuilder
    {
        public string format_ContentEnd;
        public string format_ItemLeft;
        public string format_ItemRight;

        #region Constructor
        /// <summary>
        /// 构造函数
        /// </summary>
        public DataBuilderForCustItem() : base()
        {
            sbData = new StringBuilder();
            sbItem = new StringBuilder();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatContent">模板内容头部</param>
        /// <param name="formatContentEnd">模板内容尾部</param>
        /// <param name="formatItemLeft">内容项头部</param>
        /// <param name="formatItemRight">内容项尾部</param>
        public DataBuilderForCustItem(string formatContent, string formatContentEnd, string formatItemLeft, string formatItemRight)
            : base(formatContent)
        {
            this.format_ContentEnd = formatContentEnd;
            this.format_ItemLeft = formatItemLeft;
            this.format_ItemRight = formatItemRight;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formatHead">模板头部</param>
        /// <param name="formatFoot">模板底部</param>
        /// <param name="formatContent">模板内容头部</param>
        /// <param name="formatContentEnd">模板内容尾部</param>
        /// <param name="formatItemLeft">内容项头部</param>
        /// <param name="formatItemRight">内容项尾部</param>
        public DataBuilderForCustItem(string formatHead, string formatFoot, string formatContent, string formatContentEnd, string formatItemLeft, string formatItemRight)
            : this(formatContent, formatContentEnd, formatItemLeft, formatItemRight)
        {
            this.format_Head = formatHead;
            this.format_Foot = formatFoot;
        }
        #endregion

        #region Sinle Add
        /// <summary>
        /// 生成项
        /// </summary>
        /// <param name="items">项的内容</param>
        /// <param name="fortmat">格式化模板</param>
        /// <returns></returns>
        public DataBuilderForCustItem AddItem(object items, string fortmat)
        {
            sbItem.Append(format_ItemLeft).AppendFormat(fortmat, items).Append(format_ItemRight);
            return this;
        }

        /// <summary>
        /// 生成项
        /// </summary>
        /// <param name="items">项的内容</param>
        /// <returns></returns>
        public DataBuilderForCustItem AddItem(object items)
        {
            sbItem.Append(format_ItemLeft).Append(items).Append(format_ItemRight);
            return this;
        }

        //public void AddItem(params object[] items)
        //{
        //    sbData.Append(format_Content).Append(format_ItemLeft).Append(items).Append(format_ItemRight).Append(format_ContentEnd);
        //}

        /// <summary>
        /// 生成模板内容
        /// </summary>
        public void EndSingleAdd()
        {
            sbData.Append(format_Content).Append(sbItem).Append(format_ContentEnd);
            sbItem.Remove(0, this.sbItem.Length);
        }
        #endregion
    }
}
