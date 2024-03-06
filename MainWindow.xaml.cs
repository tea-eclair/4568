using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Organy_Drag_and_Drop
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Добавляем обработчики событий для изображений
            AddDragAndDropEvents(True_Card1);
            AddDragAndDropEvents(True_Card2);
            AddDragAndDropEvents(True_Card3);
            AddDragAndDropEvents(True_Card4);
            AddDragAndDropEvents(False_Card1);
            AddDragAndDropEvents(False_Card2);
            AddDragAndDropEvents(False_Card3);
        }

        // Метод для добавления обработчиков событий для изображения
        private void AddDragAndDropEvents(Image image)
        {
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            image.MouseMove += Image_MouseMove;
            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
        }

        private bool isDragging = false;
        private Point originalMousePosition;
        private TranslateTransform originalTranslation;

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            originalMousePosition = e.GetPosition(null);
            originalTranslation = new TranslateTransform(Canvas.GetLeft((UIElement)sender), Canvas.GetTop((UIElement)sender));
            ((UIElement)sender).RenderTransform = originalTranslation;
            ((UIElement)sender).CaptureMouse();
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(null);
                Vector delta = Point.Subtract(currentPosition, originalMousePosition);
                originalTranslation.X += delta.X;
                originalTranslation.Y += delta.Y;
                Canvas.SetLeft((UIElement)sender, originalTranslation.X);
                Canvas.SetTop((UIElement)sender, originalTranslation.Y);
                originalMousePosition = currentPosition;
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            ((UIElement)sender).ReleaseMouseCapture();
        }
    }
}
