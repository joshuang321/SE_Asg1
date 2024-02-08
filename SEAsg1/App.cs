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
            apps = new ApplicationCollection(5);

            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_admin", "Admin",
                "97904893"));
            users.Add(new User("Joshua Ng", "password123!", "joshua_ng_user", "User",
                "97904893"));

            vehicles.Add(new Vehicle("SKX 1234 A", "12345678", "Car"));
            vehicles.Add(new Vehicle("FBC 5678 B", "87654321", "Motorcycle"));
            //renew season pass variables
            users.Add(new User("Tan Zhi Yuan", "1", "1", "User",
                "93896816"));
            apps.ApprovePass(new Application(users[0], new Vehicle("GHA 9012 C",
                "23456789", "Bus"),
                DateTime.Now, DateTime.Now.AddMonths(5),
                new MonthlyPass(),
                "Debit Card",
                "Monthly"));
            apps.ApprovePass(new Application(users[0], new Vehicle("SJK 7890 E",
                "34567890", "Car"),
                DateTime.Now, DateTime.Now.AddMonths(5),
                new DailyPass(),
                "Debit Card",
                "Daily"));
            apps.ApprovePass(new Application(users[0], new Vehicle("SRT 7890 Y",
                "54565890", "Car"),
                DateTime.Now.AddMonths(-2), DateTime.Now.AddMonths(-1),
                new DailyPass(),
                "Debit Card",
                "Daily"));

            /*
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
            */
            //carparks.Add(new Carpark("SKX 12A", "Jurong West", "A multi-storey carpark near the Jurong Point Shopping Centre, with 500 lots and electronic parking system."));
            //carparks.Add(new Carpark("FBC 34B", "Orchard Road", "A basement carpark under the Paragon Mall, with 800 lots and gantry parking system."));
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
                        // Not needed, why?
                        /*
                        case 6:

                            StringBuilder allPassesString = new StringBuilder(string.Empty); 
                            while(curUser!.GetPasses().MoveNext())
                            {
                                allPassesString.Append(curUser!.GetPasses().Current);
                            }
                            Console.WriteLine(allPassesString); 
                            break;
                        */
                        //case 7:
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
            Console.Write("Enter username: ");
            string? username =Console.ReadLine();

            Console.Write("Enter password: ");
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
            //Console.WriteLine("6. View season pass"); 
            Console.Write("6. Logout\n\n" +
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

        // NOTE: If this was a company, I would fire you immediately, but I am too tired and half
        // your code is almost correct.
        void ApplyPass()
        {
            Console.Clear();
            // NOTE: WHY! Please DO NOT put stupid lambdas the size of functions.
            // You know I don't like lamdas because curly brackets means the start and 
            // end of the functions for me!
            // Please do not write like a stupid javascript script kid.

            // get all the information from curUser  
            // User class constructor format -> User(string name, string password, string username, string role,string phoneNumber)
            /*Action CreateVehicleAddSinglePass = new Action(delegate ()
            {*/
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
                        Thread.Sleep(1000);
                    }
                    Console.Clear();
                }
                Console.Clear();
                // Prompt the user for the vehicle car plate number
                for (; ; )
                {
                    Console.Write("Please enter your car plate number of the vehicle: ");
                    newVehicleCarPlateNumber = Console.ReadLine()!.ToUpper();

                    Vehicle? existingVehicle = vehicles.Find(delegate (Vehicle v)
                    {
                        return v.GetPlate().Equals(newVehicleCarPlateNumber);
                    });

                    // NOTE: Regex matching not needed and not working!
                    if (!vehicleNumFormat.Match(newVehicleCarPlateNumber!).Success)
                    {
                        Console.Error.WriteLine("Format of the car plate number is not correct. Please try again!");
                        Thread.Sleep(1000);
                    }
                    else if (existingVehicle != null)
                    {
                        Console.Error.WriteLine("Vehicle already exists in the system. Please enter another car plate number!");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                    Console.Clear();
                }
                Console.Clear();
                //Now get the IU of the vehicle
                
                // NOTE: while with true does the same thing
                // I said this a couple of times to you specifically
                // you do not understand the significance of the code
                // to write this.
                // Please just write more normally.
                //for (; ; )
                while (true)
                {
                    Console.Write("Good! Now please enter the IU of your vehicle: ");
                    newVehicleIU = Console.ReadLine();

                    if (!vehicleIUFormat.Match(newVehicleIU!).Success)
                    {
                        Console.Error.WriteLine("Format of the vehicle IU is not correct. Please try again!");
                        Thread.Sleep(1000);
                    }
                    else if (string.IsNullOrEmpty(newVehicleIU!))
                    {
                        Console.Error.WriteLine("Please enter a vehicle IU!");
                        Thread.Sleep(1000);
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
                    Console.Write("Enter the type of your vehicle:");
                    newVehicleType = Console.ReadLine();

                    newVehicleType = newVehicleType![0].ToString().ToUpper() + newVehicleType.Substring(1, newVehicleType.Length - 1).ToLower();

                    if (!vehicleTypes.Contains(newVehicleType))
                    {
                        Console.Error.WriteLine("This vehicle type does not exist!");
                        Thread.Sleep(1000);
                    }
                    else if (string.IsNullOrEmpty(newVehicleType))
                    {
                        Console.Error.WriteLine("Please enter a vehicle type!");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                    Console.Clear();
                }
            {

                bool success = false;
                int months = 0;
                string? str = null; 
                while (!success)
                {
                    Console.Write("Amount of months (more than 0): ");
                    str =Console.ReadLine();
                    success = int.TryParse(str, out months);
                    if (!success && months >0)
                    {
                        Console.WriteLine("Invalid Input! Please Try Again.");
                        Thread.Sleep(1000);
                        Console.Clear();
                    }
                }
                success = false;
                str = null;
                while (!success)
                {
                    Console.Write("Payment Mode: ");
                    str = Console.ReadLine();
                    success = str != null;
                }
                apps.Add(new Application(curUser, new Vehicle(newVehicleCarPlateNumber, newVehicleIU, newVehicleType),
                    DateTime.Now, DateTime.Now.AddMonths(months), chargingStrategy, str, chargingStrategy.GetType() == typeof(MonthlyPass)
                       ? "Monthly" : "Daily"));

                Console.WriteLine("Pass application will be approved by admin. Please wait.");
                // NOTE: Please use shorter names next time.

                /*
                //Construct the vehicle object using the given information
                Vehicle newVehicle = new Vehicle(newVehicleCarPlateNumber!, newVehicleIU!, newVehicleType!);
                //Construct new season pass for the user and then bind it to a vehicle
                SeasonParking newSeasonParkingPass = new SeasonParking(DateTime.Now,
                    DateTime.Now.AddMonths(1), newVehicle, chargingStrategy);
                //add the season pass to the user's list of SeasonParking 
                curUser!.AddPass(newSeasonParkingPass);
                */
  //              Console.WriteLine("Vehicle has been created! season pass created and bound to vehicle!");
//                Thread.Sleep(1000);
    //            Console.Clear();
            //});

            // NOTE: You were not taught this. Why use this? Garbage!
            /*
            Action CreateNewSeasonPassAndAttachToExistingVehicle = new Action(delegate ()
            {*//*
                // NOTE: IS THIS A GOSH DAMN CV PASTE! WHY! Please revise programming 1!
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
                        Thread.Sleep(1000);
                    }
                    Console.Clear();
                }
                Console.Clear();

                // NOTE: You have no idea what you are doing!
                //prompt the user for his vehicle only the car plate number
                // Prompt the user for the vehicle car plate number
                for (; ; )
                {
                    Console.Write("Please enter your car plate number of the vehicle: ");
                    existingVehicleCarPlateNumber = Console.ReadLine()!.ToUpper();

                    existingVehicle = vehicles.Find(delegate (Vehicle v)
                    {
                        return v.GetPlate().Equals(existingVehicleCarPlateNumber);
                    });
                    if (existingVehicle==null)
                    {
                        break;
                    }


                    // NOTE: Bro, keep it simple.
                    /*
                    if (!vehicleNumFormat.Match(existingVehicleCarPlateNumber!).Success)
                    {
                        Console.Error.WriteLine("Sorry the format of the car plate number is not correct. Please try again!");
                        Thread.Sleep(1000);
                    }
                    else if (existingVehicle == null)
                    {
                        Console.Error.WriteLine("Sorry, this vehicle does not exist in the system. Please enter another car plate number!");
                        Thread.Sleep(1000);
                    }
                    else if (existingVehicle!.GetPass() != null)
                    {
                        Console.Error.WriteLine("Sorry, a season pass has already been bound to this vehicle. Please enter another car plate number!");
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        break;
                    }
                    */
                    Console.Clear();
                }

                // NOTE: You have clearly not read the assignment! PLEASE READ!
                //create new season pass for the user, add it to the existing vehicle, and then add to the user's list of season passes
            /*    
            SeasonParking newSeasonParkingPass = new SeasonParking(
                    DateTime.Now,
                    DateTime.Now.AddMonths(1),
                    existingVehicle,
                    chargingStrategy,
                    apps
                );

                curUser!.AddPass(newSeasonParkingPass);

                Console.WriteLine("Season pass has been created! season pass created and bound to vehicle!");
                Thread.Sleep(1000);
                Console.Clear();
            */
            //});

            // NOTE: Understand the mindset, but please understand the system we are trying to build!
            //sub procedure start
            /*
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
                            Thread.Sleep(1000);
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
                            Thread.Sleep(1000);
                        }
                        Console.Clear();
                    }
                    Console.Clear();
                }
                else
                {
                    Console.Error.WriteLine("Invalid option! Please try again!");
                    Thread.Sleep(1000);
                }
                Console.Clear();
            }
            */
        }

        void RenewPass()
        {
            Console.Clear();

            // Retrieve the user's existing season parking passes

            List<SeasonParking> userPasses = new List<SeasonParking>();
            var passesEnumerator = curUser!.GetPasses();
            while (passesEnumerator.MoveNext())
            {
                if (passesEnumerator.Current.IsExpired())
                {
                    userPasses.Add(passesEnumerator.Current);
                }
            }

            if (userPasses.Count <=0)
            {
                Console.WriteLine("This user does not vehicles that have season passes. Please apply for a pass.");
                Thread.Sleep(1000);
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
                Thread.Sleep(1000);
                return;
            }

            /*
            // Check if the pass is already expired
            else if (passToRenew.GetEndDate() < DateTime.Now)
            {
                Console.WriteLine("This pass has already expired. Please apply for a new pass.");
                Thread.Sleep(1000);
                return;
            }
            */

            // NOTE: I gave you a Renew method and you didn't 
            // use it. Like bro, what?

            // Calculate the new end date for the renewed pass (extend by one month)
            //DateTime newEndDate = passToRenew.GetEndDate().AddMonths(1);

            // Update the pass 
            passToRenew.Renew(passToRenew.GetEndDate().AddMonths(1));

            // Display the renewed pass details
            Console.WriteLine($"Season pass renewed successfully for vehicle with plate number <{plateNumber}>.\n");
            Console.WriteLine($"New End Date: {passToRenew.GetEndDate()}");
            Thread.Sleep(3000);
        }

        void TerminatePass()
        {
            Console.Clear();
            List<SeasonParking> monthlypasses = new List<SeasonParking>();
            int counter = 0;
            while (curUser!.GetPass(counter) != null)
            {

                if (curUser!.GetPass(counter).GetChargeStrategy().GetType().Name.ToString() == "MonthlyPass" && curUser!.GetPass(counter).IsExpired() == false)
                {
                    monthlypasses.Add(curUser!.GetPass(counter));
                }
                counter++;
            }
            if (monthlypasses.Count == 0) 
            {
                Console.WriteLine("You do not have any valid monthly passes to terminate.");
                Thread.Sleep(1000);
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
                while (curUser!.GetPass(counter) != null)
                {
                    if (curUser!.GetPass(counter).GetChargeStrategy().GetType().Name.ToString() == "MonthlyPass" && curUser!.GetPass(counter).IsExpired() == false)
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
                                        if (users[i].GetId == curUser!.GetId)
                                        {
                                            curUser!.GetPass(counter).Terminate();
                                            users[i].Remove(curUser!.GetPass(counter));
                                            Console.WriteLine("Monthly pass has been successfully terminated.");
                                            Thread.Sleep(1000);
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

        // NOTE: I am done parsing garbage.
        void TransferPass()
        {

            Console.Clear();
           
            //Implementation for transfer of season pass as follows for the user
            string? vehicleNum;
            string? newVehicleNum;
            string? newVehicleType;
            string? newVehicleIU;
            string vehicleType;
           
            Vehicle? targetVehicle;
            List<string> vehicleNums = new List<string>();
            var iter = curUser!.GetPasses();
            int i = 1;
            while (iter.MoveNext())
            {
                if (!iter.Current.IsTerminated())
                {
                    vehicleNums.Add(iter.Current.GetVehicle().GetPlate());
                    Console.WriteLine($"[{i++}] {iter.Current.GetVehicle().GetPlate()}");
                }
            }

            for (; ; )
            {
                Console.Write("Please enter your plate number in the correct format: ");

                vehicleNum = Console.ReadLine()!.ToUpper(); 
                
                if (!vehicleNums.Contains(vehicleNum!))
                {
                    Console.Error.WriteLine("Specified vehicle is not found within the system please try again!");
                    Thread.Sleep(1000);
                }
                else if (string.IsNullOrEmpty(vehicleNum!))
                {
                    Console.Error.WriteLine("Please specify a plate number!");
                    Thread.Sleep(1000);
                }
                else
                {
                    break;
                }
                Console.Clear();
            }

            Console.Clear();
            Console.WriteLine($"Found vehicle with number {vehicleNum!}");


            //targetVehicle = vehicles.Find(v => { return v.GetPlate().Equals(vehicleNum); });
            targetVehicle = null;
            iter = curUser!.GetPasses();
            while (iter.MoveNext())
            {
                if (iter.Current.GetVehicle().GetPlate() ==vehicleNum)
                {
                    targetVehicle = iter.Current.GetVehicle();
                }
            }



            vehicleType = targetVehicle!.GetVehicleType();
            Console.Clear();

            for (; ; )
            {
                Console.Write("Great! Now please enter your new plate number in the correct format: ");

                newVehicleNum = Console.ReadLine()!.ToUpper();

                if (vehicleNums.Contains(newVehicleNum!))
                {
                    Console.Error.WriteLine("Specified vehicle has already been registered into the system. Please try entering another vehicle number!");
                    Thread.Sleep(1000);
                }
                else if (string.IsNullOrEmpty(vehicleNum!))
                {
                    Console.Error.WriteLine("Please specify a plate number!");
                    Thread.Sleep(1000);
                }
                else if (!vehicleNumFormat.Match(newVehicleNum!).Success)
                {
                    Console.Error.WriteLine("Please input a car plate number of a correct format");
                    Thread.Sleep(1000);
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
                    Thread.Sleep(1000);
                }
                else if (!newVehicleType.Equals(vehicleType))
                {
                    Console.Error.WriteLine("Vehicle types do not match.");
                    Thread.Sleep(1000);
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
                        Thread.Sleep(1000);
                    }
                    else
                    {
                        Console.Error.WriteLine("Unable to transfer pass to vehicle. No season pass detected or the season pass has already expired");
                        Thread.Sleep(1000);
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


                    Console.Write("1. Approve\n" +
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
                                vehicles.Add(app.GetVehicle());
                                continuePrompt = false;
                                removedApps.Add(app);
                                break;

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
                Thread.Sleep(1000);
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
