using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace E_Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            List<Account> accounts = new List<Account>();
            BinaryFormatter formatter = new BinaryFormatter();
            Console.WriteLine("\tПроект \"Электронный магазин\" E-Shop\n\n");
            Helper.FirstLaunch();

            //начало работы
            using (FileStream fileStream = new FileStream(Helper.path, FileMode.OpenOrCreate))
            {
                accounts.AddRange((List<Account>)formatter.Deserialize(fileStream));
                Console.WriteLine("Все аккаунты десериализованы!");
            }

            Account user;
            //логин в акк
            do
            {
                Console.Clear();
                do user = Helper.LoginAccount(accounts);
                while (user == null);
                int i;
                do i = user.MainFunction(accounts);
                while (i == 0);
                if (i == -1) break;
            } while (true);

            {
                //foreach (Account acc in accounts)
                //{
                //    Console.WriteLine($"Выполнен вход в аккаунт типа {acc.GetType().Name}" +
                //    $"\nЛогин пользователя:\t{acc.Login}" +
                //    $"\nПароль пользователя:\t{acc.Password}" +
                //    $"\nФИО: {acc.LastName} {acc.FirstName} {acc.Patronomic}" +
                //    $"\nДата рождения: {acc.BirthdayDate.ToShortDateString()}; Возраст: {acc.Age}" +
                //    $"\nОбразование: {acc.StudyYears} лет; Опыт работы: {acc.WorkExperience} лет" +
                //    $"\nДолжность: {acc.Position}; Зарплата: {acc.Salary}");
                //    Console.WriteLine();
                //}
            }

            //конец работы
            using (FileStream fileStream = new FileStream(Helper.path, FileMode.OpenOrCreate))
            {
                formatter.Serialize(fileStream, accounts);
                Console.WriteLine("Все аккаунты сериализованы!");
            }
        }
    }
}

