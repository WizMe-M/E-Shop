using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    //Кадровик
    class Personnel : Account
    {
        
        public Personnel() : base() { Position = "Кадровик"; }
        public Personnel(string Login, string Password) : base(Login, Password) { Position = "Кадровик"; }

        void ShowUsers(List<Account> accounts)
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
                Console.WriteLine($"Данные пользователя {acc.Login}");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"СТАТУС СОТРУДНИКА - " + (acc.isHired ? "уволен" : "работает"));
                Console.ForegroundColor = acc.isHired ? ConsoleColor.DarkGray : ConsoleColor.White;
                Console.WriteLine($"ФИО сотрудника: {acc.LastName} {acc.FirstName} {acc.Patronomic}");
                Console.WriteLine($"Дата рождения: {acc.BirthdayDate.ToShortDateString()}; Возраст: {acc.Age}");
                Console.WriteLine($"Образование: {acc.StudyYears} лет; Опыт работы: {acc.WorkExperience} лет");
                Console.WriteLine($"Должность: {acc.Position}; Зарплата: {acc.Salary}");
                Console.WriteLine("Нажмите любую кнопку, чтобы продолжить...");
                Console.ReadKey();
            } while (true);
        }

        public override int MainFunction()
        {
            List<Account> accounts = Helper.GetAllAcounts();
            string[] functions = {
                "Просмотреть данные пользователя",
                "Изменить данные пользователя",
                "Зарегистрировать пользователя",
                "Удалить аккаунт",
                "Восстановить аккаунт",
                "Выйти из аккаунта",
                "Выйти из приложения" };
            ConsoleMenu adminMenu = new ConsoleMenu(functions);
            int chooseFunc = adminMenu.PrintMenu();
            Console.Clear();
            switch (chooseFunc)
            {
                case 0:
                    ShowUsers(accounts);
                    break;
                case 1:
                case 2:
                case 3:
                case 4:
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
