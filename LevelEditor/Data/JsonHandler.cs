﻿using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace LevelEditor
{
    public class JsonHandler
    {
        public bool SerializeMap(Map map)
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
                    return true;
                }
            }

            return false;
        }

        public Map DeserializeMap()
        {
            Map DeserializedMap = null;

            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a JSON map file",
                Filter = "Only supports JSON|*.json"
            };
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                using (StreamReader sr = new StreamReader(op.FileName))
                {
                    string line;

                    line = sr.ReadLine();

                    DeserializedMap = JsonConvert.DeserializeObject<Map>(line);

                    Console.WriteLine(DeserializedMap);
                    
                }
            }
            return DeserializedMap;
        }
    }
}