using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    //[Serializable]
    static class Budget
    {
        readonly static (string, double)[] nalogi = { ("НДФЛ", 13), ("ПФР", 22), ("ФФОМС", 5.1), ("ФСС", 2.9), ("Н/СЛ", 0.2), ("УСНО", 6) };

        //в расчёте за месяц
        public static double Proceeds
        {
            get
            {
                return Incomes - Costs;
            }
        }
        public static double Incomes
        {
            get
            {
                return GetIncome("месяц");
            }
        }
        public static double Costs
        {
            get
            {
                return GetCosts("месяц");
            }
        }

        //получает доход от всех заказов за указанный period
        public static double GetIncome(string period)
        {
            int dayCount = period switch
            {
                "день" => 1,
                "месяц" => 31,
                "полгода" => 180,
                "год" => 366,
                _ => -1
            };

            double money = 0.0;
            List<Receipt> receipts = Helper.DeserializeReceipt();

            foreach (Receipt receipt in receipts)
                if (receipt.isRegistered && DateTime.UtcNow.Subtract(receipt.RegistrationDate).Days <= dayCount)
                    money += receipt.FullPrice;

            //налог на УСНО
            money -= money * 0.06;
            return money;
        }

        //получает расходы на зарплаты сотрудникам за указанный period
        public static double GetCosts(string period)
        {
            double coefficient = period switch
            {
                "день" => 1.0 / 30.0,
                "месяц" => 1,
                "полгода" => 6,
                "год" => 12,
                _ => 0
            };

            List<Account> accounts = Helper.DeserializeAccount();
            double money = 0.0;

            foreach (Account account in accounts)
                money += CalculateCost(account.Salary);

            money *= coefficient;
            return money;
        }
        public static double CalculateCost(double salary)
        {
            double cost = salary;
            //22% + 5.1% + 2.9% + 0.2% = 30.2% или 0.302
            cost *= 1.302;
            return cost;
        }
        public static void CalculateSalary(double salary)
        {
            Console.WriteLine("Выплаты компании в фонды:");
            for (int i = 1; i < nalogi.Length - 1; i++)
            {
                Console.Write($"{nalogi[i].Item1}: {salary / 100 * nalogi[i].Item2}; ");
            }   
            Console.WriteLine();
        }
    }
}
