using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

// Directive of the data model
using NoraPic.Model;

namespace NoraPic.ViewModels
{
    public class NpDbViewModel : INotifyPropertyChanged
    {
        // LINQ to SQL data context for the local DB
        private NpDbContext ImageDB;

        // Constructor - creates DB object
        public NpDbViewModel(string NpDbconnectionString)
        {
            ImageDB = new NpDbContext(NpDbconnectionString);
        }

        public bool IsDataLoaded
        {
            get;
            private set;
        }

        // Main Pivot Fav items (8 max)
        private ObservableCollection<ImageItem> _AllImageItems;
        public ObservableCollection<ImageItem> AllImageItems
        {
            get { return _AllImageItems; }
            set
            {
                _AllImageItems = value;
                NotifyPropertyChanged("AllImageItems");
            }
        }

        // Main Pivot Fav items (8 max)
        private ObservableCollection<ImageItem> _AllFavImages;
        public ObservableCollection<ImageItem> AllFavImages
        {
            get { return _AllFavImages; }
            set
            {
                _AllFavImages = value;
                NotifyPropertyChanged("AllFavImages");
            }
        }

        // Main Pivot Fav items (8 max)
        private ObservableCollection<ImageItem> _MainFavImages;
        public ObservableCollection<ImageItem> MainFavImages
        {
            get { return _MainFavImages; }
            set
            {
                _MainFavImages = value;
                NotifyPropertyChanged("MainFavImages");
            }
        }

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all to-do items in the database.
            var ImagesItemsInDb = from ImageItem image in ImageDB.NPImages
                                select image;

            // Query the database and load all to-do items.
            AllImageItems = new ObservableCollection<ImageItem>(ImagesItemsInDb);

            // Query the database and load all associated items to their respective collections.
            foreach (ImageItem image in ImagesItemsInDb)
            {
                if ((bool)image.IsFav)
                {
                    AllFavImages.Add(image);
                }
            }

            // Load a list of all categories.
            MainFavImages = AllFavImages;

        }

        // Add a to-do item to the database and collections.
        public void AddImageItem(ImageItem newImageItem)
        {
            // Add a to-do item to the data context.
            ImageDB.NPImages.InsertOnSubmit(newImageItem);

            // Save changes to the database.
            ImageDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            AllImageItems.Add(newImageItem);
        }
        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        // Used to notify Silverlight that a property has changed.
        private void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        #endregion
    }
}
