using Microsoft.VisualStudio.TestTools.UnitTesting;
using LevelEditor.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LevelEditor.Data.Tests
{
    [TestClass()]
    public class ConvertersTests
    {
        [TestMethod()]
        public void getImageIdFromNameTest()
        {
            Converters converter = new Converters();
            string test = converter.getImageIdFromName("Cell1x10");

            Assert.AreEqual("1", test);
        }

        [TestMethod()]
        public void getGridIndexFromNameTest()
        {
            Converters converter = new Converters();
            string test = converter.getGridIndexFromName("Cell3x12");

            Assert.AreEqual("12", test);
        }
    }
}