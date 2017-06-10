using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Consul;
using System.Threading;

namespace ProgramCase.Consul
{
    //[Main]
    public class ConsulCase : ICase
    {
        public void ConsulTest()
        {
            IConsulClient client = new ConsulClient(config =>
            {
                config.Address = new Uri("http://127.0.0.1:8500");
            });
            var query = client.Agent.Services().Result;
            /* var addResult = client.Agent.ServiceRegister(new AgentServiceRegistration()
 {
     Address = "192.168.1.23",
     ID = "tmp",
     Port = 60,
     Name = "UserService",
     Tags = new string[] { "tag2", "tag1" }
 }).Result;*/
            //Console.WriteLine(task.Result.StatusCode);
        }

        public void Run()
        {
            Thread thread = new Thread(Test);
            thread.Start();
            Thread.Sleep(300);
            Clear(0);
            
            Thread thread2 = new Thread(Clear);
            thread2.Start(100);
        }

        private int a = 0;
        private object lockObject = new object();

        public void Test()
        {
            
            lock (lockObject)
            {
                while(true)
                {
                    Console.WriteLine("Test before");
                    if (a == 0)
                    {
                        Console.WriteLine("a 为 0");
                        Monitor.Wait(lockObject);
                    }
                    else
                    {
                        Console.WriteLine("a 为 " + a);
                        break;
                    }
                }
            }
        }

        public void Clear(object value)
        {
            int t = (int)value;
            Thread.Sleep(3000);
            lock(lockObject)
            {
                a = t;
                Monitor.Pulse(lockObject);
                Console.WriteLine("Clear");
            }
        }
    }
}