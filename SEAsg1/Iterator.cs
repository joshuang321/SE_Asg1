namespace SEAsg1
{
    public interface Iterator
    {
        public object Next();
        public void Reset();
        public bool HasMore();
    }
}