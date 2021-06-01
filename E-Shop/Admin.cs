using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace E_Shop
{
    //Администратористратор
    [Serializable]
    class Admin : Account
    {
        public Admin(string Login, string Password) : base(Login, Password)
        {
            Position = "Администратор";
            WorkPlace = "Офис";
            Functions.AddRange(new (string, Method)[] {
                    ("Просмотреть данные пользователя", ShowAccount),
                    ("Изменить данные пользователя", EditAccount),
                    ("Зарегистрировать пользователя", RegisterNewAccount),
                    ("Удалить пользователя", DeleteAccount),
                    ("Восстановить пользователя", RestoreAccount)});
        }

        void ShowAccount()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                List<string> accLogins = new List<string>();
                foreach (Account a in accounts)
                    accLogins.Add(a.Login);

                if (accLogins.Count == 0)
                {
                    Console.WriteLine("Нет пользователей, данные которых вы могли бы посмотреть...");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                }

                accLogins.Add("Назад");

                ConsoleMenu showMenu = new ConsoleMenu(accLogins.ToArray());
                int chooseShowAcc = showMenu.PrintMenu();
                if (chooseShowAcc == accLogins.Count - 1) break;
                Account acc = accounts.Find(chosen => chosen.Login == accLogins[chooseShowAcc]);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Данные аккаунта {acc.Login}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"СТАТУС АККАУНТА - " + (acc.isDeleted ? "удалён" : "активен"));
                Console.ForegroundColor = acc.isDeleted ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.WriteLine($"Логин аккаунта:\t\t{acc.Login}");
                Console.WriteLine($"Пароль аккаунта:\t{acc.Password}");
                Console.WriteLine($"ФИО пользователя: {acc.LastName} {acc.FirstName} {acc.Patronomic}");
                if (!(acc is Customer))
                {
                    Console.WriteLine($"Дата рождения: {acc.BirthdayDate.ToShortDateString()}; Возраст: {acc.Age}");
                    Console.WriteLine($"Образование: {acc.StudyYears} лет; Опыт работы: {acc.WorkExperience} лет");
                    Console.WriteLine($"Должность: {acc.Position}; Зарплата: {acc.Salary}");
                }
                else Console.WriteLine($"Почта пользователя: {(acc as Customer).Email}");

                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            }
        }
        void RegisterNewAccount()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                ConsoleMenu registerMenu = new ConsoleMenu(accountTypes);
                int chooseType = registerMenu.PrintMenu();

                if (chooseType == accountTypes.Length - 1) break;

                Account newAccount = Registration(accountTypes[chooseType]);
                if (newAccount != null)
                    accounts.Add(newAccount);
                Helper.SerializeAccount(accounts);
            }
        }
        void DeleteAccount()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                List<string> accountList = new List<string>();

                foreach (Account acc in accounts)
                    if (!acc.isDeleted)
                        accountList.Add(acc.Login);
                accountList.Remove(Login);
                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет аккаунтов, которые бы вы могли удалить");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");

                int choseDeleteAcc;
                ConsoleMenu deleteMenu = new ConsoleMenu(accountList.ToArray());
                choseDeleteAcc = deleteMenu.PrintMenu();
                if (choseDeleteAcc == accountList.Count - 1) break;

                int index = accounts.FindIndex(deleted => deleted.Login == accountList[choseDeleteAcc]);
                accounts[index].isDeleted = true;

                Helper.SerializeAccount(accounts);
                Console.WriteLine($"Аккаунт {accounts[index].Login} удалён!");
                Thread.Sleep(500);
            }
        }
        void RestoreAccount()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                List<string> accountList = new List<string>();

                foreach (Account acc in accounts)
                    if (acc.isDeleted)
                        accountList.Add(acc.Login);
                accountList.Remove(Login);
                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет аккаунтов, которые бы вы могли восттановить");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");

                int choseDeleteAcc;
                ConsoleMenu deleteMenu = new ConsoleMenu(accountList.ToArray());
                choseDeleteAcc = deleteMenu.PrintMenu();
                if (choseDeleteAcc == accountList.Count - 1) break;

                int index = accounts.FindIndex(deleted => deleted.Login == accountList[choseDeleteAcc]);
                accounts[index].isDeleted = false;

                Helper.SerializeAccount(accounts);
                Console.WriteLine($"Аккаунт {accounts[index].Login} восстановлен!");
                Thread.Sleep(500);
            }
        }
        void EditAccount()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                List<string> accLogins = new List<string>();
                foreach (Account a in accounts)
                    accLogins.Add(a.Login);
                accLogins.Add("Назад");

                ConsoleMenu editMenu = new ConsoleMenu(accLogins.ToArray());
                int chooseShowAcc = editMenu.PrintMenu();
                if (chooseShowAcc == accLogins.Count - 1) break;

                int index = accounts.FindIndex(chosen => chosen.Login == accLogins[chooseShowAcc]);

                while (true)
                {
                    string[] accountData =
                    {
                    "Логин - " + accounts[index].Login,
                    "Пароль - " + accounts[index].Password,
                    "Фамилия - " + accounts[index].FirstName,
                    "Имя - " + accounts[index].LastName,
                    "Отчество - " + accounts[index].Patronomic,
                    "Дата рождения: " + accounts[index].BirthdayDate.ToShortDateString(),
                    "Возраст - " + accounts[index].Age.ToString(),
                    "Образование - " + accounts[index].StudyYears.ToString(),
                    "Опыт работы - " + accounts[index].WorkExperience.ToString(),
                    "Назад"
                    };
                    ConsoleMenu dataMenu = new ConsoleMenu(accountData);
                    int chooseData = dataMenu.PrintMenu();
                    if (chooseData == accountData.Length - 1) break;

                    Console.Clear();
                    Console.WriteLine("Введите новые данные:");
                    string changedData = Console.ReadLine().Trim();
                    switch (chooseData)
                    {
                        case 0:
                            accounts[index].Login = changedData;
                            break;
                        case 1:
                            accounts[index].Password = changedData;
                            break;
                        case 2:
                            accounts[index].FirstName = changedData;
                            break;
                        case 3:
                            accounts[index].LastName = changedData;
                            break;
                        case 4:
                            accounts[index].Patronomic = changedData;
                            break;
                        case 5:
                            accounts[index].BirthdayDate = DateTime.Parse(changedData);
                            break;
                        case 6:
                            accounts[index].Age = int.Parse(changedData);
                            break;
                        case 7:
                            accounts[index].StudyYears = int.Parse(changedData);
                            break;
                        case 8:
                            accounts[index].WorkExperience = int.Parse(changedData);
                            break;
                    }
                    Helper.SerializeAccount(accounts);
                }
            }
        }
    }
}
