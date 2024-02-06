namespace SEAsg1
{
    internal class ApplicationIterator : Iterator
    {

        ApplicationCollection colRef;
        int index;

        public ApplicationIterator(ApplicationCollection appCollection)
        {
            colRef = appCollection;
            index = -1;
            MoveIterator();
        }

        public bool HasMore()
        {
            return colRef.Get(index) != null;
        }

        public object Next()
        {
            object obj = colRef.Get(index);
            MoveIterator();
            return obj;
        }

        void MoveIterator()
        {
            while (true)
            {
                index++;
                if (colRef.Get(index) == null)
                {
                    break;
                }
                else
                {
                    Application? app = (Application?)colRef.Get(index);
                    if (app!.GetPassType() == "Monthly" && !colRef.HasMonthlyLeft())
                    {
                        continue;
                    }
                    else
                    {
                        break;
                    }

                }
            }
        }
    }
}