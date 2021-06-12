using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace AaronsProject
{
    [Serializable]
    class UserAccount : ISerializable
    {
        public static Dictionary<string, UserAccount> UserAccountDictionary = new Dictionary<string, UserAccount>();
        public static string UserAccountFile = "UserAccount.json";

        public string Username { get; set; }
        public string Password { get; set; }
  
        public UserAccount() { }
        public UserAccount(string username, string password) 
        {
            Username = username;
            Password = password;
        }
        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Employee Username", Username);
            info.AddValue("Employee Password", Password);
        }
        public UserAccount(SerializationInfo info , StreamingContext context) 
        {
            Username = (string)info.GetValue("Employee Username", typeof(string));
            Password = (string)info.GetValue("Employee Password", typeof(string));
        }
    }
}
