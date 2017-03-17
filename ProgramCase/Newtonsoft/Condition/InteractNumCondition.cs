using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share.Customer
{
    /// <summary>
    /// 互动消息次数
    /// </summary>
    public class InteractNumCondition : CustomerGroupCondition
    {
        /// <summary>
        /// range 或者 num
        /// </summary>
        public string Type { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public int Num { get; set; }
    }
}