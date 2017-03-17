using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoFac
{
    public interface IPrint
    {
        string Id { get; set; }

        void Print();
    }

    public class APrint : IPrint ,IDisposable
    {
        public APrint()
        {
            Id = "APrint" + Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public void Dispose()
        {
            Console.WriteLine(Id + " is disposed");
        }

        public void Print()
        {
            Console.WriteLine(Id + " " + this.GetType().Name);
        }
    }

    public class BPrint : IPrint ,IDisposable
    {
        public BPrint()
        {
            Id = "BPrint" + Guid.NewGuid().ToString();
        }

        public string Id { get; set; }

        public void Print()
        {
            Console.WriteLine(Id + " " + this.GetType().Name);
        }


        public void Dispose()
        {
            Console.WriteLine(Id + " is disposed");
        }
    }
}
