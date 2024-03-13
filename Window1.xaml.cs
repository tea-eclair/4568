using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;

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

        private void NewNote_Click(object sender, RoutedEventArgs e)
        {
            var newNote = new Note
            {
                Title = "New Note",
                CreationDate = DateTime.Now
            };
            _notes.Add(newNote);
            notesList.SelectedItem = newNote;
        }

        private void SaveNote_Click(object sender, RoutedEventArgs e)
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
                    File.WriteAllText(saveFileDialog.FileName, contentTextBox.Text);
                }
            }
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
            }
        }


        private void Image_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = notesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                var openFileDialog = new OpenFileDialog
                {
                    Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp",
                    Title = "Select an Image File"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    selectedNote.ImagePath = openFileDialog.FileName;
                }
            }
        }

        private void NotesList_SelectionChanged(object sender, RoutedEventArgs e)
        {
            if (notesList.SelectedItem != null)
            {
                titleTextBox.Visibility = Visibility.Visible;
                titleTextBox.Focus();
                titleTextBox.SelectAll();
            }
        }

        private void TitleTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            titleTextBox.Visibility = Visibility.Collapsed;
            var selectedNote = notesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                selectedNote.Title = titleTextBox.Text;
            }
        }

        private void RenameMenuItem_Click(object sender, RoutedEventArgs e)
        {
            var selectedNote = notesList.SelectedItem as Note;
            if (selectedNote != null)
            {
                selectedNote.Title = titleTextBox.Text;
                titleTextBox.Visibility = Visibility.Collapsed;
            }
        }

        private void NoteTitle_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                var listItem = (FrameworkElement)sender;
                var selectedNote = listItem.DataContext as Note;

                if (selectedNote != null)
                {
                    selectedNote.Title = ((TextBlock)sender).Text;

                    titleTextBox.DataContext = selectedNote;
                    titleTextBox.Visibility = Visibility.Visible;
                    titleTextBox.Focus();
                    titleTextBox.SelectAll();
                }
            }
        }

    }
}
