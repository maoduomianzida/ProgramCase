using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    public interface IColumnFormatter
    {
        object Format(string columnName, object columnValue);
    }
}
