using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;

namespace Lab1Wnorowska
{
    public class DirectoryInfoViewModel : FileSystemInfoViewModel
    {
        public ObservableCollection<FileSystemInfoViewModel> Items { get; private set; }
            = new ObservableCollection<FileSystemInfoViewModel>();

        public FileSystemWatcher Watcher;
        private String _path;

        public bool Open(string path)
        {
            bool result = false;
            try
            {
                foreach (var dirName in Directory.GetDirectories(path))
                {
                    var dirInfo = new DirectoryInfo(dirName);
                    DirectoryInfoViewModel itemViewModel = new DirectoryInfoViewModel();
                    itemViewModel.Model = dirInfo;
                    itemViewModel.Open(dirName);
                    Items.Add(itemViewModel);
                }
                foreach (var fileName in Directory.GetFiles(path))
                {
                    var fileInfo = new FileInfo(fileName);
                    FileInfoViewModel itemViewModel = new FileInfoViewModel();
                    itemViewModel.Model = fileInfo;
                    Items.Add(itemViewModel);
                }
                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            return result;
        }
        public Exception Exception { get; private set; }

        public bool AddWatcher(string path)
        {
            _path = path;
            bool result = false;
            try
            {
                Open(path);
                Watcher = new FileSystemWatcher(path);

                Watcher.Created += OnFileSystemChanged;
                Watcher.Renamed += OnFileSystemChanged;
                Watcher.Deleted += OnFileSystemChanged;
                Watcher.Changed += OnFileSystemChanged;
                Watcher.Error += Watcher_Error;
                Watcher.EnableRaisingEvents = true;

                result = true;
            }
            catch (Exception ex)
            {
                Exception = ex;
            }
            return result;
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() => OnFileSystemChanged(e));
        }
        private void OnFileSystemChanged(FileSystemEventArgs e)
        {
            Items.Clear();
            AddWatcher(_path);
        }
    }
}
