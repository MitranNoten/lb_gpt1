using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

public class Fb2Processor
{
    private static readonly string[] _slovar = { "хоббит", "эльф", "гном", "орки", "тролль", "гоблин" };

    public static void ProcessFb2File(string inputFilePath, string outputFilePath)
    {
        try
        {
            // Загружаем XML файл
            XDocument doc = XDocument.Load(inputFilePath);
            StringBuilder stringBuilder = new StringBuilder();
            // Находим все текстовые узлы
            var textNodes = doc.DescendantNodes().OfType<XText>();

            foreach (XText node in textNodes)
            {
                // Разделяем текст на слова и удаляем все спецсимволы, оставляя теги
                string[] words = Regex.Split(node.Value, @"\s+|[^а-яА-ЯёЁa-zA-Z'\<>\/]").Where(s => !string.IsNullOrWhiteSpace(s)).ToArray();

                foreach (string word in words)
                {
                    if (Regex.IsMatch(word, @"^[а-яА-ЯёЁ]+$"))
                    {
                        string processedWord = ReplaceWordIfInSlovar(word);
                        stringBuilder.Append(processedWord + " ");
                    }
                    else
                    {
                        stringBuilder.Append(word + " ");
                    }
                }
                node.Value = stringBuilder.ToString().Trim();
                stringBuilder.Clear();
            }
            // Сохраняем изменения в новый файл
            doc.Save(outputFilePath);
        }
        catch (FileNotFoundException)
        {
            Console.WriteLine("Ошибка: Файл не найден.");
        }
        catch (XmlException ex)
        {
            Console.WriteLine($"Ошибка XML: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Непредвиденная ошибка: {ex.Message}");
        }

    }

    public static string ReplaceWordIfInSlovar(string word)
    {
        foreach (string race in _slovar)
        {
            if (string.Equals(word, race, StringComparison.OrdinalIgnoreCase))
            {
                return "человек";
            }
        }
        return word;
    }



    public static void Main(string[] args)
    {
        string inputFilePath = @"C:\Users\mitra\OneDrive\Рабочий стол\Технология программирования\Лабораторные работы\ИИ\Толкиен М. - Хоббит - 1937.fb2"; // Путь к входному .fb2 файлу
        string outputFilePath = @"C:\Users\mitra\OneDrive\Рабочий стол\Технология программирования\Лабораторные работы\ИИ\output1.fb2"; // Путь к выходному файлу

        ProcessFb2File(inputFilePath, outputFilePath);


        Console.WriteLine("Обработка файла завершена.");
    }
}