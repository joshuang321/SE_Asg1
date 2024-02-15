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
        Carpark carpark;

        string vehicleType;

        public ParkingCharge(Vehicle vehicle, Carpark carpark)
        {
            id = ++ID_AUTOINCRM;
            cost = 0f;
            entryDate = DateTime.Now;
            exitDate = entryDate;
            status = "Pending";
            this.carpark = carpark;
            this.vehicle = vehicle;

            if (vehicle.GetPass() == null)
            {
                chargeStrategy = new NoPass();
            }
            else if (!vehicle!.GetPass().IsExpired() && vehicle!.GetPass().IsTerminated())
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
