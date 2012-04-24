using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Windows.Media.Imaging;
using System.IO.IsolatedStorage;
using System.IO;
using Microsoft.Phone;

namespace NoraPic.Model
{
    public class NpDbContext : DataContext
    {
        // Pass the connection string to the base class.
        public NpDbContext(string connectionString)
            : base(connectionString)
        { }

        // Specify a table for the images.
        public Table<ImageItem> NPImages;
    }

    [Table]
    public class ImageItem : INotifyPropertyChanged, INotifyPropertyChanging
    {
        const string imageFolder = "Images";
        const string thumbFolder = "Thumbnails";

        // Define Image Name, Used as ID. Private field, public property and DB Column.
        private string _ImageName;

        [Column(Storage = "_ImageName", DbType = "NVarChar(100) NOT NULL", CanBeNull = false, IsPrimaryKey = true, AutoSync = AutoSync.OnInsert)]
        public string ImageName
        {
            get { return _ImageName; }
            set
            {
                if (_ImageName != value)
                {
                    NotifyPropertyChanging("ImageName");
                    _ImageName = value;
                    NotifyPropertyChanged("ImageName");
                }
            }
        }

        // Define if image has been uploaded to skydrive: private field, public property, and database column.
        private bool _IsSync;

        [Column(Storage="_IsSync", DbType="Bit NOT NULL")]
        public bool IsSync
        {
            get { return _IsSync; }
            set
            {
                if (_IsSync != value)
                {
                    NotifyPropertyChanging("IsSync");
                    _IsSync = value;
                    NotifyPropertyChanged("IsSync");
                }
            }
        }

        // Define if image has been downloaded from skydrive: private field, public property, and database column.
        private bool _IsCached;

        [Column(Storage="_IsCached", DbType="Bit NOT NULL")]
        public bool IsCached
        {
            get { return _IsCached; }
            set
            {
                if (_IsCached != value)
                {
                    NotifyPropertyChanging("IsCached");
                    _IsCached = value;
                    NotifyPropertyChanged("IsCached");
                }
            }
        }

        // Define if image is marked as favourite: private field, public property, and database column.
        private Nullable<bool> _IsFav;

        [Column(Storage="_IsFav", DbType="Bit")]
        public Nullable<bool> IsFav
        {
            get { return _IsFav; }
            set
            {
                if (_IsFav != value)
                {
                    NotifyPropertyChanging("IsFav");
                    _IsFav = value;
                    NotifyPropertyChanged("IsFav");
                }
            }
        }

        // Define date/time pic was taken: private field, public property, and database column.
        private DateTime _DateTaken;

        [Column(Storage="_DateTaken", DbType="DateTime NOT NULL")]
		public DateTime DateTaken
		{
			get { return this._DateTaken; }
			set
			{
				if (_DateTaken != value)
				{
                    NotifyPropertyChanging("DateTaken");
                    _DateTaken = value;
                    NotifyPropertyChanged("DateTaken");
				}
			}
		}

        // Define lat location pic was taken: private field, public property, and database column.
        private Nullable<double> _Latitude;

        [Column(Storage="_Latitude", DbType="Float")]
		public Nullable<double> Latitude
		{
			get { return this._Latitude; }
			set
			{
				if ((this._Latitude != value))
				{
                    NotifyPropertyChanging("Latitude");
                    _Latitude = value;
                    NotifyPropertyChanged("Latitude");
				}
			}
		}

        // Define long location pic was taken: private field, public property, and database column.
        private Nullable<double> _Longitude;
        
        [Column(Storage="_Longitude", DbType="Float")]
		public Nullable<double> Longitude
		{
			get { return this._Longitude; }
			set
			{
				if ((this._Longitude != value))
				{
                    NotifyPropertyChanging("Longitude");
                    _Longitude = value;
                    NotifyPropertyChanged("Longitude");
				}
			}
		}

        // Define user comment: private field, public property, and database column.
        private string _Comments;

        [Column(Storage="_Comments", DbType="NVarChar(1000)")]
        public string Comments
        {
            get { return _Comments; }
            set
            {
                if (_Comments != value)
                {
                    NotifyPropertyChanging("Comments");
                    _Comments = value;
                    NotifyPropertyChanged("Comments");
                }
            }
        }

        // Version column aids update performance.
        [Column(IsVersion = true)]
        private Binary _version;

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify that a property changed
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        #region INotifyPropertyChanging Members

        public event PropertyChangingEventHandler PropertyChanging;

        // Used to notify that a property is about to change
        private void NotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
            {
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
            }
        }

        #endregion

        #region Extensibility Method Definitions
        private void OnLoaded(){}
        private void OnValidate(ChangeAction action){}
        private void OnCreated(){}
        #endregion

        // Image URI
        public string imageUri { get; set; }
        public string thumbUri { get; set; }

        // Storage methods
        # region Methods dealing with ISO Storage

        public void SaveImage(Stream image, Stream thumbnail)
        {
            CreateURIs();

            string imgPath = imageFolder + System.IO.Path.DirectorySeparatorChar.ToString() + ImageName;
            string thumbPath = thumbFolder + System.IO.Path.DirectorySeparatorChar.ToString() + ImageName; 

            BitmapImage imgBitmap = new BitmapImage();
            imgBitmap.SetSource(image);
            WriteableBitmap capedImage = new WriteableBitmap(imgBitmap);

            BitmapImage thumbBitmap = new BitmapImage();
            thumbBitmap.SetSource(thumbnail);
            WriteableBitmap capedThumb = new WriteableBitmap(thumbBitmap);

            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!string.IsNullOrEmpty(imageFolder) && !appStore.DirectoryExists(imageFolder))
                {
                    appStore.CreateDirectory(imageFolder);
                    appStore.CreateDirectory(thumbFolder);
                }  

                using (IsolatedStorageFileStream imgStream = appStore.CreateFile(imgPath))
                {
                    capedImage.SaveJpeg(imgStream, capedImage.PixelWidth, capedImage.PixelHeight, 0, 100);
                    imgStream.Close();
                }

                using (IsolatedStorageFileStream thumbStream = appStore.CreateFile(thumbPath))
                {
                    capedThumb.SaveJpeg(thumbStream, capedThumb.PixelWidth, capedThumb.PixelHeight, 0, 100);
                    thumbStream.Close();
                }
            }
        }

        public void CreateURIs()
        {
            /**
            imageUri = String.Format("/{0}/{1}", imageFolder, ImageName);
            thumbUri = String.Format("/{0}/{1}", thumbFolder, ImageName);
            **/
            imageUri = imageFolder + System.IO.Path.DirectorySeparatorChar.ToString() + ImageName;
            thumbUri = thumbFolder + System.IO.Path.DirectorySeparatorChar.ToString() + ImageName; 
        }

        public WriteableBitmap LoadImage()
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStore.FileExists(this.ImageName))
                {
                    using (IsolatedStorageFileStream fileStream = appStore.OpenFile(this.ImageName, FileMode.Open))
                    {
                        return PictureDecoder.DecodeJpeg(fileStream);
                    }
                }
                else
                {
                    return null;
                }
            }
        }

        public void DeleteContent()
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
                appStore.DeleteFile(this.ImageName);
        }
        # endregion 

		public ImageItem()
		{
			OnCreated();
		}
    }
}
