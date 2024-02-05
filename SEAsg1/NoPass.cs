using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    class NoPass : ChargeStrategy
    {
        public virtual float Charge(string vtype, TimeSpan span)
        {
            return span.Hours * 15;
        }

        public virtual float GetMonthlyCost(string vtype)
        {
            throw new NotImplementedException();
        }
    }
}
