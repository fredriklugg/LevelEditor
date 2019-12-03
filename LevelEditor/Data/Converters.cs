
namespace LevelEditor.Data
{
    class Converters
    {
        public string getImageIdFromName(string name)
        {
            string sub = name.Substring(4, 1);
            if (name.Length > 7)
            {
                if (sub.Contains("x"))
                {
                    return sub = name.Substring(4, 2);
                }
            }

            return sub;
        }

        public string getGridIndexFromName(string name)
        {
            if (name.Length == 7)
            {
                return name.Substring(6, 1);
            }
            else if (name.Length == 8)
            {
                return name.Substring(6, 2);
            }
            else if (name.Length == 9)
            {
                return name.Substring(7, 2);
            }

            return name;
        }
    }
}
