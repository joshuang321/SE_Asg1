using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    internal interface Activity
    {
        public void HandlePrompt(ActivityStack stkref);
        public void HandleInput(ActivityStack stckref);
    }
}
