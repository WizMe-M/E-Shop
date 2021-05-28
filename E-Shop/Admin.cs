using System;
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
        public override string Role { get; } = typeof(Admin).Name;
        //регистрация администратора
        public Admin()
        {
            Console.WriteLine("Запускаю процесс регистрации администратора...");
            Thread.Sleep(2000);
            Console.Clear();

            Console.WriteLine("Введите логин: ");
            string l;
            do l = Console.ReadLine();
            while (Helper.Check(l, "логин"));
            Console.Clear();
            Console.WriteLine("Введите пароль: ");
            string p;
            do p = Console.ReadLine();
            while (Helper.Check(p, "пароль"));

            Console.Clear();
            Console.WriteLine("Регистрация завершена");
            Login = l;
            Password = p;
            Console.WriteLine($"Логин: {Login}\nПароль: {Password}");
            AddAccountAtDataBase();
            SerializeAccount();
            Console.WriteLine("Нажмите любую клавишу...");
            Console.ReadKey();
        }

        //получение экземпляра класса Админ
        public Admin(string Login, string Password)
        {
            this.Login = Login;
            this.Password = Password;
        }

        //админ регистрирует нового пользователя в базу данных
        public void RegisterAccount()
        {
            Console.WriteLine("Регистрирую нового пользователя...");
        }
    }
}
