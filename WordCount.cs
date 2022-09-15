using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace UniqueWords
{
    class WordCount
    {
        const string AbsentSignsInWords = "1234567890[](){}<>.,:;?!-*@#$%^&=+`~/|№'\"";        

        static void Main(string[] args)
        {
            string nameFile = args[0];
            string fileContent = File.ReadAllText(nameFile);
            string[] words = GetArrayWords(fileContent);
            List<Tuple<int, string>> sortListWords = GetSortAndCountListWords(words);
            WriteAllWordsInFile(sortListWords, nameFile);            
        }

        /// <summary>
        /// Возвращает массив слов из файла
        /// </summary>
        /// <param name="fileContent"> Содержимое файла </param>
        /// <returns></returns>
        public static string[] GetArrayWords(string fileContent)
        {
            StringBuilder sb = new StringBuilder(fileContent);
            foreach (char c in AbsentSignsInWords)
                sb = sb.Replace(c, ' ');
            sb = sb.Replace(Environment.NewLine, " ");
            string[] words = sb.ToString().Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            return words;
        }

        /// <summary>
        /// Возвращает сортированный набор, содержащий слова и количество их употреблений  
        /// </summary>
        /// <param name="words"> Массив слов </param>
        /// <returns></returns>
        public static List<Tuple<int, string>> GetSortAndCountListWords(string[] words)
        {
            Dictionary<string, int> wordsCount = new Dictionary<string, int>();
            foreach (string w in words.Select(x => x.ToLower()))
                if (wordsCount.TryGetValue(w, out int c))
                    wordsCount[w] = c + 1;
                else
                    wordsCount.Add(w, 1);
            List<Tuple<int, string>> sortListWords = wordsCount.Select(x => new Tuple<int, string>(x.Value, x.Key)).ToList();
            sortListWords.Sort((x, y) => y.Item1.CompareTo(x.Item1));
            return sortListWords;
        }

        /// <summary>
        /// Записывает в файл перечисление всех уникальных слов, встречающихся в тесте, 
        /// и количеством их употреблений, отсортированный в порядке убывания количества употреблений
        /// </summary>
        /// <param name="sortListWords"> Сортированный список слов </param>
        /// <param name="nameFile"> Название файла </param>
        public static void WriteAllWordsInFile(List<Tuple<int, string>> sortListWords, string nameFile)
        {
            string[] content = new string[sortListWords.Count];
            int c = 0;
            foreach (Tuple<int, string> slw in sortListWords)
            {
                content[c] = slw.Item2 + " " + slw.Item1;
                c++;
            }
            File.WriteAllLines(nameFile + ".count.txt", content);
        }
    }
}
