using System.Windows.Controls;
using System.Windows.Media;

namespace LevelEditor
{
    public class Tile
    {
        public int GridRowPos { get; set; }
        public int GridColumnPos { get; set; }
        public string TileImage { get; set; }
        public int TileIndex { get; set; }

        public Tile(int grp, int gcp, string ti, int tileIndex)
        {
            GridRowPos = grp;
            GridColumnPos = gcp;
            TileImage = ti;
            TileIndex = tileIndex;
        }

    }
}