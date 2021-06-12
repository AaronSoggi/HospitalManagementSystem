using System;
using System.Collections.Generic;


namespace AaronsProject
{
    partial class ConsolePrompts 
    {
        public static int IntValidation(string errorMessage, int minNumber, int maxNumber)
        {
            string numberAsAString;
            int userAnswer;

            while (true)
            {
                numberAsAString = Console.ReadLine().Trim();
                if (!int.TryParse(numberAsAString, out userAnswer) || (userAnswer < minNumber || userAnswer > maxNumber))
                {
                    Console.Write(errorMessage);
                }
                else
                {
                    break;
                }
            }
            return userAnswer;
        }
        public static Enum GenderValidation(string inputClarification)
        {
            bool isInputValid;
            Patient.Gender gender;
          
            do
            {
                Console.Write(inputClarification);
                var userInput = Console.ReadLine();
                isInputValid = Enum.TryParse(userInput, true, out gender) && Enum.IsDefined(typeof(Patient.Gender), gender); // adapted/reused code from link 2 in the declaration form

            } while (!isInputValid);
            return gender;
        }
        public static void RequestToReloadMethod(string nameOfExecution, Action method)
        {
            string userAnswer;

            Console.WriteLine();
            do
            {
                Console.WriteLine($"Would you like to {nameOfExecution} another patient?");
                Console.WriteLine();
                Console.Write("Press y for Yes or n for No: ");
                userAnswer = Console.ReadLine().ToLower();

                if (userAnswer != "y" && userAnswer != "n")
                {
                    Console.WriteLine("Invalid input, please try again.");
                }
            } while (userAnswer != "y" && userAnswer != "n");

            if (userAnswer == "y")
            {
                Console.Clear();
                method();
            }
            else
            {
                Console.Clear();
                Program.DisplayMenu();
            }
        }
        public static void RequestToContinueApplication(Action method)
        {
            Console.WriteLine();
            Console.WriteLine("Press Enter to continue ");

            while (Console.ReadKey(true).Key != ConsoleKey.Enter)
            {
                Console.WriteLine("Invalid input, please press enter");
            }
            Console.Clear();
            method();
        }        
        public static int CheckingIfPatientExists(string loadingMessage, Action method)
        {
            int patientID = 0;

            try
            {
                do
                {
                    patientID = Int32.Parse(Console.ReadLine());
                    Console.WriteLine();
                    Program.ProgressBar(loadingMessage);
                    Console.WriteLine();

                    if (!(Patient.PatientDictionary.ContainsKey(patientID)))
                    {
                        Console.Write("Sorry the Patient ID you have entered does not exist! please try again.\n");
                        RequestToContinueApplication(method);
                    }
                } while (!(Patient.PatientDictionary.ContainsKey(patientID)));
            }
            catch (FormatException)
            {
                ExceptionHandling.FormatExceptionNotification(method);
            }
            return patientID;
        }
        public static void ExitApplication() 
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to exit the application");
            Console.ReadKey();
            Environment.Exit(0);
        }
        
    }
}
