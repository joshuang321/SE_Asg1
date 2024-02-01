namespace SEAsg1
{
    public class SeasonParking
    {
        int parkingId;
        DateTime entryTime;
        DateTime exitTime;
        float amountCharged;
        Vehicle vehicle { get; }
        SParkingState curState;


        public TimeSpan GetTimeParked()
        {
            return exitTime.Subtract(entryTime);
        }

        public void Exit()
        {
            curState.Exit();
        }
    }
}