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
                Console.WriteLine("=== Склад запчастин ===");
                Console.WriteLine("1 — про програму");
                Console.WriteLine("2 — таблиця вартості партії деталей");
                Console.WriteLine("3 — надходження деталей за категоріями та днями");
                Console.WriteLine("0 — вихід");
                Console.Write("Ваш вибір: ");
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        ShowCostTable();
                        Pause();
                        break;
                    case "3":
                        ShowSupplyTable();
                        Pause();
                        break;
                    case "0":
                        running = false;
                        Console.WriteLine("До побачення.");
                        break;
                    default:
                        Console.WriteLine("Немає такого пункту. Спробуйте ще раз.");
                        Pause();
                        break;
                }
            }
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

        static void ShowCostTable()
        {
            Console.WriteLine("--- Табуляція вартості запчастин за вагою партії ---");
            double pricePerKg = 245.50;

            Console.WriteLine($"{"Вага (кг)",10} | {"Вартість (грн)",15}");
            Console.WriteLine(new string('-', 28));

            for (double weight = 0.5; weight <= 5.0 + 1e-9; weight += 0.5)
            {
                double totalCost = weight * pricePerKg;
                Console.WriteLine($"{weight,10:F1} | {totalCost,15:F2}");
            }
        }

        static void ShowSupplyTable()
        {
            Console.WriteLine("--- Таблиця надходження запчастин (категорії / дні) ---\n");

            Console.Write("Введіть кількість категорій запчастин: ");
            if (!int.TryParse(Console.ReadLine(), out int rows) || rows <= 0)
            {
                Console.WriteLine("Помилка: введіть ціле додатне число.");
                return;
            }

            Console.Write("Введіть кількість днів поставки: ");
            if (!int.TryParse(Console.ReadLine(), out int cols) || cols <= 0)
            {
                Console.WriteLine("Помилка: введіть ціле додатне число.");
                return;
            }

            Console.WriteLine();
            int grandTotal = 0;
            int[] colTotals = new int[cols];

            Console.Write($"{"Категорія",-14}");
            for (int j = 1; j <= cols; j++)
            {
                Console.Write($"{$"День {j}",9}");
            }
            Console.WriteLine($"{"Підсумок",11}");

            for (int i = 1; i <= rows; i++)
            {
                Console.Write($"{"Категорія " + i,-14}");
                int rowSum = 0;

                for (int j = 1; j <= cols; j++)
                {
                    int partsCount = (i * 7 + j * 13) % 50 + 5;
                    Console.Write($"{partsCount,9}");
                    rowSum += partsCount;
                    colTotals[j - 1] += partsCount;
                }

                Console.WriteLine($"{rowSum,11}");
                grandTotal += rowSum;
            }

            Console.Write($"{"Підсумок",-14}");
            for (int j = 0; j < cols; j++)
            {
                Console.Write($"{colTotals[j],9}");
            }
            Console.WriteLine($"{grandTotal,11}");
        }
    }
}