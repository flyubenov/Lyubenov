using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentConsole.Test.Unit
{
    using BannedWordsRepo;
    using NUnit.Framework;
    using System.Text.RegularExpressions;

    [TestFixture]
    public class BannedWordsProcessorTest
    {
        //[Test]
        //public void GetBannedWordsRepo_Test()
        //{
        //    BannedWordsProcessor wp = new BannedWordsProcessor();
        //    Assert.IsTrue(wp.GetBannedWordsRepo() is IBannedWordsRepo);
        //}

        //[Test]
        //public void RemoveSpecialCharactersFromText_Test()
        //{
        //    BannedWordsProcessor wp = new BannedWordsProcessor();
        //    string text = "Hello, world. This-is-dash";
        //    string result = wp.RemoveSpecialCharactersFromText(text);
        //    Assert.IsTrue(!result.Contains(","));
        //    Assert.IsTrue(!result.Contains("."));
        //    Assert.IsTrue(!result.Contains("-"));
        //    Assert.IsTrue(Regex.Matches(result, "[~!@#$%^&*()_+{}:\"<>?.-,]").Count==0);
        //}
    }
}
