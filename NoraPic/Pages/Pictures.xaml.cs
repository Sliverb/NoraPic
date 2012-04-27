using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

namespace NoraPic.Pages
{
    public partial class Pictures : PhoneApplicationPage
    {
        public Pictures()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            if (DataContext == null)
                DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(PicturePage_Loaded);
        }

        private void PicturePage_Loaded(object sender, RoutedEventArgs e)
        {
            string pivotIndex = "";
            if (NavigationContext.QueryString.TryGetValue("id", out pivotIndex))
            {
                //-1 because the Pivot is 0-indexed, so pivot item 2 has an index of 1
                picPivot.SelectedIndex = int.Parse(pivotIndex) - 1;
            }

            // Load all images from the DB to memory (observable collection)
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadCollectionsFromDatabase();
            }

            // Load all the Favourite images to an observable collection
            if (!App.ViewModel.IsAllFavLoaded)
            {
                App.ViewModel.LoadAllFavs();
            }
        }
    }
}