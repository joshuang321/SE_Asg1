namespace SEAsg1
{
    public class ValidState : PassState
    {
        SeasonParking pass;
        public ValidState(SeasonParking pass)
        {
            this.pass = pass;
        }

        bool IsExpired()
        {
            return DateTime.Now > pass.GetEndDate();
        }

        public void Renew(DateTime newEndDate)
        {
            if (IsExpired())
            {
                pass.SetState(new ExpiredState(pass));
                pass.Terminate();
                return;
            }
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
            if (IsExpired())
            {
                pass.SetState(new ExpiredState(pass));
                pass.Terminate();
                return;
            }
            float returnAmount=pass.GetChargeStrategy()
                .GetMonthlyCost(pass.GetVehicle()
                .GetVehicleType())
                
                * (pass.GetEndDate().Month - pass.GetStartDate().Month);
            
            Console.WriteLine($"Refunded: ${returnAmount}");
            pass.SetState(new TerminatedState());
        }

        public void Transfer(Vehicle vehicle)
        {
            if (IsExpired())
            {
                this.pass.SetState(new ExpiredState(this.pass));
                this.pass.Terminate();
                return;
            }
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