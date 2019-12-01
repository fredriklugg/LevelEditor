using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Navigation;

namespace LevelEditor
{
    public class Map
    {
        public int GridRows { get; set; }
        public int GridColumns { get; set; }
        public int[] Tiles { get; set; }
        public ImageSource TileSet { get; set; }

        public Map(int gr, int gc, int[] tiles, ImageSource tileset)
        {
            GridRows = gr;
            GridColumns = gc;
            Tiles = tiles;
            TileSet = tileset;
        }

    }
}
