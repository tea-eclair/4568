using Drag_and_Drop;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace drag2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Window1 window1 = new Window1();
            window1.Show();
        }
        private void RectMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragDrop.DoDragDrop(blackRect, new DataObject(DataFormats.Serializable, blackRect), DragDropEffects.Move);
            }
        }
        private void CanvasDrop(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element && !canvas.Children.Contains(element))
            {
                double elementWidth = element.RenderSize.Width;
                double elementHeight = element.RenderSize.Height;
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X - elementWidth / 2);
                Canvas.SetTop(element, dropPosition.Y - elementHeight / 2);
                canvas.Children.Add(element);
            }

        }
        private void CanvasDragOver(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element)
            {
                double elementWidth = element.RenderSize.Width;
                double elementHeight = element.RenderSize.Height;
                Point dropPosition = e.GetPosition(canvas);
                Canvas.SetLeft(element, dropPosition.X - elementWidth / 2);
                Canvas.SetTop(element, dropPosition.Y - elementHeight / 2);
            }
        }
        private void CanvasDragLeave(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element)
            {
                canvas.Children.Remove(element);
            }
        }
    }
}