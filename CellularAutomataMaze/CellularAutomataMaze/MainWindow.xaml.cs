using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CellularAutomataMaze
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Point startPoint;
        private Point stopPoint;
        private bool isLeftButtonClicked;
        private CellularAutomata cellularAutomata;
        private Rectangle[,] rectangles;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            string[] bornConditionsText = txtBoxBornConditions.Text.Split(';');
            List<ushort> bornConditions = new List<ushort>();
            foreach (var item in bornConditionsText)
            {
                bornConditions.Add(ushort.Parse(item));
            }

            string[] surviveConditionsText = txtBoxSurviveConditions.Text.Split(';');
            List<ushort> surviveConditions = new List<ushort>();
            foreach (var item in surviveConditionsText)
            {
                surviveConditions.Add(ushort.Parse(item));
            }

            ushort dimX = ushort.Parse(txtBoxDimX.Text);
            ushort dimY = ushort.Parse(txtBoxDimY.Text);

            int seed = int.Parse(txtBoxSeed.Text);
            int divider = int.Parse(txtBoxDivider.Text);

            int maxIterations = int.Parse(txtMaxIterations.Text);

            cellularAutomata = new CellularAutomata(bornConditions.ToArray(), surviveConditions.ToArray(), dimX, dimY);
            cellularAutomata.SeedMap(seed, divider);

            PrepareCanvas(dimX, dimY);

            if (checkBoxShowProgress.IsChecked != null && checkBoxShowProgress.IsChecked == true)
            {
                GenerateMazeWithPreview(dimX, dimY, maxIterations);
            }
            else
            {
                GenerateMazeWithoutPreview(dimX, dimY, maxIterations);
            }
        }

        private void PrepareCanvas(ushort dimX, ushort dimY)
        {
            rectangles = new Rectangle[dimX, dimY];
            mainCanvas.Children.Clear();
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    Rectangle rectangle = DrawRectangle(i, j);
                    rectangles[i, j] = rectangle;
                    mainCanvas.Children.Add(rectangle);
                }
            }
        }

        private void GenerateMazeWithoutPreview(ushort dimX, ushort dimY, int maxIterations)
        {
            int iterator = 0;
            while (cellularAutomata.AreCellsAlive)
            {
                cellularAutomata.NextGeneration();
                iterator++;
                if (iterator > maxIterations)
                {
                    break;
                }
                if (checkBoxBreakCalculations.IsChecked != null && checkBoxBreakCalculations.IsChecked == true)
                {
                    checkBoxBreakCalculations.IsChecked = false;
                    return;
                }
            }
            for (int i = 0; i < dimX; i++)
            {
                for (int j = 0; j < dimY; j++)
                {
                    if (cellularAutomata.Map[i, j] == true)
                    {
                        rectangles[i, j].Fill = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) };
                    }
                    else
                    {
                        rectangles[i, j].Fill = new SolidColorBrush { Color = Color.FromRgb(0, 0, 0) };
                    }
                }
            }
        }

        private void GenerateMazeWithPreview(ushort dimX, ushort dimY, int maxIterations)
        {
            Task task = Task.Run(() =>
            {
                int iterator = 0;
                while (cellularAutomata.AreCellsAlive)
                {
                    cellularAutomata.NextGeneration();
                    iterator++;
                    mainCanvas.Dispatcher.Invoke(() =>
                    {
                        for (int i = 0; i < dimX; i++)
                        {
                            for (int j = 0; j < dimY; j++)
                            {
                                if (cellularAutomata.Map[i, j] == true)
                                {
                                    rectangles[i, j].Fill = new SolidColorBrush { Color = Color.FromRgb(255, 255, 255) };
                                }
                                else
                                {
                                    rectangles[i, j].Fill = new SolidColorBrush { Color = Color.FromRgb(0, 0, 0) };
                                }
                            }
                        }
                    });
                    if (iterator > maxIterations)
                    {
                        break;
                    }
                    bool threadIterruped = false;
                    checkBoxBreakCalculations.Dispatcher.Invoke(() =>
                    {
                        if (checkBoxBreakCalculations.IsChecked != null && checkBoxBreakCalculations.IsChecked == true)
                        {
                            checkBoxBreakCalculations.IsChecked = false;
                            threadIterruped = true;
                        }
                    });
                    if (threadIterruped)
                    {
                        return;
                    }
                    Thread.Sleep(10);
                }
            });
        }

        private Rectangle DrawRectangle(int posX, int posY)
        {
            Rectangle rectangle = new Rectangle();
            Canvas.SetTop(rectangle, posY * 11);
            Canvas.SetLeft(rectangle, posX * 11);
            rectangle.Width = 10;
            rectangle.Height = 10;
            SolidColorBrush solidColorBrush = new SolidColorBrush
            {
                Color = Color.FromRgb(0, 0, 0)
            };
            rectangle.Fill = solidColorBrush;
            return rectangle;
        }
    }
}
