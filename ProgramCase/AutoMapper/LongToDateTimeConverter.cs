using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase
{
    public class LongToDateTimeConverter : ITypeConverter<long, DateTime>
    {
        public DateTime Convert(long source, DateTime destination, ResolutionContext context)
        {
            if(source == 0)
            {
                return default(DateTime);
            }
            else
            {
                DateTime date = new DateTime(1970,1,1);
                date = date.AddSeconds(source);

                return date;
            }
        }
    }
}
