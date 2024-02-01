namespace SEAsg1
{
    public class Program
    {
        static void Main(string[] args)
        {
            ActivityStack stk = ActivityStack.instance;
            stk.PushActivity(new MainActivity());

            stk.StartMainLoop();
        }
    }
}
