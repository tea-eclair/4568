using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace NoteApp
{
    public class Note : INotifyPropertyChanged
    {
        private string _title;
        private string _content;
        private DateTime _creationDate;
        private string _imagePath;
        private string _imageData; // Строка Base64 для хранения изображения
        private string _imageBase64;
        private double _imageLeft;
        private double _imageTop;
         private Canvas _canvas;

    public Canvas NoteCanvas
    {
        get => _canvas;
        set
        {
            _canvas = value;
            OnPropertyChanged();
        }
    }
    public double ImageLeft
    {
        get => _imageLeft;
        set
        {
            _imageLeft = value;
            OnPropertyChanged();
        }
    }

    public double ImageTop
    {
        get => _imageTop;
        set
        {
            _imageTop = value;
            OnPropertyChanged();
        }
    }

        
        public string ImageBase64
        {
            get => _imageBase64;
            set
            {
                _imageBase64 = value;
                OnPropertyChanged();
            }
        }
        
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public string Content
        {
            get => _content;
            set
            {
                _content = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreationDate
        {
            get => _creationDate;
            set
            {
                _creationDate = value;
                OnPropertyChanged();
            }
        }

        public string ImagePath
        {
            get => _imagePath;
            set
            {
                _imagePath = value;
                OnPropertyChanged();
            }
        }

         public string ImageData
    {
        get => _imageData;
        set
        {
            _imageData = value;
            OnPropertyChanged();
        }
    }

        public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
    }

    public partial class MainWindow : Window
    {
        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();
        private Note _selectedNote; // Переменная для хранения текущей выбранной заметки
         private string ConvertImageToBase64(BitmapImage bitmapImage)
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BitmapEncoder encoder = new PngBitmapEncoder(); // Выберите соответствующий тип кодировщика в зависимости от типа изображения
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));
            encoder.Save(ms);
            return Convert.ToBase64String(ms.ToArray());
        }
    }

        public MainWindow()
        {
            InitializeComponent();
            notesList.ItemsSource = _notes;
           contentTextBox.TextChanged += ContentTextBox_TextChanged;  // Добавляем обработчик событий при изменении выбора в списке
        }

         private void ContentTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        // Проверяем, есть ли выбранная заметка
        if (notesList.SelectedItem is Note selectedNote)
        {
            // Обновляем содержимое выбранной заметки
            selectedNote.Content = contentTextBox.Text;
        }
    }

    private BitmapImage ConvertBase64ToImage(string base64String)
{
    byte[] imageBytes = Convert.FromBase64String(base64String);
    using (MemoryStream ms = new MemoryStream(imageBytes))
    {
        BitmapImage bitmapImage = new BitmapImage();
        bitmapImage.BeginInit();
        bitmapImage.StreamSource = ms;
        bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
        bitmapImage.EndInit();
        return bitmapImage;
    }
}


        private void notesList_SelectionChanged(object sender, SelectionChangedEventArgs e)
{
    if (notesList.SelectedItem != null)
    {
        _selectedNote = notesList.SelectedItem as Note;
        contentTextBox.IsEnabled = true;
        contentTextBox.Text = _selectedNote.Content;

        // Показываем сохраненные изображения при выборе заметки
        contentCanvas.Children.Clear(); // Очищаем предыдущие элементы

        if (_selectedNote.NoteCanvas == null)
        {
            _selectedNote.NoteCanvas = new Canvas(); // Создаем новый канвас для заметки
        }

        // Восстанавливаем сохраненные изображения на канвасе заметки
        foreach (var child in _selectedNote.NoteCanvas.Children)
        {
            if (child is Image image)
            {
                _selectedNote.NoteCanvas.Children.Add(image); // Добавляем изображение на канвас заметки

                // Добавляем обработчики событий для перемещения изображения
                image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
                image.MouseMove += Image_MouseMove;
                image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
            }
        }

        // Добавляем канвас заметки в основной канвас
        contentCanvas.Children.Add(_selectedNote.NoteCanvas);
    }
    else
    {
        _selectedNote = null;
        contentTextBox.IsEnabled = false;
        contentTextBox.Text = "";
        contentCanvas.Children.Clear(); // Очищаем Canvas, если заметка не выбрана
    }
}





        private void NewNote_Click(object sender, RoutedEventArgs e)
{
    // Создаем уникальное имя на основе текущей даты и времени
    // string newTitle = "Note_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");

    // Показываем диалоговое окно для ввода названия заметки
    string newTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter the title for the new note:", "New Note", "Note_" + DateTime.Now.ToString("yyyyMMdd_HHmmss"));

  
        // Создаем новую заметку с уникальным именем

        var newNote = new Note
        {
            Title = newTitle,
            Content = "", // Пока заметка пустая
            CreationDate = DateTime.Now,

        };
        
        _notes.Add(newNote);
        notesList.SelectedItem = newNote;
        contentTextBox.IsReadOnly = false; // Включаем редактирование текстбокса
        contentTextBox.Text = ""; // Очищаем текстбокс

}


        private void OpenNote_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                var content = File.ReadAllText(openFileDialog.FileName);
                var newNote = new Note
                {
                    Title = Path.GetFileNameWithoutExtension(openFileDialog.FileName),
                    Content = content,
                    CreationDate = File.GetCreationTime(openFileDialog.FileName)
                };
                _notes.Add(newNote);
                notesList.SelectedItem = newNote;
                contentTextBox.Text = newNote.Content; // Устанавливаем текст из заметки в TextBox
                contentTextBox.IsReadOnly = false; // Устанавливаем IsReadOnly на false для редактирования содержимого
            }
        }
        


        private void EditTitle_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = notesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                string newTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter new title:", "Rename", selectedNote.Title);
                if (!string.IsNullOrEmpty(newTitle))
                {
                    selectedNote.Title = newTitle;
                    selectedNote.Content = contentTextBox.Text; // Сохраняем текст из TextBox в текущей заметке
                }
            }
        }


        private void notesList_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            ListBox listBox = sender as ListBox;
            if (listBox != null)
            {
                // Установка выбранного элемента списка при щелчке правой кнопкой мыши
                listBox.SelectedItem = (e.OriginalSource as FrameworkElement)?.DataContext;
            }
        }

        

        private void Image_Click(object sender, RoutedEventArgs e)
{
    var selectedNote = notesList.SelectedItem as Note;
    if (selectedNote != null)
    {
        Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
        dlg.DefaultExt = ".jpg";
        dlg.Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

        if (dlg.ShowDialog() == true)
        {
            BitmapImage bitmap = new BitmapImage(new Uri(dlg.FileName));

            // Создаем канвас для заметки, если он еще не создан
            if (selectedNote.NoteCanvas == null)
            {
                selectedNote.NoteCanvas = new Canvas();
                contentCanvas.Children.Add(selectedNote.NoteCanvas);
            }

            // Сохраняем данные о картинке и ее положении в заметке
            selectedNote.ImageBase64 = ConvertImageToBase64(bitmap);
            selectedNote.ImagePath = dlg.FileName;
            selectedNote.ImageLeft = 0; // Начальное положение по горизонтали
            selectedNote.ImageTop = 0; // Начальное положение по вертикали

            // Создаем и добавляем изображение на канвас заметки
            Image image = new Image();
            image.Source = bitmap;
            image.Width = 100;
            image.Height = 100;
            Canvas.SetLeft(image, selectedNote.ImageLeft);
            Canvas.SetTop(image, selectedNote.ImageTop);
            selectedNote.NoteCanvas.Children.Add(image); // Добавляем изображение на канвас заметки

            // Добавляем обработчики событий для перемещения изображения
            image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
            image.MouseMove += Image_MouseMove;
            image.MouseLeftButtonUp += Image_MouseLeftButtonUp;
        }
    }
    else
    {
        MessageBox.Show("Сначала создайте новую заметку.");
    }
}





