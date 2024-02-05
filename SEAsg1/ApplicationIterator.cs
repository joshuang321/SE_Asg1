namespace SEAsg1
{
    internal class ApplicationIterator : Iterator
    {

        string type;
        ApplicationCollection colRef;
        int index;

        public ApplicationIterator(ApplicationCollection appCollection,
            string type)
        {
            colRef = appCollection;
            this.type = type;
        }

        public bool HasMore()
        {
            return colRef.Get(index) != null;
        }

        public Collectable Next()
        {
            return (Collectable)colRef.Get(index);
        }

        public void Reset()
        {
            index = 0;
        }
    }
}