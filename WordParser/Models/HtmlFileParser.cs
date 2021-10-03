using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using WordParser.Models.Database;

namespace WordParser.Models
{
    class HtmlFileParser : BindableBase
    {
        public delegate void MessageHandler(string message);
        public event MessageHandler Notify;
        public HtmlFileParser(string filePath, int memorySize)
        {
            if (!IsNotExecuting)
            {
                IsNotExecuting = false;
                Task calc = new Task(() =>{ 
                DBAccessor accessor = new DBAccessor();

                ulong maxCapacity = ((ulong)memorySize * 1024 * 1024) / 2;
                StringBuilder stringBuilder = new StringBuilder(50, (int)maxCapacity);

                using (StreamReader streamReader = new StreamReader(filePath))
                {
                    try
                    {   
                        string line;
                        string page = "";
                        string[] clearPage;
                        int currentRow = 1;
                        while ((line = streamReader.ReadLine()) != null)
                        {
                            page += line;
                            Notify?.Invoke($"Прочитана строка: {currentRow}");
                            currentRow++;
                        }

                        Notify?.Invoke($"Обработка");
                        clearPage = ClearHtml(page);
                        foreach (string word in clearPage)
                        {
                            stringBuilder.Append(word + ";");
                        }
                    
                    }
                    catch (OutOfMemoryException)
                    {
                        accessor.InsertWords(GetResult(stringBuilder.ToString()));
                        stringBuilder = HandleExeption(stringBuilder, (int)maxCapacity);
                    }
                    catch (ArgumentOutOfRangeException)
                    {
                        accessor.InsertWords(GetResult(stringBuilder.ToString()));
                        stringBuilder = HandleExeption(stringBuilder, (int)maxCapacity);
                    }
                }
                    Notify?.Invoke($"Добавление");
                    accessor.InsertWords(GetResult(stringBuilder.ToString()));
                });

                calc.Start();
                Notify?.Invoke($"Выполнено");
                IsNotExecuting = true;
            }
        }

        StringBuilder HandleExeption(StringBuilder builder, int maxCapacity)
        {
            builder.Clear();
            GC.Collect();
            builder = new StringBuilder(50, (int)maxCapacity);
            return builder;
        }

        string[] ClearHtml(string text)
        {
            Regex regex = new Regex("<[^>]*>");


            string regexedText = regex.Replace(text, string.Empty);
            var splitedWords = regexedText.Split(new char[] {' ','=','-', ',', '.', '!', '?','"', ';', ':', '_', '\n', '\r','\t'}, StringSplitOptions.RemoveEmptyEntries);

            List<string> clearWords = new List<string>();
            foreach (string word in splitedWords)
            {
                bool is_ok = true;
                StringBuilder wordBuilder = new StringBuilder();
                foreach (char c in word)
                {
                    if ((c >= 'А' && c <= 'Я') || (c >= 'а' && c <= 'я')) {
                        wordBuilder.Append(c);
                    }
                    else
                    {
                        is_ok = false;
                    }
                }
                if (is_ok)
                {
                    clearWords.Add(wordBuilder.ToString().ToLower());
                }
            }

            return clearWords.ToArray();
        }

        Dictionary<string, int> GetResult(string finalWords)
        {
            Dictionary<string, int> wordsList = new Dictionary<string, int>();
            foreach (var word in finalWords.Split(';'))
            {
                if (wordsList.ContainsKey(word))
                    wordsList[word]++;
                else
                    wordsList.Add(word, 1);
            }
            return wordsList;
        }

        public static bool IsNotExecuting {get; private set;}
    }
}
