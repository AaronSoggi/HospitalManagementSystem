using System;
using System.IO;
using static AaronsProject.ConsolePrompts;


namespace AaronsProject 
{
    class ExceptionHandling
    {
        private static string PathToFile = AppDomain.CurrentDomain.BaseDirectory;
        public static void FormatExceptionNotification(Action method)
        {
            Console.WriteLine("\n");
            Console.WriteLine("Error, Please ensure that you enter a number.");
            RequestToContinueApplication(method);            
        }
        public static void NewtonsoftJsonExceptionNotification(string fileName)
        {
            int userAnswer;

            Program.ConsoleHeader("Warning: File Read Error", "Please see the message below: ");

            _loggingdata.LogEvent($"An error occured opening {fileName} - Reason  :  File read error");
                 
            if (fileName == SaveAndLoadData.LoadPatientDataFile)
            {
                Console.WriteLine($"Error, Unfortunately the program is unable to read the data from the file {fileName}. Please ensure that the data is in the correct format\nbefore attempting to load it into the system.");
                RequestToContinueApplication(Program.DisplayMenu);
            }
            else
            {
                Console.WriteLine("---------------------------");
                Console.WriteLine("1: Delete File ");
                Console.WriteLine("---------------------------");
                Console.WriteLine("2: Exit the application");
                Console.WriteLine("---------------------------");
                Console.WriteLine();

                Console.WriteLine($"Error, Unfortunately the program is unable to read the data from the file {fileName}.\nPlease exit the program and ensure that the data is in the correct format.\n\nThe file can be found at: {Path.Combine(PathToFile, fileName)}.\n\nIf you would like to overwrite the file and create a new one select 1, or select 2 to exit the application");
                Console.WriteLine();

                userAnswer = IntValidation("Error, Please select either 1 or 2: ", 1, 2);

                if (userAnswer == 1)
                {
                    File.Delete(fileName);
                    Console.WriteLine();
                    Program.ProgressBar("Deleting file");
                    Console.WriteLine();
                    Console.WriteLine($"{fileName} has now been succesfully deleted from the base directory folder");

                    if (fileName == Patient.PatientFile)
                    {
                        RequestToContinueApplication(Program.DisplayMenu);
                    }
                    else
                    {
                        RequestToContinueApplication(Program.HomePage);
                    }
                }
                else
                {
                    Environment.Exit(0);
                }
            }
        }
        public static void SystemIOExceptionNotification(string fileName)
        {         
            Program.ConsoleHeader("Warning - File is open in another program", "Please see the message below: ");

            _loggingdata.LogEvent($"An error occured whilst saving data to {fileName} - Reason  :  {fileName} was open in another application");

            Console.WriteLine($"Error, Unfortunately the data cannot be saved as {fileName} seems to be open in another program. Before attempting to make any further changes, please exit this program and close down any other applications that are using {fileName}");

            RequestToContinueApplication(Program.HomePage);
        }
        public static void InvalidKeyExceptionNotification(string fileName)
        {
            Program.ConsoleHeader("Warning: Invalid key read", "Please see the message below: ");
           
            _loggingdata.LogEvent($"An error occured whilst trying to open {fileName} - Reason  :  Invalid Key Read");

            if (fileName == Patient.PatientFile)
            {
                Console.WriteLine($"Error, please ensure that the Key and ID allocated to each patient within the file {fileName}, are matching and above 0.");
                Console.WriteLine($"Please note, You will not be able to use this application until the issue has been rectified.\n\nThe file can be found at { Path.Combine(PathToFile, fileName)}\n");

                ExitApplication();               
            }
            else if (fileName == SaveAndLoadData.LoadPatientDataFile)
            {
                Console.WriteLine($"Error, before attempting to load the data from {fileName}, please ensure that the key and ID allocated to each patient are matching\nand are above 0. ");
                RequestToContinueApplication(Program.DisplayMenu);
            }
            else
            {
                Console.WriteLine($"Error, The system was unable to process the account information from {fileName}.\nPlease ensure that the key for each account matches with the username stored within the file.\n");
                Console.WriteLine("For security reasons, users will not be permitted to use the application until this issue has been fixed.");
                Console.WriteLine($"\nThe file can be found at {Path.Combine(PathToFile, fileName)}\n");

                ExitApplication();                          
            }
        }
        public static void RuntimeSerializationException(string fileName)
        {
            Program.ConsoleHeader("Warning - Unable to evaluate property references", "Please see the message below:");

            _loggingdata.LogEvent($"An error occured whilst trying to read data from {fileName}\nReason  :  Invalid name associated to one or more of the string values ");

            if (fileName == SaveAndLoadData.LoadPatientDataFile || fileName == Patient.PatientFile)
            {
                Console.WriteLine($"Error, unable to retrieve the data from {fileName}, as one or more of the property references are in the incorrect format.\nPlease refer to the arrangement below:\n\nPatient ID\nPatient Name\nPatient Age\nPatient Gender\nPatient Address\nPatient Number\n\nThe file can be found at {Path.Combine(PathToFile, fileName)}");

                if(fileName == Patient.PatientFile) 
                {
                    ExitApplication();                                      
                }
                else 
                {
                    RequestToContinueApplication(Program.DisplayMenu);
                }                
            }
            else
            {
                Console.WriteLine($"Error, unable to retrieve the data from {fileName}, as one or more of the property references are in the incorrect format.\nPlease refer to the arrangement below:\n\nEmployee Username\nEmployee Password\n\nThe file can be found at {Path.Combine(PathToFile, fileName)}");
                
                ExitApplication();               
            }
        }
        public static void SystemArgumentException(string fileName) 
        {
            Console.WriteLine($"Error, the system was unable to load the data from {fileName} as one of the keys allocated to the patients already exists.");
            RequestToContinueApplication(Program.DisplayMenu);
        }
        private static ILoggingdata _loggingdata;
        public ExceptionHandling(ILoggingdata loggingdata) 
        {
            _loggingdata = loggingdata;
        }
    }
    class InvalidKeyException : Exception 
    {
        public InvalidKeyException() : base() 
        {
        }
        public InvalidKeyException(string message) : base(message)
        {
        }
    }
}

