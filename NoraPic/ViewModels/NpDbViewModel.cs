using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Linq;

// Directive of the data model
using NoraPic.Model;
using System.Diagnostics;
using System.Collections.Generic;
using NoraPic.Includes;
using System.Windows;

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

        public bool IsAllFavLoaded
        {
            get;
            private set;
        }

        public bool IsMainFavLoaded
        {
            get;
            private set;
        }

        public bool IsRecentLoaded
        {
            get;
            private set;
        }
       
        // Main Pivot Fav items (8 max)
        private ObservableCollection<ImagesInCategory> _AllImageItems;
        public ObservableCollection<ImagesInCategory> AllImageItems
        {
            get { return _AllImageItems; }
            set
            {
                _AllImageItems = value;
                NotifyPropertyChanged("AllImageItems");
            }
        }

        // Main Pivot Fav items (8 max)
        private ObservableCollection<string> _AllFavImages;
        public ObservableCollection<string> AllFavImages
        {
            get { return _AllFavImages; }
            set
            {
                _AllFavImages = value;
                NotifyPropertyChanged("AllFavImages");
            }
        }

        // Main Pivot Fav items (8 max)
        private ObservableCollection<string> _MainFavImages;
        public ObservableCollection<string> MainFavImages
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

            // Specify the query for images in the database and grouped by "month year".
            var ImagesItemsInDb = from ImageItem image in ImageDB.NPImages
                                  group image by image.DisplayDateTimeString;

            Debug.WriteLine("Query Create");

            AllImageItems = new ObservableCollection<ImagesInCategory>();

            // Query the database and load all images and group them.
            Deployment.Current.Dispatcher.BeginInvoke(() =>
            {
                foreach (var monthYear in ImagesItemsInDb)
                {
                    ImagesInCategory group = new ImagesInCategory(monthYear.Key);
                    if (monthYear.Count() != 0)
                    {
                        foreach (ImageItem image in monthYear)
                        {
                            image.CreateURIs();
                            group.Add(image);                           
                        }
                    }
                    AllImageItems.Add(group);
                }

            });                      

            Debug.WriteLine("Images now in Collection");
            
            this.IsDataLoaded = true;
        }

        public void LoadAllFavs()
        {
            AllFavImages = Settings.Favourites.Value;

            Debug.WriteLine("All Favs Created");

            this.IsAllFavLoaded = true;
        }

        public void LoadMainFavs()
        {
            // Load a list of the favourites on the main page.
            int AllFavLength = (AllFavImages == null) ? 0 : AllFavImages.Count;
            int MaxLength = 8;
            MainFavImages = new ObservableCollection<string>(AllFavImages.Reverse().Take(MaxLength));
            
            Debug.WriteLine("Main Favs Created");

            this.IsMainFavLoaded = true;
        }

        public void LoadRecents()
        {
            // Load a list of the recents on the main page.
            /**
            int AllImageLength = (AllImageItems == null) ? 0 : AllImageItems.Count;
            int MaxLength = 8;
            if (AllImageLength > 0)
            {
                MainFavImages = new ObservableCollection<ImageItem>(AllFavImages.Reverse().Take(MaxLength));
            }
            Debug.WriteLine("Main Favs Created");

            this.IsRecentLoaded = true;
            **/
        }

        // Add a image item to the database and collections.
        public void AddImageItem(ImageItem newImageItem)
        {        
            // Add a to-do item to the data context.
            ImageDB.NPImages.InsertOnSubmit(newImageItem);

            // Save changes to the database.
            ImageDB.SubmitChanges();

            // Add a to-do item to the "all" observable collection.
            var rawCategory = AllImageItems.Where(monthYear => monthYear.Key == newImageItem.DisplayDateTimeString);
            if (rawCategory.Count() > 0)
            {
                rawCategory.FirstOrDefault().Add(newImageItem);
            }
        
            Debug.WriteLine("Image Added");
        }

        public void AddFavImage(string newImageUri)
        {
            //Add the image to the list of all favourites
            //Settings.Favourites.Value.Add(newImageUri);
            AllFavImages.Add(newImageUri);

            //Update the list of main faorites to have the latest added favourites
            // remove the last one and add in the new one
            int MainFavLength = (MainFavImages == null) ? 0 : MainFavImages.Count;
            int MaxLength = 8;
            if (MainFavLength >= MaxLength)
                MainFavImages.Remove(MainFavImages.Last());

            MainFavImages.Insert(0, newImageUri);

            Debug.WriteLine("Favourites Updated");
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

        #region oldCode
        /**
        public void LoadAllFavs()
        {
            AllFavImages = new ObservableCollection<ImageItem>();

            // Query the database and load all associated items to their respective collections.
            foreach (ImageItem image in AllImageItems)
            {
                image.CreateURIs();

                if ((bool)image.IsFav)
                {
                    AllFavImages.Add(image);
                }
            }
            Debug.WriteLine("All Favs Created");

            this.IsAllFavLoaded = true;
        }          
         
        public void LoadMainFavs()
        {
            // Load a list of the favourites on the main page.
            int AllFavLength = (AllFavImages == null) ? 0 : AllFavImages.Count;
            int MaxLength = 8;
            if (AllFavLength > 0)
            {
                MainFavImages = new ObservableCollection<ImageItem>(AllFavImages.Reverse().Take(MaxLength));
            }
            Debug.WriteLine("Main Favs Created");

            this.IsMainFavLoaded = true;
        }   
          
        public void AddFavImage(string newImageUri)
        {
            //Add the image to the list of all favourites
            AllFavImages.Add(newImageUri);

            //Update the list of main faorites to have the latest added favourites
            // remove the last one and add in the new one
            int MainFavLength = (MainFavImages == null) ? 0 : MainFavImages.Count;
            int MaxLength = 8;
            if (MainFavLength > 0)
            {
                if (MainFavLength >= MaxLength)
                   MainFavImages.Remove(MainFavImages.Last());

                MainFavImages.Add(newImageUri);                
            }
            Debug.WriteLine("Favourites Updated");
        }   

        // Query database and load the collections and list used by the pivot pages.
        public void LoadCollectionsFromDatabase()
        {

            // Specify the query for all to-do items in the database.
            var ImagesItemsInDb = from ImageItem image in ImageDB.NPImages
                                  select image;
            Debug.WriteLine("Query Create");

            // Query the database and load all to-do items.
            AllImageItems = new ObservableCollection<ImageItem>(ImagesItemsInDb);
            Debug.WriteLine("Images now in Collection");

            this.IsDataLoaded = true;
        }        
         **/


        #endregion
    }
}
