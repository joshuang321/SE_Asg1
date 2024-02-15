namespace SEAsg1
{
    public abstract class SeasonPass
    {
        int id;
        string plateNumber;
        int iuid;
        DateTime expiryDate;
        SPassState curState;

        void Renew()
        {

        }

        void Refund()
        {

        }

        void Terminate()
        {

        }

        abstract public float PayParking(SeasonParking parking);
        abstract public float RefundSelf();
    }
}