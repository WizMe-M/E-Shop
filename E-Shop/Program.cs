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
            //список всех аккаунтов, которыми в ходе программы будем манипулировать
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

            Console.Clear();
            Account user;
            do
                user = Helper.LoginAccount(accounts);
            while (user == null);

            {
                //foreach (Account acc in accounts)
                //{
                //    Console.WriteLine($"Выполнен вход в аккаунт типа {acc.GetType().Name}" +
                //    $"\nЛогин пользователя:\t{acc.Login}" +
                //    $"\nПароль пользователя:\t{acc.Password}" +
                //    $"\nФИО: {acc.LastName} {acc.FirstName} {acc.Patronomic}" +
                //    $"\nДата рождения: {acc.BirthdayDate.ToShortDateString()}" +
                //    $"\nВозраст: {acc.Age}" +
                //    $"\nОбразование: {acc.StudyYears} лет" +
                //    $"\nОпыт работы: {acc.WorkExperience} лет" +
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

