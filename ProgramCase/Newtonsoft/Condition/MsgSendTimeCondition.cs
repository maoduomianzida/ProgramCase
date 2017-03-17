using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share.Customer
{
    /// <summary>
    /// 最近发送消息时间
    /// </summary>
    public class MsgSendTimeCondition : CustomerGroupCondition
    {
        /// <summary>
        /// range 或 lately
        /// </summary>
        public string Type { get; set; }

        public string Start { get; set; }

        public string End { get; set; }

        public int Lately { get; set; }
    }
}
