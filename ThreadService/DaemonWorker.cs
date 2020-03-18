using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunChicken.ThreadService
{
    public abstract class DaemonWorker<T>
    {
        const int DEFAULT_INTERVAL = 30;
        protected int intervalSeconds;
        protected BackgroundWorker worker = null;
        public event Action<T> ProgressChanged;
        public event Action WorkCompleted;
        protected bool isRunning;
        private void onProgressChanged(T state)
        {
            if (ProgressChanged != null)
            {
                ProgressChanged(state);
            }
        }

        public DaemonWorker(int intervalSeconds)
        {
            if (intervalSeconds <= 0)
            {
                //默认30秒
                intervalSeconds = DEFAULT_INTERVAL;
            }
            this.intervalSeconds = intervalSeconds;
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += Worker_ProgressChanged;
            worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
        }

        public DaemonWorker():this(DEFAULT_INTERVAL)
        {
            
        }

        public void Start()
        {
            if (isRunning)
            {
                return;
            }
            isRunning = true;
            if (worker != null)
            {
                worker.RunWorkerAsync();
            }
        }

        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            while(isRunning)
            {
                if (worker.CancellationPending)
                {
                    isRunning = false;
                    e.Cancel = true;
                    return;
                }
                DoWork();
                Thread.Sleep(intervalSeconds * 1000);
            }
        }

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            onProgressChanged((T)e.UserState);
        }

        private void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (WorkCompleted != null)
            {
                WorkCompleted();
            }
        }

        public void Close()
        {
            isRunning = false;
            if (worker != null)
            {
                worker.CancelAsync();
            }
            isRunning = false;
        }

        protected abstract void DoWork();

    }
}
