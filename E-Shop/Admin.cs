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
        public override string Role { get; } = nameof(Admin);
        //регистрация администратора
        public Admin() { }

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
            Console.WriteLine();
        }
    }
}
