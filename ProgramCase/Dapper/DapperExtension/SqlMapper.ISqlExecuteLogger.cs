using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dapper
{
    public partial class SqlMapper
    {
		public interface ISqlExecuteLogger
        {
            void Log(SqlExecuteContext sqlContext);
        }

		public interface IAsyncSqlExecuteLogger
        {
            Task LogAsync(SqlExecuteContext sqlContext);
        }
    }
}