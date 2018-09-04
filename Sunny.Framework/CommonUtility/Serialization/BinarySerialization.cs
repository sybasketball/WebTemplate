using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace CommonUtility.Serialization
{
    /// <summary>
    /// 2�������л�����
    /// </summary>
    public class BinarySerialization<T>
    {
        /// <summary>
        /// 2�������л�����
        /// </summary>
        /// <param name="obj">Դ����</param>
        /// <returns>2��������</returns>
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
        /// 2�������ݷ����л�Ϊ����
        /// </summary>
        /// <param name="data">2��������</param>
        /// <returns>object����</returns>
        public static T ByteToObject(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream memStream = new MemoryStream();
            memStream.Write(data, 0, data.Length);
            memStream.Position = 0;

            return (T)formatter.Deserialize(memStream);
        }

        /// <summary>
        /// �����¡
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
