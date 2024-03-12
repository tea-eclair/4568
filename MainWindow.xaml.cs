using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Safe
{
    public partial class MainWindow : Window
    {
        private bool isDragging = false;
        private Point lastPosition;
        private double dialRotationAngle = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeDialRotation();
        }

        private void InitializeDialRotation()
        {
            krug.MouseLeftButtonDown += Krug_MouseLeftButtonDown;
            krug.MouseMove += Krug_MouseMove;
            krug.MouseLeftButtonUp += Krug_MouseLeftButtonUp;
        }

        private void Krug_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            lastPosition = e.GetPosition(this);
        }

        private void Krug_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Point currentPosition = e.GetPosition(this);

                // Вычисляем разницу в координатах между текущей позицией и предыдущей позицией мыши
                double deltaX = currentPosition.X - lastPosition.X;
                double deltaY = currentPosition.Y - lastPosition.Y;

                // Вычисляем угол вращения в радианах с помощью арктангенса
                double angleRad = Math.Atan2(deltaY, deltaX);

                // Преобразуем угол в градусы
                double angleDeg = angleRad * (180 / Math.PI);

                // Применяем вращение к циферблату
                dialRotationAngle += angleDeg;
                RotateDial(dialRotationAngle);

                // Обновляем номер на циферблате
                UpdateNumberOnDial();

                // Обновляем позицию мыши
                lastPosition = currentPosition;
            }
        }

        private void Krug_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void RotateDial(double angle)
        {
            ((RotateTransform)krug.RenderTransform).Angle = angle;
        }

        private void UpdateNumberOnDial()
        {
            // Пересчитываем номер на циферблате на основе угла вращения
            double angle = dialRotationAngle % 360;
            if (angle < 0)
                angle += 360;

            // Приводим угол к диапазону от 0 до 100
            if (angle < 0)
                angle += 360;
            else if (angle > 360)
                angle -= 360;

            // Рассчитываем номер на циферблате в диапазоне от 0 до 99
            int number = (int)(angle / 3.6); // 360 градусов / 100 делений = 3.6 градуса на одно деление

            // Обновляем текстовый блок с номером
            number1.Text = "Number: " + number.ToString();
        }
    }
}
