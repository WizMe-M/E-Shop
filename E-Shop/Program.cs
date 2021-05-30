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
            Account user;
            Helper.FirstLaunch();
            do
            {
                Console.Clear();
                do user = Helper.LoginAccount();
                while (user == null);
                int i;
                do
                {
                    i = user.MainMenu();
                }
                while (i == 0);
                if (i == -1) break;
            } while (true);
        }
    }
}

