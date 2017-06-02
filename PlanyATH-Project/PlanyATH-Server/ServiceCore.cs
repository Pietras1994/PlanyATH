using PlanyATH_Server;
using System;
using System.Timers;

namespace PlanyATH_Server
{
    class ServiceCore
    {
        readonly Timer _timer;
        public ServiceCore()
        {
            PlanContext db = new PlanContext();
            _timer = new Timer(600000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Program.RunnAllFunction();



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
