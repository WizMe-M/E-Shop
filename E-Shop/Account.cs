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
        [NonSerialized]
        protected static string[] accountTypes = { "Администратор", "Кадровик", "Кладовщик", "Продавец", "Бухгалтер", "Покупатель", "Назад" };
        [NonSerialized]
        protected double[] Salaries = new double[6] { 80000.0, 110000.0, 30000.0, 20000.0, 90000.0, 0.0 };
        public delegate void Method();

        [NonSerialized]
        public List<(string, Method)> Functions = new List<(string, Method)>();
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
                if (this is Admin || this is Personnel || this is Accountant)
                    place = "Офис";
                else place = value;
            }
        }
        public double Salary
        {
            get
            {
                int index = 0;
                for (int i = 0; i < accountTypes.Length - 1; i++)
                    if (Position == accountTypes[i])
                    {
                        index = i;
                        break;
                    }
                return Salaries[index];
            }
        }


        public Account()
        {
            Functions = new List<(string, Method)>();
            birthday = DateTime.Now.ToShortDateString();
            age = 0;
            study = 0;
        }
        public Account(string Login, string Password) : this()
        {
            this.Login = Login;
            this.Password = Password;
        }

        public virtual void OnDeserializing()
        {
            Functions = new List<(string, Method)>();
            Salaries = new double[6] { 80000.0, 110000.0, 30000.0, 20000.0, 90000.0, 0.0 };
        }
        public static Account Registration(string role)
        {
            Console.Clear();
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
                "Бухгалтер" => new Accountant(l, p),
                "Продавец" => new Seller(l, p),
                _ => null,
            };

            Console.Write("Введите фамилию: ");
            account.FirstName = Console.ReadLine().Trim();
            Console.Write("\nВведите имя: ");
            account.LastName = Console.ReadLine().Trim();
            Console.Write("\nВведите отчество: ");
            account.Patronomic = Console.ReadLine().Trim();

            return account;
        }
        public void MainMenu()
        {
            while (true)
            {
                List<string> func = new List<string>();
                foreach ((string, Method) pair in Functions)
                    func.Add(pair.Item1);
                func.AddRange(new string[] { "Выйти из аккаунта", "Выйти из приложения" });
                ConsoleMenu adminMenu = new ConsoleMenu(func.ToArray());
                int chooseFunc = adminMenu.PrintMenu();
                if (chooseFunc == func.Count - 2) return;
                if (chooseFunc == func.Count - 1) Program.Terminate();
                Console.Clear();
                Functions[chooseFunc].Item2();
            }
        }
    }
}
