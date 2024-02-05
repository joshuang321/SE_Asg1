namespace SEAsg1
{
    public class Vehicle
    {
        string licensePlate;
        string IUNumber;
        string type;
        string status = "Not Parked";
        SeasonParking? pass;

        public Vehicle(string plate, string IUNumber, string type)
        {
            this.licensePlate = plate;
            this.IUNumber = IUNumber;
            this.type = type;
        }

        public void SetPass(SeasonParking? pass)
        {
           this.pass = pass;
        }

        public string GetPlate() => licensePlate;
        public string GetIUNumber() => IUNumber;
        public string GetVehicleType() => type;
        public SeasonParking? GetPass() => pass;
        public bool IsParked() => status == "Parked";
        public void Exit() => status = "Not Parked";
    }
}