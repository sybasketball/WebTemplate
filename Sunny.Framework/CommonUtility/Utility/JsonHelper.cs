using System;
using Newtonsoft.Json;
using System.Data;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace CommonUtility.Utility
{
    /// <summary>
    /// Json对象处理
    /// </summary>
    public class JsonHelper
    {
        #region Json对象处理
        /// <summary>
        /// 将数据进行Json序列化
        /// </summary>
        /// <param name="data">需要序列化的对象</param>
        /// <returns></returns>
        public static string SerializeData(object data)
        {
            return JsonConvert.SerializeObject(data);
        }

        /// <summary>
        /// 将json字串转换为T对象
        /// </summary>
        /// <typeparam name="T">转换的对象</typeparam>
        /// <param name="jsonString">json字串</param>
        /// <returns></returns>
        public static T DeserializeData<T>(string jsonString)
        {
            return JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static JObject GetJsonObject(string jsonString)
        {
           return JObject.Parse(jsonString);
        }

        public static string GetJsonObject(JObject jsnValues,string filedName)
        {
            JToken token = jsnValues[filedName];
            if (token != null && token.Type != JTokenType.Null)
                return Convert.ToString(token).Replace("\"", string.Empty);
            return string.Empty;
        }
        public static string MergerJsonObject(string jsonObjStr1, string jsonObjStr2)
        {
            Regex rex = new Regex("^{(.*)}$", RegexOptions.IgnoreCase);
            string str1 = rex.Replace(jsonObjStr1, "$1").Trim();
            string str2 = rex.Replace(jsonObjStr2, "$1").Trim();
            string splitChar = (!string.IsNullOrEmpty(str1) && !string.IsNullOrEmpty(str2)) ? "," : "";
            string result = "{" + str1 + splitChar + str2 + "}";
            return result;
        }
        /// <summary>
        /// 根据Json更新DataTable中，设置Row的委托
        /// </summary>
        /// <param name="row"></param>
        /// <param name="index"></param>
        public delegate void UpdateDataTable_SetRow(DataRow row, int index);

        /// <summary>
        /// 根据Json更新DataTable
        /// </summary>
        /// <param name="jsonString">Json字符串(必须为DataTable格式的)</param>
        /// <param name="dt">需要更新的DataTable</param>
        /// <param name="columns">需要更新的列</param>
        public static void Json_UpdateDataTable(string jsonString, DataTable dt, UpdateDataTable_SetRow addRow)
        {
            DataTable dtJson = DeserializeData<DataTable>(jsonString);
            int i = 0;
            if (dtJson != null && dtJson.Rows.Count > 0)
            {
                bool isAdd = dt.Rows.Count == 0;
                //对每行进行赋值
                foreach (DataRow updr in dtJson.Rows)
                {
                    DataRow dr = null;
                    if (isAdd)
                    {
                        dr = dt.NewRow();
                        if (addRow != null)
                            addRow(dr, i);
                    }
                    else
                        dr = dt.Rows[i];

                    foreach (DataColumn column in dtJson.Columns)
                        dr[column.ColumnName] = updr[column.ColumnName];

                    i++;
                    if (isAdd)
                        dt.Rows.Add(dr);
                    else
                        isAdd = dt.Rows.Count <= i;
                }
            }

            //清除多余的行
            if (dt.Rows.Count > i)
            {
                for (int j = dt.Rows.Count - i; j > 0; j--)
                    dt.Rows[dt.Rows.Count - j].Delete();
            }
        }
        #endregion


        #region DataTable转化为树形数据
        /// <summary>
        /// 获取树格式对象
        /// </summary>
        /// <param name="list">线性数据</param>
        /// <param name="id">ID的字段名</param>
        /// <param name="pid">PID的字段名</param>
        /// <returns></returns>
        public static object ArrayToTreeData(IList<Hashtable> list, string id, string pid)
        {
            var h = new Hashtable(); //数据索引 
            var r = new List<Hashtable>(); //数据池,要返回的 
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                h[item[id].ToString()] = item;
            }
            foreach (var item in list)
            {
                if (!item.ContainsKey(id)) continue;
                if (!item.ContainsKey(pid) || item[pid] == null || !h.ContainsKey(item[pid].ToString()))
                {
                    r.Add(item);
                }
                else
                {
                    var pitem = h[item[pid].ToString()] as Hashtable;
                    if (!pitem.ContainsKey("children"))
                        pitem["children"] = new List<Hashtable>();
                    var children = pitem["children"] as List<Hashtable>;
                    children.Add(item);
                }
            }
            return r;
        }
        /// <summary>
        /// 将db reader转换为Hashtable列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<Hashtable> DbReaderToHash(IDataReader reader)
        {
            var list = new List<Hashtable>();
            while (reader.Read())
            {
                var item = new Hashtable();

                for (var i = 0; i < reader.FieldCount; i++)
                {
                    var name = reader.GetName(i);
                    var value = reader[i];
                    item[name] = value;
                }
                list.Add(item);
            }
            return list;
        }
        /// <summary>
        /// 将DataTable转换为Hashtable列表
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<Hashtable> DataTableToHash(DataTable table)
        {
            var list = new List<Hashtable>();
            DataRowCollection rows = table.Rows;
            DataColumnCollection columns = table.Columns;
            int conCount = table.Columns.Count;
            for (var j = 0; j < rows.Count; j++)
            {
                var item = new Hashtable();
                for (var i = 0; i < conCount; i++)
                {
                    var name = columns[i].ColumnName;
                    var value = ValidatorHelper.ToStr(rows[j][i],"");
                    item[name] = value;
                }
                list.Add(item);
            }
            return list;
        }
        public static object DataTableToTreeData(DataTable table, string id, string pid)
        {
            return ArrayToTreeData(DataTableToHash(table), id, pid);
        }
        #endregion
    }
}
