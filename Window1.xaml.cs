using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace drag2
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }

        private void Canvas2DragOver(object sender, DragEventArgs e)
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
        private void Canvas2DragLeave(object sender, DragEventArgs e)
        {
            object data = e.Data.GetData(DataFormats.Serializable);
            if (data is UIElement element)
            {
                canvas.Children.Remove(element);
            }
        }

        public void Canvas2Drop(object sender, DragEventArgs e)
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
    }
}
