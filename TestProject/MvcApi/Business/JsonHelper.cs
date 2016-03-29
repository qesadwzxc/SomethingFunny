using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MvcApi.Business
{
    public static class JsonHelper
    {
        /// <summary>将JSON反序列化为实体
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T JsonDeserialize<T>(string json)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json));
            T obj = (T)serializer.ReadObject(stream);
            return obj;
        }

        /// <summary>
        /// 将实体序列化为Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string JsonSerialize<T>(T model)
        {
            DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(T));
            MemoryStream stream = new MemoryStream();
            serializer.WriteObject(stream, model);

            byte[] dataBytes = new byte[stream.Length];
            stream.Position = 0;
            stream.Read(dataBytes, 0, (int)stream.Length);
            string dataString = Encoding.UTF8.GetString(dataBytes);
            stream.Close();

            return null;
        }
    }
}