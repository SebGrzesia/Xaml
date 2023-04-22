using System;
using System.Collections.Generic;
using System.IO;
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

namespace Lab1Wnorowska
{
    /// <summary>
    /// Interaction logic for CreateNewFileWindow.xaml
    /// </summary>
    public partial class CreateNewFileWindow : Window
    {
        private readonly string _rootPath;

        public CreateNewFileWindow(string rootPath)
        {
            _rootPath = rootPath;
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var filePath = _rootPath + $"\\{FileNameTextBox.Text}";

            if (DirectoryRadioButton.IsChecked == true)
            {
                var directory = Directory.CreateDirectory(filePath);

                if(ArchiveCheckox.IsChecked == true)
                {
                    directory.Attributes |= FileAttributes.Archive;
                }
                if (ReadOnlyCheckox.IsChecked == true)
                {
                    directory.Attributes |= FileAttributes.ReadOnly;
                }
                if (SystemCheckox.IsChecked == true)
                {
                    directory.Attributes |= FileAttributes.System;
                }
                if (ReadOnlyCheckox.IsChecked == true)
                {
                    directory.Attributes |= FileAttributes.Hidden;
                }
                ModalValues.File = false;
            }
            else if (FileRadioButton.IsChecked == true)
            {
                var file = File.Create(filePath);
                file.Dispose();

                FileAttributes attributes = File.GetAttributes(filePath);

                if (ArchiveCheckox.IsChecked == true)
                {
                    attributes |= FileAttributes.Archive;
                }
                if (ReadOnlyCheckox.IsChecked == true)
                {
                    attributes |= FileAttributes.ReadOnly;
                }
                if (SystemCheckox.IsChecked == true)
                {
                    attributes |= FileAttributes.System;
                }
                if (HiddenCheckox.IsChecked == true)
                {
                    attributes |= FileAttributes.Hidden;
                }

                File.SetAttributes(filePath, attributes);
                ModalValues.File = true;
            }

            ModalValues.FilePath = filePath;
            DialogResult = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
