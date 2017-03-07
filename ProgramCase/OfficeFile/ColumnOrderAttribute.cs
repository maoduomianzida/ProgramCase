using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    [AttributeUsage(AttributeTargets.Property)]
    public class ColumnOrderAttribute : Attribute
    {
        public int Order { get; set; }

        public ColumnOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
