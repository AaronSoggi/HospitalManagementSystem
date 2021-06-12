using System;
using System.IO;


namespace AaronsProject
{
    public interface ILoggingdata 
    {
        void LogEvent(string loginfo);
        void OpeningLogFile();
    }
     public class LoggingData : ILoggingdata
    {             
        private string baseDirectory { get; set; }
        private string fileName { get; set; }
        private string filePath { get; set; }
       
        public LoggingData() 
        {          
            baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            fileName = "logging.txt";
            filePath = Path.Combine(baseDirectory, fileName);           
        }
        public void LogEvent(string logInfo) 
        {            
            StreamWriter sw = new StreamWriter(File.Open(filePath,FileMode.Append));
            sw.WriteLine("----------------------------------------------------------------------------------------------------------------------------------------------");
            sw.Write("Log Event  :  ");
            sw.Write($"{DateTime.Now.ToLongTimeString()} , {DateTime.Now.ToLongDateString()} , ");
            sw.WriteLine($"INFO  {logInfo}");          
            sw.Close();
        }
        public void OpeningLogFile() 
        {
            Console.Clear();
            StreamReader sr = new StreamReader(File.Open(filePath, FileMode.Open));
            string line = sr.ReadLine();
        
            while (line != null) 
            {
                Console.WriteLine(line);
                line = sr.ReadLine();
            }
            sr.Close();
            Console.WriteLine();
            ConsolePrompts.RequestToContinueApplication(Program.DisplayMenu);
        }
    }
}
