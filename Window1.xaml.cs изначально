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



namespace NoteApp
{
    public class Note : INotifyPropertyChanged
    {
        private string _title;
        private string _content;
        private DateTime _creationDate;
        private string _imagePath;

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

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public partial class MainWindow : Window
    {
        private ObservableCollection<Note> _notes = new ObservableCollection<Note>();

        public MainWindow()
        {
            InitializeComponent();
            notesList.ItemsSource = _notes;
        }

        private void NewNote_Click(object sender, RoutedEventArgs e) // создает новую заметку и добавляет ее в список заметок.
        {
            var newNote = new Note
            {
                Title = "New Note",
                CreationDate = DateTime.Now
            };
            _notes.Add(newNote);
            notesList.SelectedItem = newNote;
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e) //сохраняет выбранную заметку в текстовый файл.
        {
            var selectedNote = notesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                var saveFileDialog = new SaveFileDialog
                {
                    Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*",
                    DefaultExt = ".txt"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    File.WriteAllText(saveFileDialog.FileName, selectedNote.Content);
                }
            }
        }

        private void OpenNote_Click(object sender, RoutedEventArgs e) //открывает текстовый файл с заметкой и добавляет его в список заметок.
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

private void EditTitle_Click(object sender, RoutedEventArgs e)
{
    var selectedNote = notesList.SelectedItem as Note;
    if (selectedNote != null)
    {
        string newTitle = Microsoft.VisualBasic.Interaction.InputBox("Enter new title:", "Rename", selectedNote.Title);
        if (!string.IsNullOrEmpty(newTitle))
        {
            selectedNote.Title = newTitle;
        }
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
                            Image image = new Image();
                            BitmapImage bitmap = new BitmapImage(new Uri(dlg.FileName));
                            image.Source = bitmap;
                            image.Width = 100;
                            image.Height = 100;
                            SlideCanvas.Children.Add(image);
                            Canvas.SetLeft(image, 0);
                            Canvas.SetTop(image, 0);
                            Panel.SetZIndex(image, SlideCanvas.Children.Count); 
                        }
                    }
                    else
                    {
                        MessageBox.Show("Сначала создайте новую заметку.");
                    }
                }




    }
}
