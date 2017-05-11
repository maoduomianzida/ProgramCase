using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public partial class SqlMapper
    {
        public class SqlExecuteContext
        {
            /// <summary>
            /// SQL执行耗时
            /// </summary>
            public double ElapsedTime { get; }

            /// <summary>
            /// Sql 语句
            /// </summary>
            public string Sql { get; }

            /// <summary>
            /// 参数信息
            /// </summary>
            public IDictionary<string, object> Params { get; }

            /// <summary>
            /// 语句的执行次数
            /// </summary>
            public int ExecuteTimes { get; }
        }
    }
}