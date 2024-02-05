namespace SEAsg1
{
    public class ApplicationCollection : Collectable
    {
        List<Application> apps = new List<Application>();

        public Iterator CreateIterator(string type)
        {
            return new ApplicationIterator(this, type);
        }

        public object? Get(int index)
        {
            if (index < apps.Count)
            {
                return apps[index];
            }
            return null;
        }

        public void Remove(int index)
        {
            apps.RemoveAt(index);
        }

        public void Add(Application app)
        {
            apps.Add(app);
        }
    }
}