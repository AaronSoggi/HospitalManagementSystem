using System;
using System.Collections.Generic;
using System.Linq;
using static AaronsProject.ConsolePrompts;

namespace AaronsProject
{
    class PatientDataManagement : Patient
    {
        public static void AddPatient()
        {
            int id = 0, age;
            int minimumAgeEntry = Properties.Settings.Default.MinimumAgeEntry;
            int maximumAgeEntry = Properties.Settings.Default.MaximumAgeEntry;
            int minimumIdEntry = Properties.Settings.Default.MinimumIdEntry;
            string name, address, phonenumber;

            Program.ConsoleHeader("Add Patient", "Please enter the details of the new patient you would like to add to the system.");

            try
            {
                while (true)
                {
                    Console.Write("Patient ID: ");
                    id = Int32.Parse(Console.ReadLine());
                    if (PatientDictionary.ContainsKey(id))
                    {
                        Console.WriteLine();
                        Console.WriteLine("Sorry that ID already exists! Please enter another ID\n");
                    }
                    else if (id < minimumIdEntry)
                    {
                        Console.WriteLine();
                        Console.WriteLine($"Error, The system does not accept ID's that are below {minimumIdEntry}.\n");
                    }
                    else
                    {
                        break;
                    }
                }
            }
            catch (FormatException)
            {
                ExceptionHandling.FormatExceptionNotification(AddPatient);
            }

            Console.Write("Patient Name: ");
            name = Console.ReadLine();

            Console.Write("Patient Age: ");
            age = IntValidation($"Invalid input, please enter an age from {minimumAgeEntry} to {maximumAgeEntry}: ", minimumAgeEntry, maximumAgeEntry);

            var gender = (Patient.Gender)GenderValidation("Patient Gender (Male/Female): ");

            Console.Write("Patient Address: ");
            address = Console.ReadLine();
            Console.Write("Patient Phone Number: ");
            phonenumber = Console.ReadLine();
            Console.WriteLine();

            Patient patient = new Patient(id, name, age, gender, address, phonenumber);
            PatientDictionary.Add(patient.PatientID, patient);

            SaveAndLoadData.SerializeData(PatientFile, PatientDictionary);

            _loggingdata.LogEvent($"Patient was added - ID : {patient.PatientID} Name: {patient.PatientName}");

            Program.ProgressBar("Adding patient");
            Console.WriteLine();
            Console.WriteLine("Data Saved succesfully!");
            
            RequestToReloadMethod("add", AddPatient);
        }
        public static void RemovePatient()
        {
            int patientID;

            Program.ConsoleHeader("Remove Patient", "Please enter the patient ID you would like to remove: ");

            patientID = CheckingIfPatientExists("Searching patient ID", RemovePatient);
            PatientDictionary.Remove(patientID);
            Console.WriteLine("Patient information has been removed from the database.");

            SaveAndLoadData.SerializeData(PatientFile, PatientDictionary);

            _loggingdata.LogEvent($"Patient was removed - ID : {patientID} ");

            RequestToReloadMethod("remove", RemovePatient);

        }
        public static void UpdatePatientData()
        {
            int minimumAgeEntry = Properties.Settings.Default.MinimumAgeEntry;
            int maximumAgeEntry = Properties.Settings.Default.MaximumAgeEntry;
            int userAnswer, patientID;
            string stringValue;

            Program.ConsoleHeader("Update Patient", "Please enter the patient ID you would like to update: ");
            patientID = CheckingIfPatientExists("Searching patient ID", UpdatePatientData);

            Patient p = PatientDictionary[patientID];

            Console.WriteLine("==============================================================================================================");
            Console.WriteLine("ID: {0}    Name: {1}    Age: {2}    Gender: {3}    Address: {4}    Contact Number: {5}", p.PatientID, p.PatientName, p.PatientAge, p.PatientGender, p.PatientAddress, p.PatientPhoneNumber);
            Console.WriteLine("==============================================================================================================\n\n");           

            Console.WriteLine("Please enter one of the options from the menu below: ");
            Console.WriteLine();
            Console.WriteLine("1: Patient Name ");
            Console.WriteLine("2: Patient Age ");
            Console.WriteLine("3: Patient Gender (Male/Female) ");
            Console.WriteLine("4: Patient Address ");
            Console.WriteLine("5: Patient Phone Number\n");           

            userAnswer = IntValidation("Error, Please select a number from 1 to 5: ", 1, 5);
            Console.WriteLine();

            if (userAnswer == 2)
            {
                Console.Write("New age for patient: ");
                p.PatientAge = IntValidation($"Invalid input, please enter an age from {minimumAgeEntry} to {maximumAgeEntry}: ", minimumAgeEntry, maximumAgeEntry);
            }
            else if (userAnswer == 3)
            {
                p.PatientGender = (Patient.Gender)GenderValidation("New gender for patient (Male/Female): ");
            }
            else
            {
                Console.Write("New data for patient: ");
                stringValue = (Console.ReadLine());
                switch (userAnswer)
                {
                    case 1:
                        p.PatientName = stringValue;
                        break;
                    case 4:
                        p.PatientAddress = stringValue;
                        break;
                    case 5:
                        p.PatientPhoneNumber = stringValue;
                        break;
                }
            }
            SaveAndLoadData.SerializeData(PatientFile, PatientDictionary);

            _loggingdata.LogEvent($"Patients details was updated - ID : {p.PatientID} ");

            Console.WriteLine("\n");
            Console.WriteLine("Data Saved succesfully!");
            
            RequestToReloadMethod("update", UpdatePatientData);
        }
        public static void SortPatientData()
        {
            int userAnswer;

            Program.ConsoleHeader("Sort Data", "How would you like to sort the patient data: ");

            Console.WriteLine("1: Sort by ID");
            Console.WriteLine("2: Sort by name\n");            

            userAnswer = IntValidation("Error:  Please select number 1 or 2: ", 1, 2);

            if (userAnswer == 1)
            {
                PatientDictionary = PatientDictionary.OrderBy(x => x.Key).ToDictionary(x => x.Key, x => x.Value);
            }
            else
            {
                PatientDictionary = PatientDictionary.OrderBy(x => x.Value.PatientName).ToDictionary(x => x.Key, x => x.Value);
            }

            SaveAndLoadData.SerializeData(PatientFile, PatientDictionary);
            Console.WriteLine();
            Program.ProgressBar("Sorting data");
            Console.WriteLine();
            Console.WriteLine("Data has been updated succesfully!");
            
            RequestToContinueApplication(Program.DisplayMenu);
        }
        public static void ViewAllPatientData()
        {
            int totalNumberOfRecords = PatientDictionary.Count();

            Program.ConsoleHeader("All Patient Records", $"The system currently holds {totalNumberOfRecords} items: ");

            foreach (KeyValuePair<int, Patient> kvp in PatientDictionary)
            {
                Patient p = kvp.Value;
                Console.WriteLine("==================================================================================================================");
                Console.WriteLine("ID: {0}    Name: {1}    Age: {2}    Gender: {3}    Address: {4}    Contact Number: {5}", p.PatientID, p.PatientName, p.PatientAge, p.PatientGender, p.PatientAddress, p.PatientPhoneNumber);
                Console.WriteLine("==================================================================================================================\n");
            }
           
            RequestToContinueApplication(Program.DisplayMenu);
        }
        public static void SearchForPatient()
        {
            int patientID;

            Program.ConsoleHeader("Search for patient", "Please enter the patient ID you would like to search for: ");

            patientID = CheckingIfPatientExists("Searching patient ID", SearchForPatient);

            Patient p = PatientDictionary[patientID];
            Console.WriteLine("==================================================================================================================");
            Console.WriteLine("ID: {0}    Name: {1}    Age: {2}    Gender: {3}    Address: {4}    Contact Number: {5}", p.PatientID, p.PatientName, p.PatientAge, p.PatientGender, p.PatientAddress, p.PatientPhoneNumber);
            Console.WriteLine("==================================================================================================================\n");
         
            RequestToReloadMethod("search", SearchForPatient);
        }
        private static ILoggingdata _loggingdata;
        public PatientDataManagement(ILoggingdata loggingdata)
        {
            _loggingdata = loggingdata;
        }
    }
}

