using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    public class DateTimeToLongConverter : ITypeConverter<DateTime, long>
    {
        public long Convert(DateTime source, long destination, ResolutionContext context)
        {
            if(source == default(DateTime))
            {
                return 0;
            }
            else
            {
                return (long)(source - new DateTime(1970, 1, 1)).TotalSeconds;
            }
        }
    }
}
