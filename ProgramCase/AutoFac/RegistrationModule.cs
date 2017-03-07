using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgramCase.AutoFac
{
    public class RegistrationModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(typeof(Program).Assembly).AsImplementedInterfaces();
            //builder.Register(context => new IPrint[] { new APrint(), new BPrint() }).As<IEnumerable<IPrint>>();
        }
    }
}