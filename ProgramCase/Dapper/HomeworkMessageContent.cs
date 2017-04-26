using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.Dapper
{
    public class HomeworkMessageContent : TipMessageContent
    {
        /// <summary>
        /// 作业消息类型 1.视频作业 2.试题作业
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 作业ID
        /// </summary>
        public int HomeworkID { get; set; }

        /// <summary>
        /// 作业标题
        /// </summary>
        public string HomeworkTitle { get; set; }

        /// <summary>
        /// 班级ID
        /// </summary>
        public int GroupID { get; set; }

        /// <summary>
        /// 班级名称
        /// </summary>
        public string GroupTitle { get; set; }

        /// <summary>
        /// 教师ID
        /// </summary>
        public int TeacherID { get; set; }

        /// <summary>
        /// 教师名称
        /// </summary>
        public string TeacherName { get; set; }

        /// <summary>
        /// 视频课程ID
        /// </summary>
        public int LessonID { get; set; }

        /// <summary>
        /// 科目名称
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 截止日期
        /// </summary>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// 浏览次数
        /// </summary>
        public int ViewCount { get; set; }

        /// <summary>
        /// 赞次数
        /// </summary>
        public int PraiseCount { get; set; }
    }
}
