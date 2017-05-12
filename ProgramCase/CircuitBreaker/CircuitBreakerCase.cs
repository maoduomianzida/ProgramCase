using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProgramCase.CircuitBreaker
{
    [Main]
    public class CircuitBreakerCase : ICase
    {
        private CircuitBreaker context;

        private bool haveError = true;

        public void Run()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            context = new CircuitBreaker(new CircuitBreakerSetting
            {
                ProtectAction = Fail,
                AllowFailInterval = TimeSpan.FromSeconds(4),
                AllowFailTimes = 11,
                ExceptionProcess = err => Console.WriteLine(err.Message),
                HalfOpenDuration = TimeSpan.FromSeconds(5),
                HalfOpenRequestLimit = 3
            });
            Console.WriteLine(context._currentState is CloseState ? "熔断器关闭" : "熔断器开启");
            for (int i = 0; i < 10; i++)
            {
                Thread thread = new Thread((state) =>
                {
                    try
                    {
                        context.Execute();
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                });
                thread.Start();
            }
            Thread.Sleep(500);
            Console.WriteLine(context._currentState is OpenState ? "熔断器开启" : "熔断器关闭");
            Thread.Sleep(5000);
            Console.WriteLine(context._currentState is HalfOpenState ? "熔断器半开启" : "熔断器关闭");
            haveError = false;
            for (int i = 0; i < 5; i++)
            {
                Thread thread = new Thread((state) =>
                {
                    try
                    {
                        context.Execute();
                    }
                    catch (Exception err)
                    {
                        Console.WriteLine(err.Message);
                    }
                });
                thread.Start();
            }
            Thread.Sleep(500);
            Console.WriteLine(context._currentState.GetType());

            Console.Read();
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Console.WriteLine("错误" + (e.ExceptionObject as Exception).Message);
        }

        public void Fail()
        {
            if (haveError)
            {
                throw new RemoteResourceException("API 异常");
            }
            else
            {
                Console.WriteLine("API 成功执行");
            }
        }
    }
}