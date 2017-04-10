using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoMapper
{
    public class StringJsonFormatterConverter : ITypeConverter<string, IJsonFormatter>
    {
        public IJsonFormatter Convert(string source, IJsonFormatter destination, ResolutionContext context)
        {
            var t = context.Options.CreateInstance<IJsonFormatter>();
            if (destination != null && !string.IsNullOrEmpty(source))
                destination.Deserialize(source);

            return destination;
        }
    }

    public class JsonFormatterStringConverter : ITypeConverter<IJsonFormatter, string>
    {
        public string Convert(IJsonFormatter source, string destination, ResolutionContext context)
        {
            if (source != null)
            {
                return source.Serialize();
            }

            return null;
        }
    }
}
