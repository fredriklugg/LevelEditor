using System;
using System.IO;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Newtonsoft.Json;

namespace LevelEditor
{
    public class JsonHandler
    {
        public void SerializeMap(MapData map)
        {
            string path;

            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Save map as JSON",
                Filter = "JSON|*.json"
            };

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                path = Path.GetFullPath(sfd.FileName);

                JsonSerializer serializer = new JsonSerializer();
                serializer.NullValueHandling = NullValueHandling.Ignore;

                using (StreamWriter sw = new StreamWriter(path))
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, map);
                }

                string output = JsonConvert.SerializeObject(map);
                Console.WriteLine(output);
            }
        }
    }
}