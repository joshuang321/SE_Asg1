namespace SEAsg1
{
    public interface PassState
    {
        public void Renew(DateTime newEndDate);
        public void Terminate();
        public void Transfer(Vehicle vehicle);
    }
}