using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share.Customer
{
    public class FansWxBindCondition : CustomerGroupCondition
    {
        /// <summary>
        /// 绑定类型 bind 或者unbind
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 绑定店铺信息
        /// </summary>
        public List<WxBindShop> BindShops { get; set; }
    }

    public class WxBindShop
    {
        public string Name { get; set; }

        public string Id { get; set; }
    }
}
