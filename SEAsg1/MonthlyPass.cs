using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class MonthlyPass : ChargeStrategy
    {
        public static int MONTHLY_PASS_AVAILABLE = 5;
        private static readonly object block = new object();
        private static readonly Thread countdown = new Thread(new ThreadStart(delegate () {
            while (true)
            {
                Thread.Sleep(new TimeSpan(0, 0, 0, 30));
                lock (block)
                {
                    MONTHLY_PASS_AVAILABLE = 5;
                }
            }
        }));
        private static bool countDownIsStarted = default(bool); 
            
        public MonthlyPass()
        {
            lock (block)
            {
                if (MONTHLY_PASS_AVAILABLE > 0)
                {
                    MONTHLY_PASS_AVAILABLE--;
                }
                else
                {
                    Console.Error.WriteLine("Sorry, no more monthly season pass available. Please come back after ");
                }
            }

            if (MONTHLY_PASS_AVAILABLE < 5 && !countDownIsStarted)
            {
                countDownIsStarted = !countDownIsStarted;
                countdown.Start(); 
            }
        }

        public float Charge(string vtype, TimeSpan span)
        {
            return 0f;
        }

        public float GetMonthlyCost(string vtype)
        {
            return 100f;
        }
    }
}
