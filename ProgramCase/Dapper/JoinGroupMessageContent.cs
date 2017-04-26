using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.Dapper
{
    public class JoinGroupMessageContent : TipMessageContent
    { /// <summary>
      /// 班级ID
      /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string GroupTitle { get; set; }

        /// <summary>
        /// 申请加入班级ID
        /// </summary>
        public int ApplyID { get; set; }

        /// <summary>
        /// 消息模板
        /// </summary>
        public string MessageTemplate { get; set; }

        /// <summary>
        /// 消息类型 1. 加入班级成功  2.被拒绝加入
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 拒绝申请ID
        /// </summary>
        public int DenyID { get; set; }

        /// <summary>
        /// 拒绝理由
        /// </summary>
        public string DenyReason { get; set; }
    }
}
