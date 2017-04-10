using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoMapper
{
    public interface IJsonFormatter
    {
        string Serialize();

        void Deserialize(string data);
    }
}
