using BannedWordsRepo;
using Moq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

//Author: Filip Lyubenov, f_lubenov@hotmail.com

namespace ContentConsole
{    
    public class BannedWordsProcessor
    {        
        internal IBannedWordsRepo GetBannedWordsRepo()
        {
            Mock<IBannedWordsRepo> mock = new Mock<IBannedWordsRepo>();
            mock.Setup<IList<string>>(r => r.GetBannedWordsList()).Returns(new List<string>() { "swine", "bad", "nasty", "horrible" });
            return mock.Object;
        }

        public List<string> GetBannedWordsFromConfig()
        {
            string words = string.Empty;
            var connectionManagerDatabaseServers = ConfigurationManager.GetSection("bannedWordsList") as NameValueCollection;
            if (connectionManagerDatabaseServers != null)
            {
                words = connectionManagerDatabaseServers["words"].ToString();
                return words.Split(',').Select(s => s = s.Trim()).ToList();
            }
            return null;
        }

        internal string RemoveSpecialCharactersFromText(string text)
        {
            return text==null ? null : text.Replace(".", " ").Replace("-", " ").Replace(",", " ");
        }        

        public int CountBannedWords(string text)
        {
            return CountBannedWords(text, GetBannedWordsRepo());                        
        }

        public int CountBannedWords(string text, IBannedWordsRepo bannedWordsRepo)
        {
            if (String.IsNullOrEmpty(text)) return 0;
            var textToLookup = RemoveSpecialCharactersFromText(text)
                              .Split(null)
                              .GroupBy(s => s).Select(s => new { word = s.Key.Trim(), occurances = s.Count() })
                              .ToDictionary(g => g.word, g => g.occurances);
            Console.WriteLine("--------------------");
            Console.WriteLine("Banned words found: ");
            int count = 0;            
            
            var bannedWordsCollection = bannedWordsRepo.GetBannedWordsList();            
            bannedWordsCollection.ForEach((w) =>
            {
                if (textToLookup.ContainsKey(w.Trim()))
                {
                    count += textToLookup[w];
                    Console.Write(w + " ");
                }
            });
            Console.WriteLine();

            return count;
        }

        internal string MaskWord(string word)
        {
            if (String.IsNullOrEmpty(word)) return word;
            
            char[] arr = new char[word.Length];

            for (int i = 0; i < word.Length; i++)
            {
                if (i == 0 || i == word.Length - 1)
                {
                    arr[i] = word[i];
                    continue;                    
                }
                arr[i] = '#';
            }

            return new String(arr);
        }


        public string FilterOutNegativeWords(string text, bool skipFiltering, IBannedWordsRepo bannedWordsRepo)
        {
            var bannedWordsCollection = bannedWordsRepo.GetBannedWordsList();
            int count = 0;
            if (!String.IsNullOrEmpty(text))
            {
                bannedWordsCollection.ForEach((w) =>
                {
                    if (text.Contains(w))
                    {
                        if (!skipFiltering) text = text.Replace(w, MaskWord(w));
                        count += 1;
                    }
                });
            }
            
            Console.WriteLine("Negative words found: {0}", count);
            Console.WriteLine("Negative words filtered: {0}", skipFiltering ? 0 : count);
            Console.WriteLine("Text after filtering: {0}", text);
            return text;
        }

        public string FilterOutNegativeWords(string text, bool skipFiltering)
        {
            return FilterOutNegativeWords(text, skipFiltering, GetBannedWordsRepo());
        }   
    }
}
