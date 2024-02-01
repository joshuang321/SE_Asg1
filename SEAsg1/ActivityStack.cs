using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    public class ActivityStack
    {
        public static ActivityStack instance = new ActivityStack();

        Stack<Activity> activities;
        Activity? curActivity = null;

        ActivityStack()
        {
            activities = new Stack<Activity>();
        }
        public void PushActivity(Activity activity)
        {
            activities.Push(activity);
            curActivity = activity;
        }
        public void PopActivity()
        {
            activities.Pop();
            activities.TryPeek(out curActivity);
        }
        public void StartMainLoop()
        {
            while (curActivity !=null)
            {
                curActivity!.HandlePrompt(this);
                curActivity!.HandleInput(this);
                Console.Clear();
            }
        }
    }
}
