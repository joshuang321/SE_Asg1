namespace SEAsg1
{
    public class Carpark
    {
        string carparkNo { get; }
        string location { get; }
        string description { get; }

        public Carpark(string carparkNo, string location, string description)
        {
            this.carparkNo = carparkNo;
            this.location = location;
            this.description = description;
        }

        string GetNumber() => carparkNo;
        string GetLocation() => location;
        string GetDesc() => description;
    }
}