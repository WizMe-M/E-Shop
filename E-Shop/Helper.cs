using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace E_Shop
{

    static class Helper
    {
        public static string pathAccounts = Directory.GetCurrentDirectory() + @"\E-Shop\database.bd";
        public static string pathStorage = Directory.GetCurrentDirectory() + @"\E-Shop\storage.bd";
        public static string pathReceipt = Directory.GetCurrentDirectory() + @"\E-Shop\receipt.bd";
        public static string pathShop = Directory.GetCurrentDirectory() + @"\E-Shop\shop.bd";

        public static void FirstLaunch()
        {
            DirectoryInfo dir = new DirectoryInfo(Directory.GetCurrentDirectory() + @"\E-Shop");
            if (!dir.Exists)
            {
                Console.WriteLine("Запускаю протокол первичного запуска...");
                Console.WriteLine($"Зарегистрирован новый пользователь (администратор)");
                Admin admin = new Admin
                {
                    Login = "admin",
                    Password = "PassWorD123!!"
                };

                Console.WriteLine($"Ваши:\nЛогин\t- {admin.Login}\nПароль\t- {admin.Password}");

                dir.Create();
                List<Account> accounts = new List<Account>() { admin };
                BinaryFormatter formatter = new BinaryFormatter();
                using FileStream fileStream = new FileStream(pathAccounts, FileMode.OpenOrCreate);
                formatter.Serialize(fileStream, accounts);
                fileStream.Close();
                Console.WriteLine("Все аккаунты сериализованы!");
                Console.WriteLine("Нажмите любую клавишу...");
                Console.ReadKey();
            }
        }
        public static Account LoginAccount()
        {
            try
            {
                List<Account> accounts = DeserializeAccount();
                Console.Clear();
                Console.WriteLine("Запускаю процесс входа в аккаунт...");
                Console.WriteLine("Введите логин:");
                string login = Console.ReadLine().Trim();
                Console.WriteLine("Введите пароль:");
                string password = Console.ReadLine().Trim();

                foreach (Account acc in accounts)
                {
                    if (login.Equals(acc.Login) && password.Equals(acc.Password))
                    {
                        if (acc.isDeleted)
                            throw new Exception("-- ЭТОТ АККАУНТ БЫЛ УДАЛЁН! --" +
                                "\nЧтобы войти в систему, аккаунт должен быть рабочим." +
                                "\nЕсли ваш аккаунт был удалён, обратитесь к администратору, чтобы восстановить его");

                        if (!acc.isHired)
                            throw new Exception("-- ЭТОТ СОТРУДНИК БЫЛ УВОЛЕН! --" +
                                "\nЧтобы войти в систему, нужно быть сотрудником предприятия или покупателем." +
                                "\nНанять сотрудника может кадровик, а зарегистрироваться" +
                                " - самостоятельно или с помощью администратора");

                        return acc.Position switch
                        {
                            "Администратор" => (Admin)acc,
                            "Кадровик" => (Personnel)acc,
                            "Кладовщик" => (Warehouseman)acc,
                            "Продавец" => (Seller)acc,
                            "Бухгалтер" => (Accountant)acc,
                            "Покупатель" => (Customer)acc,
                            _ => throw new Exception("Что-то пошло не так..." +
                            "\nСудя по всему, аккаунт имеет неправильные настройки!"),
                        };


                    }
                }
                throw new Exception("Аккаунта с такими данными не существует!");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
                return null;
            }
        }

        public static int SafeParseInt(this string s)
        {
            int i;
            while (!int.TryParse(s, out _))
            {
                Console.WriteLine("Невозможно преобразовать введенную строку к числу. ");
                Thread.Sleep(500);
                Console.Clear();
                Console.Write("Введите корректное число: ");
                s = Console.ReadLine().Trim();
            }
            i = int.Parse(s);
            return i;
        }
        public static double SafeParseDouble(this string s)
        {
            double i;
            while (!double.TryParse(s, out _))
            {
                Console.WriteLine("Невозможно преобразовать введенную строку к десятичной дроби.");
                Thread.Sleep(500);
                Console.Clear();
                Console.WriteLine("Введите корректное число: ");
                s = Console.ReadLine().Trim();
            }
            i = double.Parse(s);
            return i;
        }

        public static Shop ChooseShop()
        {
            List<Shop> shops = DeserializeShops();
            List<string> names = new List<string>();
            foreach (Shop s in shops)
                names.Add(s.Name);
            ConsoleMenu shopMenu = new ConsoleMenu(names.ToArray());
            int choose = shopMenu.PrintMenu();
            return shops[choose];
        }



        public static void AddReceiptToBD(Customer customer, Shop shop)
        {
            List<Receipt> receipts = DeserializeReceipt();
            receipts.Add(new Receipt(customer, shop));
            SerializeReceipt(receipts);
        }
        public static void SerializeReceipt(List<Receipt> receipts)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathReceipt, FileMode.Truncate);
            formatter.Serialize(fileStream, receipts);
            fileStream.Close();
        }
        public static List<Receipt> DeserializeReceipt()
        {
            List<Receipt> receipts = new List<Receipt>();
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathReceipt, FileMode.OpenOrCreate);
            if (fileStream.Length != 0)
                receipts = (List<Receipt>)formatter.Deserialize(fileStream);
            fileStream.Close();
            return receipts;
        }



        public static List<Shop> DeserializeShops()
        {
            List<Shop> shops = new List<Shop>();
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathShop, FileMode.OpenOrCreate);
            if (fileStream.Length != 0)
                shops = (List<Shop>)formatter.Deserialize(fileStream);
            fileStream.Close();
            return shops;
        }
        public static void AddShopToBD(Shop shop)
        {
            List<Shop> shops = DeserializeShops();
            shops.Add(shop);
            SerializeShops(shops);
        }
        public static void SerializeShops(List<Shop> shops)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathShop, FileMode.Truncate);
            formatter.Serialize(fileStream, shops);
            fileStream.Close();
        }



        public static void SerializeStorage(List<Storage> storage)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathStorage, FileMode.Truncate);
            formatter.Serialize(fileStream, storage);
            fileStream.Close();
        }
        public static void AddStorageToBD(Storage storage)
        {
            List<Storage> storages = DeserializeStorage();
            storages.Add(storage);
            SerializeStorage(storages);
        }
        public static List<Storage> DeserializeStorage()
        {
            List<Storage> storages = new List<Storage>();
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathStorage, FileMode.OpenOrCreate);
            if (fileStream.Length != 0)
                storages = (List<Storage>)formatter.Deserialize(fileStream);
            fileStream.Close();
            return storages;
        }




        public static void SerializeAccount(List<Account> accounts)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathAccounts, FileMode.Truncate);
            formatter.Serialize(fileStream, accounts);
            fileStream.Close();
        }
        public static List<Account> DeserializeAccount()
        {
            List<Account> accounts = new List<Account>();
            BinaryFormatter formatter = new BinaryFormatter();
            using FileStream fileStream = new FileStream(pathAccounts, FileMode.Open);
            if (fileStream.Length != 0)
                accounts = (List<Account>)formatter.Deserialize(fileStream);
            fileStream.Close();
            for (int i = 0; i < accounts.Count; i++)
                accounts[i].OnDeserializing();
            return accounts;
        }
    }
}
