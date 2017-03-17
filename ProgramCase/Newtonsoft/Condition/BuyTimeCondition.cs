using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    public class BuyTimeCondition : CustomerGroupCondition
    {
        public int Start { get; set; }

        public int End { get; set; }
    }
}
