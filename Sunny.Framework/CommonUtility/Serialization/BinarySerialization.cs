using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CommonUtility.Serialization
{
    /// <summary>
    /// 2进制序列化工具
    /// </summary>
    public class BinarySerialization<T>
    {
        /// <summary>
        /// 2进制序列化对象
        /// </summary>
        /// <param name="obj">源对象</param>
        /// <returns>2进制数据</returns>
        public static byte[] ObjectToByte(T obj)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            formatter.Serialize(memStream, obj);

            memStream.Position = 0;
            byte[] data = new byte[(int)memStream.Length];

            memStream.Read(data, 0, data.Length);

            return data;

        }

        /// <summary>
        /// 2进制数据反序列化为对象
        /// </summary>
        /// <param name="data">2进制数据</param>
        /// <returns>object对象</returns>
        public static T ByteToObject(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;

            return (T)formatter.Deserialize(memStream);
        }

        /// <summary>
        /// 对象克隆
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T CopyFrom(T obj)
        {
            byte[] bytes = ObjectToByte(obj);

            return ByteToObject(bytes);
        }
    }
}
