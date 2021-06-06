using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
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
        public List<(string, Method)> Functions;

        public bool isDeleted { get; set; } = false;
        public bool isHired { get; set; } = true;
        public string Position { get; set; }
        public double Salary
        {
            get
            {
                for (int i = 0; i < accountTypes.Length - 1; i++)
                    if (Position == accountTypes[i])
                        return Salaries[i];
                return 0;
            }
        }


        string login;
        public string Login
        {
            get
            {
                return login;
            }
            set
            {
                string pattern = @"^(?:[a-zA-Z]{3,10})$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Логин должен состоять из латинских букв и быть длиной от 3 до 10 символов!");
                    Console.Write("Введите логин ещё раз: ");
                    value = Console.ReadLine().Trim();
                }
                login = value;
            }
        }

        string password;
        public string Password
        {
            get { return password; }
            set
            {
                string pattern = @"^(?!.*[A-Z]{3,})(?=.*[A-Z](?=.*[A-Z](?=.*[A-Z])))(?=.*\d(?=.*\d(?=.*\d)))(?=.*[@$!%*?&](?=.*[@$!%*?&]))[A-Za-z\d@$!%*?&]{8,}$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Пароль должен быть написан латинскими буквами, иметь три заглавные буквы (не подряд), " +
                        "три цифры, два спец. символа \nи быть длиной не менее 8 символов");
                    Console.Write("Введите пароль: ");
                    value = Console.ReadLine().Trim();
                }
                password = value;
            }
        }

        string familiya;
        public string LastName
        {
            get
            {
                return familiya;
            }
            set
            {
                string pattern = @"^(([A-Z](?:[a-z]*))|([А-Я](?:[а-я]*)))$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Фамилия должно состоять из одного слова, начинаться с заглавной буквы и " +
                        "состоять целиком либо из латиницы, либо из кириллицы.");
                    Console.Write("Введите фамилию ещё раз: ");
                    value = Console.ReadLine().Trim();
                }
                familiya = value;
            }
        }

        string imya;
        public string FirstName
        {
            get
            {
                return imya;
            }
            set
            {
                string pattern = @"^(([A-Z](?:[a-z]*))|([А-Я](?:[а-я]*)))$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Имя должно состоять из одного слова, начинаться с заглавной буквы и " +
                        "состоять целиком либо из латиницы, либо из кириллицы.");
                    Console.Write("Введите имя ещё раз: ");
                    value = Console.ReadLine().Trim();
                }
                imya = value;
            }
        }

        string otchestvo;
        public string Patronomic
        {
            get
            {
                return otchestvo;
            }
            set
            {
                string pattern = @"^(([A-Z](?:[a-z]*))|([А-Я](?:[а-я]*)))$";
                while (!Regex.IsMatch(value, pattern))
                {
                    Console.Clear();
                    Console.WriteLine("Отчество должно состоять из одного слова, начинаться с заглавной буквы и " +
                        "состоять целиком либо из латиницы, либо из кириллицы.");
                    Console.Write("Введите отчество ещё раз: ");
                    value = Console.ReadLine().Trim();
                }
                otchestvo = value;
            }
        }

        DateTime birthday;
        public string BirthdayDate
        {
            get
            {
                return birthday.ToShortDateString();
            }
            set
            {
                string pattern = @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)
                (?:0?[1,3-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?
                (?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))
                $|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$";
                while (!Regex.IsMatch(value, pattern) || (DateTime.Parse(value).Year < 1960 || DateTime.Parse(value) > DateTime.UtcNow))
                {
                    Console.Clear();
                    Console.WriteLine("Принимаются даты формата дд.мм.гггг или дд/мм/гггг или дд-мм-гггг, в том числе високосные года.");
                    Console.Write("Введите дату рождения ещё раз: ");
                    value = Console.ReadLine().Trim();
                }
                birthday = DateTime.Parse(value);
            }
        }

        int age;
        public int Age
        {
            get
            {
                return age;
            }
            set
            {
                if (this is Customer)
                    while (value < 6 || value > 100)
                    {
                        Console.Clear();
                        Console.WriteLine("Покупатели не могут быть младше 6 и старше 100 лет!");
                        Console.Write("Введите возраст покупателя: ");
                        value = Console.ReadLine().SafeParseInt();
                    }
                else
                    while (value < 18 || value > 60)
                    {
                        Console.Clear();
                        Console.WriteLine("Сотрудники не могут быть младше 18 и старше 60 лет!");
                        Console.Write("Введите возраст сотрудника: ");
                        value = Console.ReadLine().SafeParseInt();
                    }
                age = value;
            }
        }

        int study;
        public int StudyYears
        {
            get
            {
                return study;
            }
            set
            {
                while (value < 0 || value > 15 || value > Age - 6)
                {
                    Console.Clear();
                    Console.WriteLine("Количество лет потраченное на обучение не может быть отрицательным, " +
                        "быть больше 15 лет или превышать возраст человека (минус шесть первых лет жизни).");
                    Console.Write("Введите годы обучения: ");
                    value = Console.ReadLine().SafeParseInt();
                }
                study = value;
            }
        }

        int work;
        public int WorkExperience
        {
            get
            {
                return work;
            }
            set
            {
                while (value < 0 || value > Age - StudyYears - 6)
                {
                    Console.Clear();
                    Console.WriteLine("Трудовой стаж (опыт работы) не может быть отрицательным или " +
                        "превышать разницу возраста (минус шесть первых лет жизни) и лет обучения.");
                    Console.Write("Введите опыт работы: ");
                    value = Console.ReadLine().SafeParseInt();
                }
                work = value;
            }
        }

        string place;
        public string WorkPlace
        {
            get { return place; }
            set
            {
                if (this is Admin || this is Personnel || this is Accountant)
                {
                    if (value != "Офис")
                    {
                        Console.Clear();
                        Console.WriteLine("Сотрудникам, находящимся на должностях: \nадминистратор, кадровик и бухгалтер " +
                            "автоматически присваивается место работы \"Офис\"");
                        Thread.Sleep(500);
                        value = "Офис";
                    }
                }
                else
                {
                    string pattern = "(^(?:[A-Za-z `\']+)$)|(^(?:[А-Яа-яёЁ \"]+)$)";
                    while (!Regex.IsMatch(value, pattern))
                    {
                        Console.Clear();
                        Console.WriteLine("Место работы пишется латиницей (допускается ввод апострофа и/или одинарной кавычки) " +
                            "или кириллицей (допускаются двойные кавычки). Также разрешены пробелы между словами.");
                        Console.Write("Введите место работы: ");
                        value = Console.ReadLine().Trim();
                    }
                }
                place = value;
            }
        }


        public Account()
        {
            Functions = new List<(string, Method)>();
            LastName = "Фамилия";
            FirstName = "Имя";
            Patronomic = "Отчество";
            BirthdayDate = "01.01.1960";
            Age = 18;
            StudyYears = 0;
            WorkExperience = 0;
            WorkPlace = "Офис";
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
            Account account = role switch
            {
                "Покупатель" => new Customer(),
                "Администратор" => new Admin(),
                "Кадровик" => new Personnel(),
                "Кладовщик" => new Warehouseman(),
                "Бухгалтер" => new Accountant(),
                "Продавец" => new Seller(),
                _ => null,
            };
            Console.Clear();
            Console.Write("Введите логин: ");
            account.Login = Console.ReadLine().Trim();
            Console.Clear();
            Console.Write("Введите пароль: ");
            account.Password = Console.ReadLine().Trim();
            Console.Clear();
            Console.Write("Введите фамилию: ");
            account.LastName = Console.ReadLine().Trim();
            Console.Clear();
            Console.Write("Введите имя: ");
            account.FirstName = Console.ReadLine().Trim();
            Console.Clear();
            Console.Write("Введите отчество: ");
            account.Patronomic = Console.ReadLine().Trim();
            if (account is Customer)
            {
                Console.Clear();
                Console.WriteLine("Введите электронный адрес, на который хотите получать чеки:");
                (account as Customer).Email = Console.ReadLine().Trim();
            }
            else
            {
                Console.Clear();
                Console.Write("Введите дату рождения: ");
                account.BirthdayDate = Console.ReadLine().Trim();
                Console.Clear();
                Console.Write("Введите возраст: ");
                account.Age = Console.ReadLine().SafeParseInt();
                Console.Clear();
                Console.Write("Введите годы обучения: ");
                account.StudyYears = Console.ReadLine().SafeParseInt();
                Console.Clear();
                Console.Write("Введите опыт работы: ");
                account.WorkExperience = Console.ReadLine().SafeParseInt();
                Console.Clear();
                Console.Write("Введите место работы: ");
                account.WorkPlace = Console.ReadLine().Trim();
            }

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
