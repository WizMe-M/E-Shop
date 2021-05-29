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
        public Admin() : base() { Position = "Администратор"; }
        public Admin(string Login, string Password) : base(Login, Password) { Position = "Администратор"; }

        //админ регистрирует нового пользователя в базу данных
        void ShowAccount(List<Account> accounts)
        {
            List<string> accLogins = new List<string>();
            foreach (Account a in accounts)
                accLogins.Add(a.Login);
            accLogins.Add("Назад");
            int chooseShowAcc;
            do
            {
                ConsoleMenu showMenu = new ConsoleMenu(accLogins.ToArray());
                chooseShowAcc = showMenu.PrintMenu();
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
                Console.WriteLine($"Дата рождения: {acc.BirthdayDate.ToShortDateString()}; Возраст: {acc.Age}");
                Console.WriteLine($"Образование: {acc.StudyYears} лет; Опыт работы: {acc.WorkExperience} лет");
                Console.WriteLine($"Должность: {acc.Position}; Зарплата: {acc.Salary}");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            } while (true);
        }
        Account RegisterNewAccount()
        {
            string[] accountTypes = { "Администратор", "Кадровик", "Назад" };
            do
            {
                ConsoleMenu registerMenu = new ConsoleMenu(accountTypes);
                int choseType = registerMenu.PrintMenu();
                return choseType switch
                {
                    0 => (Admin)Registration("Администратор"),
                    1 => (Personnel)Registration("Кадровик"),
                    _ => null,
                };
            } while (true);
        }
        void ChangeAccountDeleteStatus(ref List<Account> accounts, bool toDeleteStatus)
        {
            List<string> accountList = new List<string>();
            do
            {
                if (toDeleteStatus)
                {
                    foreach (Account acc in accounts)
                        if (acc != this && !acc.isDeleted)
                            accountList.Add(acc.Login);
                }
                else
                {
                    foreach (Account acc in accounts)
                        if (acc != this && acc.isDeleted)
                            accountList.Add(acc.Login);
                }
                accountList.Add("Назад");
                int choseDeleteAcc;
                ConsoleMenu deleteMenu = new ConsoleMenu(accountList.ToArray());
                choseDeleteAcc = deleteMenu.PrintMenu();
                if (choseDeleteAcc == accountList.Count - 1) break;
                int index = accounts.FindIndex(deleted => deleted.Login == accountList[choseDeleteAcc]);
                accounts[index].isDeleted = toDeleteStatus;
                Console.WriteLine($"Аккаунт {accounts[index].Login} " + (accounts[index].isDeleted ? "удалён" : "восстановлен") + "!");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
                accountList.RemoveRange(0, accountList.Count);
                accountList.TrimExcess();
            } while (true);
        }
        void EditAccount(ref List<Account> accounts)
        {
            List<string> accLogins = new List<string>();
            foreach (Account a in accounts)
                accLogins.Add(a.Login);
            accLogins.Add("Назад");
            int chooseShowAcc;
            do
            {
                ConsoleMenu editMenu = new ConsoleMenu(accLogins.ToArray());
                chooseShowAcc = editMenu.PrintMenu();
                if (chooseShowAcc == accLogins.Count - 1) break;
                int index = accounts.FindIndex(chosen => chosen.Login == accLogins[chooseShowAcc]);
                do
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
                } while (true);
            } while (true);
        }

        public override int MainFunction()
        {
            List<Account> accounts = Helper.GetAllAcounts();
            string[] functions = {
                "Просмотреть данные пользователя",
                "Изменить данные пользователя",
                "Зарегистрировать пользователя",
                "Удалить пользователя",
                "Восстановить аккаунт пользователя",
                "Выйти из аккаунта",
                "Выйти из приложения" };
            ConsoleMenu adminMenu = new ConsoleMenu(functions);
            int chooseFunc = adminMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
                    ShowAccount(accounts);
                    break;
                case 1:
                    EditAccount(ref accounts);
                    break;
                case 2:
                    Account acc = RegisterNewAccount();
                    if (acc != null)
                        accounts.Add(acc);
                    break;
                case 3:
                    ChangeAccountDeleteStatus(ref accounts, true);
                    break;
                case 4:
                    ChangeAccountDeleteStatus(ref accounts, false);
                    break;
                default:
                    if (chooseFunc == functions.Length - 1)
                        return -1;
                    else return 1;
            }
            Helper.SaveAllAcounts(accounts);
            return 0;
        }
    }
}
