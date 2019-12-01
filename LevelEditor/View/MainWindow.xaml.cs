using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using GalaSoft.MvvmLight.CommandWpf;
using LevelEditor.ViewModel;
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

        public MainWindow()
        {
            InitializeComponent();
            GridRows = 10;
            GridColumns = 10;
            InitGrid(GridColumns, GridRows);
            BackgroundSelected = true;
        }

        private void InitGrid(int columns, int rows)
        {
            var grid = new UniformGrid();

            grid.Height = 400;
            grid.Width = 400;
            grid.HorizontalAlignment = HorizontalAlignment.Left;
            grid.VerticalAlignment = VerticalAlignment.Top;
            grid.Margin = new Thickness(365, 35, 0, 0);

            grid.Columns = columns;
            grid.Rows = rows;

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    var button = new Button
                    {
                        Name = "button" + column + row,
                        Background = new SolidColorBrush(Colors.White)
                    };

                    button.Click += new RoutedEventHandler(Draw);

                    grid.Children.Add(button);
                }
            }

            MainGrid.Children.Add(grid);
        }

        private void SetTileSet(ImageSource tiles)
        {
            for (int i = 0; i < tiles.Width / 16; i++)
            {
                for (int j = 0; j < tiles.Height / 16; j++)
                {
                    Image tileImg = new Image();
                    CroppedBitmap cb = new CroppedBitmap((BitmapSource) tiles, new Int32Rect(j * 16, i * 16, 16, 16));
                    tileImg.Source = cb;

                    TileBox.Items.Add(tileImg);
                    TileBox.Items.Refresh();
                }
            }
            Console.WriteLine("Added tileset");
        }

        private void Draw(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;

            if (BackgroundSelected)
            {
                if (SelectedTile != null)
                {
                    button.Background = new BitmapCacheBrush((Visual)SelectedTile);
                }
                else
                {
                    button.Background = new SolidColorBrush(Colors.Black);
                }
            }
            
            if(ForegroundSelected)

            {
                if (SelectedTile != null)
                {
                    button.Foreground = new BitmapCacheBrush((Visual)SelectedTile);
                }
                else
                {
                    button.Foreground = new SolidColorBrush(Colors.Black);
                }
            }

            Console.WriteLine("Button Clicked " + button.Name);
        }
        private void setGridSize(object sender, RoutedEventArgs e)
        {
            GridRows = Convert.ToInt32(RowsTxtBox.Text);
            GridColumns = Convert.ToInt32(ColumnTxtBox.Text);

            InitGrid(Convert.ToInt32(ColumnTxtBox.Text), Convert.ToInt32(RowsTxtBox.Text));
            //Console.WriteLine(ColumnTxtBox.Text + " " + RowsTxtBox.Text);
        }

        private void TileBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedTile = TileBox.SelectedItem;
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
            Map map = new Map(GridRows, GridColumns, new []{10,10}, TileSet);
            
            var json = new JsonHandler();
            json.SerializeMap(map);
        }

        private void LoadMap_Click(object sender, RoutedEventArgs e)
        {
            var json = new JsonHandler();
            json.DeserializeMap();
        }
    }
}
