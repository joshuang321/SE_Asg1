namespace SEAsg1
{
    public class Carpark
    {
        int carparkNo { get; }
        string location { get; }
        string description { get; }

        public Carpark(int carparkNo, string location, string description)
        {
            this.carparkNo = carparkNo;
            this.location = location;
            this.description = description;
        }
    }
}