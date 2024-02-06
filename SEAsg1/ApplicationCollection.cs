namespace SEAsg1
{
    public class ApplicationCollection : Collectable
    {
        int monthlyPassesLeft;
        List<Application> apps = new List<Application>();

        public ApplicationCollection(int amtOfMonthlyPasses)
        {
            monthlyPassesLeft = amtOfMonthlyPasses;
        }

        public ApplicationCollection()
        {
            monthlyPassesLeft = 0;
        }

        public Iterator CreateIterator()
        {
            return new ApplicationIterator(this);
        }

        public bool HasMonthlyLeft() => monthlyPassesLeft > 0;

        public void ApprovePass(Application app)
        {
            if (app.GetPassType() == "Monthly" && monthlyPassesLeft<=0)
            {
                Console.WriteLine("Failed to approve monthly pass. Limit reached.");
            }
            float charge = app.GetCharge();
            Console.WriteLine($"Approved {app.GetPassType()} Season Parking for {charge}.");
            SeasonParking pass =new SeasonParking(app.GetStartMonth(), app.GetEndMonth(),
                app.GetVehicle(),
                app.GetChargeStrategy());

            app.GetUser().AddPass(pass);
            Remove(app);
        }
        
        public object? Get(int index)
        {
            if (index < apps.Count)
            {
                return apps[index];
            }
            return null;
        }

        public void Remove(object app)
        {
            apps.Remove((Application)app);
        }

        public void Add(object app)
        {
            apps.Add((Application)app);
        }
    }
}