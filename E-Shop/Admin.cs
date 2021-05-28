﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace E_Shop
{
    [Serializable]
    class Admin : Account
    {
        public override string Role { get; } = nameof(Admin);


        public Admin() { }
        //получение экземпляра класса Админ
        public Admin(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }

        //админ регистрирует нового пользователя в базу данных
        void RegisterAccount()
        {
            Console.WriteLine("Регистрирую нового пользователя...");
            Console.WriteLine();
        }

        public override int MainFunction(List<Account> accounts)
        {
            string[] functions = { "Зарегистрировать пользователя", "Выйти из аккаунта", "Выйти из приложения" };
            int i = 0;
            if (i == functions.Length - 2) return 1;
            else if (i == functions.Length - 1) return -1;
            else return 0;
        }
    }
}
