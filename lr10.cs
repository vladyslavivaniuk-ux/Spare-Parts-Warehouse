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
            int[] ids = new int[Capacity];
            string[] articles = new string[Capacity];
            string[] names = new string[Capacity];
            int[] categoryIds = new int[Capacity];
            int[] quantities = new int[Capacity];
            string[] minStocks = new string[Capacity];
            double[] prices = new double[Capacity];
            string[] shelves = new string[Capacity];

            int count = 0;
            int nextId = 1;
            bool running = true;

            while (running)
            {
                ShowMenu();
                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        PrintAllParts(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, count);
                        Pause();
                        break;
                    case "2":
                        AddPart(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, ref count, ref nextId, Capacity);
                        Pause();
                        break;
                    case "3":
                    case "4":
                    case "5":
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                    case "10":
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                    case "11":
                        ShowAbout();
                        Pause();
                        break;
                    case "12":
                        RunCostTabulation();
                        Pause();
                        break;
                    case "13":
                        RunSupplyReport();
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
            Console.WriteLine("1  — показати всі записи");
            Console.WriteLine("2  — додати запис");
            Console.WriteLine("3  — знайти за умовою");
            Console.WriteLine("4  — змінити запис");
            Console.WriteLine("5  — видалити запис");
            Console.WriteLine("6  — зведена таблиця");
            Console.WriteLine("7  — зберегти у файл");
            Console.WriteLine("8  — завантажити з файлу");
            Console.WriteLine("9  — експорт у CSV");
            Console.WriteLine("10 — заповнити тестовими даними");
            Console.WriteLine("11 — про програму");
            Console.WriteLine("12 — таблиця вартості партії деталей");
            Console.WriteLine("13 — надходження деталей за категоріями та днями");
            Console.WriteLine("0  — вихід");
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

        static string ReadText(string prompt, int minLength)
        {
            while (true)
            {
                Console.Write(prompt);
                string input = Console.ReadLine();
                if (input != null && input.Trim().Length >= minLength)
                {
                    return input.Trim();
                }

                Console.WriteLine("Помилка: рядок не може бути порожнім (мін. символів: {0}).", minLength);
            }
        }

        static int ReadInt(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                int value;
                if (int.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine("Помилка: введіть ціле число від {0} до {1}.", min, max);
            }
        }

        static double ReadDouble(string prompt, double min, double max)
        {
            while (true)
            {
                Console.Write(prompt);
                double value;
                if (double.TryParse(Console.ReadLine(), out value) && value >= min && value <= max)
                {
                    return value;
                }

                Console.WriteLine("Помилка: введіть число від {0} до {1}.", min, max);
            }
        }

        static void AddPart(
            int[] ids,
            string[] articles,
            string[] names,
            int[] categoryIds,
            int[] quantities,
            string[] minStocks,
            double[] prices,
            string[] shelves,
            ref int count,
            ref int nextId,
            int capacity)
        {
            if (count >= capacity)
            {
                Console.WriteLine("Сховище заповнене, видаліть щось зайве.");
                return;
            }

            Console.WriteLine("--- Додавання нової деталі ---");
            string article = ReadText("Артикул (наприклад, BRK-001): ", 2);
            string name = ReadText("Найменування деталі: ", 2);
            int categoryId = ReadInt("Код категорії з довідника (1 - 5): ", 1, 5);
            int quantity = ReadInt("Кількість на складі (0 - 100000): ", 0, 100000);
            int minStockNum = ReadInt("Мінімальний залишок (0 - 100000): ", 0, 100000);
            double price = ReadDouble("Ціна за одиницю (грн, 0,01 - 1000000): ", 0.01, 1000000.0);
            string shelf = ReadText("Полиця зберігання (наприклад, A-12): ", 1);

            ids[count] = nextId;
            articles[count] = article;
            names[count] = name;
            categoryIds[count] = categoryId;
            quantities[count] = quantity;
            minStocks[count] = minStockNum.ToString();
            prices[count] = price;
            shelves[count] = shelf;

            count++;
            nextId++;

            Console.WriteLine("\nДодано деталь №{0}.", ids[count - 1]);
        }

        static int CountLowStockParts(int[] quantities, string[] minStocks, int count)
        {
            if (count == 0)
            {
                return 0;
            }

            int lowStockCount = 0;
            for (int i = 0; i < count; i++)
            {
                int minStockValue;
                if (int.TryParse(minStocks[i], out minStockValue))
                {
                    if (quantities[i] > minStockValue)
                    {
                        lowStockCount++;
                    }
                }
            }

            return lowStockCount;
        }

        static void PrintAllParts(
            int[] ids,
            string[] articles,
            string[] names,
            int[] categoryIds,
            int[] quantities,
            string[] minStocks,
            double[] prices,
            string[] shelves,
            int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Записів поки немає.");
                return;
            }

            Console.WriteLine(
                "{0,4} | {1,-12} | {2,-24} | {3,5} | {4,9} | {5,10} | {6,11} | {7,-8}",
                "№", "Артикул", "Найменування", "Кат.", "Кількість", "Мін. зал.", "Ціна (грн)", "Полиця");
            Console.WriteLine(new string('-', 104));

            for (int i = 0; i < count; i++)
            {
                Console.WriteLine(
                    "{0,4} | {1,-12} | {2,-24} | {3,5} | {4,9} | {5,10} | {6,11:F2} | {7,-8}",
                    ids[i],
                    articles[i],
                    names[i],
                    categoryIds[i],
                    quantities[i],
                    minStocks[i],
                    prices[i],
                    shelves[i]);
            }

            int lowStock = CountLowStockParts(quantities, minStocks, count);
            Console.WriteLine("\nЗаписів: {0}. Позицій, яких менше за мінімальний залишок: {1}.", count, lowStock);
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