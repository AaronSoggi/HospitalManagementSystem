using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace AaronsProject
{
    [Serializable]
    class Patient  : ISerializable 
    {
        public static Dictionary<int, Patient> PatientDictionary = new Dictionary<int, Patient>();
        public static string PatientFile = "PatientDatabase.json";

        public int PatientID { get; set; }     
        public string PatientName { get; set; }
        public int PatientAge { get; set; }
        public enum Gender
        {
            male,
            female
        }        
        private Gender gender;
        public Gender PatientGender
        {
            get { return gender; }            
            set 
            {
                gender = value;
            }
        }        
        public string PatientPhoneNumber { get; set; }
        public string PatientAddress { get; set; }
     
        public Patient() { }
        public Patient(int id, string name, int age, Gender gender, string address, string phoneNumber)
        {
            PatientID = id;
            PatientName = name;
            PatientAge = age;
            PatientGender = gender ; 
            PatientAddress = address;
            PatientPhoneNumber = phoneNumber;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Patient ID", PatientID);
            info.AddValue("Patient Name", PatientName);
            info.AddValue("Patient Age", PatientAge);
            info.AddValue("Patient Gender", PatientGender);
            info.AddValue("Patient Address", PatientAddress);
            info.AddValue("Patient Number", PatientPhoneNumber);
        }
        public Patient(SerializationInfo info, StreamingContext context)
        {
            PatientID = (int)info.GetValue("Patient ID", typeof(int));
            PatientName = (string)info.GetValue("Patient Name", typeof(string));
            PatientAge = (int)info.GetValue("Patient Age", typeof(int));
            PatientGender = (Gender)info.GetValue("Patient Gender", typeof(Gender));
            PatientAddress = (string)info.GetValue("Patient Address", typeof(string));
            PatientPhoneNumber = (string)info.GetValue("Patient Number", typeof(string));
        }
    }
}

