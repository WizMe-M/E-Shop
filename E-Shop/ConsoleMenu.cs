using System;
using System.Collections.Generic;
using System.Text;

namespace E_Shop
{
    class ConsoleMenu
    {
        string[] menuItems;
        int counter = 0;
        public ConsoleMenu(string[] menuItems)
        {
            this.menuItems = menuItems;
        }

        public int PrintMenu()
        {
            ConsoleKeyInfo key;
            do
            {
                Console.Clear();
                for (int i = 0; i < menuItems.Length; i++)
                {
                    if (counter == i)
                    {
                        Console.BackgroundColor = ConsoleColor.Cyan;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(menuItems[i]);
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                        Console.WriteLine(menuItems[i]);
                }

                key = Console.ReadKey(true);
                switch (key.Key)
                {
                    case ConsoleKey.UpArrow:
                        counter--;
                        if (counter == -1)
                            counter = menuItems.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        counter++;
                        if (counter == menuItems.Length)
                            counter = 0;
                        break;
                    //возвращаем строчку выбранного элемента
                    case ConsoleKey.Enter:
                        return counter;
                    //возвращаем -1, чтобы выйти на предыдущий уровень меню
                    case ConsoleKey.Backspace:
                    case ConsoleKey.Escape:
                        return -1;
                }
            }
            while (true);
        }
    }
}
