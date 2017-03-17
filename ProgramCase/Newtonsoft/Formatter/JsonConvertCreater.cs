using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Sodao.JukeTool.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share
{
    public static class JsonConvertCreater
    {
        public static JsonSerializerSettings GetLowercaseNameSetting(bool ignoreCaseName,params JsonConverter[] converters)
        {
            JsonSerializerSettings setting = new JsonSerializerSettings();
            IsoDateTimeConverter timeConverter = new IsoDateTimeConverter();
            timeConverter.DateTimeFormat = "yyyy'-'MM'-'dd' 'HH':'mm':'ss";
            setting.Converters.Add(timeConverter);
            if (ignoreCaseName)
            {
                setting.ContractResolver = new LowercaseNameContractResolver();
            }
            foreach(JsonConverter tmp in converters.Where(t => t != null))
            {
                setting.Converters.Add(tmp);
            }

            return setting;
        }
    }
}