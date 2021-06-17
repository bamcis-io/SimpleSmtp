using System;
using System.Security.Principal;
using System.Threading;

namespace BAMCIS.SimpleSmtp
{
    internal abstract class Worker
    {
        protected Thread _thread;
        protected bool _shouldStop = true;
        protected bool _stopped = true;
        protected bool _paused = false;
        protected Guid _id;
        protected TimeSpan _sleepTime = TimeSpan.FromSeconds(1);
       

        public Guid Id
        {
            get
            {
                return this._id;
            }
        }
        public bool IsStopped
        {
            get
            {
                return this._stopped;
            }
        }

        protected Worker()
        {
            this._id = Guid.NewGuid();
        }

        public void Start()
        {
            WindowsIdentity Identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal Principal = new WindowsPrincipal(Identity);

            //Ensure service is running with the appropriate privileges
            if (Principal.IsInRole(WindowsBuiltInRole.Administrator) || Identity.IsSystem)
            {
                this._stopped = false;
                this._shouldStop = false;

                //Only 1 thread instance can be created
                if (this._thread == null)
                {
                    this._thread = new Thread(this.Run);
                }
                else if (this._thread.IsAlive)
                {
                    this._thread.Abort();
                    this._thread = new Thread(this.Run);
                }

                //If the thead is not alive, start it
                if (!this._thread.IsAlive)
                {
                    this._thread.Start();
                }
            }
            else
            {
                this.Stop();
                throw new Exception("This service must be run as system or with admin credentials.");
            }
        }

        public void Stop()
        {
            this._shouldStop = true;
            this._stopped = true;
        }

        public void Pause()
        {
            if (!this._paused)
            {             
                this.Stop();
                this._paused = true;
            }
        }

        public void Continue()
        {
            if (this._paused)
            {              
                this.Start();
                this._paused = false;
            }
        }

        protected abstract void Run();
    }
}
