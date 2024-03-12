using System.Data;
using System.DirectoryServices;
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

namespace safee
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
public partial class MainWindow : Window;


    private int password = 99;
    private int result = 0;

    private Point startPoint;
    private bool isDragging = false;

        public MainWindow()
        {
            InitializeComponent();

            krug.MouseLeftbuttonDown += Krug_MouseLeftButtonDown;
            krug.MouseMove =+ Krug_MouseMove;
            krug.MouseLeftbuttonUp += Krug_MouseLeftButtonUp;
        }

        private void Krug_MouseLeftButtonDown (object sender, MouseButtonEventArgs e)
    {
        startPoint = new Point (Canvas.Getleft(this) + this.ActualWidth /2, Canvas.GetTop(this) + this.ActualHeight /2);
        isDragging = true;
    }

    private void Krug_MouseMove(object sender, MouseEventArgs e)

    {
        if (isDragging)
        {
            Point currentPoint = e.GetPosition (MainCanvas);
            Vector delta = currentPoint - startPoint;
            double angle = Math.Atan2(delta.Y, delta.X) * (180 / Math.PI);
            angle = (angle + 360) % 360;
            double mappedNumber = Map(angle, 0, 360, 0, 99);
            result= Int32.Parse(mappedNumber.ToString("0"));
            ((RotateTransform)krug.RenderTransform).Angle = angle;
    }
}

private void Button_Click(object sender, RoutedEventArgs e){
    if (Int32.TryParse(mynumber.Text, out int numValue)){
        password = numValue;
        mynumber.Text = "Пароль задан. Угадайте: " + password;
        MessageBox.Show("Введите число");
    }
}

private void Krug_MouseLeftButtonUp (object sender, MouseButtonEventArgs e)
{
    isDragging = false;
    var checker = PasswordCheck(password, resulr);
    if (checker){
    MessageBox.Show("Все правильно!");    
    }
    else{
        MessageBox.Show("Не правильно. Вы ввели:" + result.ToString());
    }
}

private bool PasswordCheck(int a, int b){
    return a == b;
}
}
