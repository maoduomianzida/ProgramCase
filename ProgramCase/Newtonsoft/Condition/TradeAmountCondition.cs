using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    public class TradeAmountCondition : CustomerGroupCondition
    {
        public decimal Start { get; set; }

        public decimal End { get; set; }
    }
}
