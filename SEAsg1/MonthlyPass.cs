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
