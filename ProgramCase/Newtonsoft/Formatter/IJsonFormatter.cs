using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sodao.Juketool.Share
{
    public interface IJsonFormatter
    {
        string Serialize();

        void Deserialize(string data);
    }
}