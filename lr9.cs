using System;
using System.Text;

namespace SparePartsWarehouse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            const int Capacity = 100;
            double[] partPrices = new double[Capacity];
            int count = 0;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        ShowAbout();
                        Pause();
                        break;
                    case "2":
                        RunCostTabulation();
                        Pause();
                        break;
                    case "3":
                        RunSupplyReport();
                        Pause();
                        break;
                    case "4":
                        double newPrice = ReadDouble("Введіть ціну запчастини (грн, 0,01 - 100000): ", 0.01, 100000.0);
                        AddPrice(partPrices, ref count, newPrice, Capacity);
                        Console.WriteLine("Поточна кількість записів у Main: {0}", count);
                        Pause();
                        break;
                    case "5":
                        PrintPrices(partPrices, count);
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

        static void ShowMenu()
        {
            Console.WriteLine("=== Склад запчастин ===");
            Console.WriteLine("1 — про програму");
            Console.WriteLine("2 — таблиця вартості партії деталей");
            Console.WriteLine("3 — надходження деталей за категоріями та днями");
            Console.WriteLine("4 — додати ціну деталі");
            Console.WriteLine("5 — показати всі ціни деталей");
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

        static void AddPrice(double[] prices, ref int count, double newPrice, int capacity)
        {
            if (count >= capacity)
            {
                Console.WriteLine("Сховище заповнене. Більше не можна додати записів.");
                return;
            }

            prices[count] = newPrice;
            count++;
            Console.WriteLine("Ціну успішно додано.");
        }

        static void PrintPrices(double[] prices, int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список цін деталей порожній.");
                return;
            }

            Console.WriteLine("{0,5} | {1,15}", "№", "Ціна (грн)");
            Console.WriteLine(new string('-', 23));
            for (int i = 0; i < count; i++)
            {
                Console.WriteLine("{0,5} | {1,15:F2}", i + 1, prices[i]);
            }
        }

        static double CalculateBatchCost(double weight, double pricePerKg)
        {
            return weight * pricePerKg;
        }

        static void RunCostTabulation()
        {
            Console.WriteLine("--- Налаштування табуляції вартості партії деталей ---");
            double from = ReadDouble("Введіть початкову вагу в кг (0,1 - 100): ", 0.1, 100.0);
            double to = ReadDouble(string.Format("Введіть кінцеву вагу в кг ({0} - 500): ", from), from, 500.0);
            double step = ReadDouble("Введіть крок ваги в кг (0,1 - 50): ", 0.1, 50.0);

            Console.WriteLine();
            PrintCostTable(from, to, step);
        }

        static void PrintCostTable(double from, double to, double step)
        {
            double pricePerKg = 245.50;

            Console.WriteLine("{0,10} | {1,15}", "Вага (кг)", "Вартість (грн)");
            Console.WriteLine(new string('-', 28));

            for (double weight = from; weight <= to + 1e-9; weight += step)
            {
                double totalCost = CalculateBatchCost(weight, pricePerKg);
                Console.WriteLine("{0,10:F1} | {1,15:F2}", weight, totalCost);
            }
        }

        static void RunSupplyReport()
        {
            Console.WriteLine("--- Таблиця надходження запчастин (категорії / дні) ---\n");
            int rows = ReadInt("Введіть кількість категорій запчастин (1 - 20): ", 1, 20);
            int cols = ReadInt("Введіть кількість днів поставки (1 - 14): ", 1, 14);

            Console.WriteLine();
            PrintSupplyTable(rows, cols);
        }

        static void PrintSupplyTable(int rows, int cols)
        {
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