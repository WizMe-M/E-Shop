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
        public static string[] accountTypes = { "Покупатель", "Администратор", "Кадровик", "Кладовщик", "Продавец", "Назад" };
        public bool isDeleted { get; set; } = false;
        public bool isHired { get; set; } = true;
        public string Position { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; } = "Не указано";
        public string FirstName { get; set; } = "Не указано";
        public string Patronomic { get; set; } = "Не указано";

        string birthday;
        public DateTime BirthdayDate { get; set; } = DateTime.Now;

        int age;
        public int Age { get; set; } = 0;

        int study;
        public int StudyYears { get; set; } = 0;
        public int WorkExperience { get; set; } = 0;
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
            Console.WriteLine("Введите логин: ");
            string l = Console.ReadLine();
            Console.WriteLine("Введите пароль: ");
            string p = Console.ReadLine();
            Account account = role switch
            {
                "Покупатель" => new Customer(l, p),
                "Администратор" => new Admin(l, p),
                "Кадровик" => new Personnel(l, p),
                "Кладовщик" => new Warehouseman(l, p),
                "Продавец" => new Seller(l, p),
                _ => null,
            };
            return account;
        }
        public abstract int MainMenu();
    }
}
