using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Collections.Specialized;
using BannedWordsRepo;
using Moq;

//Author: Filip Lyubenov, f_lubenov@hotmail.com

namespace ContentConsole
{
    public static class Program
    { 
        public static void Main(string[] args)
        {
            string content = "The weather, in Manchester in winter is bad bad. It rains all the time - it must be horrible for people visiting.";
            bool skipFiltering = false;
            if (args.Length == 1)
                content = args[0];
            if (args.Length == 2)
            {
                content = args[0];
                if (args[0] == "-skipFilter")
                    skipFiltering = true;
            }

            BannedWordsProcessor wordProcessor = new BannedWordsProcessor();
            

            Console.WriteLine("Scanned the text:");
            Console.WriteLine(content);
            Console.WriteLine("Total Number of negative words: " + wordProcessor.CountBannedWords(content));
            wordProcessor.FilterOutNegativeWords(content, skipFiltering);

            Console.WriteLine("Press ANY key to exit.");
            Console.ReadKey();
        }

        
    }

}
