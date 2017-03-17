using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share.Customer
{
    public class ConditionCollection : Dictionary<string, CustomerGroupCondition>, IJsonFormatter
    {
        internal static readonly List<CustomerGroupCondition> Optionals;
        
        static ConditionCollection()
        {
            Optionals = new List<CustomerGroupCondition>();
            Optionals.Add(new ActivitysCondition());
            Optionals.Add(new AveragePriceCondition());
            Optionals.Add(new BuyTimeCondition());
            Optionals.Add(new FansWxBindCondition());
            Optionals.Add(new FollowTimeCondition());
            Optionals.Add(new GenderCondition());
            Optionals.Add(new InteractNumCondition());
            Optionals.Add(new JoinNumCondition());
            Optionals.Add(new JoinTimeCondition());
            Optionals.Add(new MsgSendTimeCondition());
            Optionals.Add(new ProductsCondition());
            Optionals.Add(new SMSNumCondition());
            Optionals.Add(new SMSTimeCondition());
            Optionals.Add(new TagFilterCondition());
            Optionals.Add(new TradeAmountCondition());
            Optionals.Add(new TradeDateCondition());
            Optionals.Add(new TradeNumCondition());
            Optionals.Add(new WxBindCondition());
        }

        protected JsonSerializerSettings _setting;

        public ConditionCollection()
        {
            _setting = JsonConvertCreater.GetLowercaseNameSetting(true,new CustomerGroupConditionConverter());
        }

        public void Add(CustomerGroupCondition condition)
        {
            if (condition == null) throw new ArgumentNullException(nameof(condition));
            if (ContainsKey(condition.Name)) throw new ArgumentException($"{condition.Name}已经存在");
            if (!Optionals.Exists(tmp => tmp.Name == condition.Name)) throw new ArgumentException($"{condition.Name}不是可选项");

            Add(condition.Name, condition);
        }

        public void Deserialize(string data)
        {
            if (!string.IsNullOrWhiteSpace(data))
            {
                JsonConvert.PopulateObject(data, this, _setting);
            }
        }

        public string Serialize()
        {
            return JsonConvert.SerializeObject(this, _setting);
        }
    }
}