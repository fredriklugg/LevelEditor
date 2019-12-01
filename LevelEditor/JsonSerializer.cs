using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace LevelEditor
{
    public class JsonSerializer
    {
        public void SerializeMap()
        {
            ImageSource tiles = new BitmapImage(new Uri(@"C:\Users\Fredrik\Desktop\Skole\3.År\Semester 1\Tools Prog\Kode\LevelEditor\LevelEditor\overworld_tileset_grass.png"));

            MapData map = new MapData(10, 10, new int[] { 10, 10, 10, 10, 10 }, tiles);

            string output = JsonConvert.SerializeObject(map);

            Console.WriteLine(output);
        }


    }
}