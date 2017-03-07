using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    [AttributeUsage(AttributeTargets.Property)]
    public abstract class ColumnFormatAttribute : Attribute
    {
        public abstract object Format(string columnName, object columnValue);
    }
}
