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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Windows.Themes;
using System.Globalization;
using System.Collections.ObjectModel;

namespace Lab1Wnorowska
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<ItemsControl> Items { get; set; }
        //private string currentRoot;

        public MainWindow()
        {
            Items = new ObservableCollection<ItemsControl>();
            //DataContext = new MainWindowViewModel();
            InitializeComponent();
            var _fileExplorer = new FileExplorer();
            DataContext = _fileExplorer;
            _fileExplorer.PropertyChanged += Property_Changed;
        }

        private void Property_Changed(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FileExplorer.Lang))
                CultureResources.ChangeCulture(CultureInfo.CurrentUICulture);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog()
            {
                Description = "Select directory to open",
            };

            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MainTreeView.ItemsSource = Items;
                Items.Clear();

                string fileName = dlg.SelectedPath.Substring(dlg.SelectedPath.LastIndexOf('\\') + 1);

                //currentRoot = dlg.SelectedPath;
                //var fileExplorer = new FileExplorer();
                //fileExplorer.OpenRoot(currentRoot);
                //DataContext = fileExplorer;

                //Console.WriteLine("filename: " + fileName);
                //Console.WriteLine("dlgpath: " + dlg.SelectedPath);

                var rootTreeView = new TreeViewItem()
                {
                    Header = fileName,
                    Tag = dlg.SelectedPath,
                    IsExpanded = true
                };

                var contextMenu = new ContextMenu();

                var createFileMenuItem = new MenuItem()
                {
                    Header = "_Create",

                };
                createFileMenuItem.Click += createFile;

                contextMenu.Items.Add(createFileMenuItem);

                var openFileMenuItem = new MenuItem()
                {
                    Header = "_Delete",

                };

                openFileMenuItem.Click += deleteFolder;
                contextMenu.Items.Add(openFileMenuItem);
                rootTreeView.ContextMenu = contextMenu;

                LoadFolder(rootTreeView);
                Items.Add(rootTreeView);
            }
        }

        private void LoadFolder(TreeViewItem root)
        {
            var rootFolderPAth = (root.Tag as string)!;

            List<string> listOfAllFilePaths = Directory.GetFiles(rootFolderPAth).ToList();
            List<string> listOfAllFolderPaths = Directory.GetDirectories(rootFolderPAth).ToList();

            foreach (string folderPath in listOfAllFolderPaths)
            {
                var currentFolderRootTreeView = ConvertFolderPathToTreeViewItem(folderPath);

                LoadFolder(currentFolderRootTreeView);

                root.Items.Add(currentFolderRootTreeView);
            }

            foreach (string filePath in listOfAllFilePaths)
            {
                var fileTreeViewItem = ConvertFilePathToTreeViewItem(filePath);

                root.Items.Add(fileTreeViewItem);
            }
        }
     
        private TreeViewItem ConvertFolderPathToTreeViewItem(string folderPath)
        {
            string fileName = GetFileNameFromPath(folderPath);

            var folderTreeViewItem = new TreeViewItem()
            {
                Header = fileName,
                Tag = folderPath,
                IsExpanded = false
            };

            var contextMenu = new ContextMenu();

            var createFileMenuItem = new MenuItem()
            {
                Header = "_Create",

            };
            createFileMenuItem.Click += createFile;

            contextMenu.Items.Add(createFileMenuItem);

            var openFileMenuItem = new MenuItem()
            {
                Header = "_Delete",

            };
            openFileMenuItem.Click += deleteFolder;

            contextMenu.Items.Add(openFileMenuItem);

            folderTreeViewItem.ContextMenu = contextMenu;

            return folderTreeViewItem;
        }

        private void createFile(object sender, RoutedEventArgs e)
        {
            if(MainTreeView.SelectedItem == null)
            {
                return;
            }

            var selectedTreeItem = (MainTreeView.SelectedItem as TreeViewItem)!;
            var path = (selectedTreeItem.Tag as string)!;

            var window = new CreateNewFileWindow(path);

            var result = window.ShowDialog();

            if (result == true)
            {
                TreeViewItem newTreeItem;
                if (ModalValues.File == true)
                {
                    newTreeItem = ConvertFilePathToTreeViewItem(ModalValues.FilePath);
                } else // it is a directory
                {
                    newTreeItem = ConvertFolderPathToTreeViewItem(ModalValues.FilePath);
                }
                selectedTreeItem.Items.Add(newTreeItem);
            }
            else
            {
                //file or directory was not created
            }

        }

        private TreeViewItem ConvertFilePathToTreeViewItem(string filePath)
        {
            string fileName = GetFileNameFromPath(filePath);

            TreeViewItem item = new TreeViewItem()
            {
                Header = fileName,
                Tag = filePath
            };

            var contextMenu = new ContextMenu();

            if (fileName.EndsWith("txt"))
            {
                var openFileMenuItem = new MenuItem()
                {
                    Header = "_Open",

                };
                openFileMenuItem.Click += openTextFile;

                contextMenu.Items.Add(openFileMenuItem);
            }

            var removeFileMenuItem = new MenuItem()
            {
                Header = "_Delete",

            };
            removeFileMenuItem.Click += deleteFile;

            contextMenu.Items.Add(removeFileMenuItem);
            item.ContextMenu = contextMenu;

            return item;
        }

        private string GetFileNameFromPath(string path)
        {
            return path.Substring(path.LastIndexOf('\\') + 1);
        }

        private void openTextFile(object sender, RoutedEventArgs e)
        {
            if (MainTreeView.SelectedItem == null)
            {
                return;
            }

            var selectedTreeItem = (MainTreeView.SelectedItem as TreeViewItem)!;
            var filePath = (selectedTreeItem.Tag as string)!;

            using (var textReader = File.OpenText(filePath))
            {
                string text = textReader.ReadToEnd();
                TextOutput.Text = text;
            }
        }

        public ItemsControl GetSelectedTreeViewItemParent(TreeViewItem item)
        {
            DependencyObject parent = VisualTreeHelper.GetParent(item);
            while (!(parent is TreeViewItem || parent is System.Windows.Controls.TreeView))
            {
                parent = VisualTreeHelper.GetParent(parent);
            }

            return parent as ItemsControl;
        }

        private void deleteFile(object sender, RoutedEventArgs e)
        {
            if (MainTreeView.SelectedItem == null)
            {
                return;
            }
            var selectedTreeItem = (MainTreeView.SelectedItem as TreeViewItem)!;

            ItemsControl parentObject = GetSelectedTreeViewItemParent(selectedTreeItem);

            var parentTreeViewItem = parentObject as TreeViewItem;

            var filePath = (selectedTreeItem.Tag as string)!;
            
            File.Delete(filePath);
            parentTreeViewItem.Items.Remove(selectedTreeItem);

            FileProperties.Text = string.Empty;
        }

        private void deleteFolder(object sender, RoutedEventArgs e)
        {
            if (MainTreeView.SelectedItem == null)
            {
                return;
            }
            var selectedTreeItem = (MainTreeView.SelectedItem as TreeViewItem)!;

            ItemsControl parentObject = GetSelectedTreeViewItemParent(selectedTreeItem);

            var parentTreeViewItem = parentObject as TreeViewItem;

            var filePath = (selectedTreeItem.Tag as string)!;

            Directory.Delete(filePath, true);

            if (parentTreeViewItem != null)
            {
                parentTreeViewItem.Items.Remove(selectedTreeItem);
            }
            else
            {
                MainTreeView.Items.Remove(selectedTreeItem);
            }

            FileProperties.Text = string.Empty;
        }

        private void MainTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            if(MainTreeView.SelectedItem == null)
            {
                return;
            }
            var selectedTreeItem = (MainTreeView.SelectedItem as TreeViewItem)!;
            var path = (selectedTreeItem.Tag as string)!;

            var fileAttributes = File.GetAttributes(path);

            string directoryType = "-";
            if((fileAttributes & FileAttributes.Directory) == FileAttributes.Directory)
            {
                directoryType = "d";
            }

            string archiveType = "-";
            if ((fileAttributes & FileAttributes.Archive) == FileAttributes.Archive)
            {
                archiveType = "a";
            }

            string readonlyType = "-";
            if ((fileAttributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
            {
                readonlyType = "r";
            }

            string hiddenType = "-";
            if ((fileAttributes & FileAttributes.Hidden) == FileAttributes.Hidden)
            {
                hiddenType = "h";
            }

            string systemType = "-";
            if ((fileAttributes & FileAttributes.System) == FileAttributes.System)
            {
                systemType = "s";
            }

            var rash = directoryType + archiveType + readonlyType + hiddenType + systemType + "-";
            FileProperties.Text = rash;
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
