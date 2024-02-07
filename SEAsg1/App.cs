using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SEAsg1
{
    class App
    {
        static public App instance = new App();
        
        List<User> users;
        List<SeasonParking> seasonPasses;
        List<Vehicle> vehicles;
        List<Carpark> carparks;
        User? curUser;
        ApplicationCollection apps;

        readonly Regex vehicleNumFormat = new Regex(@"[A-Z]{3} [0-9]{4} [A-Z]{1}");
        readonly Regex vehicleIUFormat = new Regex(@"[0-9]{8}");
        readonly List<string> vehicleTypes = new List<string>
        {
            "Car", "Motorcycle", "Bus", "Van"
        };
        
        App()
        {
            users = new List<User>();
            vehicles = new List<Vehicle>();
            carparks = new List<Carpark>();
            seasonPasses = new List<SeasonParking>();
            curUser = null;
            apps = new ApplicationCollection(0);

            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_admin", "Admin",
                "97904893"));
            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_user", "User",
                "97904893"));

            vehicles.Add(new Vehicle("SKX 1234 A", "12345678", "Car"));
            vehicles.Add(new Vehicle("FBC 5678 B", "87654321", "Motorcycle"));
            vehicles.Add(new Vehicle("SJK 7890 E", "34567890", "Car"));
            //renew season pass variables
            users.Add(new User("Tan Zhi Yuan", "1", "1", "User",
                "93896816"));
/*            users[2].AddPass(new SeasonParking(DateTime.Now.AddMonths(-3), DateTime.Now.AddMonths(-1), vehicles[0],null,null));
            users[2].AddPass(new SeasonParking(DateTime.Now.AddMonths(-24), DateTime.Now.AddMonths(-6), vehicles[2],null,null));
            users[2].AddPass(new SeasonParking(DateTime.Now.AddMonths(-24), DateTime.Now, vehicles[2],null,null));*/
            //renew season pass variables

