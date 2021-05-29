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
                Console.WriteLine($"Зарегистрирован новый пользователь (администратор)");
                Admin admin = new Admin("admin", "admin");
                Console.WriteLine($"Ваши:\nЛогин - {admin.Login}\nПароль - {admin.Password}");

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
                Console.WriteLine("Введите логин:");
                string login = Console.ReadLine().Trim();
                Console.WriteLine("Введите пароль:");
                string password = Console.ReadLine().Trim();

                foreach (Account acc in accounts)
                {
                    if (login.Equals(acc.Login) && password.Equals(acc.Password))
                    {
                        if (acc.isDeleted)
                            throw new Exception("-- ЭТОТ АККАУНТ БЫЛ УДАЛЁН! --" +
                                "\nЧтобы войти в систему, аккаунт должен быть рабочим." +
                                "\nЕсли ваш аккаунт был удалён, обратитесь к администратору, чтобы восстановить его");

                        if (!acc.isHired)
                            throw new Exception("-- ЭТОТ СОТРУДНИК БЫЛ УВОЛЕН! --" +
                                "\nЧтобы войти в аккаунт, нужно быть сотрудником предприятия." +
                                "\nНанять сотрудника может кадровик");

                        return acc.Position switch
                        {
                            "Администратор" => (Admin)acc,
                            "Кадровик" => (Personnel)acc,
                            _ => throw new Exception("Что-то пошло не так..." +
                            "\nСудя по всему, аккаунт имеет неправильные настройки!"),
                        };


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
            formatter.Serialize(fileStream, accounts);
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
