
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

        ApplicationCollection ac; 

        public SeasonParking(DateTime startDate, DateTime endDate,
                Vehicle vehicle,
                ChargeStrategy chargeStrategy, ApplicationCollection ac)
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
            this.ac = ac; 
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
        public PassState GetState()
        {
            return curState;
        }

        public void SetNewEndDate(DateTime newEndDate)
        {
            endDate = newEndDate;
        }

        public int GetId() => id;
        public DateTime GetStartDate() => startDate;
        public DateTime GetEndDate() => endDate;
        public ChargeStrategy GetChargeStrategy() => chargeStrategy;
        public ApplicationCollection GetApplicationCollection() => ac;

        public bool IsExpired() => curState.GetType() == typeof(ExpiredState);
        public bool IsTerminated() => curState.GetType() == typeof(TerminatedState);

        public override string ToString()
        {
            return $"Season Parking{{Id: {GetId()}, Start Date: {GetStartDate()}, End Date: {GetEndDate()}, Charge Strategy: {GetChargeStrategy()}}}";
        }
    }
}