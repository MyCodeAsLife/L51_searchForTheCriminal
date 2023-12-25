using System;
using System.Collections.Generic;
using System.Linq;

namespace L51_searchForTheCriminal
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CriminalDatabase database = new CriminalDatabase();

            database.Run();
        }
    }

    class CriminalDatabase
    {
        private List<Criminal> _criminals = new List<Criminal>();

        public CriminalDatabase()
        {
            Fill();
        }

        enum Menu
        {
            Find = 1,
            ShowAll = 2,
            Exit = 3,
        }

        public void Run()
        {
            bool isOpen = true;

            while (isOpen)
            {
                Console.Clear();
                Console.WriteLine($"База данных преступников.\n" + new string(FormatOutput.DelimiterSymbolString, FormatOutput.DelimiterLenght) +
                                  $"\n{(int)Menu.Find} - Поиск преступников по параметрам.\n{(int)Menu.ShowAll} - Отобразить всех " +
                                  $"преступников.\n{(int)Menu.Exit} - Выйти из программы.\n" +
                                  new string(FormatOutput.DelimiterSymbolMenu, FormatOutput.DelimiterLenght));

                Console.Write("Выберите действие: ");

                if (int.TryParse(Console.ReadLine(), out int number))
                {
                    Console.Clear();

                    switch ((Menu)number)
                    {
                        case Menu.Find:
                            ShowFound();
                            break;

                        case Menu.ShowAll:
                            ShowAll();
                            break;

                        case Menu.Exit:
                            isOpen = false;
                            continue;

                        default:
                            Error.Show();
                            break;
                    }
                }
                else
                {
                    Error.Show();
                }

                Console.ReadKey(true);
            }
        }

        private void ShowAll()
        {
            Show(_criminals);
        }

        private bool TrySearch(out List<Criminal> filteredCriminal)
        {
            Console.WriteLine("Данные для поиска преступника.\n" + new string(FormatOutput.DelimiterSymbolMenu, FormatOutput.DelimiterLenght));

            Console.Write("Введите национальность: ");
            string nationality = Console.ReadLine();

            Console.Write("Введите рост: ");

            if (int.TryParse(Console.ReadLine(), out int height))
            {
                Console.Write("Введите вес: ");

                if (int.TryParse(Console.ReadLine(), out int weigth))
                {
                    filteredCriminal = _criminals.Where(criminal => criminal.Height == height).
                                                  Where(criminal => criminal.Weight == weigth).
                                                  Where(criminal => criminal.Nationality == nationality).
                                                  Where(criminal => criminal.IsInPrison == false).ToList();

                    if (filteredCriminal.Count > 0)
                        return true;
                }
                else
                {
                    Error.Show();
                }
            }
            else
            {
                Error.Show();
            }

            filteredCriminal = null;
            return false;
        }

        private void ShowFound()
        {
            if (TrySearch(out List<Criminal> finedCriminal))
                Show(finedCriminal);
            else
                Console.WriteLine("По указанным параметрам, преступников на свободе не найдено.");
        }

        private void Show(List<Criminal> _criminals)
        {
            Console.WriteLine("Данные преступников.\n" + new string(FormatOutput.DelimiterSymbolMenu, FormatOutput.DelimiterLenght));

            foreach (var criminal in _criminals)
            {
                Console.WriteLine($"ФИО: {criminal.FullName}\nНациональность: {criminal.Nationality}\n" +
                                  $"Рост: {criminal.Height}\nВес: {criminal.Weight}\n" +
                                  $"Статус: {(criminal.IsInPrison ? "Под стражей" : "На свободе")}");
                Console.WriteLine(new string(FormatOutput.DelimiterSymbolString, FormatOutput.DelimiterLenght));
            }
        }

        private void Fill()
        {
            _criminals.Add(new Criminal("Раскольников Родион Романович", "славянин", 80, 192, false));
            _criminals.Add(new Criminal("Лектер Ганнибал", "славянин", 78, 186, true));
            _criminals.Add(new Criminal("Капоне Альфонсе Габриэль", "италиани", 80, 192, false));
            _criminals.Add(new Criminal("Чичиков Павел Иванович", "славянин", 108, 160, true));
        }

        internal class Error
        {
            public static void Show()
            {
                Console.WriteLine("\nВы ввели некорректное значение.");
            }
        }

        internal class FormatOutput
        {
            static FormatOutput()
            {
                DelimiterSymbolMenu = '=';
                DelimiterSymbolString = '-';
                DelimiterLenght = 75;
            }

            public static char DelimiterSymbolMenu { get; private set; }
            public static char DelimiterSymbolString { get; private set; }
            public static int DelimiterLenght { get; private set; }
        }
    }

    class Criminal
    {
        public Criminal(string fullName, string nationality, int weight, int height, bool isInPrison)
        {
            FullName = fullName;
            Nationality = nationality;
            Weight = weight;
            Height = height;
            IsInPrison = isInPrison;
        }

        public string FullName { get; private set; }
        public string Nationality { get; private set; }
        public int Weight { get; private set; }
        public int Height { get; private set; }
        public bool IsInPrison { get; private set; }
    }
}
