using System;

namespace AaronsProject
{
    class UserAccountManagement : UserAccount
    {
        public static void CreateAccount()
        {
            int passwordMaxLength = Properties.Settings.Default.PasswordMaxLength;
            int passwordMinLength = Properties.Settings.Default.PasswordMinLength;
            int userNameMinLength = Properties.Settings.Default.UserNameMinLength;
            string username, password;

            Program.ConsoleHeader("Create Account", $"Enter a username and password below. (Passwords must be atleast {passwordMinLength} characters and must NOT exceed {passwordMaxLength} characters).");

            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine().Trim();
                Console.WriteLine();
                if (UserAccountDictionary.ContainsKey(username))
                {
                    Console.WriteLine("Sorry, that username already exists! Please use a different username\n");
                }
                else if (username.Length < userNameMinLength)
                {
                    Console.WriteLine($"Username must be atleast {userNameMinLength} characters\n");
                }
                else
                {
                    break;
                }
            } while (true);

            do
            {
                Console.Write("Password: ");
                password = Console.ReadLine().Trim();
                Console.WriteLine();
                if (password.Length < passwordMinLength)
                {
                    Console.WriteLine($"Password must be atleast {passwordMinLength} characters.\n");
                }
                else if (password.Length > passwordMaxLength)
                {
                    Console.WriteLine($"Password must not exceed {passwordMaxLength} characters.\n");
                }
                else
                {
                    break;
                }
            } while (true);

            UserAccount user = new UserAccount(username, password);
            UserAccountDictionary.Add(user.Username, user);

            _loggingdata.LogEvent($"User account was created : {username}");

            SaveAndLoadData.SerializeData(UserAccountFile, UserAccountDictionary);            
            Program.ProgressBar("Creating account");
            Console.WriteLine();
            Console.WriteLine("Account has been succesfully created!");
            
            ConsolePrompts.RequestToContinueApplication(Program.HomePage);
        }
        public static void UpdatePassword()
        {
            string username, password, newPassword;
            int passwordMaxLength = Properties.Settings.Default.PasswordMaxLength;
            int passwordMinLength = Properties.Settings.Default.PasswordMinLength;

            Program.ConsoleHeader("Update Password", "Please enter your username and password below: ");

            do
            {
                Console.Write("Username: ");
                username = Console.ReadLine().Trim();
                Console.Write("Password: ");
                password = Console.ReadLine().Trim(); ;
                Console.WriteLine();

                UserAccount useraccount = null;
                if (UserAccountDictionary.TryGetValue(username, out useraccount))
                {
                    if (useraccount.Password == password)
                    {
                        Console.WriteLine("Access granted.\n");
                    Again:
                        Console.Write("Enter your new password: ");
                        newPassword = Console.ReadLine().Trim();
                        Console.WriteLine();
                        if (newPassword.Length > passwordMinLength && newPassword.Length < passwordMaxLength)
                        {
                            useraccount.Password = newPassword;
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Please ensure that your password is between {passwordMinLength} and {passwordMaxLength} characters\n");
                            goto Again;
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error, your username or password is incorrect\n");
                    }
                }
                else
                {
                    Console.WriteLine("Error, your username or password is incorrect\n");
                }
            } while (true);

            _loggingdata.LogEvent($"User password was changed: {username}");

            SaveAndLoadData.SerializeData(UserAccountFile, UserAccountDictionary);            
            Program.ProgressBar("Changing password");
            Console.WriteLine();
            Console.WriteLine("Account has been succesfully updated!");           
            
            ConsolePrompts.RequestToContinueApplication(Program.HomePage);
        }
        private static ILoggingdata _loggingdata;
        public UserAccountManagement(ILoggingdata loggingdata)
        {
            _loggingdata = loggingdata;
        }
    }
}

