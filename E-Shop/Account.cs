using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

namespace E_Shop
{
    [Serializable]
    abstract class Account
    {
        public abstract string[] Functions { get; }
        public abstract Helper.Method[] MyFunctions { get; }
        public abstract string Role { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; } = "Не указано";
        public string FirstName { get; set; } = "Не указано";
        public string Patronomic { get; set; } = "Не указано";

        [NonSerialized]
        string birthday;
        public DateTime BirthdayDate { get; set; } = DateTime.Now;

        [NonSerialized]
        int age;
        public int Age { get; set; } = 0;

        [NonSerialized]
        int study;
        public int StudyYears { get; set; } = 0;

        public int WorkExperience { get; set; } = 0;
        public string Position { get; set; } = "Не указано";
        public string WorkPlace { get; set; } = "Не указано";
        public double Salary { get; set; } = 0;

        //первичная регистрация аккаунта
        public static Account RegisterNewAccount(string role)
        {
            Console.WriteLine($"Запускаю процесс регистрации аккаунта типа \"{role}\"...");
            Thread.Sleep(2000);
            Console.Clear();

            Console.WriteLine("Введите логин: ");
            string l;
            do l = Console.ReadLine();
            while (Helper.Check(l, "логин"));
            Console.Clear();
            Console.WriteLine("Введите пароль: ");
            string p;
            do p = Console.ReadLine();
            while (Helper.Check(p, "пароль"));

            Console.Clear();
            Console.WriteLine("Регистрация завершена");
            Account account = null;
            switch (role)
            {
                case "Admin":
                    account = new Admin(l, p);
                    break;
            }
            Console.WriteLine($"Тип аккаунта: {account.Role}");
            Console.WriteLine($"Логин: {account.Login}\nПароль: {account.Password}");
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
            return account;
        }
    }
}
