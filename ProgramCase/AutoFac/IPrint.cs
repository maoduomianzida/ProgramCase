using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoFac
{
    public interface IPrint
    {
        void Print();
    }

    public class APrint : IPrint
    {
        public void Print()
        {
            Console.WriteLine(this.GetType().Name);
        }
    }

    public class BPrint : IPrint
    {
        public void Print()
        {
            Console.WriteLine(this.GetType().Name);
        }
    }
}
