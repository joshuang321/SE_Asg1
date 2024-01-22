using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    internal class MainActivity : Activity
    {
        public void HandleInput(ActivityStack stckref)
        {
            string? input =Console.ReadLine();
            if (input !=null)
            {
                if (int.TryParse(input, out int actualInput))
                {
                    switch (actualInput)
                    {
                        case 1:
                            stckref.PushActivity(new ApplySeasonPassActivity());
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        case 4:
                            break;
                        case 5:
                            break;
                        case 6:
                            stckref.PopActivity();
                            break;
                        default:
                            PrintError(input);
                            break;
                    }
                }
                else
                {
                    PrintError(input);
                }
                
            }
        }

        void PrintError(string input)
        {
            Console.WriteLine($"Invalid input '{input}', please try again.");
            System.Threading.Thread.Sleep(1000);
        }

        public void HandlePrompt(ActivityStack stkref)
        {
            Console.WriteLine(
                "Parking Management System\n" +
                "==========================\n" +
                "1. Apply Season Pass\n" +
                "2. Renew Season Pass\n" +
                "3. Transfer Season Pass\n" +
                "4. Terminate Season Pass\n" +
                "5. Record Parking\n" +
                "6. Quit");
        }
    }
}
