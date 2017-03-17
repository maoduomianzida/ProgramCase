using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    /// <summary>
    /// 分组过滤条件
    /// </summary>
    public abstract class CustomerGroupCondition : IJsonFormatter
    {
        protected JsonSerializerSettings _setting;

        protected CustomerGroupCondition()
        {
            Name = GetConditionName(GetType());
            _setting = JsonConvertCreater.GetLowercaseNameSetting(true);
        }

        internal static readonly string ConditionSuffix = "Condition";

        internal static string GetConditionName(Type type)
        {
            if (!typeof(CustomerGroupCondition).IsAssignableFrom(type)) throw new Exception("type必须继承CustomerGroupCondition类");

            return type.Name.Replace(ConditionSuffix,string.Empty).ToLower();
        }

        public virtual string Serialize()
        {
            return JsonConvert.SerializeObject(this, _setting);
        }

        public virtual void Deserialize(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                JsonConvert.PopulateObject(data, this, _setting);
            }
        }

        [JsonIgnore]
        public virtual string Name { get; private set; }
    }
}