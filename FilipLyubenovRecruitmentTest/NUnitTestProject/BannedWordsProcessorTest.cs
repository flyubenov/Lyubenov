using System;

//Author: Filip Lyubenov, f_lubenov@hotmail.com

namespace NUnitTestProject
{
    using BannedWordsRepo;
    using ContentConsole;
    using Moq;
    using NUnit.Framework;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;

    [TestFixture]
    public class BannedWordsProcessorTest
    {
        [Test]
        public void GetBannedWordsRepo_Test()
        {
            BannedWordsProcessor wp = new BannedWordsProcessor();
            Assert.IsTrue(wp.GetBannedWordsRepo() is IBannedWordsRepo);
        }

        [Test]
        public void RemoveSpecialCharactersFromText_Test()
        {
            BannedWordsProcessor wp = new BannedWordsProcessor();
            string text = "Hello, world. This-is-dash";
            string result = wp.RemoveSpecialCharactersFromText(text);
            Assert.IsTrue(Regex.Matches(result, "[,.-]+").Count == 0);
        }
        [Test]
        public void CountBannedWords_Test()
        {
            string text = "bad word 1, horrible nasty nasty nasty";
            BannedWordsProcessor wp = new BannedWordsProcessor();
            Mock<IBannedWordsRepo> mock = new Mock<IBannedWordsRepo>();
            mock.Setup(t=>t.GetBannedWordsList()).Returns(new List<string>());
            Assert.IsTrue(wp.CountBannedWords(text, mock.Object) == 0);
            
            mock.Setup(t => t.GetBannedWordsList()).Returns(new List<string>() { "bad", "horrible" });
            Assert.IsTrue(wp.CountBannedWords(text, mock.Object) == 2);
            
            Assert.IsTrue(wp.CountBannedWords("", mock.Object) == 0);
            Assert.IsTrue(wp.CountBannedWords(null, mock.Object) == 0);
            
            mock.Setup(t => t.GetBannedWordsList()).Returns(new List<string>() { "nasty" });
            Assert.IsTrue(wp.CountBannedWords(text, mock.Object) == 3);
        }

        [Test]
        public void MaskWord_Test()
        {
            string wordToMask = "horrible";
            BannedWordsProcessor wp = new BannedWordsProcessor();
            string masked = wp.MaskWord(wordToMask);
            Assert.IsTrue(masked.Substring(1,wordToMask.Length-2)=="######"); //not good, use of knowledge of what the filter charecter is
            masked = wp.MaskWord(null);
            Assert.IsTrue(String.IsNullOrEmpty(masked));
            masked = wp.MaskWord("");
            Assert.IsTrue(String.IsNullOrEmpty(masked));
            masked = wp.MaskWord("########");
            Assert.IsTrue(masked.Substring(1, wordToMask.Length - 2) == "######");
        }

        [Test]
        public void FilterOutNegativeWords_Test(){

            string text = "bad word 1, horrible nasty nasty nasty";
            BannedWordsProcessor wp = new BannedWordsProcessor();
            Mock<IBannedWordsRepo> mock = new Mock<IBannedWordsRepo>();
            mock.Setup(t => t.GetBannedWordsList()).Returns(new List<string>() { "bad", "horrible", "nasty" });
            string filtered = wp.FilterOutNegativeWords(text, false, mock.Object);

            Assert.IsTrue(filtered == "b#d word 1, h######e n###y n###y n###y");
            
            filtered = wp.FilterOutNegativeWords(text, true, mock.Object);
            Assert.IsTrue(filtered == text);

            mock.Setup(t => t.GetBannedWordsList()).Returns(new List<string>() { "unknown banned word" });
            filtered = wp.FilterOutNegativeWords(text, false, mock.Object);

            Assert.IsTrue(filtered == text);

            filtered = wp.FilterOutNegativeWords("", false, mock.Object);

            Assert.IsTrue(filtered == "");

            filtered = wp.FilterOutNegativeWords(null, false, mock.Object);

            Assert.IsTrue(filtered == null);
        }
    }
}