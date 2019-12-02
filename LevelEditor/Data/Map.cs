using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;

namespace LevelEditor
{
    public class Map
    {
        public int GridRows { get; set; }
        public int GridColumns { get; set; }
        public List<object> TileList = new List<object>();
        public ImageSource TileSet { get; set; }

        public Map(int gr, int gc, List<object> tiles, ImageSource tileset)
        {
            GridRows = gr;
            GridColumns = gc;
            TileList.AddRange(tiles);
            TileSet = tileset;
        }

    }
}
