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
            //builder.RegisterAssemblyTypes(typeof(Program).Assembly).AsImplementedInterfaces();
            /*builder.Register((context, paramArr) =>
            {
                string type = paramArr.Named<string>("type");

                IPrint result;
                if (type == "A")
                {
                    result = new APrint();
                }
                else
                {
                    result = new BPrint();
                }

                return result;
            }).As<IPrint>().InstancePerOwned<IPrint>();*/
            builder.RegisterType<BPrint>().As<IPrint>().InstancePerLifetimeScope().ExternallyOwned();
        }
    }
}