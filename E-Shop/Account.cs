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
        public static string[] accountTypes = { "Администратор", "Кадровик", "Кладовщик", "Назад" };
        public bool isDeleted { get; set; } = false;
        public bool isHired { get; set; } = true;
        public string Position { get; set; }
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
        [NonSerialized]
        string place;
        public string WorkPlace
        {
            get { return place; }
            set
            {
                if (this is Admin || this is Personnel)
                    place = "Офис";
                else place = value;
            }
        }
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

        public static Account Registration(string role)
        {
            Console.WriteLine($"Аккаунт типа \"{role}\"");
            Thread.Sleep(2000);
            Console.Clear();

            Console.WriteLine("Введите логин: ");
            string l = Console.ReadLine();
            Console.WriteLine("Введите пароль: ");
            string p = Console.ReadLine();
            Account account = role switch
            {
                "Администратор" => new Admin(l, p),
                "Кадровик" => new Personnel(l, p),
                "Кладовщик" => new Warehouseman(l, p),
                _ => null,
            };
            return account;
        }
        public abstract int MainMenu();
    }
}
