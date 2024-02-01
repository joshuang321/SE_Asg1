namespace SEAsg1
{
    public class Vehicle
    {
        string licensePlate { get; }
        string IUNumber { get; }
        string vehicleType { get; }

        private SeasonPass? pass;

        public SeasonPass? Pass { get { return pass; } }

        void AssignPass(SeasonPass pass)
        {
            if (pass!=null)
            {
                this.pass = pass;
            }
        }

        void RemovePass()
        {
            this.pass = null;
        }

        public static void TransferPass(Vehicle vehicle1, Vehicle vehicle2)
        {
            SeasonPass pass =vehicle1.pass;
            vehicle1.RemovePass();
            vehicle2.AssignPass(pass);
        }
    }
}