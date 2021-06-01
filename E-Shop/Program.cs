using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace E_Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            Account user = null;
            Helper.FirstLaunch();
            ConsoleMenu RegisterOrLoginMenu = new ConsoleMenu(new string[]
            { "Зарегистрироваться", "Войти в аккаунт", "Выйти из приложения" });

            while (true)
            {
                Console.Clear();
                int choose = RegisterOrLoginMenu.PrintMenu();
                switch (choose)
                {
                    case 0:
                        user = Account.Registration("Покупатель");

                        //вынести в отдельную функцию
                        List<Account> accounts = Helper.DeserializeAccount();
                        accounts.Add(user);
                        Helper.SerializeAccount(accounts);
                        break;
                    case 1:
                        do user = Helper.LoginAccount();
                        while (user == null);
                        break;
                    case 2:
                        return;
                }
                int i;
                
                do i = user.MainMenu();
                while (i == 0);
                if (i == -1) Terminate();
            }
        }

        static void MainMenu(Account account)
        {

        }

        //заготовка на переделку главной функции акков (неплохо бы делегаты для них сделать....)
        public static void Terminate()
        {
            Environment.Exit(0);
        }
    }
}

