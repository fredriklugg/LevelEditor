using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Button = System.Windows.Controls.Button;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;
using MouseEventHandler = System.Windows.Input.MouseEventHandler;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Windows.Size;

namespace LevelEditor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public int GridRows { get; set; }
        public int GridColumns { get; set; }
        public object SelectedTile { get; set; }
        public int SelectedTileIndex { get; set; }
        public bool ForegroundSelected { get; set; }
        public bool BackgroundSelected { get; set; }
        public ImageSource TileSet { get; set; }
        public string TileSetSource { get; set; }
        public List<Tile> TileList = new List<Tile>();

        public MainWindow()
        {
            ImageSource standardTiles = new BitmapImage(new Uri(@"C:\Users\Fredrik\Desktop\Skole\3.År\Semester 1\Tools Prog\Kode\LevelEditor\LevelEditor\resources\tilesedited.png"));

            InitializeComponent();
            SetTileSet(standardTiles);
            SelectedTile = TileBox.Items.GetItemAt(32);
            GridRows = 10;
            GridColumns = 10;
            InitGrid(GridColumns, GridRows, GridMap);
            BackgroundSelected = true;
        }

        private void ResetGrid(int rows, int columns, Grid grid)
        {
            grid.Children.Clear();
            grid.ColumnDefinitions.Clear();
            grid.RowDefinitions.Clear();
            for (int row = 0; row < rows; row++) { grid.RowDefinitions.Add(new RowDefinition());}
            for (int column = 0; column < columns; column++) { grid.ColumnDefinitions.Add(new ColumnDefinition());}
        }

        private void InitGrid(int columns, int rows, Grid grid)
        {
            ResetGrid(rows, columns, grid);
            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    Image image = new Image
                    {
                        Name = "Cell" + column + row,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Stretch = Stretch.Fill,
                        Source = SelectedTile as BitmapSource
                        
                    };
                    Tile tile = new Tile(rows, column, image);

                    Border border = new Border();

                    image.AddHandler(Image.MouseLeftButtonDownEvent, new RoutedEventHandler(Draw), true);
                    image.AddHandler(Mouse.PreviewMouseMoveEvent, new RoutedEventHandler(Draw), true);

                    Grid.SetColumn(image, column);
                    Grid.SetRow(image, row);
                    
                    grid.Children.Add(image);
                }
            }
        }

        private void SetTileSet(ImageSource tiles)
        {
            ResetGrid((int)tiles.Height / 16, (int)tiles.Width / 16, TilesetGrid);

            for (int i = 0; i < tiles.Width / 16; i++)
            {
                for (int j = 0; j < tiles.Height / 16; j++)
                {
                    CroppedBitmap cb = new CroppedBitmap((BitmapSource) tiles, new Int32Rect(i * 16, j * 16, 16, 16));
                    Image image = new Image();
                    image.Source = cb;

                    image.Height = cb.Height * 2;
                    image.Width = cb.Width * 2;

                    TileBox.Items.Add(cb);
                    TileBox.Items.Refresh();
                }
            }
        }

        private void Draw(object sender, RoutedEventArgs e)
        {
            Image image = sender as Image;

            if (Mouse.LeftButton == MouseButtonState.Released)
            {
                return;
            }

            if (BackgroundSelected)
            {
                if (SelectedTile != null)
                {
                    image.Source = (BitmapSource) SelectedTile;
                }

            }
            else if(ForegroundSelected)

            {
                if (SelectedTile != null)
                {
                    image.Source = (BitmapSource) SelectedTile;
                }
            }
            Console.WriteLine("Button Clicked " + image.Name);
        }

        private void setGridSize(object sender, RoutedEventArgs e)
        {
            GridRows = Convert.ToInt32(RowsTxtBox.Text);
            GridColumns = Convert.ToInt32(ColumnTxtBox.Text);

            InitGrid(Convert.ToInt32(ColumnTxtBox.Text), Convert.ToInt32(RowsTxtBox.Text), GridMap);
            //Console.WriteLine(ColumnTxtBox.Text + " " + RowsTxtBox.Text);
        }

        private void TileBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedTile = TileBox.SelectedItem as BitmapSource;
            SelectedTileIndex = TileBox.SelectedIndex;
            Console.WriteLine("CHANGED SELECTED TILE " + SelectedTileIndex);
        }

        private void Foreground_Click(object sender, RoutedEventArgs e)
        {
            if (BackgroundSelected)
            {
                ForegroundSelected = true;
                BackgroundSelected = false;
                Console.WriteLine("Foreground selected");
            }
        }

        private void Background_Click(object sender, RoutedEventArgs e)
        {
            if (ForegroundSelected)
            {
                BackgroundSelected = true;
                ForegroundSelected = false;
                Console.WriteLine("Background selected");
            }
        }

        private void LoadTileset(object sender, RoutedEventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog
            {
                Title = "Select a picture",
                Filter = "All supported graphics|*.jpg;*.jpeg;*.png|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png"
            };
            if (op.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                TileSet = new BitmapImage(new Uri(op.FileName));
                TileSetSource = op.FileName;
                SetTileSet(TileSet);
            }
        }

        private void SaveMap_Click(object sender, RoutedEventArgs e)
        {
            addTileToList(GridMap);
            Map map = new Map(GridRows, GridColumns, TileList, TileSet);
            var json = new JsonHandler();
            json.SerializeMap(map);
            TileList.Clear();

        }

        private void LoadMap_Click(object sender, RoutedEventArgs e)
        {
            //var json = new JsonHandler();
            //json.DeserializeMap();
            addTileToList(GridMap);
        }

        private void addTileToList(Grid grid)
        {
            foreach (var tile in grid.Children)
            {
                TileList.Add(tile);
                Console.WriteLine(tile);
            }
        }
    }
}
