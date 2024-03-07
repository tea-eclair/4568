using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Organy_Drag_and_Drop
{
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private bool isDraggingDone = false;

        public MainWindow()
        {
            InitializeComponent();

            // Добавляем обработчики событий для всех изображений
            AddDragAndDropEvents(canvas);

            // Разбрасываем элементы при запуске приложения
            ScatterElements();
        }


        private void AddDragAndDropEvents(Canvas canvas)
        {
            // Добавляем обработчики событий для перемещения элементов
            canvas.MouseLeftButtonDown += Canvas_MouseLeftButtonDown;
            canvas.MouseMove += Canvas_MouseMove;
            canvas.MouseLeftButtonUp += Canvas_MouseLeftButtonUp;
        }

        private UIElement draggedElement;
        private Point originalMousePosition;
        private Point originalElementPosition;

        private void Canvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Находим элемент, на который нажали
            draggedElement = e.Source as UIElement;
            if (draggedElement != null)
            {
                // Запоминаем начальные координаты мыши и элемента
                originalMousePosition = e.GetPosition(canvas);
                originalElementPosition = new Point(Canvas.GetLeft(draggedElement), Canvas.GetTop(draggedElement));
                draggedElement.CaptureMouse();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Проверяем, нажата ли левая кнопка мыши и перетаскивается ли элемент
            if (e.LeftButton == MouseButtonState.Pressed && draggedElement != null)
            {
                // Вычисляем смещение мыши
                Point currentPosition = e.GetPosition(canvas);
                Vector delta = Point.Subtract(currentPosition, originalMousePosition);

                // Перемещаем элемент на новую позицию
                Canvas.SetLeft(draggedElement, originalElementPosition.X + delta.X);
                Canvas.SetTop(draggedElement, originalElementPosition.Y + delta.Y);
            }
        }

        private void Canvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Освобождаем захваченный элемент
            if (draggedElement != null)
            {
                draggedElement.ReleaseMouseCapture();
                draggedElement = null;
            }
        }

        private void ScatterElements()
        {
            // Получаем размеры окна
            double width = canvas.ActualWidth;
            double height = canvas.ActualHeight;

            // Перемещаем каждый элемент на случайную позицию в пределах окна
            foreach (UIElement element in canvas.Children)
            {
                double left = random.NextDouble() * (width - element.RenderSize.Width);
                double top = random.NextDouble() * (height - element.RenderSize.Height);
                Canvas.SetLeft(element, left);
                Canvas.SetTop(element, top);
            }
        }

        private void Button_Scatter_Click(object sender, RoutedEventArgs e)
        {
            // Разбрасываем элементы по окну
            ScatterElements();
            isDraggingDone = true; // Устанавливаем флаг, что фигуры были разбросаны
            ((Button)sender).IsEnabled = false; // Запрещаем нажатие кнопки "Собрать" до следующего раза
        }

        private void Button_Collect_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, были ли фигуры перетащены перед нажатием кнопки "Собрать"
            if (isDraggingDone)
            {
                // Собираем элементы обратно в исходные позиции
                foreach (UIElement element in canvas.Children)
                {
                    Canvas.SetLeft(element, 0);
                    Canvas.SetTop(element, 0);
                }

                // Включаем возможность повторного разброса элементов
             

                // Проверяем выполнение задания
                CheckTaskCompletion();
            }
            else
            {
                MessageBox.Show("Сначала разбросьте элементы");
            }
        }

        private void CheckTaskCompletion()
        {
            // Проверяем выполнение задания
            // Если выполнено, выводим сообщение
            // Здесь должна быть ваша логика проверки
            MessageBox.Show("Задание выполнено правильно");
        }


    }
}
