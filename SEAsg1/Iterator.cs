namespace SEAsg1
{
    public interface Iterator
    {
        public Collectable Next();
        public void Reset();
        public bool HasMore();
    }
}