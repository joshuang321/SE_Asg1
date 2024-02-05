﻿using System;
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
        
        App()
        {
            users = new List<User>();
            curUser = null;
            vehicles = new List<Vehicle>();
            apps = new ApplicationCollection(0);

            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_admin", "Admin",
                "97904893"));
            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_user", "User",
                "97904893"));

            vehicles.Add(new Vehicle("SKX 1234 A", "12345678", "Car"));
            vehicles.Add(new Vehicle("FBC 5678 B", "87654321", "Motorcycle"));
            //vehicles.Add(new Vehicle("SJK 7890 E", "34567890", "Car"));
            //vehicles.Add(new Vehicle("GDE 2345 F", "45678901", "Motorcycle"));
            //vehicles.Add(new Vehicle("SBC 6789 G", "56789012", "Bus"));
            //vehicles.Add(new Vehicle("FGH 1234 H", "67890123", "Van"));
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
                while (!Login()) ;
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
                            if (curUser!.GetRole() != "Admin")
                            {
                                Console.WriteLine("Invalid Option! Try Again.");
                                break;
                            }
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

        bool Login()
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
                return false;
            }
            Console.WriteLine($"Successfully Logged in as {username}.");
            curUser = user;
            Thread.Sleep(300);
            return true;
        }

        int SelectOption()
        {
            Console.Clear();
            Console.WriteLine("Season Parking System\n" +
                              "=====================\n" +
                              "1. Apply Pass\n" +
                              "2. Renew Pass\n" +
                              "3. Terminate Pass\n" +
                              "4. Transfer Pass");
            if (curUser!.GetRole() == "Admin")
            {
                Console.WriteLine("5. Process Application");
            }
            Console.WriteLine("6. Logout\n\n" +
                              "Your choice? ");
            string? option = Console.ReadLine();
            if (int.TryParse(option, out int op))
            {
                return op;
            }
            else
            {
                Console.WriteLine("Invalid Option! Try Again.");
                Thread.Sleep(300);
            }
            return -1;
        }

        void ApplyPass()
        {
            Console.Clear();
        }

        void RenewPass()
        {
            Console.Clear();
        }

        void TerminatePass()
        {
            Console.Clear();
        }

        void TransferPass()
        {
            Console.Clear();
        }

        void ProcessApplication()
        {
            int op=-1;
            bool continueApp = true;
            Iterator iter =apps.CreateIterator();
            while (iter.HasMore() && continueApp)
            {
                Application app =(Application)iter.Next();
                Console.Clear();
                while (continueApp)
                {

                    Console.WriteLine($"User: {app.GetUser().GetUsername()}\n\n" +
                                      $"Vehicle Details:\n" +
                                      $"\tPlate Number: {app.GetVehicle().GetPlate()}\n" +
                                      $"\tIUNumber: {app.GetVehicle().GetIUNumber()}\n" +
                                      $"\tVehicle Type: {app.GetVehicle().GetVehicleType()}\n" +
                                      $"\nPass Type: {app.GetPassType()}\n" +
                                      $"\nStart Date: {app.GetStartMonth()}\n" +
                                      $"\nEnd Date: {app.GetEndMonth()}\n");


                    Console.WriteLine("1. Approve\n" +
                                      "2. Reject\n" +
                                      "3. Quit\n" +
                                      "\nYour Choice? ");

                    string? choice =Console.ReadLine();
                    if (int.TryParse(choice, out op) && op>0 && op<=3)
                    {
                        switch (op)
                        {
                            case 1:
                                apps.ApprovePass(app);
                                break;

                            case 2:
                                apps.Remove(app);
                                break;

                            case 3:
                                continueApp = false;
                                break;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid Input! Try Again.");
                    }
                }
            }
            if (op!=3)
            {
                Console.WriteLine("No more applications to be approved. Thank you.");
                Thread.Sleep(300);
            }
        }

        void Logout()
        {
            Console.Clear();
            Console.WriteLine("Loggin Out..");
            Thread.Sleep(300);
            curUser = null;
        }
    }
}
