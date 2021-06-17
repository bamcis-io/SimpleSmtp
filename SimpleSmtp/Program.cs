using Topshelf;

namespace BAMCIS.SimpleSmtp
{
    class Program
    {
        static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<SmtpWorker>(s =>
                {
                    s.ConstructUsing(svc => new SmtpWorker());
                    s.WhenStarted(svc => svc.Start());
                    s.WhenStopped(svc => svc.Stop());
                    s.WhenPaused(svc => svc.Pause());
                    s.WhenContinued(svc => svc.Continue());
                });
                x.RunAsLocalSystem();

                x.SetDescription("A simple SMTP server to filter emails.");
                x.SetDisplayName("SimpleSmtp");
                x.SetServiceName("SimpleSmtp");
                x.StartAutomatically();

                x.EnablePauseAndContinue();
            });
        }
    }
}
