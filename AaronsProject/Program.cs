using System;

namespace AaronsProject
{
    class Program 
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(150, 40);
            CreationOfLoggingData();
            HomePage();                                
            Console.ReadLine();
        }
     
        public static void CreationOfLoggingData()
        {
            LoggingData logdata = new LoggingData();
            new UserAccountManagement(logdata);
            new PatientDataManagement(logdata);
            new ExceptionHandling(logdata);
            new Program(logdata);
            new SaveAndLoadData(logdata);
        }
        public static void SignIn()
        {
            string usernameGuess, passwordGuess;
            int loginLimit = Properties.Settings.Default.LoginLimit;
            int loginAttempts = 0, resultValue = 0;

            ConsoleHeader("Sign in", "Please enter you username and password below. ");

            do
            {
                Console.Write("Username: ");
                usernameGuess = Console.ReadLine().Trim();
                Console.Write("Password: ");
                passwordGuess = Console.ReadLine().Trim();
                Console.WriteLine();
                loginAttempts++;

                UserAccount useraccount = null;

                if (UserAccount.UserAccountDictionary.TryGetValue(usernameGuess, out useraccount) == true)
                {
                    if (useraccount.Password == passwordGuess)
                    {
                        resultValue = 1;
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error, Your password or username is incorrect.\n");
                    }
                }
                else
                {
                    Console.WriteLine("Error, Your password or username is incorrect.\n");
                    resultValue = 0;
                }
            } while (loginAttempts < loginLimit);

            if (resultValue == 1)
            {
                Console.WriteLine();
                ProgressBar("Logging in");

                _loggingdata.LogEvent($"User logged into the system : {usernameGuess}");

                DisplayMenu();
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Your account has been temporarily locked due to excessive login failures.\n");
                ConsolePrompts.RequestToContinueApplication(HomePage);
            }
        }
        public static void HomePage()
        {
            int userAnswer;

            ConsoleHeader("Home Page", "Please select from one of the following options: ");
            SaveAndLoadData.CheckingFileData(UserAccount.UserAccountFile);

            Console.WriteLine("---------------------------");
            Console.WriteLine("1: Sign in to account");
            Console.WriteLine("---------------------------");
            Console.WriteLine("2: Create account");
            Console.WriteLine("---------------------------");
            Console.WriteLine("3: Update password");
            Console.WriteLine("---------------------------");
            Console.WriteLine("4: Exit application");
            Console.WriteLine("---------------------------");

            userAnswer = ConsolePrompts.IntValidation("Invalid input, Please select a number from 1 to 4 : ", 1, 4);

            if (userAnswer == 1)
            {
                SignIn();
            }
            else if (userAnswer == 2)
            {
                UserAccountManagement.CreateAccount();
            }
            else if (userAnswer == 3)
            {
                UserAccountManagement.UpdatePassword();
            }
            else
            {
                Environment.Exit(0);
            }
        }
        public static void DisplayMenu()
        {
            int userAnswer;

            ConsoleHeader(" [+] Medical Information System", "Please select one of the following options: ");
            SaveAndLoadData.CheckingFileData(Patient.PatientFile);

            Console.WriteLine("---------------------------");
            Console.WriteLine("1: Add Patient Record ");
            Console.WriteLine("---------------------------");
            Console.WriteLine("2: Update Patient Record");
            Console.WriteLine("---------------------------");
            Console.WriteLine("3: Remove Patient Record");
            Console.WriteLine("---------------------------");
            Console.WriteLine("4: Search Patient by ID");
            Console.WriteLine("---------------------------");
            Console.WriteLine("5: View all Patient Records");
            Console.WriteLine("---------------------------");
            Console.WriteLine("6: Sort Patient Data");
            Console.WriteLine("---------------------------");
            Console.WriteLine("7: Display Log File");
            Console.WriteLine("---------------------------");
            Console.WriteLine("8: Load data from file");
            Console.WriteLine("---------------------------");
            Console.WriteLine("9: Exit the application");
            Console.WriteLine("---------------------------");
            Console.WriteLine("10: Log out");
            Console.WriteLine("---------------------------");
            Console.WriteLine();

            userAnswer = ConsolePrompts.IntValidation("Invalid input, please enter a number from 1 to 10: ", 1, 10);

            switch (userAnswer)
            {
                case 1:
                    PatientDataManagement.AddPatient();
                    break;
                case 2:
                    PatientDataManagement.UpdatePatientData();
                    break;
                case 3:
                    PatientDataManagement.RemovePatient();
                    break;
                case 4:
                    PatientDataManagement.SearchForPatient();
                    break;
                case 5:
                    PatientDataManagement.ViewAllPatientData();
                    break;
                case 6:
                    PatientDataManagement.SortPatientData();
                    break;
                case 7:
                    _loggingdata.OpeningLogFile();
                    break;
                case 8:
                    SaveAndLoadData.LoadDataFromFile();
                    break;
                case 9:
                    Environment.Exit(0);
                    break;
                case 10:
                    Console.Clear();
                    HomePage();
                    break;
            }
        }
        public static void ProgressBar(string loadingMessage)
        {
            Console.WriteLine();
            Console.Write($"{loadingMessage}");
            Console.Write("  [");
            for (int elapsedTime = 0; elapsedTime < 8; elapsedTime++)
            {               
                Console.Write("=");               
                System.Threading.Thread.Sleep(600);
            }
            Console.Write("]");
            Console.WriteLine("\n");
        }
        public static void ConsoleHeader(string title, string userInstruction)
        {
            Console.Clear();
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("                                         {0}", title);
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("\n");
            Console.WriteLine("{0}", userInstruction);
            Console.WriteLine("\n");
        }
        private static ILoggingdata _loggingdata;
        public Program(ILoggingdata loggingdata)
        {
            _loggingdata = loggingdata;
        }

    }       
}

    

    

    

