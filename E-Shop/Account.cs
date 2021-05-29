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
        public bool isDeleted { get; set; } = false;
        public bool isHired { get; set; } = false;
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

        public Account() 
        {
            birthday = DateTime.Now.ToShortDateString();
            age = 0;
            study = 0;
        }
        public Account(string Login, string Password) : this()
        {
            this.Login = Login;
            this.Password = Password;
        }

        //первичная регистрация аккаунта
        public static Account Registration(string role)
        {
            Console.WriteLine($"Аккаунт типа \"{role}\"");
            Thread.Sleep(2000);
            Console.Clear();

            Console.WriteLine("Введите логин: ");
            string l;
            l = Console.ReadLine();
            //do l = Console.ReadLine();
            ////while (Helper.Check(l, "логин"));
            //Console.Clear();
            Console.WriteLine("Введите пароль: ");
            string p;
            p = Console.ReadLine();
            //do p = Console.ReadLine();
            //while (Helper.Check(p, "пароль"));

            Console.Clear();
            Console.WriteLine("Регистрация завершена");
            Account account = role switch
            {
                "Admin" => new Admin(l,p),
                _ => null,
            };

            Console.WriteLine($"Тип аккаунта: {account.Role}");
            Console.WriteLine($"Логин: {account.Login}\nПароль: {account.Password}");
            Console.WriteLine("Нажмите любую клавишу, чтобы добавить данный аккаунт в базу данных...");
            Console.ReadKey();
            return account;
        }
        public abstract int MainFunction();
    }
}
