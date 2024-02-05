using System.Security.Cryptography.X509Certificates;

namespace SEAsg1
{
    public class Application
    {
        DateTime startMonth;
        DateTime endMonth;
        string paymentMode;
        string passType;
        ChargeStrategy chargeStrategy;

        Vehicle vehicle;
        User user;

        public Application(User user, Vehicle vehicle, DateTime startMonth,
            DateTime endMonth,
            ChargeStrategy chargeStrategy,
            string paymentMode,
            string passType)
        {
            this.startMonth = startMonth;
            this.endMonth = endMonth;
            this.paymentMode = paymentMode;
            this.chargeStrategy = chargeStrategy;
            this.vehicle = vehicle;
            this.user = user;
            this.passType = passType;
        }

        public float GetCharge()
        {
            return chargeStrategy.GetMonthlyCost(vehicle.GetVehicleType()) *
                    (endMonth.Month - startMonth.Month);
        }

        public Vehicle GetVehicle() => vehicle;
        public User GetUser() => user;
        public DateTime GetStartMonth() => startMonth;
        public DateTime GetEndMonth() => endMonth;
        public ChargeStrategy GetChargeStrategy() => chargeStrategy;
        public string GetPassType() => passType;
    }
}