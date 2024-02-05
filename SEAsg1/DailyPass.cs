using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    class DailyPass : NoPass
    {
        const float CHARGE_LIMIT = 135f;
        public override float Charge(string vtype, TimeSpan span)
        {
            float amt =base.Charge(vtype, span);
            if (amt > CHARGE_LIMIT)
            {
                amt = CHARGE_LIMIT;
            }
            return amt;
        }

        public override float GetMonthlyCost(string vtype)
        {
            return 30f;
        }
    }
}
