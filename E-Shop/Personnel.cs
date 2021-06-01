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
        public Personnel(string Login, string Password) : base(Login, Password)
        {
            Position = "Кадровик";
            WorkPlace = "Офис";
            Functions.AddRange(new (string, Method)[]
            {
                ("Просмотреть данные сотрудников", ShowUsers),
                ("Изменить данные сотрудников", EditUsers),
                ("Нанять сотрудника", HireUser),
                ("Уволить сотрудника", FireUser),
                ("Перевести сотрудника на другую должность", TransferToNewPosition)
            });
        }

        private void ShowUsers()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
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
        private void EditUsers()
        {
            List<Account> accounts = Helper.DeserializeAccount();
            List<string> accLogins = new List<string>();
            while (true)
            {
                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer))
                        accLogins.Add(a.Login);

                if (accLogins.Count == 0)
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
                    Helper.SerializeAccount(accounts);
                }
            }
        }
        private void TransferToNewPosition()
        {
            List<Account> accounts = Helper.DeserializeAccount();
            List<string> accountList = new List<string>();
            ConsoleMenu editMenu = new ConsoleMenu(accountList.ToArray());
            while (true)
            {
                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer))
                        if (a.isHired)
                            accountList.Add(a.Login);
                accountList.Remove(Login);

                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет сотрудников, которые бы вы могли перевести на другую должность");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");

                int chooseAcc = editMenu.PrintMenu();
                if (chooseAcc == accountList.Count - 1) break;

                //индекс акка, который переводим на новую должность
                int index = accounts.FindIndex(chosen => chosen.Login == accountList[chooseAcc]);
                ChangePosition(ref accounts, index);
                Helper.SerializeAccount(accounts);
            }
        }
        private void HireUser()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                List<string> accountList = new List<string>();

                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer) && a.isHired)
                        accountList.Add(a.Login);
                accountList.Remove(Login);
                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет сотрудников, которые вы бы могли нанять");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");

                ConsoleMenu deleteMenu = new ConsoleMenu(accountList.ToArray());
                int choseDeleteAcc = deleteMenu.PrintMenu();
                if (choseDeleteAcc == accountList.Count - 1) break;

                int index = accounts.FindIndex(deleted => deleted.Login == accountList[choseDeleteAcc]);
                accounts[index].isHired = true;

                Helper.SerializeAccount(accounts);
                Console.WriteLine($"Сотрудник {accounts[index].Login} нанят!");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            }
        }
        private void FireUser()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                List<string> accountList = new List<string>();

                foreach (Account a in accounts)
                    if (!a.isDeleted && !(a is Customer) && !a.isHired)
                        accountList.Add(a.Login);
                accountList.Remove(Login);

                if (accountList.Count == 0)
                {
                    Console.WriteLine($"Нет сотрудников, которые вы бы могли уволить");
                    Console.WriteLine("Нажмите любую кнопку...");
                    Console.ReadKey();
                    break;
                }
                accountList.Add("Назад");

                ConsoleMenu deleteMenu = new ConsoleMenu(accountList.ToArray());
                int choseDeleteAcc = deleteMenu.PrintMenu();
                if (choseDeleteAcc == accountList.Count - 1) break;

                int index = accounts.FindIndex(deleted => deleted.Login == accountList[choseDeleteAcc]);
                accounts[index].isHired = false;

                Helper.SerializeAccount(accounts);
                Console.WriteLine($"Сотрудник {accounts[index].Login} уволен!");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            }
        }

        //Вспомогательная функция 
        void ChangePosition(ref List<Account> accounts, int index)
        {
            List<string> positions = new List<string>();
            foreach (string s in accountTypes)
                if (s != accounts[index].Position && s != "Покупатель")
                    positions.Add(s);
            positions.Add("Отмена");

            ConsoleMenu positionMenu = new ConsoleMenu(positions.ToArray());
            int posNumber = positionMenu.PrintMenu();
            if (posNumber == positions.Count - 1) return;

            Account AccountNewPosition = positions[posNumber] switch
            {
                "Администратор" => new Admin(accounts[index].Login, accounts[index].Password),
                "Кадровик" => new Personnel(accounts[index].Login, accounts[index].Password),
                "Кладовщик" => new Warehouseman(accounts[index].Login, accounts[index].Password),
                "Продавец" => new Seller(accounts[index].Login, accounts[index].Password),
                _ => null,
            };
            accounts.RemoveAt(index);

            accounts.Insert(index, AccountNewPosition);
        }
    }
}
