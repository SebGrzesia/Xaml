using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1Wnorowska
{
    public class FileExplorer : ViewModelBase
    {
        //public DirectoryInfoViewModel Root { get; set; }

        public void OpenRoot(string path)
        {
            Root = new DirectoryInfoViewModel();
            Root.Open(path);
        }

        public string Lang
        {
            get { return CultureInfo.CurrentUICulture.TwoLetterISOLanguageName; }
            set
            {
                if (value != null)
                    if (CultureInfo.CurrentUICulture.TwoLetterISOLanguageName != value)
                    {
                        CultureInfo.CurrentUICulture = new CultureInfo(value);
                        NotifyPropertyChanged();
                    }
            }
        }
        public DirectoryInfoViewModel Root { get; set; }
        public FileSystemWatcher Watcher { get; private set; } = new FileSystemWatcher();
        public string RootPath { get; private set; }
        public FileExplorer()
        {
            Root = new DirectoryInfoViewModel();
            RootPath = string.Empty;
            NotifyPropertyChanged(nameof(this.Lang));
        }

        //public void OpenRoot(string path)
        //{
        //    Watcher = new FileSystemWatcher(path);
        //    Watcher.Created += OnFileSystemChanged;
        //    Watcher.Renamed += OnFileSystemChanged;
        //    Watcher.Deleted += OnFileSystemChanged;
        //    Watcher.Changed += OnFileSystemChanged;
        //    Watcher.Error += Watcher_Error;
        //    Watcher.EnableRaisingEvents = true;
        //    Root.Open(path);
        //    RootPath = path;
        //}
        public void OnFileSystemChanged(object sender, FileSystemEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => OnFileSystemChanged(e));
        }
        private void OnFileSystemChanged(FileSystemEventArgs e)
        {
            Root.Items.Clear();
            Root.Open(RootPath);
        }
        public void Watcher_Error(object sender, ErrorEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() => Watcher_Error(e));
        }
        private void Watcher_Error(ErrorEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
