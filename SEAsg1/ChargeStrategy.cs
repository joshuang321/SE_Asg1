namespace SEAsg1
{
    public interface ChargeStrategy
    {
        float GetMonthlyCost(string vtype);
        float Charge(string vtype, TimeSpan span);
    }
}