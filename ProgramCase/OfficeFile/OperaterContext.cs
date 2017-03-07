using NPOI.SS.UserModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    public class OperaterContext
    {
        public OperaterContext(ISheet sheet,int rowIndex)
        {
            Sheet = sheet;
            RowIndex = rowIndex;
        }

        public int RowIndex { get; set; }

        public ISheet  Sheet { get;  }
    }
}
