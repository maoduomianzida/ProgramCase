using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    [AttributeUsage(AttributeTargets.Property)]
    public class DateTimeFormatAttribute : ColumnFormatAttribute 
    {
        private IColumnFormatter formatter;

        public DateTimeFormatAttribute(string formatString)
        {
            formatter = new DateTimeFormatter(formatString);
        }

        public override object Format(string columnName, object columnValue)
        {
            return formatter.Format(columnName,columnValue);
        }
    }
}
