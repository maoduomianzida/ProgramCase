using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    public static class IExcelOperaterExtension
    {
        public static void Create(this IExcelOperater @this,string filePath)
        {
            if(@this == null)
            {
                throw new ArgumentNullException(nameof(@this));
            }
            Stream stream = @this.Create();
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
            {
                stream.CopyTo(fileStream);
                fileStream.Close();
                stream.Close();
            }
        }
    }
}
