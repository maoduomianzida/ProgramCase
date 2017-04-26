using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.Dapper
{
    public class TipMessage
    {
        public int ID { get; set; }

        /// <summary>
        /// 消息内容（JSON格式）
        /// </summary>
        public TipMessageContent Content { get; set; }

        /// <summary>
        /// 消息添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 消息状态 0.未读 1.已读
        /// </summary>
        public int ReadStatus { get; set; }

        /// <summary>
        /// 消息类型 1.作业消息 2.申请加入班级消息
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 消息接收人ID
        /// </summary>
        public int ReceiverID { get; set; }
    }
}
