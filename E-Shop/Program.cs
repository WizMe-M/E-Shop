using System;
using System.IO;

namespace E_Shop
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\tПроект \"Электронный магазин\" E-Shop\n\n");
            Helper.FirstLaunch();
            Account user = Helper.LoginAccount();
            user.SerializeAccount();
            Console.Clear();
            Console.WriteLine($"Выполнен вход в аккаунт типа {user.GetType().Name}" +
                $"\nЛогин пользователя:\t{user.Login}" +
                $"\nПароль пользователя:\t{user.Password}" +
                $"\nФИО: {user.LastName} {user.FirstName} {user.Patronomic}" +
                $"\nДата рождения: {user.BirthdayDate.ToShortDateString()}" +
                $"\nВозраст: {user.Age}" +
                $"\nОбразование: {user.StudyYears} лет" +
                $"\nОпыт работы: {user.WorkExperience} лет" +
                $"\nДолжность: {user.Position}; Зарплата: {user.Salary}");
        }
    }
}