#if DEBUG
            apps.ApprovePass(new Application(users[1], new Vehicle("GHA 9012 C",
                "23456789", "Bus"),
                DateTime.Now, DateTime.Now.AddMonths(5),
                new MonthlyPass(),
                "Debit Card",
                "Monthly"));
            apps.Add(new Application(users[1], new Vehicle("SJK 7890 E",
                "34567890", "Car"),
                DateTime.Now, DateTime.Now.AddMonths(5),
                new DailyPass(),
                "Debit Card",
                "Daily"));
            //renew season pass
            apps.Add(new Application(users[2], new Vehicle("SGP 6816 A",
                "00000000", "Car"),
                DateTime.Now, DateTime.Now,
                new MonthlyPass(),
                "Debit Card",
                "Monthly"));
            apps.Add(new Application(users[2], new Vehicle("SGP 9389 A",
                "11111111", "Car"),
                DateTime.Now.AddMonths(-1), DateTime.Now.AddMonths(-1),
                new MonthlyPass(),
                "Debit Card",
                "Monthly"));



            carparks.Add(new Carpark("SKX 12A", "Jurong West", "A multi-storey carpark near the Jurong Point Shopping Centre, with 500 lots and electronic parking system."));
            carparks.Add(new Carpark("FBC 34B", "Orchard Road", "A basement carpark under the Paragon Mall, with 800 lots and gantry parking system."));
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
                            StringBuilder allPassesString = new StringBuilder(string.Empty); 
                            while(curUser!.GetPasses().MoveNext())
                            {
                                allPassesString.Append(curUser!.GetPasses().Current);
                            }
                            Console.WriteLine(allPassesString); 
                            break;
                        case 7:
                            
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
            /* 
            if (curUser!.GetRole() == "Admin")
            {
                Console.WriteLine("5. Process Application");
            }
            */
            Console.WriteLine("6. View season pass"); 
            Console.WriteLine("7. Logout\n\n" +
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

        /// <summary>
        ///     The user is able to apply for a new season pass
        /// </summary>
        void ApplyPass()
        {
            // get all the information from curUser  
            // User class constructor format -> User(string name, string password, string username, string role,string phoneNumber)
            Action CreateVehicleAddSinglePass = new Action(delegate ()
            {
                ChargeStrategy chargingStrategy;
                string? newVehicleCarPlateNumber;
                string? newVehicleIU;
                string? newVehicleType;
                for (; ; )
                {
                    Console.WriteLine("Welcome to the season pass registration service. To begin, select the type of season pass that you would like to apply for: ");
                    Console.WriteLine("[0] Quit to previous menu");
                    Console.WriteLine("[1] Monthly");
                    Console.WriteLine("[2] Daily");
                    Console.Write("Your choice? ");
                    int choice;
                    int.TryParse(Console.ReadLine(), out choice);
                    if (choice.Equals(1))
                    {
                        chargingStrategy = new MonthlyPass();
                        break;
                    }
                    else if (choice.Equals(2))
                    {
                        chargingStrategy = new DailyPass();
                        break;
                    }
                    else if (choice.Equals(0))
                    {
                        //quit the function
                        return;
                    }
                    else
                    {
                        Console.Error.WriteLine("Invalid option! Please try again!");
                        Thread.Sleep(2000);
                    }
                    Console.Clear();
                }
                Console.Clear();
                // Prompt the user for the vehicle car plate number
                for (; ; )
                {
                    Console.Write("Excellent! Now please enter your car plate number of the vehicle: ");
                    newVehicleCarPlateNumber = Console.ReadLine()!.ToUpper();

                    Vehicle? existingVehicle = vehicles.Find(delegate (Vehicle v)
                    {
                        return v.GetPlate().Equals(newVehicleCarPlateNumber);
                    });

                    if (!vehicleNumFormat.Match(newVehicleCarPlateNumber!).Success)
                    {
                        Console.Error.WriteLine("Sorry the format of the car plate number is not correct. Please try again!");
                        Thread.Sleep(2000);
                    }
                    else if (existingVehicle != null)
                    {
                        Console.Error.WriteLine("Sorry, this vehicle already exists in the system. Please enter another car plate number!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.Clear();
                //Now get the IU of the vehicle
                for (; ; )
                {
                    Console.Write("Good! Now please enter the IU of your vehicle: ");
                    newVehicleIU = Console.ReadLine();

                    if (!vehicleIUFormat.Match(newVehicleIU!).Success)
                    {
                        Console.Error.WriteLine("Sorry the format of the vehicle IU is not correct. Please try again!");
                        Thread.Sleep(2000);
                    }
                    else if (string.IsNullOrEmpty(newVehicleIU!))
                    {
                        Console.Error.WriteLine("Please enter a vehicle IU!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.Clear();
                //Now get the type of the Vehicle
                for (; ; )
                {
                    Console.Write("Great! Now enter the type of the vehicle that you own");
                    newVehicleType = Console.ReadLine();

                    newVehicleType = newVehicleType![0].ToString().ToUpper() + newVehicleType.Substring(1, newVehicleType.Length - 1).ToLower();

                    if (!vehicleTypes.Contains(newVehicleType))
                    {
                        Console.Error.WriteLine("This vehicle type does not exist!");
                        Thread.Sleep(2000);
                    }
                    else if (string.IsNullOrEmpty(newVehicleType))
                    {
                        Console.Error.WriteLine("Please enter a vehicle type!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        break;
                    }
                    Console.Clear();
                }
                //Construct the vehicle object using the given information
                Vehicle newVehicle = new Vehicle(newVehicleCarPlateNumber!, newVehicleIU!, newVehicleType!);
                //Construct new season pass for the user and then bind it to a vehicle
                SeasonParking newSeasonParkingPass = new SeasonParking(DateTime.Now,
                    DateTime.Now.AddMonths(1), newVehicle, chargingStrategy);
                //add the season pass to the user's list of SeasonParking 
                curUser!.AddPass(newSeasonParkingPass);

                Console.WriteLine("Vehicle has been created! season pass created and bound to vehicle!");
                Thread.Sleep(2000);
                Console.Clear();
            });

            Action CreateNewSeasonPassAndAttachToExistingVehicle = new Action(delegate ()
            {
                ChargeStrategy chargingStrategy;
                string? existingVehicleCarPlateNumber;
                Vehicle? existingVehicle;
                for (; ; )
                {
                    Console.WriteLine("Welcome to the season pass registration service. To begin, select the type of season pass that you would like to apply for: ");
                    Console.WriteLine("[0] Quit to previous menu");
                    Console.WriteLine("[1] Monthly");
                    Console.WriteLine("[2] Daily");
                    Console.Write("Your choice? ");
                    int choice;
                    int.TryParse(Console.ReadLine(), out choice);
                    if (choice.Equals(1))
                    {
                        chargingStrategy = new MonthlyPass();
                        break;
                    }
                    else if (choice.Equals(2))
                    {
                        chargingStrategy = new DailyPass();
                        break;
                    }
                    else if (choice.Equals(0))
                    {
                        //quit the function
                        return;
                    }
                    else
                    {
                        Console.Error.WriteLine("Invalid option! Please try again!");
                        Thread.Sleep(2000);
                    }
                    Console.Clear();
                }
                Console.Clear();
                //prompt the user for his vehicle only the car plate number
                // Prompt the user for the vehicle car plate number
                for (; ; )
                {
                    Console.Write("Excellent! Now please enter your car plate number of the vehicle: ");
                    existingVehicleCarPlateNumber = Console.ReadLine()!.ToUpper();

                    existingVehicle = vehicles.Find(delegate (Vehicle v)
                    {
                        return v.GetPlate().Equals(existingVehicleCarPlateNumber);
                    });

                    if (!vehicleNumFormat.Match(existingVehicleCarPlateNumber!).Success)
                    {
                        Console.Error.WriteLine("Sorry the format of the car plate number is not correct. Please try again!");
                        Thread.Sleep(2000);
                    }
                    else if (existingVehicle == null)
                    {
                        Console.Error.WriteLine("Sorry, this vehicle does not exist in the system. Please enter another car plate number!");
                        Thread.Sleep(2000);
                    }
                    else if (existingVehicle!.GetPass() != null)
                    {
                        Console.Error.WriteLine("Sorry, a season pass has already been bound to this vehicle. Please enter another car plate number!");
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        break;
                    }
                    Console.Clear();
                }
                //create new season pass for the user, add it to the existing vehicle, and then add to the user's list of season passes
                SeasonParking newSeasonParkingPass = new SeasonParking(
                    DateTime.Now,
                    DateTime.Now.AddMonths(1),
                    existingVehicle,
                    chargingStrategy
                );
                curUser!.AddPass(newSeasonParkingPass);

                Console.WriteLine("Season pass has been created! season pass created and bound to vehicle!");
                Thread.Sleep(2000);
                Console.Clear();
            });

            //sub procedure start
            for (; ; )
            {
                Console.WriteLine("[0] Quit");
                Console.WriteLine("[1] Register new vehicle and new season pass");
                Console.WriteLine("[2] Register new season pass and add it to an existing vehicle");
                Console.Write("Your choice? ");
                int decision;
                int.TryParse(Console.ReadLine(), out decision);
                if (decision.Equals(0))
                {
                    //quit the function entirely
                    Console.Clear();
                    return;
                }
                else if (decision.Equals(1))
                {
                    CreateVehicleAddSinglePass.Invoke();
                    char choice;
                    for (; ; )
                    {
                        Console.Write("Would you like to repeat the above step? (Y/N)");
                        choice = Convert.ToChar(Console.ReadKey().KeyChar.ToString().ToUpper());

                        if (choice.Equals('Y'))
                        {
                            CreateVehicleAddSinglePass.Invoke();
                        }
                        else if (choice.Equals('N'))
                        {
                            break;
                        }
                        else
                        {
                            Console.Error.WriteLine("Invalid option please try again!");
                            Thread.Sleep(2000);
                        }
                        Console.Clear();
                    }
                    Console.Clear();
                }
                else if (decision.Equals(2))
                {
                    CreateNewSeasonPassAndAttachToExistingVehicle.Invoke();
                    char choice;
                    for (; ; )
                    {
                        Console.Write("Would you like to repeat the above step? (Y/N)");
                        choice = Convert.ToChar(Console.ReadKey().KeyChar.ToString().ToUpper());

                        if (choice.Equals('Y'))
                        {
                            CreateNewSeasonPassAndAttachToExistingVehicle.Invoke();
                        }
                        else if (choice.Equals('N'))
                        {
                            break;
                        }
                        else
                        {
                            Console.Error.WriteLine("Invalid option please try again!");
                            Thread.Sleep(2000);
                        }
                        Console.Clear();
                    }
                    Console.Clear();
                }
                else
                {
                    Console.Error.WriteLine("Invalid option! Please try again!");
                    Thread.Sleep(2000);
                }
                Console.Clear();
            }
        }

        void RenewPass()
        {
            {
                Console.WriteLine();

                // Retrieve the user's existing season parking passes

                List<SeasonParking> userPasses = new List<SeasonParking>();
                var passesEnumerator = curUser!.GetPasses();

 

                while (passesEnumerator.MoveNext())
                {
                    userPasses.Add(passesEnumerator.Current);
                }

                if (userPasses.Count <=0)
                {
                    Console.WriteLine("This user does not vehicles that have season passes. Please apply for a pass.");
                    Thread.Sleep(2000);
                    return;

                }



                // Display the user's existing passes and prompt for renewal

                Console.WriteLine("Your current season parking passes:\n");
                foreach (SeasonParking pass in userPasses)
                {
                    Console.WriteLine("----------------------------------------------");
                    Console.WriteLine($"Plate Number: {pass.GetVehicle().GetPlate()}");
                    Console.WriteLine($"Pass Type: {pass.GetType()}");
                    Console.WriteLine($"Start Date: {pass.GetStartDate()}");
                    Console.WriteLine($"End Date: {pass.GetEndDate()}\n");
                }

                Console.WriteLine("----------------------------------------------");


                Console.WriteLine("Enter the plate number of the vehicle you want to renew the pass for:\n");
                string plateNumber = Console.ReadLine().ToUpper();


                // Find the pass associated with the provided plate number
                SeasonParking passToRenew = userPasses.Find(pass => pass.GetVehicle().GetPlate() == plateNumber);

                // Check if the pass exists
                if (passToRenew == null)
                {
                    Console.WriteLine($"No season pass found for vehicle with plate number {plateNumber}.");
                    Thread.Sleep(2000);
                    return;
                }

                // Check if the pass is already expired
                else if (passToRenew.GetEndDate() < DateTime.Now)
                {
                    Console.WriteLine("This pass has already expired. Please apply for a new pass.");
                    Thread.Sleep(2000);
                    return;
                }

                // Calculate the new end date for the renewed pass (extend by one month)
                DateTime newEndDate = passToRenew.GetEndDate().AddMonths(1);

                // Update the pass 
                passToRenew.Renew(newEndDate);

                // Display the renewed pass details
                Console.WriteLine($"Season pass renewed successfully for vehicle with plate number <{plateNumber}>.\n");
                Console.WriteLine($"New End Date: {newEndDate}");
                Thread.Sleep(3000);


            }


        }

        void TerminatePass()
        {
            List<SeasonParking> monthlypasses = new List<SeasonParking>();
            int counter = 0;
            while (curUser.GetPass(counter) != null)
            {
                if (curUser.GetPass(counter).GetChargeStrategy().GetType().Name.ToString() == "MonthlyPass" && DateTime.Now.Date < curUser.GetPass(counter).GetEndDate().Date)
                {
                    monthlypasses.Add(curUser.GetPass(counter));
                }
                counter++;
            }
            //IEnumerator<SeasonParking> userPasses = curUser.GetPasses();
            //if (userPasses.Current != null) 
            //{
            //    if (userPasses.Current.GetChargeStrategy().GetType().ToString() == "MonthlyPass" && DateTime.Now < Convert.ToDateTime(userPasses.Current.GetEndDate))
            //    {
            //        monthlypasses.Add(userPasses.Current);
            //    }
            //    while (userPasses.MoveNext())
            //    {
            //        if (userPasses.Current != null)
            //        {
            //            if (userPasses.Current.GetChargeStrategy().GetType().ToString() == "MonthlyPass" && DateTime.Now < Convert.ToDateTime(userPasses.Current.GetEndDate))
            //            {
            //                monthlypasses.Add(userPasses.Current);
            //            }
            //        }
            //    }
            //}
            if (monthlypasses.Count == 0) 
            {
                Console.WriteLine("You do not have any valid monthly passes to terminate.");
                Thread.Sleep(2000);
            }
            else
            {
                int count = 0;
                Console.WriteLine("{0,-5}{1,-12}{2,-27}", "", "License No", "End Date");
                foreach (SeasonParking s in monthlypasses)
                {
                    count++;
                    Vehicle v = s.GetVehicle();
                    Console.WriteLine("{0,-5}{1,-12}{2,-27}", (count + ")"), v.GetPlate(), s.GetEndDate());
                }
                Console.Write("Please select a monthly pass to terminate: ");
                int input = Convert.ToInt32(Console.ReadLine());
                //userPasses = curUser.GetPasses();
                int track = 0;
                counter = 0;
                while (curUser.GetPass(counter) != null)
                {
                    if (curUser.GetPass(counter).GetChargeStrategy().GetType().Name.ToString() == "MonthlyPass" && DateTime.Now.Date < curUser.GetPass(counter).GetEndDate().Date)
                    {
                        track++;
                        if (track == input)
                        {
                            string reason = "";
                            while (reason == "")
                            {
                                Console.Write("Please provide a reason for termination: ");
                                reason = Convert.ToString(Console.ReadLine());
                                if (reason != "")
                                {
                                    for (int i = 0; i < users.Count; i++)
                                    {
                                        if (users[i].GetId == curUser.GetId)
                                        {
                                            curUser.GetPass(counter).Terminate();
                                            users[i].Remove(curUser.GetPass(counter));
                                            Console.WriteLine("Monthly pass has been successfully terminated.");
                                            Thread.Sleep(2000);
                                            break;
                                        }
                                    }
                                    break;
                                }
                            }
                        }
                    }
                    counter++;
                }
            }
            //Need to update available total monthly season passes to incease by 1
            Console.Clear();
        }

        /// <summary>
        ///    Allows the user to be able to transfer a valid season pass from one vehicle to another
        /// </summary>
        void TransferPass()
        {
            //Implementation for transfer of season pass as follows for the user
            string? vehicleNum;
            string? newVehicleNum;
            string? newVehicleType;
            string? newVehicleIU;
            string vehicleType;
            Vehicle? targetVehicle;
            List<string> vehicleNums = vehicles.Select<Vehicle, string>(v => {
                return v.GetPlate();
            }).ToList<string>();

            Console.Clear(); 

            for (; ; )
            {
                Console.Write("Please enter your vehicular number in the correct format: ");

                vehicleNum = Console.ReadLine()!.ToUpper(); 
                
                if (!vehicleNums.Contains(vehicleNum!))
                {
                    Console.Error.WriteLine("Specified vehicle is not found within the system please try again!");
                    Thread.Sleep(2000);
                }
                else if (string.IsNullOrEmpty(vehicleNum!))
                {
                    Console.Error.WriteLine("Please specify a vehicle number!");
                    Thread.Sleep(2000);
                }
                else
                {
                    break;
                }
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine($"Found vehicle with number {vehicleNum!}");
            targetVehicle = vehicles.Find(v => { return v.GetPlate().Equals(vehicleNum); });
            vehicleType = targetVehicle!.GetVehicleType();
            Console.Clear();

            for (; ; )
            {
                Console.Write("Great! Now please enter your new vehicular number in the correct format: ");

                newVehicleNum = Console.ReadLine()!.ToUpper();

                if (vehicleNums.Contains(newVehicleNum!))
                {
                    Console.Error.WriteLine("Specified vehicle has already been registered into the system. Please try entering another vehicle number!");
                    Thread.Sleep(500);
                }
                else if (string.IsNullOrEmpty(vehicleNum!))
                {
                    Console.Error.WriteLine("Please specify a vehicle number!");
                    Thread.Sleep(500);
                }
                else if (!vehicleNumFormat.Match(newVehicleNum!).Success)
                {
                    Console.Error.WriteLine("Please input a car plate number of a correct format");
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
                Console.Clear();
            }

            Console.Clear();

            for (; ; )
            {
                Console.WriteLine($"The vehicle type of the previous vehicle has been declared as {vehicleType}");
                Console.Write("Excellent! Now please enter the vehicle type of your new vehicle: ");

                newVehicleType = Console.ReadLine();

                newVehicleType = newVehicleType![0].ToString().ToUpper() + newVehicleType.Substring(1, newVehicleType.Length-1).ToLower();

                Console.WriteLine(newVehicleType); 

                if (string.IsNullOrEmpty(newVehicleType!))
                {
                    Console.Error.WriteLine("Please enter a vehicle type!");
                    Thread.Sleep(500);
                }
                else if (!newVehicleType.Equals(vehicleType))
                {
                    Console.Error.WriteLine("Vehicle types do not match.");
                    Thread.Sleep(500);
                }
                else
                {
                    break;
                }
                Console.Clear();
            }

            Console.Clear();

            for (; ; )
            {
                Console.Write("Good. Now enter the IU of your new vehicle: ");

                newVehicleIU = Console.ReadLine()!.ToUpper();

                if (!vehicleIUFormat.Match(newVehicleIU!).Success)
                {
                    Console.Error.WriteLine("The format for the vehicle IU is not correct please try again!");
                }
                else
                {
                    break;
                }
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine("Are you sure you want to transfer your pass to the new vehicle? (Y/N)");
            char choice;

            for (; ; )
            {
                Console.Write("Your choice: ");
                choice = Convert.ToChar(Console.ReadLine()!);

                if (choice.Equals('N'))
                {
                    Console.WriteLine("Process aborted! Season pass not transferred");
                    return;
                }
                else
                {
                    //update the pass as requested
                    SeasonParking? pass = targetVehicle.GetPass();
                    //check if the pass is still valid before transferring
                    if (pass != null && pass!.GetEndDate() > DateTime.Now) //pass expiry date must be later than or equal to the current date and time
                    {
                        vehicles.Add(new Vehicle(newVehicleNum!, newVehicleIU!, newVehicleType!));
                        Vehicle newVehicle = vehicles.Find(delegate (Vehicle v) {
                            return v.GetPlate().Equals(newVehicleNum);
                        })!;
                        newVehicle.SetPass(pass);
                        vehicles.Remove(targetVehicle);
                        Console.WriteLine($"Season pass transferred successfully to vehicle {newVehicleNum} :)");
                        Thread.Sleep(500);
                    }
                    else
                    {
                        Console.Error.WriteLine("Unable to transfer pass to vehicle. No season pass detected or the season pass has already expired");
                        Thread.Sleep(500);
                        return; 
                    }
                    Console.Clear(); 
                }
            }
        }

        void ProcessApplication()
        {
            int op=-1;
            bool continueApp = true;
            Iterator iter =apps.CreateIterator();
            ApplicationCollection removedApps = new ApplicationCollection();

            Console.Clear();
            while (iter.HasMore() && continueApp)
            {
                bool continuePrompt = true;
                Application app =(Application)iter.Next();
                while (continueApp && continuePrompt)
                {
                    Console.Clear();
                    Console.WriteLine($"User: {app.GetUser().GetUsername()}\n\n" +
                                      $"Vehicle Details:\n" +
                                      $"\tPlate Number: {app.GetVehicle().GetPlate()}\n" +
                                      $"\tIUNumber: {app.GetVehicle().GetIUNumber()}\n" +
                                      $"\tVehicle Type: {app.GetVehicle().GetVehicleType()}\n" +
                                      $"\nPass Type: {app.GetPassType()}\n" +
                                      $"\nStart Date: {app.GetStartMonth()}\n" +
                                      $"End Date: {app.GetEndMonth()}\n");


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
                                goto case 2;

                            case 2:
                                continuePrompt = false;
                                removedApps.Add(app);
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
            iter = removedApps.CreateIterator();
            while (iter.HasMore())
            {
                Application app = (Application)iter.Next();
                apps.Remove(app);
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
