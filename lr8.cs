using System;
using System.Text;

namespace SparePartsWarehouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1": ShowAbout(); Pause(); break;
                    case "2": ShowCostTable(); Pause(); break;
                    case "3": ShowSupplyTable(); Pause(); break;
                    case "0": running = false; Console.WriteLine("До побачення."); break;
                    default: Console.WriteLine("Немає такого пункту. Спробуйте ще раз."); Pause(); break;
                }
            }
        }

        static void ShowMenu()
        {
            Console.WriteLine("=== Склад запчастин ===");
            Console.WriteLine("1 — про програму");
            Console.WriteLine("2 — таблиця вартості партії деталей");
            Console.WriteLine("3 — надходження деталей за категоріями та днями");
            Console.WriteLine("0 — вихід");
            Console.Write("Ваш вибір: ");
        }

        static void Pause()
        {
            Console.WriteLine("\nНатисніть будь-яку клавішу...");
            Console.ReadKey(true);
            Console.Clear();
        }

        static void ShowAbout()
        {
            Console.WriteLine("Тема: Склад запчастин");
            Console.WriteLine("Сутності: Користувач, Деталь, Категорія (довідник)");
            Console.WriteLine("Головна сутність: Деталь");
            Console.WriteLine("Перелік полів та їх типи даних:");
            Console.WriteLine("  - string артикул");
            Console.WriteLine("  - string найменування");
            Console.WriteLine("  - int код — категорія, з довідника");
            Console.WriteLine("  - int кількість на складі");
            Console.WriteLine("  - string мінімальний залишок");
            Console.WriteLine("  - double ціна");
            Console.WriteLine("  - string полиця");
            Console.WriteLine("Підсумкова величина: позиції, яких менше за мінімальний залишок");
        }

        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                double value;
                if (!double.TryParse(Console.ReadLine(), out value))
                {
                    Console.WriteLine("Помилка: введіть коректне дійсне число.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine("Помилка: число має бути в межах від {0} до {1}.", min, max);
                    continue;
                }

                return value;
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                int value;
                if (!int.TryParse(Console.ReadLine(), out value))
                {
                    Console.WriteLine("Помилка: введіть коректне ціле число.");
                    continue;
                }

                if (value < min || value > max)
                {
                    Console.WriteLine("Помилка: число має бути в межах від {0} до {1}.", min, max);
                    continue;
                }

                return value;
            }
        }

        static void ShowCostTable()
        {
            Console.WriteLine("--- Табуляція вартості запчастин за вагою партії ---");
            double pricePerKg = 245.50;

            Console.WriteLine("{0,10} | {1,15}", "Вага (кг)", "Вартість (грн)");
            Console.WriteLine(new string('-', 28));

            for (double weight = 0.5; weight <= 5.0 + 1e-9; weight += 0.5)
            {
                double totalCost = weight * pricePerKg;
                Console.WriteLine("{0,10:F1} | {1,15:F2}", weight, totalCost);
            }
        }

        static void ShowSupplyTable()
        {
            Console.WriteLine("--- Таблиця надходження запчастин (категорії / дні) ---\n");

            int rows;
            Console.Write("Введіть кількість категорій запчастин: ");
            if (!int.TryParse(Console.ReadLine(), out rows) || rows <= 0)
            {
                Console.WriteLine("Помилка: введіть ціле додатне число.");
                return;
            }

            int cols;
            Console.Write("Введіть кількість днів поставки: ");
            if (!int.TryParse(Console.ReadLine(), out cols) || cols <= 0)
            {
                Console.WriteLine("Помилка: введіть ціле додатне число.");
                return;
            }

            Console.WriteLine();
            int grandTotal = 0;
            int[] colTotals = new int[cols];

            Console.Write("{0,-14}", "Категорія");
            for (int j = 1; j <= cols; j++)
            {
                Console.Write("{0,9}", "День " + j);
            }
            Console.WriteLine("{0,11}", "Підсумок");

            for (int i = 1; i <= rows; i++)
            {
                Console.Write("{0,-14}", "Категорія " + i);
                int rowSum = 0;

                for (int j = 1; j <= cols; j++)
                {
                    int partsCount = (i * 7 + j * 13) % 50 + 5;
                    Console.Write("{0,9}", partsCount);
                    rowSum += partsCount;
                    colTotals[j - 1] += partsCount;
                }

                Console.WriteLine("{0,11}", rowSum);
                grandTotal += rowSum;
            }

            Console.Write("{0,-14}", "Підсумок");
            for (int j = 0; j < cols; j++)
            {
                Console.Write("{0,9}", colTotals[j]);
            }
            Console.WriteLine("{0,11}", grandTotal);
        }
    }
}