using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoMapper
{
    public class AutoMapperCase : ICase
    {
        public void Run()
        {

        }
    }


    public class SuperB : Collection<B>
    {
        public bool IsLeaf { get; set; }
    }

    public class TProfile : Profile
    {
        public TProfile()
        {
            CreateMap<DateTime, long>().ConvertUsing<DateTimeToLongConverter>();
            CreateMap<long, DateTime>().ConvertUsing<LongToDateTimeConverter>();
        }
    }

    public class A
    {
        public int Id { get; set; }

        public DateTime CreateTime { get; set; }
    }

    public class B
    {
        public int Id { get; set; }

        public long CreateTime { get; set; }
    }
}
