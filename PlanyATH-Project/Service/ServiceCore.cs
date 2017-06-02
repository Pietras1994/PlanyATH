using PlanyATH_Server;
using System;
using System.Timers;

namespace Service
{
    class ServiceCore
    {
        readonly Timer _timer;
        public ServiceCore()
        {
            PlanContext db = new PlanContext();
            _timer = new Timer(30000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => PlanyATH_Server.Program.RunnAllFunction();//wasza funkcja



            Console.WriteLine("Database write successes !!!");
            Console.ReadKey();
        }
        public void Start()
        {
            _timer.Start();
            Console.WriteLine("Service started");
        }

        public void Stop()
        {
            _timer.Stop();
            Console.WriteLine("Service stopped");
        }
    }
}
