using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RunChicken.ThreadService
{
    
    public class CountDownWorker:DaemonWorker<int>
    {
        private int seconds;
        private int elapsedSeconds = 0;

        public CountDownWorker(int seconds):base()
        {
            this.intervalSeconds = 1;
            if (seconds <= 0)
            {
                seconds = 60;
            }
            this.seconds = seconds;
        }

        protected override void DoWork()
        {
            this.elapsedSeconds++;
            worker.ReportProgress(0,seconds - elapsedSeconds);
            isRunning = elapsedSeconds < seconds;
        }
    }
}
