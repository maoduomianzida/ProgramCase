using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    /// <summary>
    /// 操作Office文件工具类
    /// </summary>
    public static class OfficeFileUtils
    {
        [ThreadStatic]
        private static IExcelOperater _excelOperater;

        public static IExcelOperater ExcelOperater
        {
            get
            {
                if(_excelOperater == null)
                {
                    _excelOperater = new ExcelFileOperater();
                }

                return _excelOperater;
            }
        }
    }
}
