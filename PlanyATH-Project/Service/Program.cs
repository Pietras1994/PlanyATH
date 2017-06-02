using Topshelf;
namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(z =>
            {
                z.Service<ServiceCore>(s =>
                {
                    s.ConstructUsing(c => new ServiceCore());
                    s.WhenStarted(c => c.Start());
                    s.WhenStopped(c => c.Stop());
                });
                z.RunAsLocalSystem();
                z.StartAutomatically();
            });
        }
    }
}
