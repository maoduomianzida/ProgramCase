using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    public class DateTimeFormatter : IColumnFormatter
    {
        private string FormatString { get; set; }

        public DateTimeFormatter(string format)
        {
            FormatString = format;
        }

        public object Format(string columnName, object columnValue)
        {
            DateTime date = TypeUtils.Convert<DateTime>(columnValue);

            return date.ToString(FormatString);
        }
    }
}
