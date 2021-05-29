using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace E_Shop
{
    static class Helper
    {
        public static string path = Directory.GetCurrentDirectory() + @"\E-Shop\accounts\database.bd";
        public static void FirstLaunch()
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\E-Shop");
            if (!dir.Exists)
            {
                Console.WriteLine("Запускаю протокол первичного запуска...");
                Console.WriteLine($"Зарегистрирован новый пользователь ({nameof(Admin)})");
                Admin admin = new Admin("admin", "admin");
                Console.WriteLine($"Логин: {admin.Login}\nПароль: {admin.Password}");

                dir.Create();
                dir.CreateSubdirectory("accounts");
                List<Account> accounts = new List<Account>() { admin };
                BinaryFormatter formatter = new BinaryFormatter();
                using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                formatter.Serialize(fileStream, accounts);
                fileStream.Close();
                Console.WriteLine("Все аккаунты сериализованы!");
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        public static Account LoginAccount(List<Account> accounts)
        {
            try
            {
                Console.Clear();
                Console.WriteLine("Запускаю процесс входа в аккаунт...");
                Console.WriteLine("\nВведите логин:");
                string login = Console.ReadLine().Trim();
                Console.WriteLine("\nВведите пароль:");
                string password = Console.ReadLine().Trim();

                foreach (Account acc in accounts)
                {
                    if (login.Equals(acc.Login) && password.Equals(acc.Password))
                    {
                        if (!acc.isDeleted)
                        {
                            return acc.GetType().Name switch
                            {
                                "Admin" => (Admin)acc,

                                _ => throw new Exception("Что-то пошло не так..." +
                                "\nСудя по всему, аккаунт с такими данными имеет неправильную роль." +
                                "\nОбратитесь к администратору, чтобы исправить это."),
                            };
                        }
                        else
                        {
                            throw new Exception("-- ЭТОТ АККАУНТ БЫЛ УДАЛЁН! --" +
                                "\nЧтобы войти в этот аккаунт, попросите администратора восстановить его.");
                        }

                    }
                }
                throw new Exception("Аккаунта с такими данными не существует!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return null;
            }
        }
        public static void SaveAllAcounts(List<Account> accounts)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(path, FileMode.Truncate);
            foreach (Account a in accounts)
                formatter.Serialize(fileStream, a);
            fileStream.Close();
        }
        public static List<Account> GetAllAcounts()
        {
            List<Account> accounts = new List<Account>();
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(path, FileMode.Open);
            accounts.AddRange((List<Account>)formatter.Deserialize(fileStream));
            fileStream.Close();
            return accounts;
        }
        public static int PrintConsoleMenu(string[] menuItems)
        {
            ConsoleKeyInfo key;
            int counter = 0;
            do
            {
                Console.Clear();
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (counter == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(menuItems[i]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.WriteLine(menuItems[i]);
                }

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        counter--;
                        if (counter == -1)
                            counter = menuItems.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        counter++;
                        if (counter == menuItems.Length)
                            counter = 0;
                        break;
                    case ConsoleKey.Enter:
                        return counter;
                }
            }
            while (true);
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