//  // Обработчики событий для перемещения изображения
//                     image.MouseLeftButtonDown += Image_MouseLeftButtonDown;
//                     image.MouseMove += Image_MouseMove;
//                     image.MouseLeftButtonUp += Image_MouseLeftButtonUp;



        // Переменные для перемещения изображения
        private bool isDragging = false;
        private Point startPoint;

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isDragging = true;
            startPoint = e.GetPosition(contentCanvas);
        }

        private void Image_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging)
            {
                Image image = sender as Image;
                if (image != null)
                {
                    Point currentPoint = e.GetPosition(contentCanvas);
                    double offsetX = currentPoint.X - startPoint.X;
                    double offsetY = currentPoint.Y - startPoint.Y;

                    Canvas.SetLeft(image, Canvas.GetLeft(image) + offsetX);
                    Canvas.SetTop(image, Canvas.GetTop(image) + offsetY);

                    startPoint = currentPoint;
                }
            }
        }

        private void Image_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isDragging = false;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            // Создаем новое пустое окно
            Window helpWindow = new Window();

            // Устанавливаем размеры и заголовок окна
            helpWindow.Width = 1000;
            helpWindow.Height = 800;
            helpWindow.Title = "Помощь";

            // Создаем текстовый блок с руководством
            TextBlock guideText = new TextBlock();
            guideText.Height = 1500;


            Run run = new Run();
            run.Text = "Введение\n";
            run.FontSize = 16;
            run.FontWeight = FontWeights.Bold;
            guideText.Inlines.Add(run);
            Run run2 = new Run();
            run2.Text = "Приложение \"Note App\" представляет собой удобный инструмент для создания, сохранения и управления заметками.\n " +
                "Оно позволяет пользователям создавать новые заметки, открывать существующие заметки из текстовых файлов, редактировать и сохранять их.\n " +
                "Кроме того, приложение поддерживает добавление изображений в заметки и их перемещение внутри текстового блока.\n" +
                "\n \n ";
            run2.FontWeight = FontWeights.Regular;
            guideText.Inlines.Add(run2);

            Run run3 = new Run();
            run3.Text = "Использование\n\n";
            run3.FontSize = 16;
            run3.FontWeight = FontWeights.Bold;
            guideText.Inlines.Add(run3);
            Run run4 = new Run();
            run4.Text = "1.Создание новой заметки\r\n" +
                "2.Запустите приложение \"Note App\".\r\n" +
                "3.Нажмите кнопку \"New\", чтобы создать новую заметку.\r\n" +
                "4.Введите заголовок для вашей заметки.\r\n" +
                "5.Введите текст вашей заметки в текстовом поле.\r\n" +
                 "\n\n ";
            run4.FontWeight = FontWeights.Regular;
            guideText.Inlines.Add(run4);

            Image image = new Image();
            string imagePath = "NewButt.jpeg";
            ImageSource imageSource = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            image.Source = imageSource;
            image.Width = 100;
            image.Height = 100;
            guideText.Inlines.Add(image);

            Image image2 = new Image();
            string imagePath2 = "Rename.jpeg";
            ImageSource imageSource2 = new BitmapImage(new Uri(imagePath2, UriKind.RelativeOrAbsolute));
            image2.Source = imageSource2;
            image2.Width = 200;
            image2.Height = 200;
            guideText.Inlines.Add(image2);

            Image image3 = new Image();
            string imagePath3 = "Edit.jpeg";
            ImageSource imageSource3 = new BitmapImage(new Uri(imagePath3, UriKind.RelativeOrAbsolute));
            image3.Source = imageSource3;
            image3.Width = 200;
            image3.Height = 200;
            guideText.Inlines.Add(image3);

            Run run5 = new Run();
            run5.Text = "\nОткрытие и редактирование существующей заметки \n\n";
            run5.FontSize = 16;
            run5.FontWeight = FontWeights.Bold;
            guideText.Inlines.Add(run5);
            Run run6 = new Run();
            run6.Text = "1.Запустите приложение \"Note App\".\r\n" +
                "2.Нажмите кнопку \"Open\", чтобы выбрать существующий файл с заметкой.\r\n" +
                "3.Выберите файл с заметкой из диалогового окна.\r\n" +
                "4.Редактируйте текст заметки в текстовом поле по вашему усмотрению.\r\n" +
                "\n\n";
            run6.FontWeight = FontWeights.Regular;
            guideText.Inlines.Add(run6);

            Image image4 = new Image();
            string imagePath4 = "OpenButt.jpeg";
            ImageSource imageSource4 = new BitmapImage(new Uri(imagePath4, UriKind.RelativeOrAbsolute));
            image4.Source = imageSource4;
            image4.Width = 100;
            image4.Height = 100;
            guideText.Inlines.Add(image4);

            Run run7 = new Run();
            run7.Text = "\nДобавление изображений в заметку \n\n";
            run7.FontSize = 16;
            run7.FontWeight = FontWeights.Bold;
            guideText.Inlines.Add(run7);
            Run run8 = new Run();
            run8.Text = "1.Запустите приложение \"Note App\".\r\n" +
                "2.Выберите существующую заметку или создайте новую.\r\n" +
                "3.Нажмите кнопку \"Image\", чтобы выбрать изображение для добавления.\r\n" +
                "4.Выберите изображение из файловой системы.\r\n" +
                "5.Изображение будет добавлено в текстовое поле заметки и отображено в нем.\r\n" +
                "\n\n";
            run8.FontWeight = FontWeights.Regular;
            guideText.Inlines.Add(run8);
            Run run9 = new Run();
            run9.Text = "Сохранение и загрузка заметок\n\n";
            run9.FontSize = 16;
            run9.FontWeight = FontWeights.Bold;
            guideText.Inlines.Add(run9);
            Run run10 = new Run();
            run10.Text = "Все созданные заметки сохраняются в файловой системе вашего компьютера. Вы можете загрузить заметки из текстовых файлов,\r\n" +
                " а также сохранить их в файлы для последующего использования.\r\n" +
                "\n\n";
            run10.FontWeight = FontWeights.Regular;
            guideText.Inlines.Add(run10);
            Run run11 = new Run();
            run11.Text = "Заключение\n\n";
            run11.FontSize = 16;
            run11.FontWeight = FontWeights.Bold;
            guideText.Inlines.Add(run11);
            Run run12 = new Run();
            run12.Text = "Приложение \"Note App\" предоставляет простой и эффективный способ создания и управления заметками.\r\n" +
                "Оно предоставляет удобный интерфейс для работы с текстом и изображениями, что делает его идеальным инструментом для организации вашей информации и идей.\r\n";
            run12.FontWeight = FontWeights.Regular;
            guideText.Inlines.Add(run12);


            Image image1 = new Image();
            string imagePath1 = "tigro-3.jpg";
            ImageSource imageSource1 = new BitmapImage(new Uri(imagePath, UriKind.RelativeOrAbsolute));
            image1.Source = imageSource;
            image1.Width = 100;
            image1.Height = 100;
            guideText.Inlines.Add(image1);


            ScrollViewer scrollViewer = new ScrollViewer();
            scrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            scrollViewer.Content = guideText;

            // Добавляем скроллер в окно
            helpWindow.Content = scrollViewer;

            // Отображаем новое окно
            helpWindow.Show();

        }
    }}