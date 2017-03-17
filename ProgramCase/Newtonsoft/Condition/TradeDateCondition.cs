using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sodao.Juketool.Share.Customer
{
    public class TradeDateCondition : CustomerGroupCondition
    {
        public string Start { get; set; }

        public string End { get; set; }

        public int Num { get; set; }

        public string Type { get; set; }
    }
}
