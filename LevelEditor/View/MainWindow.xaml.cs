using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using LevelEditor.Data;
using HorizontalAlignment = System.Windows.HorizontalAlignment;
using Image = System.Windows.Controls.Image;


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
        public ImageSource TileSet { get; set; }
        public string TileSetSource { get; set; }

        public List<Tile> TileList = new List<Tile>();

        public List<int> TileSetList = new List<int>();

        Converters converter = new Converters();


        public MainWindow()
        {
            ImageSource standardTiles = new BitmapImage(new Uri(@"C:\LevelEditor\LevelEditor\resources\tiles-simple.png"));

            InitializeComponent();
            SetTileSet(standardTiles);
            SelectedTile = TileBox.Items.GetItemAt(4);
            SelectedTileIndex = 4;
            GridRows = 10;
            GridColumns = 10;
            InitGrid(GridColumns, GridRows);
        }

        private void ResetGrid(int rows, int columns)
        {
            GridMap.Children.Clear();
            GridMap.ColumnDefinitions.Clear();
            GridMap.RowDefinitions.Clear();
            for (int row = 0; row < rows; row++) { GridMap.RowDefinitions.Add(new RowDefinition());}
            for (int column = 0; column < columns; column++) { GridMap.ColumnDefinitions.Add(new ColumnDefinition());}
        }

        private void InitGrid(int columns, int rows)
        {
            ResetGrid(rows, columns);
            TileList.Clear(); //Make sure tilelist is empty

            for (int row = 0; row < rows; row++)
            {
                for (int column = 0; column < columns; column++)
                {
                    int indexPos = columns * row + column; 
                    Image image = new Image
                    {
                        Name = "Cell" + SelectedTileIndex+"x"+indexPos,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Stretch = Stretch.Fill,
                        Source = SelectedTile as BitmapSource
                    };

                    Tile tile = new Tile(row, column, converter.getImageIdFromName(image.Name), indexPos);
                    TileList.Add(tile);

                    image.AddHandler(Image.MouseLeftButtonDownEvent, new RoutedEventHandler(Draw), true);

                    Grid.SetColumn(image, column);
                    Grid.SetRow(image, row);
                    
                    GridMap.Children.Add(image);
                }
            }
        }

        private void LoadGrid(Map map)
        {
            ResetGrid(map.GridRows, map.GridColumns);
            //TileList.Clear(); //Make sure tilelist is empty

            for (int row = 0; row < map.GridRows; row++)
            {
                for (int column = 0; column < map.GridColumns; column++)
                {
                    int indexPos = map.GridColumns * row + column;
                    Image image = new Image
                    {
                        Name = "Cell" + map.TileList[indexPos].TileImage + "x" + indexPos,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Stretch = Stretch.Fill,
                        Source = TileBox.Items.GetItemAt(Convert.ToInt32(map.TileList[indexPos].TileImage)) as BitmapSource
                    };
                    Tile tile = new Tile(row, column, map.TileList[indexPos].TileImage, indexPos);
                    TileList[indexPos] = tile;

                    image.AddHandler(Image.MouseLeftButtonDownEvent, new RoutedEventHandler(Draw), true);

                    Grid.SetColumn(image, column);
                    Grid.SetRow(image, row);

                    GridMap.Children.Add(image);
                }
            }
        }

        private void SetTileSet(ImageSource tiles)
        {
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
            string gridIndex = converter.getGridIndexFromName(image.Name);
            Console.WriteLine(gridIndex);
            image.Name = "";

            if (SelectedTile != null)
            {
                image.Source = (BitmapSource) SelectedTile;
                image.Name = "Cell" + SelectedTileIndex + "x" + gridIndex;
                if (gridIndex == converter.getGridIndexFromName(image.Name))
                {
                    TileList[Convert.ToInt32(gridIndex)].TileImage = converter.getImageIdFromName(image.Name);
                    checkNeighbourTiles(Convert.ToInt32(converter.getGridIndexFromName(image.Name)));
                }
            }
            Console.WriteLine("Button Clicked " + image.Name);
        }

        private void setGridSize(object sender, RoutedEventArgs e)
        {
            if (GridDimentions.Text == String.Empty || Convert.ToInt32(GridDimentions.Text) > 10) 
            {
                return;
            }
            GridColumns = Convert.ToInt32(GridDimentions.Text);
            GridRows = Convert.ToInt32(GridDimentions.Text);

            InitGrid(Convert.ToInt32(GridDimentions.Text), Convert.ToInt32(GridDimentions.Text));
        }

        private void TileBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            SelectedTile = TileBox.SelectedItem as BitmapSource;
            SelectedTileIndex = TileBox.SelectedIndex;
            Console.WriteLine("CHANGED SELECTED TILE " + SelectedTileIndex);
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
            Map map = new Map(GridRows, GridColumns, TileList, TileSet);
            var json = new JsonHandler();
            json.SerializeMap(map);
        }

        private void LoadMap_Click(object sender, RoutedEventArgs e)
        {
            var json = new JsonHandler();
            Map map = json.DeserializeMap();
            LoadGrid(map);
        }

        private void checkNeighbourTiles(int tileIndex)
        {
            //Neighbours: L = Left, R = Right, T= Top, B = bottom 
            int L = tileIndex - 1;
            int R = tileIndex + 1;
            int T = tileIndex - 10;
            int TL = T - 1;
            int TR = T + 1;
            int B = tileIndex + 10;
            int BL = B - 1;
            int BR = B + 1;

            int MaxGridSize = GridColumns * GridRows;

            //Select element 0 in the ListView to use automatic road creator
            if (TL < 0 || TR < 0 || T < 0 || B > MaxGridSize || BL > MaxGridSize || BR > MaxGridSize)
            {
                return;
            }
            if (TileList[tileIndex].TileImage == "0")
            {
                if (TileList[T].TileImage == "0" || TileList[T].TileImage == "2" || TileList[T].TileImage == "3" || TileList[T].TileImage == "5" || TileList[T].TileImage == "6" || TileList[T].TileImage == "8" && TileList[B].TileImage == "0" || TileList[B].TileImage == "2" || TileList[B].TileImage == "3" || TileList[B].TileImage == "5" || TileList[B].TileImage == "6" || TileList[B].TileImage == "8")
                {
                    TileList[T].TileImage = "1";
                    TileList[B].TileImage = "1";
                    TileList[tileIndex].TileImage = "1";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if(TileList[T].TileImage == "0" || TileList[T].TileImage == "2" || TileList[T].TileImage == "3" || TileList[T].TileImage == "5" || TileList[T].TileImage == "6" || TileList[T].TileImage == "8")
                {
                    TileList[T].TileImage = "1";
                    TileList[tileIndex].TileImage = "1";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[B].TileImage == "0" || TileList[B].TileImage == "2" || TileList[B].TileImage == "3" || TileList[B].TileImage == "5" || TileList[B].TileImage == "6" || TileList[B].TileImage == "8")
                {
                    TileList[B].TileImage = "1";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[TR].TileImage == "0" || TileList[TR].TileImage == "1" || TileList[TR].TileImage == "7")
                {
                    TileList[R].TileImage = "8";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[BR].TileImage == "0" || TileList[BR].TileImage == "1" || TileList[BR].TileImage == "7")
                {
                    TileList[B].TileImage = "2";
                    TileList[BR].TileImage = "6";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[BL].TileImage == "0" || TileList[BL].TileImage == "1" || TileList[BL].TileImage == "7")
                {
                    TileList[B].TileImage = "8";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[TL].TileImage == "0" || TileList[TL].TileImage == "1" || TileList[TL].TileImage == "7")
                {
                    TileList[L].TileImage = "2";
                    TileList[tileIndex].TileImage = "6";

                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[L].TileImage == "0" || TileList[L].TileImage == "1" || TileList[L].TileImage == "2" || TileList[L].TileImage == "6" || TileList[L].TileImage == "7" || TileList[L].TileImage == "8" && TileList[R].TileImage == "0" || TileList[R].TileImage == "1" || TileList[R].TileImage == "2" || TileList[R].TileImage == "6" || TileList[R].TileImage == "7" || TileList[R].TileImage == "8")
                {
                    TileList[L].TileImage = "3";
                    TileList[R].TileImage = "3";
                    TileList[tileIndex].TileImage = "3";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[L].TileImage == "0" || TileList[L].TileImage == "1" || TileList[L].TileImage == "2" || TileList[L].TileImage == "6" || TileList[L].TileImage == "7" || TileList[L].TileImage == "8")
                {
                    TileList[L].TileImage = "3";
                    TileList[tileIndex].TileImage = "3";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
                if (TileList[R].TileImage == "0" || TileList[R].TileImage == "1" || TileList[R].TileImage == "2" || TileList[R].TileImage == "6" || TileList[R].TileImage == "7" || TileList[R].TileImage == "8")
                {
                    TileList[R].TileImage = "3";
                    TileList[tileIndex].TileImage = "3";
                    Map updateMap = new Map(GridRows, GridColumns, TileList, TileSet);
                    LoadGrid(updateMap);
                    return;
                }
            }
        }
    }
}
