using System;
using System.Diagnostics;
using System.Threading;

namespace BAMCIS.SimpleSmtp
{
    internal class SmtpWorker : Worker
    {
        protected override async void Run()
        {
            Thread HandlerThread = null;
            try
            {
                while (!this._shouldStop)
                {
                    Debug.WriteLine("Creating new SmtpServer handler.");
                    SmtpServer Handler = new SmtpServer(await SmtpServer.InitializeListener());
                    HandlerThread = new Thread(new ThreadStart(Handler.HandleSession));
                    HandlerThread.Start();

                    this._stopped = false;
                    Thread.Sleep(this._sleepTime);
                }
            }
            catch (ThreadAbortException)
            {
                Thread.ResetAbort();
            }
            catch (Exception)
            { }
            finally
            {
                this._thread = null;
                this._stopped = true;

                int Counter = 0;

                while (HandlerThread != null && HandlerThread.IsAlive && Counter < 20)
                {
                    Thread.Sleep(1000);
                    Counter++;
                }

                if (Counter == 20)
                {
                    HandlerThread.Abort();
                }
            }
        }
    }
}
