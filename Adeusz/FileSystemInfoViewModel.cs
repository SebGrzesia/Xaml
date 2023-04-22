using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1Wnorowska
{
    public class FileSystemInfoViewModel : ViewModelBase
    {
        private Dictionary<string, string> _images = new Dictionary<string, string>(){
            {".avi",   "Icons/avi.png"},
            {".bmp",   "Icons/bmp.png"},
            {".css",   "Icons/css.png"},
            {".dll",   "Icons/dll.png"},
            {".doc",   "Icons/doc.png"},
            {".gif",   "Icons/gif.png"},
            {".html",   "Icons/html.png"},
            {".jpg",   "Icons/jpg.png"},
            {".js",   "Icons/js.png"},
            {".mp3",   "Icons/mp3.png"},
            {".pdf",   "Icons/pdf.png"},
            {".php",   "Icons/php.png"},
            {".png",   "Icons/png.png"},
            {".ppt",   "Icons/ppt.png"},
            {".svg",   "Icons/svg.png"},
            {".txt",   "Icons/txt.png"},
            {".ui",   "Icons/ui.png"},
            {".xls",   "Icons/xls.png"},
            {".xml",   "Icons/xml.png"},
            {".zip",   "Icons/zip.png"}
        };
        public DateTime LastWriteTime
        {
            get { return _lastWriteTime; }
            set
            {
                if (_lastWriteTime != value)
                {
                    _lastWriteTime = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Caption
        {
            get { return _caption; }
            set
            {
                if (_caption != value)
                {
                    _caption = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public string Image
        {
            get { return _image; }
            set
            {
                if (_image != value)
                {
                    _image = value;
                    NotifyPropertyChanged();
                }
            }
        }
        public FileSystemInfo Model
        {
            get { return _fileSystemInfo; }
            set
            {
                if (_fileSystemInfo != value)
                {
                    _fileSystemInfo = value;
                    //…
                    this.LastWriteTime = value.LastWriteTime;
                    this.Caption = value.Name;
                    if (_images.ContainsKey(value.Extension))
                    {
                        this.Image = _images[value.Extension];
                    }
                    else
                    {
                        this.Image = "Icons/error.png";
                    }
                    //…
                    NotifyPropertyChanged();
                }
            }
        }
        private DateTime _lastWriteTime;
        private string _caption;
        private FileSystemInfo _fileSystemInfo;
        private string _image;
    }
}

