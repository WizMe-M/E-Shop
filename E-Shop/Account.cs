using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace E_Shop
{
    //нужен?
    [Serializable]
    abstract class Account
    {
        public abstract string Role { get; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string LastName { get; set; } = "Не указано";
        public string FirstName { get; set; } = "Не указано";
        public string Patronomic { get; set; } = "Не указано";

        [NonSerialized]
        string birthday;
        public DateTime BirthdayDate { get; set; } = DateTime.Now;

        [NonSerialized]
        int age;
        public int Age { get; set; } = 0;

        [NonSerialized]
        int study;
        public int StudyYears { get; set; } = 0;

        public int WorkExperience { get; set; } = 0;
        public string Position { get; set; } = "Не указано";
        public string WorkPlace { get; set; } = "Не указано";
        public double Salary { get; set; } = 0;

        //сериализация аккаунта
        public void SerializeAccount()
        {
            string path = Directory.GetCurrentDirectory() + @$"\E-Shop\accounts\{Role}\{Login}.acc";
            BinaryFormatter formatter = new BinaryFormatter();
            if (File.Exists(path))
            {
                using FileStream fileStream = new FileStream(path, FileMode.Truncate);
                formatter.Serialize(fileStream, this);
                fileStream.Close();
            }
            else
            {
                using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
                formatter.Serialize(fileStream, this);
                fileStream.Close();
            }
        }
        public Account DeserializeAccount()
        {
            string path = Directory.GetCurrentDirectory() + @$"\E-Shop\accounts\{Role}\{Login}.acc";
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(path, FileMode.OpenOrCreate);
            switch (Role)
            {
                case "Admin":
                    Admin admin = (Admin)formatter.Deserialize(fileStream);
                    fileStream.Close();
                    return admin;
                default:
                    return null;
            }

        }
        public void AddAccountAtDataBase()
        {
            string path = Directory.GetCurrentDirectory() + @"\E-Shop\accounts\database.bd";
            using BinaryWriter writer = new BinaryWriter(File.Open(path, FileMode.Append));
            writer.Write(Login);
            writer.Write(Password);
            writer.Write(GetType().Name);
        }
    }
}
