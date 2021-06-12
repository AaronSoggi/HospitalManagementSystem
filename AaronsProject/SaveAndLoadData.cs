using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using static AaronsProject.ConsolePrompts;

namespace AaronsProject
{
    class SaveAndLoadData 
    {
        private static string PathToFile = AppDomain.CurrentDomain.BaseDirectory;
        public static string LoadPatientDataFile = "PatientLoadFile.json";
        public static void LoadDataFromFile()
        {
            int UserAnswer;            

            Program.ConsoleHeader("Load File Data", "please select one of the options below:");

            Console.WriteLine($"1: Load data from {LoadPatientDataFile} ");
            Console.WriteLine("2: Return to main menu\n");           
            UserAnswer = IntValidation("Error, please select number 1 or 2: ", 1, 2);
            Console.WriteLine("\n");

            if (UserAnswer == 1)
            {
                try
                {
                    if (File.Exists(LoadPatientDataFile) && new FileInfo(LoadPatientDataFile).Length > 1)
                    {
                        string readJsonData= File.ReadAllText(Path.Combine(PathToFile, LoadPatientDataFile)); // used/adapted code from reference number 4 in declartion form. - has also been used in function "CheckingFileData"

                        var patients = JsonConvert.DeserializeObject<Dictionary<int, Patient>>(readJsonData); // used/adapted code from reference number 4 in declaration form. - has also been used in function "CheckingFileData"

                        foreach (KeyValuePair<int, Patient> kvp in patients)
                        {
                            Patient p = kvp.Value;
                            if (kvp.Key < 0 || kvp.Key != p.PatientID)
                            {
                                throw new InvalidKeyException();
                            }
                            else
                            {
                                Patient.PatientDictionary.Add(p.PatientID, p);
                            }
                        }

                        _loggingdata.LogEvent($"{patients.Count} items were loaded to {Patient.PatientFile} from {LoadPatientDataFile}");

                        SerializeData(Patient.PatientFile, Patient.PatientDictionary);

                        Program.ProgressBar("Loading data from file");
                        Console.WriteLine();
                        Console.WriteLine("Data has been loaded succesfully!");
                        
                        RequestToContinueApplication(Program.DisplayMenu);

                    }
                    else if (File.Exists(LoadPatientDataFile) && new FileInfo(LoadPatientDataFile).Length == 0)
                    {                       
                        Console.WriteLine($"Error, The system was unable to load any data from the file {LoadPatientDataFile} as it contains 0 records.  ");
                        RequestToContinueApplication(Program.DisplayMenu);
                    }
                    else
                    {
                        Console.WriteLine($"Error, The system was unable to locate the file {LoadPatientDataFile}. Before attempting to load data from another file, please ensure that it exists\nwithin the base directory.\n\n");
                        Console.WriteLine($"Would you like the system to create a new file?\n");
                        
                        Console.WriteLine($"1: Create New file ({LoadPatientDataFile})");
                        Console.WriteLine("2: Go back to Main menu");

                        UserAnswer = IntValidation("Error, please select number 1 or 2: ", 1, 2);

                        switch (UserAnswer)
                        {
                            case 1:
                                File.Create(Path.Combine(PathToFile, LoadPatientDataFile));
                                Console.WriteLine();
                                Program.ProgressBar("Creating File");
                                Console.WriteLine("File has been created succesfully!");
                                RequestToContinueApplication(Program.DisplayMenu);
                                break;
                            case 2:
                                Program.DisplayMenu();
                                break;
                        }
                    }
                }
                catch (System.IO.IOException)
                {
                    ExceptionHandling.SystemIOExceptionNotification(Patient.PatientFile);
                }
                catch (Newtonsoft.Json.JsonReaderException)
                {
                    ExceptionHandling.NewtonsoftJsonExceptionNotification(LoadPatientDataFile);
                }
                catch (Newtonsoft.Json.JsonSerializationException)
                {
                    ExceptionHandling.NewtonsoftJsonExceptionNotification(LoadPatientDataFile);
                }
                catch (InvalidKeyException)
                {
                    ExceptionHandling.InvalidKeyExceptionNotification(LoadPatientDataFile);
                }
                catch (System.Runtime.Serialization.SerializationException)
                {
                    ExceptionHandling.RuntimeSerializationException(LoadPatientDataFile);
                }
                catch (System.ArgumentException) 
                {
                    ExceptionHandling.SystemArgumentException(LoadPatientDataFile);
                }
            }
            else
            {
                Program.DisplayMenu();
            }
        }
        public static void SerializeData(string fileName, object dictionaryType)
        {
            try
            {
                File.WriteAllText(Path.Combine(PathToFile, fileName), JsonConvert.SerializeObject(dictionaryType, Formatting.Indented)); // used/adapted code from reference number 4 in declaration form.
            }
            catch (System.IO.IOException)
            {
                ExceptionHandling.SystemIOExceptionNotification(fileName);
            }
        }
        public static void CheckingFileData(string fileName)
        {           
            try
            {
                if (File.Exists(fileName) && new FileInfo(fileName).Length == 0)
                {
                    Console.WriteLine($"Notification for user: {fileName} is currently empty.\n");
                }
                else if (File.Exists(fileName) && new FileInfo(fileName).Length > 1)
                {
                    string readJsonData = File.ReadAllText(Path.Combine(PathToFile, fileName)); 
                    Patient.PatientDictionary.Clear();

                    if (fileName == "PatientDatabase.json")
                    {
                        Patient.PatientDictionary = JsonConvert.DeserializeObject<Dictionary<int, Patient>>(readJsonData);

                        foreach (KeyValuePair<int, Patient> kvp in Patient.PatientDictionary)
                        {
                            Patient patient = kvp.Value;
                            if (kvp.Key < 0 || kvp.Key != patient.PatientID)
                            {
                                throw new InvalidKeyException();
                            }
                        }
                    }
                    else
                    {
                        UserAccount.UserAccountDictionary = JsonConvert.DeserializeObject<Dictionary<string, UserAccount>>(readJsonData);

                        foreach (KeyValuePair<string, UserAccount> kvp in UserAccount.UserAccountDictionary)
                        {
                            UserAccount useraccount = kvp.Value;
                            if (kvp.Key != useraccount.Username)
                            {
                                throw new InvalidKeyException();
                            }
                        }
                    }
                }
                else
                {
                    File.Create(Path.Combine(PathToFile, fileName));
                }
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                ExceptionHandling.RuntimeSerializationException(fileName);
            }
            catch (Newtonsoft.Json.JsonReaderException)
            {
                ExceptionHandling.NewtonsoftJsonExceptionNotification(fileName);
            }
            catch (Newtonsoft.Json.JsonSerializationException)
            {
                ExceptionHandling.NewtonsoftJsonExceptionNotification(fileName);
            }
            catch (InvalidKeyException)
            {
                ExceptionHandling.InvalidKeyExceptionNotification(fileName);
            }
        }
        private static ILoggingdata _loggingdata;
        public SaveAndLoadData(ILoggingdata loggingdata )
        {
            _loggingdata = loggingdata;
        }
    }
}
