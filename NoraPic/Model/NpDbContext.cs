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

        // Define
        private WriteableBitmap _CapturedImage;

        public WriteableBitmap CapturedImage
        {
            get { return _CapturedImage; }
            set
            {
                if (_CapturedImage != value)
                {
                    NotifyPropertyChanging("CapturedImage");
                    _CapturedImage = value;
                    NotifyPropertyChanged("CapturedImage");
                }
            }
        }

        private WriteableBitmap _PreviewImage;

        public WriteableBitmap PreviewImage
        {
            get { return _PreviewImage; }
            set
            {
                if (_PreviewImage != value)
                {
                    NotifyPropertyChanging("PreviewImage");
                    _PreviewImage = value;
                    NotifyPropertyChanged("PreviewImage");
                }
            }
        }

        // Storage methods
        # region Methods dealing with ISO Storage

        public void SaveImage(WriteableBitmap CapedImage)
        {
            CapturedImage = CapedImage;
            //GeneratePreview();
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
            using (IsolatedStorageFileStream fileStream = appStore.CreateFile(this.ImageName))
                Extensions.SaveJpeg(CapedImage, fileStream, CapedImage.PixelWidth, CapedImage.PixelHeight, 0, 85);
        }

        public void LoadImage()
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (appStore.FileExists(this.ImageName))
                {
                    using (IsolatedStorageFileStream fileStream = appStore.OpenFile(this.ImageName, FileMode.Open))
                    {
                        CapturedImage = PictureDecoder.DecodeJpeg(fileStream);
                        //GeneratePreview();
                    }
                }
            }
        }

        public void DeleteContent()
        {
            using (IsolatedStorageFile appStore = IsolatedStorageFile.GetUserStoreForApplication())
                appStore.DeleteFile(this.ImageName);
        }
        # endregion 

        private WriteableBitmap GeneratePreview(WriteableBitmap WBImage, double maxWidth, double maxHeight)
        {
            double scaleX = 1;
            double scaleY = 1;

            if (WBImage.PixelHeight > maxHeight)
                scaleY = Math.Min(maxHeight / WBImage.PixelHeight, 1);
            if (WBImage.PixelWidth > maxWidth)
                scaleX = Math.Min(maxWidth / WBImage.PixelWidth, 1);

            // maintain aspect ratio by picking the most severe scale
            double scale = Math.Min(scaleY, scaleX);


            WriteableBitmap retImage = WBImage.Resize((int)(scale * WBImage.PixelWidth), (int)(scale * WBImage.PixelHeight), WriteableBitmapExtensions.Interpolation.Bilinear);
            MessageBox.Show("Dim: " + PreviewImage.PixelHeight + "x" + PreviewImage.PixelWidth);
            return retImage;
        }
		
		public ImageItem()
		{
			OnCreated();
		}
    }
}
