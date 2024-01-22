using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    internal class ActivityStack
    {
        public static ActivityStack instance = new ActivityStack();

        List<Activity> activities;
        ActivityStack()
        {
            activities = new List<Activity>();
        }
        public void PushActivity(Activity activity)
        {
            activities.Add(activity);
        }
        public void PopActivity()
        {
            activities.RemoveAt(activities.Count - 1);
        }
        public void MainLoop()
        {
            while (activities.Count!=0)
            {
                activities[activities.Count - 1].HandlePrompt(this);
                activities[activities.Count - 1].HandleInput(this);
                Console.Clear();
            }
        }
    }
}
