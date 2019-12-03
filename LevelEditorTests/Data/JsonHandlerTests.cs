using Microsoft.VisualStudio.TestTools.UnitTesting;
using LevelEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace LevelEditor.Tests
{
    [TestClass()]
    public class JsonHandlerTests
    {
        [TestMethod()]
        public void SerializeMapTest()
        {
            ImageSource Is = new BitmapImage(new Uri(@"C:\Users\Fredrik\Desktop\Skole\3.År\Semester 1\Tools Prog\Kode\LevelEditor\LevelEditor\resources\tiles-simple.png"));
            Map map = new Map(10, 10, new List<Tile>(), Is);
            JsonHandler json = new JsonHandler();


            Assert.IsTrue(json.SerializeMap(map));
        }
        [TestMethod()]
        public void DeserializeMapTest()
        {
            ImageSource Is = new BitmapImage(new Uri(@"C:\Users\Fredrik\Desktop\Skole\3.År\Semester 1\Tools Prog\Kode\LevelEditor\LevelEditor\resources\tiles-simple.png"));
            Map map = new Map(10, 10, new List<Tile>(), Is);
            JsonHandler json = new JsonHandler();
            Map deserializedMap = json.DeserializeMap();

            Console.WriteLine(deserializedMap);
            Console.WriteLine(map);

            Assert.AreEqual(map, deserializedMap);
        }
    }
}