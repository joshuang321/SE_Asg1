using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class TerminatedState : PassState
    {
        public TerminatedState(SeasonParking pass)
        {
            pass.GetApplicationCollection().IncrementMonthlyPass(); 
        }

        public void Renew(DateTime newEndDate)
        {
            Console.WriteLine("Cannot renew terminated pass!");
        }

        public void Terminate()
        {
            Console.WriteLine("Pass has already been terminated!");
        }

        public void Transfer(Vehicle vehicle)
        {
            Console.WriteLine("Cannot transfer terminated pass!");
        }
    }
}
