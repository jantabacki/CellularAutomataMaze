using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CellularAutomataMaze
{
    public partial class MainWindow
    {
        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Canvas element = mainCanvas;
            Point position = e.GetPosition(element);
            MatrixTransform transform = element.RenderTransform as MatrixTransform;
            Matrix matrix = transform.Matrix;
            double scale = e.Delta >= 0 ? 1.1 : (1.0 / 1.1);
            matrix.ScaleAtPrepend(scale, scale, position.X, position.Y);
            transform.Matrix = matrix;
        }

        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = e.GetPosition(this);
            isLeftButtonClicked = true;
        }

        private void Window_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isLeftButtonClicked = false;
        }

        private void MoveCanvasToNewCoordinates()
        {
            Canvas.SetTop(mainCanvas, Canvas.GetTop(mainCanvas) + (stopPoint.Y - startPoint.Y));
            Canvas.SetLeft(mainCanvas, Canvas.GetLeft(mainCanvas) + (stopPoint.X - startPoint.X));
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (isLeftButtonClicked)
            {
                stopPoint = e.GetPosition(this);
                MoveCanvasToNewCoordinates();
                startPoint = stopPoint;
            }
        }
    }
}
