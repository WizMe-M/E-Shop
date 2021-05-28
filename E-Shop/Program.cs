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

            user.Age = 18;
            user.LastName = "Тимкин";
            user.FirstName = "Максим";
            user.Patronomic = "Дмитриевич";
            user.Salary = 300000000;
            user.SerializeAccount();
            Account some = user.DeserializeAccount();

            Console.WriteLine($"Выполнен вход в аккаунт типа {some.GetType().Name}" +
                $"\nЛогин пользователя:\t{some.Login}" +
                $"\nПароль пользователя:\t{some.Password}" +
                $"\nФИО: {some.LastName} {some.FirstName} {some.Patronomic}" +
                $"\nДата рождения: {some.BirthdayDate.ToShortDateString()}" +
                $"\nВозраст: {some.Age}" +
                $"\nОбразование: {some.StudyYears} лет" +
                $"\nОпыт работы: {some.WorkExperience} лет" +
                $"\nДолжность: {some.Position}; Зарплата: {some.Salary}");
        }
    }
}

