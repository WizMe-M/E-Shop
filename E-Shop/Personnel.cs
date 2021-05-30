using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    //Кадровик
    [Serializable]
    class Personnel : Account
    {
        public Personnel() : base() 
        {
            Position = "Кадровик";
            WorkPlace = "Офис";
        }
        public Personnel(string Login, string Password) : base(Login, Password) 
        { 
            Position = "Кадровик";
            WorkPlace = "Офис";
        }

        void ShowUsers()
        {
            while (true)
            {
                List<Account> accounts = Helper.GetAllAcounts();
                List<string> accLogins = new List<string>();
                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer))
                        accLogins.Add(a.Login);

                if (accLogins.Count == 0)
                {
                    Console.WriteLine("Нет сотрудников, данные которых вы могли бы посмотреть...");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                }

                accLogins.Add("Назад");

                ConsoleMenu showMenu = new ConsoleMenu(accLogins.ToArray());
                int chooseAcc = showMenu.PrintMenu();
                if (chooseAcc == accLogins.Count - 1) break;
                Account acc = accounts.Find(chosen => chosen.Login == accLogins[chooseAcc]);

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine($"Данные пользователя {acc.Login}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"СТАТУС СОТРУДНИКА - " + (acc.isHired ? "работает" : "уволен"));
                Console.ForegroundColor = acc.isHired ? ConsoleColor.White : ConsoleColor.DarkGray;
                Console.WriteLine($"ФИО сотрудника: {acc.LastName} {acc.FirstName} {acc.Patronomic}");
                Console.WriteLine($"Дата рождения: {acc.BirthdayDate.ToShortDateString()}; Возраст: {acc.Age}");
                Console.WriteLine($"Образование: {acc.StudyYears} лет; Опыт работы: {acc.WorkExperience} лет");
                Console.WriteLine($"Должность: {acc.Position}; Зарплата: {acc.Salary}");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            }
        }
        void EditAccount()
        {
            while (true)
            {
                List<Account> accounts = Helper.GetAllAcounts();
                List<string> accLogins = new List<string>();
                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer))
                        accLogins.Add(a.Login);

                if(accLogins.Count == 0)
                {
                    Console.WriteLine("Нет аккаунтов сотрудников для изменения.");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                }

                accLogins.Add("Назад");

                ConsoleMenu editMenu = new ConsoleMenu(accLogins.ToArray());
                int chooseAcc = editMenu.PrintMenu();
                if (chooseAcc == accLogins.Count - 1) break;
                int index = accounts.FindIndex(chosen => chosen.Login == accLogins[chooseAcc]);

                while (true)
                {
                    string[] accountData = {
                    "Фамилия - " + accounts[index].FirstName,
                    "Имя - " + accounts[index].LastName,
                    "Отчество - " + accounts[index].Patronomic,
                    "Дата рождения: " + accounts[index].BirthdayDate.ToShortDateString(),
                    "Возраст - " + accounts[index].Age.ToString(),
                    "Образование - " + accounts[index].StudyYears.ToString(),
                    "Опыт работы - " + accounts[index].WorkExperience.ToString(),
                    "Назад" };
                    ConsoleMenu dataMenu = new ConsoleMenu(accountData);
                    int chooseData = dataMenu.PrintMenu();
                    if (chooseData == accountData.Length - 1) break;
                    Console.Clear();
                    Console.WriteLine("Введите новые данные:");
                    string changedData = Console.ReadLine().Trim();
                    switch (chooseData)
                    {
                        case 0:
                            accounts[index].FirstName = changedData;
                            break;
                        case 1:
                            accounts[index].LastName = changedData;
                            break;
                        case 2:
                            accounts[index].Patronomic = changedData;
                            break;
                        case 3:
                            accounts[index].BirthdayDate = DateTime.Parse(changedData);
                            break;
                        case 4:
                            accounts[index].Age = int.Parse(changedData);
                            break;
                        case 5:
                            accounts[index].StudyYears = int.Parse(changedData);
                            break;
                        case 6:
                            accounts[index].WorkExperience = int.Parse(changedData);
                            break;
                        case 7:
                            accounts[index].Salary = double.Parse(changedData);
                            break;
                    }
                    Helper.SaveAllAcounts(accounts);
                }
            }
        }
        void TransferToNewPosition()
        {
            while (true)
            {
                List<Account> accounts = Helper.GetAllAcounts();
                List<string> accountList = new List<string>();

                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer))
                        if (a.isHired && a != this)
                            accountList.Add(a.Login);

                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет аккаунтов, которые бы вы могли перевести на другую должность");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");

                ConsoleMenu editMenu = new ConsoleMenu(accountList.ToArray());
                int chooseAcc = editMenu.PrintMenu();

                if (chooseAcc == accountList.Count - 1) break;

                Account newPosition = accounts.Find(chosen => chosen.Login == accountList[chooseAcc]);
                accounts.Remove(newPosition);
                List<string> positions = new List<string>() { "Администратор", "Кадровик", "Кладовщик", "Назад" };
                positions.Remove(newPosition.Position);
                ConsoleMenu positionMenu = new ConsoleMenu(positions.ToArray());
                int posNumber = positionMenu.PrintMenu();
                newPosition.Position = positions[posNumber];
                accounts.Add(newPosition);
                Helper.SaveAllAcounts(accounts);
            }
        }
        void ChangeHireStatus(bool toHireStatus)
        {
            while (true)
            {
                List<Account> accounts = Helper.GetAllAcounts();
                List<string> accountList = new List<string>();
                if (toHireStatus)
                {
                    foreach (Account a in accounts)
                        if (!a.isDeleted && !(a is Customer))
                            if (a != this && !a.isHired)
                                accountList.Add(a.Login);
                }
                else
                {
                    foreach (Account a in accounts)
                        if (!a.isDeleted && !(a is Customer))
                            if (a != this && a.isHired)
                                accountList.Add(a.Login);
                }

                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет аккаунтов, которые бы вы могли {(toHireStatus ? "нанять" : "уволить")}");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");
                ConsoleMenu deleteMenu = new ConsoleMenu(accountList.ToArray());
                int choseDeleteAcc = deleteMenu.PrintMenu();
                if (choseDeleteAcc == accountList.Count - 1) break;
                int index = accounts.FindIndex(deleted => deleted.Login == accountList[choseDeleteAcc]);
                accounts[index].isHired = toHireStatus;
                Helper.SaveAllAcounts(accounts);
                Console.WriteLine($"Аккаунт {accounts[index].Login} " + (accounts[index].isHired ? "нанят" : "уволен") + "!");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        public override int MainMenu()
        {
            string[] functions = {
                "Просмотреть данные сотрудников",
                "Изменить данные сотрудников",
                "Нанять сотрудника",
                "Уволить сотрудника",
                "Перевести сотрудника на другую должность",
                "Выйти из аккаунта",
                "Выйти из приложения" };
            ConsoleMenu adminMenu = new ConsoleMenu(functions);
            int chooseFunc = adminMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
                    ShowUsers();
                    break;
                case 1:
                    EditAccount();
                    break;
                case 2:
                    ChangeHireStatus(true);
                    break;
                case 3:
                    ChangeHireStatus(false);
                    break;
                case 4:
                    TransferToNewPosition();
                    break;
                default:
                    if (chooseFunc == functions.Length - 1)
                        return -1;
                    else return 1;
            }
            return 0;
        }

    }
}
