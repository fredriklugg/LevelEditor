using System.Collections.Generic;
using System.Windows.Media;

namespace LevelEditor
{
    public class Map
    {
        public int GridRows { get; set; }
        public int GridColumns { get; set; }
        public List<Tile> TileList = new List<Tile>();
        public ImageSource TileSet { get; set; }

        public Map(int gr, int gc, List<Tile> tiles, ImageSource tileset)
        {
            GridRows = gr;
            GridColumns = gc;
            TileList = tiles;
            TileSet = tileset;
        }

    }
}
