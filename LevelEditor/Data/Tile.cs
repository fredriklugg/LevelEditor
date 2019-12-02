using System.Windows.Controls;
using System.Windows.Media;

namespace LevelEditor
{
    public class Tile
    {
        public int GridRowPos { get; set; }
        public int GridColumnPos { get; set; }
        public ImageSource TileImage { get; set; }

        public Tile(int grp, int gcp, Image ti)
        {
            GridRowPos = grp;
            GridColumnPos = gcp;
            TileImage = ti;
        }
    }
}