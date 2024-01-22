namespace SEAsg1
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ActivityStack stk = ActivityStack.instance;
            stk.PushActivity(new MainActivity());

            stk.MainLoop();
        }
    }
}
