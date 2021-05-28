using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace E_Shop
{
    class Helper
    {
        public static void FirstLaunch()
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\E-Shop");
            if (!dir.Exists)
            {
                Console.WriteLine("Запускаю протокол первичного запуска...");
                Console.WriteLine("Зарегистрирован новый пользователь (администратор)");
                Admin admin = new Admin("admin", "admin");
                Console.WriteLine($"Логин: {admin.Login}\nПароль: {admin.Password}");
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();

                dir.Create();
                dir.CreateSubdirectory("accounts");
                dir.CreateSubdirectory("accounts\\Admin");
                admin.AddAccountAtDataBase();
                admin.SerializeAccount();
            }
        }
        public static Account LoginAccount()
        {
            string path = Directory.GetCurrentDirectory() + @"\E-Shop\accounts\database.bd";
            string log;
            string pass;
            string role;

            Console.Clear();
            Console.WriteLine("Запускаю процесс входа в аккаунт...");

            //цикличный ввод логина и пароля
            do
            {
                Console.WriteLine("\nВведите логин:");
                string login = Console.ReadLine().Trim();
                Console.WriteLine("\nВведите пароль:");
                string password = Console.ReadLine().Trim();

                using BinaryReader reader = new BinaryReader(File.OpenRead(path));
                while (reader.PeekChar() > -1)
                {
                    log = reader.ReadString();
                    pass = reader.ReadString();
                    role = reader.ReadString();

                    if (login.Equals(log) && password.Equals(pass))
                    {
                        switch (role)
                        {
                            case "Admin":
                                Admin admin = new Admin(log, pass);
                                return admin;
                            default:
                                return null;
                        }
                    }
                }
                reader.Close();
                Console.WriteLine("Аккаунта с такими данными не существует!");
                Console.WriteLine("Нажмите любую кнопку, чтобы попробовать войти ещё раз...");
                Console.ReadKey();
            } while (true);
        }
        public static bool Check(string s, string type)
        {
            //эти проверки должны быть в свойствах Account
            string pattern;
            s = s.Trim();
            switch (type)
            {
                case "логин":
                    pattern = @"^[a-zA-Z]{3-15}$";
                    if (!Regex.IsMatch(s, pattern))
                    {
                        Console.Clear();
                        Console.WriteLine("Логин должен состоять из латинских букв и быть длиной от 3 до 15 символов!\nВведите логин ещё раз:");
                        return false;
                    }
                    break;

                case "пароль":
                    pattern = @"^((?<![A-Z])[A-Z]{3}(?![A-Z]))(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
                    if (!Regex.IsMatch(s, pattern))
                    {
                        Console.Clear();
                        Console.WriteLine("Пароль должен быть минимум из восьми символов, содержать три заглавные буквы не подряд, " +
                            "минимум 2 специальных символа и минимум три цифры (буквы должны быть латинского алфавита)." +
                            "\nВведите пароль ещё раз:");
                        return false;
                    }
                    break;
            }

            return true;
        }
    }
}
