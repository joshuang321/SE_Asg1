using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class SysClock
    {
        static public SysClock instance = new SysClock();
        Thread thread;
        public DateTime curTime { get; private set; }

        SysClock()
        {
            thread = new Thread(new ThreadStart(SysLoop));
            thread.Start();
        }


        List<SysClockObserver> observers;

        public void SysLoop()
        {
            while (true)
            {
                curTime = DateTime.Now;
                NotifyObservers();
                Thread.Sleep(100);
            }
        }

        void NotifyObservers()
        {
            foreach (SysClockObserver ob in observers)
            {
                ob.Update(curTime);
            }
        }
    }
}
