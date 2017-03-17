using Newtonsoft.Json.Converters;
using Sodao.Juketool.Share.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Sodao.Juketool.Share
{
    public class CustomerGroupConditionConverter : CustomCreationConverter<CustomerGroupCondition>
    {
        public override CustomerGroupCondition Create(Type objectType)
        {
            return Activator.CreateInstance(objectType) as CustomerGroupCondition;
        }

        public virtual Type GetConditionType(string conditionName)
        {
            CustomerGroupCondition condition = ConditionCollection.Optionals.FirstOrDefault(tmp => tmp.Name == conditionName);
            if (condition == null)
                throw new NullReferenceException($"{conditionName}对应的分组过滤条件不存在");

            return condition.GetType();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
             Type realType = GetConditionType(reader.Path);

            return base.ReadJson(reader, realType, existingValue,serializer);
        }
    }
}