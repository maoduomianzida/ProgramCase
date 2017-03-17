using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sodao.JukeTool.Share
{
    /// <summary>
    /// 适用于内容为json，但属性为string类型的特殊转换
    /// </summary>
    public class JsonContentConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(string);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            object value = serializer.Deserialize(reader);
            if (value != null)
            {
                return JsonConvert.SerializeObject(value);
            }

            return null;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteRawValue(value as string);
        }
    }
}