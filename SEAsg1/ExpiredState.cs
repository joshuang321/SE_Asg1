using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class ExpiredState : PassState
    {
        SeasonParking pass;

        public ExpiredState(SeasonParking pass)
        {
            this.pass = pass;
        }

        public void Renew(DateTime newEndDate)
        {
            if (newEndDate > pass.GetEndDate())
            {
                int extraMonths = newEndDate.Month - pass.GetEndDate().Month;
                float extraCost = extraMonths * pass.GetChargeStrategy()
                    .GetMonthlyCost(pass.GetVehicle()
                        .GetVehicleType());

                Console.WriteLine($"Renewed season pass, cost: ${extraCost}. Pass expires on {newEndDate}.");
                pass.SetNewEndDate(newEndDate);
                pass.SetState(new ValidState(pass));
            }
            
        }

        public void Terminate()
        {
            Console.WriteLine("Terminated expired pass.");
            pass.SetState(new TerminatedState());
        }

        public void Transfer(Vehicle vehicle)
        {
            SeasonParking? pass = vehicle.GetPass();
            Vehicle curVehicle = this.pass.GetVehicle();

            if (pass != null && vehicle.GetVehicleType() == curVehicle.GetVehicleType()
                && !vehicle.IsParked())
            {
                curVehicle.SetPass(null);
                vehicle.SetPass(pass);
            }
        }
    }
}
