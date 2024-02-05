
namespace SEAsg1
{
    public class SeasonParking
    {
        static int ID_AUTOINCRM = 0;
        int id;
        DateTime startDate;
        DateTime endDate;
        Vehicle vehicle;
        ChargeStrategy chargeStrategy;

        PassState curState;

        public SeasonParking(DateTime startDate, DateTime endDate,
                Vehicle vehicle,
                ChargeStrategy chargeStrategy)
        {
            id = ++ID_AUTOINCRM;
            this.startDate = startDate;
            this.endDate = endDate;
            this.vehicle =vehicle;
            this.chargeStrategy = chargeStrategy;
            if (DateTime.Now> endDate)
            {
                curState = new ExpiredState(this);
            }
            else
            {
                curState = new ValidState(this);
            }
            this.vehicle.SetPass(this);
        }

        public Vehicle GetVehicle()
        {
            return vehicle;
        }

        public void Renew(DateTime newEndDate)
        {
            curState.Renew(newEndDate);
        }

        public void Terminate()
        {
            curState.Terminate();
        }

        public void SetState(PassState state)
        {
            curState = state;
        }

        public void SetNewEndDate(DateTime newEndDate)
        {
            endDate = newEndDate;
        }

        public int GetId() => id;
        public DateTime GetStartDate() => startDate;
        public DateTime GetEndDate() => endDate;
        public ChargeStrategy GetChargeStrategy() => chargeStrategy;
    }
}