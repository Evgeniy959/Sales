﻿using System;

namespace CLI
{
    public static class Show
    {
        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] {message}");
            Console.ResetColor();
        }

        public static void Success(string message)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] {message}");
            Console.ResetColor();
        }
        
        public static void Info(string message)
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine($"{message}");
            Console.ResetColor();
        }

        public static void PrintLn(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(message);
            Console.ResetColor();
        }
        public static void Print(string message)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write(message);
            Console.ResetColor();
        }

        public static void Menu()
        {
            Info("Выберите режим работы");
            Info("1. Оформление заказа");
            Info("2. Добавление данных о покупке"); // Добавил для домашнего задания
            Info("3. Экспорт списка продуктов");
            Info("4. Импорт списка продуктов");
            Info("5. Экспорт списка заказов"); // Добавил для домашнего задания
            Info("6. Экспорт списка пользователей с указанием их дисконтной программы"); // Добавил для домашнего задания
            Info("7. Импорт списка пользователей с указанием их дисконтной программы"); // Добавил для домашнего задания
        }
    }
}