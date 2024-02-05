using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class ParkingCharge
    {
        static int ID_AUTOINCRM = 0;
        float cost;
        DateTime entryDate;
        DateTime exitDate;
        int id;
        string status;
        ChargeStrategy chargeStrategy;
        Vehicle vehicle;

        string vehicleType;

        public ParkingCharge(Vehicle vehicle)
        {
            id = ++ID_AUTOINCRM;
            cost = 0f;
            entryDate = DateTime.Now;
            exitDate = entryDate;
            status = "Pending";
            this.vehicle = vehicle;

            if (vehicle.GetPass() == null)
            {
                chargeStrategy = new NoPass();
            }
            else
            {
                chargeStrategy = vehicle.GetPass()!.GetChargeStrategy();
            }
            vehicleType = vehicle.GetVehicleType();
        }

        void Exit()
        {
            vehicle.Exit();
            exitDate = DateTime.Now;
            status = "Completed";
            chargeStrategy.Charge(vehicleType, exitDate-entryDate);
        }
    }
}
