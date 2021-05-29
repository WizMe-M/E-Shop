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
            List<Account> accounts;
            Helper.FirstLaunch();

            //начало работы
            accounts = Helper.GetAllAcounts();

            Account user;
            //логин в аккаунт (с возможностью перелогиниться)
            do
            {
                Console.Clear();
                do user = Helper.LoginAccount(accounts);
                while (user == null);
                int i;
                do
                {
                    i = user.MainFunction();
                }
                while (i == 0);
                if (i == -1) break;
            } while (true);

            //конец работы
            Helper.SaveAllAcounts(accounts);
        }
    }
}

