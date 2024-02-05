using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SEAsg1
{
    class App
    {
        static public App instance = new App();
        
        List<User> users;
        List<Vehicle> vehicles;
        User? curUser;
        ApplicationCollection apps;
        
        public App()
        {
            users = new List<User>();
            curUser = null;
            vehicles = new List<Vehicle>();
            apps = new ApplicationCollection();

            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_admin", "Admin",
                "97904893"));
            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_user", "User",
                "97904893"));

            vehicles.Add(new Vehicle("SKX 1234 A", "12345678", "Car"));
            vehicles.Add(new Vehicle("FBC 5678 B", "87654321", "Motorcycle"));
#if DEBUG
            SeasonParking pass = new SeasonParking(DateTime.Now.AddMonths(-4),
                    DateTime.Now.AddMonths(-1),
                    vehicles[0],
                    new MonthlyPass());
            users[1].AddPass(pass);

            pass = new SeasonParking(DateTime.Now.AddMonths(-3),
                DateTime.Now.AddMonths(2),
                vehicles[1],
                new DailyPass());
            users[1].AddPass(pass);

            apps.Add(new Application(users[1], new Vehicle("GHA 9012 C",
                "23456789", "Bus"),
                DateTime.Now, DateTime.Now.AddMonths(5),
                new MonthlyPass(),
                "Debit Card",
                "Monthly"));
#endif
        }

        public void Run()
        {
            bool isRunning = true;
            while (isRunning)
            {
                while (Login()) ;
                bool doOptLoop = true;
                while (doOptLoop)
                {
                    int op =  SelectOption();
                    switch (op)
                    {
                        case 1:
                            ApplyPass();
                            break;
                        case 2:
                            RenewPass();
                            break;
                        case 3:
                            TerminatePass();
                            break;
                        case 4:
                            TransferPass();
                            break;
                        case 5:
                            ProcessApplication();
                            break;
                        case 6:
                            doOptLoop = false;
                            Logout();
                            break;
                    }
                }
            }
        }

        public bool Login()
        {
            Console.Clear();
            Console.WriteLine("Enter username: ");
            string? username =Console.ReadLine();

            Console.WriteLine("Enter password:");
            string? password = Console.ReadLine();

            if (username ==password && password == null)
            {
                Console.WriteLine("Invalid input! Try Again.");
                return false;
            }
            User? user =users.Find((User u) =>
            {
                if (u.GetPassword() == password
                && u.GetUsername() == username)
                    return true;
                return false;
            });
            if (user==null)
            {
                Console.WriteLine("Failed to login, invalid username and/or password!");
            }
            else
            {
                Console.WriteLine($"Successfully Logged in as {username}.");
            }
            Thread.Sleep(500);
            return true;
        }

        public int SelectOption()
        {
            Console.Clear();
            Console.WriteLine("Season Parking System\n" +
                              "=====================\n" +
                              "1. Apply Pass\n" +
                              "2. Renew Pass\n" +
                              "3. Terminate Pass\n" +
                              "4. Transfer Pass\n" +
                              "5. Process Application\n" +
                              "6. Logout\n\n" +
                              "Your choice? ");
            string? option = Console.ReadLine();
            if (int.TryParse(option, out int op))
            {
                return op;
            }
            else
            {
                Console.WriteLine("Invalid Option! Try Again.");
            }
            return -1;
        }

        public void ApplyPass()
        {
            Console.Clear();
        }

        public void RenewPass()
        {
            Console.Clear();
        }

        public void TerminatePass()
        {
            Console.Clear();
        }

        public void TransferPass()
        {
            Console.Clear();
        }

        public void ProcessApplication()
        {
            Console.Clear();
        }

        public void Logout()
        {
            Console.Clear();
            curUser = null;
        }
    }
}
