using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClassLibrarySpec2012;
using System.Data;

namespace ConsoleApplicationKravD1
{
    class Program
    {
        static List<int> tools = new List<int>();
        static DateTime start = DateTime.Now;
        static DateTime end = DateTime.Now;
        static bool isService = false;

        static int location = -1;
        static int employee = -1;

        static void Main(string[] args)
        {
            displayMenu();
        }

        private static void displayMenu()
        {
            Console.Clear();
            Console.WriteLine("Multibooking program");
            Console.WriteLine("-------------------------");
            Console.WriteLine("t - TOOLS         : " + getTools());
            Console.WriteLine("s - TIME STARTED  : " + start.ToString());
            Console.WriteLine("e - TIME ENDED    : " + end.ToString());

            if (isService)
            {
                Console.WriteLine("i - IS SERVICE    : YES");
            }
            else
            {
                Console.WriteLine("i - IS SERVICE    : NO");
                Console.WriteLine("l - LOCATION      : " + getLocation());
                Console.WriteLine("p - EMPLOYEE      : " + getEmployee());
            }

            Console.WriteLine("x - End program");
            Console.WriteLine("------ c to submit -------");

            string letter = Console.ReadLine();

            switch (letter)
            {
                case "t":
                    selectTools();
                    break;
                case "s":
                    selectStart();
                    break;
                case "e":
                    selectEnd();
                    break;
                case "i":
                    service();
                    break;
                case "x":
                    Environment.Exit(0);
                    break;
            }

            if (!isService)

                switch (letter)
                {
                    case "l":
                        selectLocation();
                        break;
                    case "p":
                        selectEmployee();
                        break;
                }

            if (letter.Equals("c"))
            {
                submit();
            }
            else
            {
                displayMenu();
            }
        }

        private static void service()
        {
            Console.Clear();
            Console.WriteLine("Is this a service reservation? y - n");
            string result = Console.ReadLine();
            if (result.Equals("y")) isService = true;
            else if (result.Equals("n")) isService = false;

            displayMenu();
        }

        private static void selectTools()
        {
            Console.Clear();
            Console.WriteLine("----- Select tools -----");
            try
            {
                foreach (string s in Service.getInstance().getToolsAsStrings())
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("--- Type IDs seperate with ',' ---");

                string[] ids = Console.ReadLine().Split(',');

                List<int> t = new List<int>();

                foreach (string s in ids)
                {
                    t.Add(Convert.ToInt32(s));
                }

                tools = t;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Any key to continue...");
                Console.ReadKey();
            }

            displayMenu();
        }

        private static void submit()
        {
            if (isDataValid())
            {
                try
                {
                    if (isService) Service.getInstance().createMultiToolReservation(start, end, tools, -1, -1);
                    else Service.getInstance().createMultiToolReservation(start, end, tools, employee, location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                    Console.WriteLine("Any key to continue...");
                    Console.ReadKey();
                }
            }
            else
            {
                Console.WriteLine("Any key to continue...");
                Console.ReadKey();
            }
            displayMenu();
        }

        private static bool isDataValid()
        {
            bool result = false;

            if (start > end) Console.WriteLine("Invalid data: Start is later than end");
            else if (start.Hour < 7 || start.Hour > 15) Console.WriteLine("Invalid data: Start hour is out of range");
            else if (end.Hour < 8 || end.Hour > 16) Console.WriteLine("Invalid data: End hour is out of range");
            else if (!isService && employee == -1 || !isService && location == -1) Console.WriteLine("Invalid data: Missing information");
            else if (tools.Count < 1) Console.WriteLine("Invalid data: No tool selected");
            else result = true;

            return result;
        }

        private static void selectStart()
        {
            Console.Clear();
            Console.WriteLine("--- Write starting date ---");
            Console.WriteLine("YEAR-MONTH-DAY ex 2012-05-27");
            Console.WriteLine("---------------------------");

            DateTime sStart;

            try
            {
                sStart = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("--- Write hour to start ---");
                Console.WriteLine("---- 7 to 15 are valid ----");

                sStart = sStart.AddHours(Convert.ToInt32(Console.ReadLine()));

                start = sStart;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Any key to continue...");
                Console.ReadKey();
            }

            displayMenu();
        }

        private static void selectEnd()
        {
            Console.Clear();
            Console.WriteLine("--- Write ending date ---");
            Console.WriteLine("YEAR-MONTH-DAY ex 2012-05-28");
            Console.WriteLine("---------------------------");

            DateTime sEnd;

            try
            {
                sEnd = DateTime.Parse(Console.ReadLine());

                Console.WriteLine("--- Write hour to end ---");
                Console.WriteLine("---- 8 to 16 are valid ----");

                sEnd = sEnd.AddHours(Convert.ToInt32(Console.ReadLine()));

                end = sEnd;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Any key to continue...");
                Console.ReadKey();
            }

            displayMenu();
        }

        private static void selectLocation()
        {
            Console.Clear();
            Console.WriteLine("--- Available locations ---");

            try
            {
                foreach (string s in Service.getInstance().getLocationsAsStrings())
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("--- Type the ID to select ---");


                int locationId = Convert.ToInt32(Console.ReadLine());
                string loc = Service.getInstance().getLocation(locationId);
                if (loc.Equals("")) throw new Exception("Location not found");

                location = locationId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Any key to continue...");
                Console.ReadKey();
            }

            displayMenu();
        }

        private static void selectEmployee()
        {
            Console.Clear();
            Console.WriteLine("--- Available Employees ---");

            try
            {
                foreach (string s in Service.getInstance().getEmployeesAsStrings())
                {
                    Console.WriteLine(s);
                }

                Console.WriteLine("--- Type the ID to select ---");


                int employeeId = Convert.ToInt32(Console.ReadLine());
                Employee emp = Service.getInstance().getEmployee(employeeId);
                if (emp == null) throw new Exception("Employee not found");

                employee = employeeId;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                Console.WriteLine("Any key to continue...");
                Console.ReadKey();
            }

            displayMenu();
        }

        private static string getLocation()
        {
            string result = "None selected";

            if (location > -1)
            {
                try
                {
                    result = Service.getInstance().getLocation(location);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }

            return result;
        }

        private static string getEmployee()
        {
            string result = "None selected";

            if (employee > -1)
            {
                try
                {
                    result = Service.getInstance().getEmployee(employee).name;
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
            }
            return result;
        }

        private static string getTools()
        {
            string result = "None selected";

            if (tools.Count == 1)
                result = "[" + tools.First().ToString() + "]";
            else if (tools.Count > 1)
            {
                result = "[" + tools.First().ToString();

                for (int i = 1; i < tools.Count; i++)
                {
                    result += "," + tools[i].ToString();
                }

                result += "]";

            }

            return result;
        }
    }
}
