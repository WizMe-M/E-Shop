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
            Account user;
            Helper.FirstLaunch();

            do
            {
                Console.Clear();
                accounts = Helper.GetAllAcounts();
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

            Helper.SaveAllAcounts(accounts);
        }
    }
}

