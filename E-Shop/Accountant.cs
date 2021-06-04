using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace E_Shop
{
    [Serializable]
    class Accountant : Account
    {
        public Accountant(string Login, string Password) : base(Login, Password)
        {
            Position = "Бухгалтер";
            Functions.AddRange(new (string, Method)[] {
                ("Посмотреть доходы сотрудников", EmployeeIncome),
                ("Посмотреть доходы от продаж", CompanyIncome),
                ("Посмотреть выручку", CompanyProceeds)});
        }

        private void EmployeeIncome()
        {
            while (true)
            {
                List<Account> accounts = Helper.DeserializeAccount();
                accounts.RemoveAll(a => a is Customer);
                List<string> accountLogins = new List<string>();
                foreach (Account a in accounts)
                    if (!(a is Customer) && a.isHired)
                        accountLogins.Add(a.Login);
                if (accountLogins.Count == 0)
                {
                    Console.WriteLine("Нет сотрудников?...");
                    Thread.Sleep(1000);
                    return;
                }
                accountLogins.Add("Назад");

                ConsoleMenu accountMenu = new ConsoleMenu(accountLogins.ToArray());
                int choose = accountMenu.PrintMenu();
                if (choose == accountLogins.Count - 1) return;

                //вывод
                Console.WriteLine($"Зарплата сотрудника {accounts[choose].FirstName} " +
                    $"{accounts[choose].LastName} aka {accounts[choose].Login}:");
                Console.WriteLine($"Зарплата: {accounts[choose].Salary}; " +
                    $"Выплаты НДФЛ: {accounts[choose].Salary * 0.13}; " +
                    $"На руки: {accounts[choose].Salary * 0.87} рублей");
                Budget.CalculateSalary(accounts[choose].Salary);
                Console.WriteLine("Нажмите любую кнопку...");
                Console.ReadKey();
                //не сериализуй аккаунты!
            }
        }
        private void CompanyIncome()
        {
            Console.WriteLine("Доходы компании от продаж:");
            Console.WriteLine($"День - {Budget.GetIncome("день")}; " +
                $"Месяц - {Budget.GetIncome("месяц")}; " +
                $"Полгода - {Budget.GetIncome("полгода")}; " +
                $"Год - {Budget.GetIncome("год")} рублей");
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();
        }
        private void CompanyProceeds()
        {
            Console.WriteLine("Расходы компании на выплату ЗП:");
            Console.WriteLine($"День - {Budget.GetCosts("день")}; " +
                $"Месяц - {Budget.GetCosts("месяц")}; " +
                $"Полгода - {Budget.GetCosts("полгода")}; " +
                $"Год - {Budget.GetCosts("год")} рублей");
            Console.WriteLine();
            Console.WriteLine("Доходы компании от продаж:");
            Console.WriteLine($"День - {Budget.GetIncome("день")}; " +
                $"Месяц - {Budget.GetIncome("месяц")}; " +
                $"Полгода - {Budget.GetIncome("полгода")}; " +
                $"Год - {Budget.GetIncome("год")} рублей");
            Console.WriteLine();
            Console.WriteLine($"Ежемесячная выручка: {Budget.Incomes} - {Budget.Costs} = {Budget.Proceeds} рублей");
            Console.WriteLine("Нажмите любую кнопку...");
            Console.ReadKey();

        }
        public override void OnDeserializing()
        {
            base.OnDeserializing();
            Functions.AddRange(new (string, Method)[] {
                ("Доходы сотрудников", EmployeeIncome),
                ("Посмотреть доходы от продаж", CompanyIncome),
                ("Бюджет компании", CompanyProceeds)});
        }
    }
}
