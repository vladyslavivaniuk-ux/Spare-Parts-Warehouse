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
                        SearchParts(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, count);
                        Pause();
                        break;
                    case "4":
                        UpdatePart(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, count);
                        Pause();
                        break;
                    case "5":
                        DeletePart(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, ref count);
                        Pause();
                        break;
                    case "6":
                    case "7":
                    case "8":
                    case "9":
                        Console.WriteLine("Поки не зроблено.");
                        Pause();
                        break;
                    case "10":
                        SeedTestData(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, ref count, ref nextId, Capacity);
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

        static int FindIndexById(int[] ids, int count, int targetId)
        {
            for (int i = 0; i < count; i++)
            {
                if (ids[i] == targetId)
                {
                    return i;
                }
            }

            return -1;
        }

        static void PrintPartHeader()
        {
            Console.WriteLine(
                "{0,4} | {1,-12} | {2,-24} | {3,5} | {4,9} | {5,10} | {6,11} | {7,-8}",
                "№", "Артикул", "Найменування", "Кат.", "Кількість", "Мін. зал.", "Ціна (грн)", "Полиця");
            Console.WriteLine(new string('-', 104));
        }

        static void PrintPartRow(
            int[] ids,
            string[] articles,
            string[] names,
            int[] categoryIds,
            int[] quantities,
            string[] minStocks,
            double[] prices,
            string[] shelves,
            int index)
        {
            Console.WriteLine(
                "{0,4} | {1,-12} | {2,-24} | {3,5} | {4,9} | {5,10} | {6,11:F2} | {7,-8}",
                ids[index],
                articles[index],
                names[index],
                categoryIds[index],
                quantities[index],
                minStocks[index],
                prices[index],
                shelves[index]);
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
                    if (quantities[i] < minStockValue)
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

            PrintPartHeader();
            for (int i = 0; i < count; i++)
            {
                PrintPartRow(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, i);
            }

            int lowStock = CountLowStockParts(quantities, minStocks, count);
            Console.WriteLine("\nЗаписів: {0}. Позицій, яких менше за мінімальний залишок: {1}.", count, lowStock);
        }

        static void SearchParts(
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
                Console.WriteLine("Список порожній. Немає чого шукати.");
                return;
            }

            Console.WriteLine("--- Пошук запчастин ---");
            Console.WriteLine("1 — пошук за кодом категорії");
            Console.WriteLine("2 — пошук за частиною найменування або артикулу");
            int mode = ReadInt("Оберіть режим пошуку (1 - 2): ", 1, 2);

            int foundCount = 0;

            if (mode == 1)
            {
                int targetCat = ReadInt("Введіть код категорії (1 - 5): ", 1, 5);
                Console.WriteLine();
                PrintPartHeader();
                for (int i = 0; i < count; i++)
                {
                    if (categoryIds[i] == targetCat)
                    {
                        PrintPartRow(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, i);
                        foundCount++;
                    }
                }
            }
            else
            {
                string query = ReadText("Введіть текст для пошуку: ", 1).ToLower();
                Console.WriteLine();
                PrintPartHeader();
                for (int i = 0; i < count; i++)
                {
                    if (names[i].ToLower().Contains(query) || articles[i].ToLower().Contains(query))
                    {
                        PrintPartRow(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, i);
                        foundCount++;
                    }
                }
            }

            if (foundCount == 0)
            {
                Console.WriteLine("За вашим запитом записів не знайдено.");
            }
            else
            {
                Console.WriteLine("\nЗнайдено записів: {0}.", foundCount);
            }
        }

        static void UpdatePart(
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
                Console.WriteLine("Список порожній. Немає чого змінювати.");
                return;
            }

            PrintAllParts(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, count);
            Console.WriteLine();

            int targetId = ReadInt("Введіть № деталі для редагування: ", 1, int.MaxValue);
            int index = FindIndexById(ids, count, targetId);

            if (index == -1)
            {
                Console.WriteLine("Запис з №{0} не знайдено.", targetId);
                return;
            }

            Console.WriteLine("\nОбраний запис:");
            PrintPartHeader();
            PrintPartRow(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, index);

            Console.WriteLine("\nЩо саме змінити?");
            Console.WriteLine("1 — кількість на складі");
            Console.WriteLine("2 — ціну");
            Console.WriteLine("3 — мінімальний залишок");
            Console.WriteLine("4 — полицю зберігання");
            Console.WriteLine("5 — найменування");
            Console.WriteLine("0 — скасувати");

            int fieldChoice = ReadInt("Ваш вибір: ", 0, 5);
            switch (fieldChoice)
            {
                case 1:
                    quantities[index] = ReadInt("Нова кількість на складі (0 - 100000): ", 0, 100000);
                    Console.WriteLine("Кількість оновлено.");
                    break;
                case 2:
                    prices[index] = ReadDouble("Нова ціна (грн, 0,01 - 1000000): ", 0.01, 1000000.0);
                    Console.WriteLine("Ціну оновлено.");
                    break;
                case 3:
                    int newMin = ReadInt("Новий мінімальний залишок (0 - 100000): ", 0, 100000);
                    minStocks[index] = newMin.ToString();
                    Console.WriteLine("Мінімальний залишок оновлено.");
                    break;
                case 4:
                    shelves[index] = ReadText("Нова полиця зберігання: ", 1);
                    Console.WriteLine("Полицю оновлено.");
                    break;
                case 5:
                    names[index] = ReadText("Нове найменування: ", 2);
                    Console.WriteLine("Найменування оновлено.");
                    break;
                case 0:
                    Console.WriteLine("Редагування скасовано.");
                    break;
            }
        }

        static void DeletePart(
            int[] ids,
            string[] articles,
            string[] names,
            int[] categoryIds,
            int[] quantities,
            string[] minStocks,
            double[] prices,
            string[] shelves,
            ref int count)
        {
            if (count == 0)
            {
                Console.WriteLine("Список порожній. Немає чого видаляти.");
                return;
            }

            PrintAllParts(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, count);
            Console.WriteLine();

            int targetId = ReadInt("Введіть № деталі для видалення: ", 1, int.MaxValue);
            int index = FindIndexById(ids, count, targetId);

            if (index == -1)
            {
                Console.WriteLine("Запис з №{0} не знайдено.", targetId);
                return;
            }

            Console.WriteLine("\nВи збираєтесь видалити запис:");
            PrintPartHeader();
            PrintPartRow(ids, articles, names, categoryIds, quantities, minStocks, prices, shelves, index);

            Console.Write("\nПідтвердити видалення? (1 — так, 0 — ні): ");
            string confirm = Console.ReadLine();
            if (confirm != "1")
            {
                Console.WriteLine("Видалення скасовано.");
                return;
            }

            for (int i = index; i < count - 1; i++)
            {
                ids[i] = ids[i + 1];
                articles[i] = articles[i + 1];
                names[i] = names[i + 1];
                categoryIds[i] = categoryIds[i + 1];
                quantities[i] = quantities[i + 1];
                minStocks[i] = minStocks[i + 1];
                prices[i] = prices[i + 1];
                shelves[i] = shelves[i + 1];
            }

            count--;
            Console.WriteLine("Запис №{0} видалено.", targetId);
        }

        static void SeedTestData(
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
            string[] demoArticles = { "FLT-102", "BRK-405", "SPK-019", "BLT-880", "OIL-530", "SHK-214" };
            string[] demoNames = { "Фільтр масляний", "Колодки гальмівні", "Свічка запалювання", "Ремінь ГРМ", "Олива синтетична 5W-30", "Амортизатор передній" };
            int[] demoCats = { 1, 2, 3, 1, 4, 5 };
            int[] demoQuantities = { 4, 15, 3, 12, 6, 2 };
            string[] demoMinStocks = { "10", "8", "12", "5", "10", "4" };
            double[] demoPrices = { 320.00, 1450.50, 210.00, 980.00, 1650.00, 2890.00 };
            string[] demoShelves = { "A-01", "B-04", "A-03", "C-11", "D-02", "B-09" };

            int added = 0;
            for (int i = 0; i < demoArticles.Length; i++)
            {
                if (count >= capacity)
                {
                    Console.WriteLine("Сховище заповнене. Додано лише {0} тестових записів.", added);
                    return;
                }

                ids[count] = nextId;
                articles[count] = demoArticles[i];
                names[count] = demoNames[i];
                categoryIds[count] = demoCats[i];
                quantities[count] = demoQuantities[i];
                minStocks[count] = demoMinStocks[i];
                prices[count] = demoPrices[i];
                shelves[count] = demoShelves[i];

                count++;
                nextId++;
                added++;
            }

            Console.WriteLine("Додано {0} тестових записів про запчастини.", added);
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