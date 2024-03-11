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
        private Point peopleOriginalPosition; // Переменная для хранения исходной позиции картинки "People"

        public MainWindow()
        {
            InitializeComponent();

            // Добавляем обработчики событий для всех изображений
            AddDragAndDropEvents(canvas);

            // Разбрасываем элементы при запуске приложения
            ScatterElements();

            // Смещаем картинку "People" влево на 100 пикселей
            Canvas.SetLeft(People, 350);

           
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
            // Находим элемент, на который нажали, и проверяем, не является ли он картинкой "People"
            draggedElement = e.Source as UIElement;
            if (draggedElement != null && draggedElement != People)
            {
                // Запоминаем начальные координаты мыши и элемента
                originalMousePosition = e.GetPosition(canvas);
                originalElementPosition = new Point(Canvas.GetLeft(draggedElement), Canvas.GetTop(draggedElement));
                draggedElement.CaptureMouse();
            }
        }

        private void Canvas_MouseMove(object sender, MouseEventArgs e)
        {
            // Проверяем, нажата ли левая кнопка мыши, перетаскивается ли элемент и не является ли он картинкой "People"
            if (e.LeftButton == MouseButtonState.Pressed && draggedElement != null && draggedElement != People)
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
            // Освобождаем захваченный элемент, если это не картинка "People"
            if (draggedElement != null && draggedElement != People)
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

            // Перемещаем каждый элемент на случайную позицию в пределах окна с погрешностью от -15 до +15
            foreach (UIElement element in canvas.Children)
            {
                if (element != People) // Исключаем картинку "People"
                {
                    double left = random.NextDouble() * (width - element.RenderSize.Width - 30) + 15; // С погрешностью от -15 до +15
                    double top = random.NextDouble() * (height - element.RenderSize.Height - 30) - 15; // С погрешностью от -15 до +15
                    Canvas.SetLeft(element, left);
                    Canvas.SetTop(element, top);
                }
            }
        }

        private void Button_Scatter_Click(object sender, RoutedEventArgs e)
        {
           

            // Разбрасываем все элементы, кроме картинки "People"
            ScatterElements();

            isDraggingDone = true; // Устанавливаем флаг, что фигуры были разбросаны
        }

        private bool CheckCardPlacement()
        {
            // Задаем координаты человека и погрешность
            double peopleLeft = 350; // Новая координата для человека
            double peopleTop = 10; // Новая координата для человека
            double peopleRight = peopleLeft + People.Width;
            double peopleBottom = peopleTop + People.Height;
            double tolerance = 5; // Погрешность

            // Создаем словарь для хранения правильных координат карточек
            Dictionary<string, Point> correctCardCoordinates = new Dictionary<string, Point>
    {
        { "True_Card4", new Point(458, 10) },
        { "True_Card1", new Point(441, 70) },
        { "True_Card2", new Point(440, 117) },
        { "True_Card3", new Point(474, 95) }
    };

            // Переменная для отслеживания правильности сборки
            bool isCorrectlyPlaced = true;

            // Проверяем, находятся ли все правильные карточки на своих местах с учетом погрешности
            foreach (var kvp in correctCardCoordinates)
            {
                Image card = (Image)canvas.FindName(kvp.Key); // Получаем изображение по имени
                if (card != null)
                {
                    double cardLeft = Canvas.GetLeft(card);
                    double cardTop = Canvas.GetTop(card);

                    // Получаем правильные координаты карточки из словаря
                    Point correctCoordinates = kvp.Value;

                    // Проверяем, находится ли текущая карточка внутри заданной области с учетом погрешности
                    if (!((cardLeft >= correctCoordinates.X - tolerance &&
                          cardLeft <= correctCoordinates.X + tolerance &&
                          cardTop >= correctCoordinates.Y - tolerance &&
                          cardTop <= correctCoordinates.Y + tolerance) ||
                          (cardLeft >= peopleLeft - tolerance &&
                          cardLeft <= peopleRight + tolerance &&
                          cardTop >= peopleTop - tolerance &&
                          cardTop <= peopleBottom + tolerance)))
                    {
                        // Если карточка находится не на своем месте или слишком далеко, считаем их неправильно собранными
                        isCorrectlyPlaced = false;
                        break;
                    }
                }
            }

            return isCorrectlyPlaced; // Возвращаем результат проверки
        }



        private void Button_Collect_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем, были ли фигуры перетащены перед нажатием кнопки "Собрать"
            if (isDraggingDone)
            {
                // Проверяем правильность расстановки картинок
                bool isCorrectlyPlaced = CheckCardPlacement();
                if (isCorrectlyPlaced)
                {
                    MessageBox.Show("Правильно расставлено");
                }
                else
                {
                    MessageBox.Show("Не верно собрано");
                }
            }
            else
            {
                MessageBox.Show("Сначала разбросьте элементы");
            }
        }

       
    }
}
