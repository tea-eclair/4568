using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Safe
{
    public partial class MainWindow : Window
    {
        private int password = 99;
        private int result = 0;

        private Point startPoint;
        private bool isDragging = false;

        public MainWindow()
        {
            InitializeComponent();

            krug.MouseLeftButtonDown += Krug_MouseLeftButtonDown;
            krug.MouseMove += Krug_MouseMove;
            krug.MouseLeftButtonUp += Krug_MouseLeftButtonUp;

            // Добавляем обработчик события PreviewTextInput для TextBox
            mynumber.PreviewTextInput += Mynumber_PreviewTextInput;
        }

        // Метод обработки события PreviewTextInput для проверки вводимых символов
        private void Mynumber_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            // Проверяем, является ли вводимый символ числом
            if (!char.IsDigit(e.Text, e.Text.Length - 1))
            {
                // Если символ не является числом, отменяем его ввод
                e.Handled = true;
            }
        }

        private void Krug_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            startPoint = new Point(Canvas.GetLeft(krug) + krug.ActualWidth / 2, Canvas.GetTop(krug) + krug.ActualHeight / 2);
            isDragging = true;
        }

        private void Krug_MouseMove(object sender, MouseEventArgs e)
{
    if (isDragging)
    {
        Point currentPoint = e.GetPosition(MainCanvas);
        Vector delta = currentPoint - startPoint;
        double angle = Math.Atan2(delta.Y, delta.X) * (180 / Math.PI);
        angle = (angle + 360) % 360;
        double mappedNumber = Map(angle, 0, 360, 99, 0); // Измененный маппинг
        result = Convert.ToInt32(mappedNumber);
        ((RotateTransform)krug.RenderTransform).Angle = angle;
    }
}


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Int32.TryParse(mynumber.Text, out int numValue))
            {
                password = numValue;
                mynumber.Text = "Пароль задан. Угадайте: " + password;
                MessageBox.Show("Введите число");
            }
        }

        private void Krug_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
            var checker = PasswordCheck(password, result);
            if (checker)
            {
                MessageBox.Show("Все правильно!");
            }
            else
            {
                MessageBox.Show("Не правильно. Вы ввели:" + result.ToString());
            }
        }

        private bool PasswordCheck(int a, int b)
        {
            return a == b;
        }

        private double Map(double value, double fromLow, double fromHigh, double toLow, double toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
    }
}
